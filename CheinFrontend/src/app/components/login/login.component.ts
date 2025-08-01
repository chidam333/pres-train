import { Component, inject } from '@angular/core';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { ProductService } from '../../services/product.service';
import { CategoryService } from '../../services/category.service';
import { OrderService } from '../../services/order.service';
import { NewsService } from '../../services/news.service';
import { ColorService } from '../../services/color.service';

@Component({
  selector: 'app-login',
  template: `
    <h2 class="text-2xl font-bold mb-4">Login</h2>
    <form [formGroup]="loginForm" (ngSubmit)="onSubmit()" class="space-y-4">
      <div>
        <label for="username" class="block text-sm font-medium text-gray-700">Username</label>
        <input id="username" formControlName="username" class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
      </div>
      <div>
        <label for="password" class="block text-sm font-medium text-gray-700">Password</label>
        <input id="password" type="password" formControlName="password" class="mt-1 block w-full px-3 py-2 border border-gray-300 rounded-md shadow-sm focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 sm:text-sm">
      </div>
      <button type="submit" class="bg-blue-500 text-white px-4 py-2 rounded">Login</button>
    </form>
  `,
  imports: [ReactiveFormsModule]
})
export class LoginComponent {
  fb = inject(FormBuilder);
  authService = inject(AuthService);
  productService = inject(ProductService);
  categoryService = inject(CategoryService);
  orderService = inject(OrderService);
  newsService = inject(NewsService);
  colorService = inject(ColorService);
  router = inject(Router);

  loginForm = this.fb.group({
    username: '',
    password: ''
  });

  onSubmit() {
    this.authService.login(this.loginForm.value).subscribe(response => {
      this.authService.token.set(response.token);
      this.loginForm.reset();

      // Resetting the state by reloading data
      this.productService.loadProducts();
      this.categoryService.loadCategories();
      this.orderService.loadOrders();
      this.newsService.loadNews();
      this.colorService.loadColors();

      // Navigate to home page after login
      this.router.navigate(['/']);
    });
  }
}