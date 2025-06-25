import {
  Component,
  computed,
  effect,
  ElementRef,
  inject,
  input,
  model,
  OnInit,
  signal,
  ViewChild,
  WritableSignal,
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
export class MaterialListInstructor implements OnInit {
  lessonId = input.required<number>();
  refreshTrigger = input.required<WritableSignal<number>>();
  materialSequenceNo = signal<number>(1);
  setSuccessMessage = signal<boolean>(false);
  materials = signal<any[]>([]);
  sortedMaterials = computed(() => {
    return this.materials().sort((a, b) => a.sequenceNo - b.sequenceNo);
  });
  message = signal<string[]>([]);
  file: File | null = null;
  materialService = inject(Material);
  @ViewChild('mdialog') dialog: ElementRef<HTMLDialogElement> | undefined;
  title = '';

  constructor() {
    effect(() => {
      this.refreshTrigger()();
      this.fetchMaterials();
    });
  }

  ngOnInit() {
    this.fetchMaterials();
  }

  fetchMaterials() {
    this.materialService
      .getMaterialsByLessonId(this.lessonId())
      .then((response) => {
        if ('error' in response) {
          alert('Failed to fetch materials: ' + response.error);
        } else {
          this.materials.set(response);
          console.log('Materials fetched:', this.materials());
        }
      });
  }

  triggerRefreshAll() {
    this.refreshTrigger().update((val) => val + 1);
  }

  openDialog() {
    this.dialog?.nativeElement.showModal();
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.file = input.files[0];
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
      this.message.set([`Material ${this.title} added successfully!`]);
      this.setSuccessMessage.set(true);
      this.dialog?.nativeElement.close();
      this.triggerRefreshAll();
    } else {
      alert('Failed to add material: ' + response.error);
      console.error(response.error);
    }
  }

  async deleteMaterial(materialId: number) {
    const response = await this.materialService.deleteMaterial(materialId);
    if ('error' in response) {
      alert('Failed to delete material: ' + response.error);
    } else {
      this.message.set([`Material ${materialId} deleted successfully!`]);
      this.setSuccessMessage.set(true);
      this.triggerRefreshAll();
    }
  }
}
