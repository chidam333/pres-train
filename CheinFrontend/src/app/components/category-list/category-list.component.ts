
import { Component, inject } from '@angular/core';
import { CategoryService } from '../../services/category.service';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-category-list',
  template: `
    <h2 class="text-2xl font-bold mb-4">Categories</h2>
    <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
      @for(category of categoryService.categories$ | async; track category.id) {
        <div class="border p-4 rounded-lg">
          <h3 class="text-lg font-semibold">{{ category.name }}</h3>
        </div>
      }
    </div>
  `,
  imports: [AsyncPipe]
})
export class CategoryListComponent {
  categoryService = inject(CategoryService);
}
