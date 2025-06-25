import {
  Component,
  ElementRef,
  inject,
  input,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Material } from '../Services/material';
import { SuccessToast } from '../success-toast/success-toast';

@Component({
  selector: 'app-material-list-instructor',
  imports: [FormsModule, SuccessToast],
  templateUrl: './material-list-instructor.html',
  styleUrl: './material-list-instructor.css',
})
export class MaterialListInstructor {
  materials = input.required<any[]>();
  lessonId = input.required<number>();
  materialSequenceNo = signal<number>(1);
  setSuccessMessage = signal<boolean>(false);
  file: File | null = null;
  materialService = inject(Material);
  @ViewChild('mdialog') dialog: ElementRef<HTMLDialogElement> | undefined;
  title = '';
  openDialog() {
    console.log('Opening dialog');
    this.dialog?.nativeElement.showModal();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
      console.log('File selected:', this.file);
    }
  }

  async addMaterial() {
    if (this.file == null) {
      alert('Please select a file to upload.');
      return;
    }
    const response = await this.materialService.uploadMaterial(
      this.lessonId(),
      this.title,
      this.materialSequenceNo(),
      this.file
    );
    if (response.success) {
      this.setSuccessMessage.set(true);
      this.dialog?.nativeElement.close();
      console.log('Material added successfully:', response.id);
    } else {
      alert('Failed to add material: ' + response.error);
      console.error(response.error);
    }
  }
}
