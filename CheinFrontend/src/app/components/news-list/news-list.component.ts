import { Component, inject, signal } from '@angular/core';
import { NewsService } from '../../services/news.service';
import { AsyncPipe, DatePipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { saveAs } from 'file-saver';
import { News, NewsCreate, NewsUpdate } from '../../models/news.model';

@Component({
  selector: 'app-news-list',
  template: `
    <div class="container mx-auto p-4">
      <div class="flex justify-between items-center mb-6">
        <h2 class="text-3xl font-bold text-gray-800">News Management</h2>
        <div class="flex space-x-2">
          <button 
            (click)="showAddForm()"
            class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-lg transition-colors">
            Add News
          </button>
          <button 
            (click)="exportCsv()" 
            class="bg-green-500 hover:bg-green-600 text-white px-4 py-2 rounded-lg transition-colors">
            Export CSV
          </button>
          <button 
            (click)="exportExcel()" 
            class="bg-indigo-500 hover:bg-indigo-600 text-white px-4 py-2 rounded-lg transition-colors">
            Export Excel
          </button>
        </div>
      </div>

      <!-- Add/Edit Form -->
      @if (showForm()) {
        <div class="bg-white rounded-lg shadow-md p-6 mb-6">
          <h3 class="text-xl font-semibold mb-4">{{ editingNews() ? 'Edit' : 'Add' }} News</h3>
          <form (ngSubmit)="saveNews()" class="space-y-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Title</label>
              <input
                type="text"
                [(ngModel)]="formData.title"
                name="title"
                required
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter news title">
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Short Description</label>
              <textarea
                [(ngModel)]="formData.shortDescription"
                name="shortDescription"
                required
                rows="2"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter short description"></textarea>
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Content</label>
              <textarea
                [(ngModel)]="formData.content"
                name="content"
                required
                rows="4"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter news content"></textarea>
            </div>
            
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Image URL</label>
              <input
                type="url"
                [(ngModel)]="formData.image"
                name="image"
                class="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:ring-2 focus:ring-blue-500"
                placeholder="Enter image URL (optional)">
            </div>
            
            <div>
              <label class="flex items-center">
                <input
                  type="checkbox"
                  [(ngModel)]="formData.status"
                  name="status"
                  class="mr-2">
                <span class="text-sm font-medium text-gray-700">Active</span>
              </label>
            </div>
            
            <div class="flex space-x-2">
              <button
                type="submit"
                class="bg-blue-500 hover:bg-blue-600 text-white px-4 py-2 rounded-lg transition-colors">
                {{ editingNews() ? 'Update' : 'Create' }}
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

      <!-- News List -->
      <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        @for(news of newsService.news$ | async; track news.newsId) {
          <div class="bg-white rounded-lg shadow-md overflow-hidden">
            <div class="p-4">
              <h3 class="text-lg font-semibold text-gray-800 mb-2">{{ news.title }}</h3>
              <p class="text-gray-600 text-sm mb-2">{{ news.shortDescription }}</p>
              <p class="text-gray-700 mb-3">{{ news.content }}</p>
              <div class="text-xs text-gray-500 mb-3">
                <p>Created: {{ news.createdDate | date:'medium' }}</p>
                <p>Status: <span class="px-2 py-1 rounded text-xs" 
                   [class]="news.status ? 'bg-green-100 text-green-800' : 'bg-red-100 text-red-800'">
                   {{ news.status ? 'Active' : 'Inactive' }}
                 </span></p>
              </div>
              <div class="flex space-x-2">
                <button
                  (click)="editNews(news)"
                  class="bg-yellow-500 hover:bg-yellow-600 text-white px-3 py-1 rounded text-sm transition-colors">
                  Edit
                </button>
                <button
                  (click)="deleteNews(news.newsId)"
                  class="bg-red-500 hover:bg-red-600 text-white px-3 py-1 rounded text-sm transition-colors">
                  Delete
                </button>
              </div>
            </div>
          </div>
        }
      </div>
    </div>
  `,
  imports: [AsyncPipe, DatePipe, FormsModule, CommonModule]
})
export class NewsListComponent {
  newsService = inject(NewsService);
  showForm = signal(false);
  editingNews = signal<News | null>(null);
  
  formData: any = {
    title: '',
    shortDescription: '',
    content: '',
    image: '',
    status: true
  };

  showAddForm() {
    this.resetForm();
    this.editingNews.set(null);
    this.showForm.set(true);
  }

  editNews(news: News) {
    this.editingNews.set(news);
    this.formData = {
      title: news.title,
      shortDescription: news.shortDescription,
      content: news.content,
      image: news.image || '',
      status: news.status === 1
    };
    this.showForm.set(true);
  }

  saveNews() {
    const newsData = {
      ...this.formData,
      userId: 1, // You might want to get this from auth service
      createdDate: new Date()
    };

    if (this.editingNews()) {
      const updateData: NewsUpdate = {
        newsId: this.editingNews()!.newsId,
        ...newsData
      };
      
      this.newsService.update(this.editingNews()!.newsId, updateData).subscribe({
        next: () => {
          this.newsService.loadNews();
          this.cancelForm();
        },
        error: (error) => console.error('Error updating news:', error)
      });
    } else {
      this.newsService.create(newsData as NewsCreate).subscribe({
        next: () => {
          this.newsService.loadNews();
          this.cancelForm();
        },
        error: (error) => console.error('Error creating news:', error)
      });
    }
  }

  deleteNews(id: number) {
    if (confirm('Are you sure you want to delete this news item?')) {
      this.newsService.delete(id).subscribe({
        next: () => {
          this.newsService.loadNews();
        },
        error: (error) => console.error('Error deleting news:', error)
      });
    }
  }

  cancelForm() {
    this.showForm.set(false);
    this.editingNews.set(null);
    this.resetForm();
  }

  resetForm() {
    this.formData = {
      title: '',
      shortDescription: '',
      content: '',
      image: '',
      status: true
    };
  }

  exportCsv() {
    this.newsService.exportCsv().subscribe(blob => {
      saveAs(blob, 'news.csv');
    });
  }

  exportExcel() {
    this.newsService.exportExcel().subscribe(blob => {
      saveAs(blob, 'news.xlsx');
    });
  }
}