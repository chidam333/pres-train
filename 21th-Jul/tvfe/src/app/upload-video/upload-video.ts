import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CreateVideo } from './service/create-video';

@Component({
  selector: 'app-upload-video',
  imports: [RouterLink, ReactiveFormsModule],
  templateUrl: './upload-video.html',
  styleUrl: './upload-video.css'
})
export class UploadVideo {
  private createVideoService = inject(CreateVideo);
  private router = inject(Router);
  private fb = inject(FormBuilder);

  uploadForm: FormGroup;
  selectedFile: File | null = null;
  isUploading = false;
  uploadProgress = 0;
  errorMessage = '';
  formSubmitted = false;

  constructor() {
    this.uploadForm = this.fb.group({
      title: ['', [Validators.required, Validators.minLength(1)]],
      description: ['', [Validators.required, Validators.minLength(1)]]
    });
  }

  onFileSelected(event: Event): void {
    const target = event.target as HTMLInputElement;
    console.log('File selected:', target.files);
    if (target.files && target.files.length > 0) {
      this.selectedFile = target.files[0];
      this.errorMessage = '';
    }
  }

  async onSubmit(): Promise<void> {
    this.formSubmitted = true;
    this.errorMessage = '';
    this.uploadForm.markAllAsTouched();
    console.log('Form submitted:', this.uploadForm.value, 'Selected file:', this.selectedFile);
    if (this.uploadForm.valid && this.selectedFile) {
      this.isUploading = true;
      this.uploadProgress = 0;

      try {
        const formData = new FormData();
        formData.append('File', this.selectedFile, this.selectedFile.name);

        await this.uploadVideo(formData, this.uploadForm.get('title')?.value, this.uploadForm.get('description')?.value);
      } catch (error) {
        this.errorMessage = 'Failed to upload video. Please try again.';
        console.error('Upload error:', error);
      } finally {
        this.isUploading = false;
        this.uploadProgress = 0;
      }
    } else {
      if (!this.selectedFile) {
        this.errorMessage = 'Please select a video file to upload.';
      }
      console.log('Form is invalid or no file selected');
    }
  }

  async uploadVideo(videoData: FormData, title: string, description: string): Promise<void> {
    console.log('Uploading video...');

    const progressInterval = setInterval(() => {
      if (this.uploadProgress < 90) {
        this.uploadProgress += 10;
      }
    }, 200);

    try {
      const result = await this.createVideoService.createVideo(videoData, title, description);

      clearInterval(progressInterval);
      this.uploadProgress = 100;
      
      if (result.error) {
        this.errorMessage = 'Error uploading video: ' + result.error;
        console.error('Error uploading video:', result.error);
      } else {
        console.log('Video uploaded successfully:', result);
        setTimeout(() => {
          this.router.navigate(['/']);
        }, 500);
      }
    } catch (error) {
      clearInterval(progressInterval);
      throw error;
    }
  }
}
