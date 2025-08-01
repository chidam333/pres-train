
import { Component, inject, signal } from '@angular/core';
import { ColorService } from '../../services/color.service';
import { AsyncPipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Color, ColorCreate, ColorUpdate } from '../../models/color.model';

@Component({
  selector: 'app-color-list',
  template: `
    <div class="container mx-auto p-4">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-3xl font-bold text-gray-800">Color Management</h2>
        <button 
          (click)="showAddForm()"
          class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-lg transition-colors">
          Add Color
        </button>
      </div>

      <!-- Add/Edit Form -->
      @if (showForm()) {
        <div class="bg-white rounded-lg shadow-md p-6 mb-6">
          <h3 class="text-xl font-semibold mb-4">{{ editingColor() ? 'Edit' : 'Add' }} Color</h3>
          <form (ngSubmit)="saveColor()" class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Color Name</label>
              <input
                type="text"
                [(ngModel)]="formData.name"
                name="name"
                required
                maxlength="50"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter color name">
            </div>
            
            <div class="flex space-x-2">
              <button
                type="submit"
                [disabled]="!formData.name.trim()"
                class="bg-blue-500 hover:bg-blue-600 disabled:bg-gray-400 text-white px-4 py-2 rounded-lg transition-colors">
                {{ editingColor() ? 'Update' : 'Create' }}
              </button>
              <button
                type="button"
                (click)="cancelForm()"
                class="bg-gray-500 hover:bg-gray-600 text-white px-4 py-2 rounded-lg transition-colors">
                Cancel
              </button>
            </div>
          </form>
        </div>
      }

      <!-- Colors Grid -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4 gap-4">
        @for(color of colorService.colors$ | async; track color.colorId) {
          <div class="bg-white rounded-lg shadow-md p-4 border-l-4 border-blue-500">
            <div class="flex items-center justify-between">
              <div class="flex items-center space-x-3">
                <h3 class="text-lg font-semibold text-gray-800">{{ color.name }}</h3>
              </div>
              <div class="flex space-x-2">
                <button
                  (click)="editColor(color)"
                  class="bg-yellow-500 hover:bg-yellow-600 text-white px-2 py-1 rounded text-sm transition-colors">
                  Edit
                </button>
                <button
                  (click)="deleteColor(color.colorId)"
                  class="bg-red-500 hover:bg-red-600 text-white px-2 py-1 rounded text-sm transition-colors">
                  Delete
                </button>
              </div>
            </div>
            <div class="mt-2 text-sm text-gray-600">
              ID: {{ color.colorId }}
            </div>
          </div>
        }
      </div>

      @if ((colorService.colors$ | async)?.length === 0) {
        <div class="text-center py-8">
          <p class="text-gray-500 text-lg">No colors found. Add your first color!</p>
        </div>
      }
    </div>
  `,
  imports: [AsyncPipe, FormsModule, CommonModule]
})
export class ColorListComponent {
  colorService = inject(ColorService);
  showForm = signal(false);
  editingColor = signal<Color | null>(null);
  
  formData = {
    name: ''
  };

  showAddForm() {
    this.resetForm();
    this.editingColor.set(null);
    this.showForm.set(true);
  }

  editColor(color: Color) {
    this.editingColor.set(color);
    this.formData = {
      name: color.name
    };
    this.showForm.set(true);
  }

  saveColor() {
    if (!this.formData.name.trim()) {
      return;
    }

    const colorData = {
      name: this.formData.name.trim()
    };

    if (this.editingColor()) {
      this.colorService.update(this.editingColor()!.colorId, colorData as ColorUpdate).subscribe({
        next: () => {
          this.colorService.loadColors();
          this.cancelForm();
        },
        error: (error) => console.error('Error updating color:', error)
      });
    } else {
      this.colorService.create(colorData as ColorCreate).subscribe({
        next: () => {
          this.colorService.loadColors();
          this.cancelForm();
        },
        error: (error) => console.error('Error creating color:', error)
      });
    }
  }

  deleteColor(id: number) {
    if (confirm('Are you sure you want to delete this color?')) {
      this.colorService.delete(id).subscribe({
        next: () => {
          this.colorService.loadColors();
        },
        error: (error) => console.error('Error deleting color:', error)
      });
    }
  }

  cancelForm() {
    this.showForm.set(false);
    this.editingColor.set(null);
    this.resetForm();
  }

  resetForm() {
    this.formData = {
      name: ''
    };
  }
}
