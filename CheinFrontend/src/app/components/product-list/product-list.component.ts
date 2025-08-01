import { Component, inject, signal, computed, effect } from '@angular/core';
import { ProductService } from '../../services/product.service';
import { CurrencyPipe } from '@angular/common';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { CategoryService } from '../../services/category.service';
import { Product } from '../../models/product.model';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-product-list',
  template: `
    <div class="flex">
      <!-- Category Sidebar -->
      <div class="w-1/4 pr-8">
        <h3 class="text-xl font-semibold mb-4">Categories</h3>
        <ul class="space-y-2">
          <li>
            <a href="#" 
               (click)="onSelectCategory(null); $event.preventDefault()" 
               class="hover:text-blue-500" 
               [class.font-bold]="selectedCategory() === null">All Products</a>
          </li>
          @for(category of categories(); track category.categoryId) {
            <li>
              <a href="#" 
                 (click)="onSelectCategory(category.name); $event.preventDefault()" 
                 class="hover:text-blue-500" 
                 [class.font-bold]="selectedCategory() === category.name">{{ category.name }}</a>
            </li>
          }
        </ul>
      </div>

      <!-- Product Grid -->
      <div class="w-3/4">
        <h2 class="text-2xl font-bold mb-4">Products</h2>
        <div class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
          @for(product of filteredProducts(); track product.productId) {
            <div class="border p-4 rounded-lg flex flex-col">
              <img [src]="product.image" alt="{{ product.productName }}" class="w-full h-48 object-cover mb-4 rounded">
              <div class="flex-grow">
                <h3 class="text-lg font-semibold">{{ product.productName }}</h3>
                <p class="text-gray-600 text-sm mt-1">{{ product.categoryName }}</p>
              </div>
              <div class="flex justify-between items-center mt-4">
                <span class="text-xl font-bold">{{ product.price | currency:'INR' }}</span>
                <button (click)="addToCart(product)" class="bg-blue-500 text-white px-4 py-2 rounded hover:bg-blue-600 transition-colors">Add to Cart</button>
              </div>
            </div>
          }
        </div>
      </div>
    </div>
  `,
  imports: [CurrencyPipe]
})
export class ProductListComponent {
  // Services
  productService = inject(ProductService);
  cartService = inject(ShoppingCartService);
  categoryService = inject(CategoryService);

  // Signals
  products = this.productService.products;
  categories = this.categoryService.categories;
  selectedCategory = signal<string | null>(null);

  // A computed signal that automatically re-evaluates when its dependencies (products or selectedCategory) change.
  filteredProducts = computed(() => {
    const categoryName = this.selectedCategory();
    const allProducts = this.products();

    console.log(`Filtering for category Name: ${categoryName}`);
    console.log(`All products count: ${allProducts.length}`);

    if (!Array.isArray(allProducts)) {
      console.error('ProductService.products() did not return an array!', allProducts);
      return []; // Return empty array to prevent errors
    }

    if (categoryName === null) {
      console.log('Returning all products.');
      return allProducts;
    }

    const filtered = allProducts.filter(p => p.categoryName === categoryName);
    console.log(`Filtered products count for category ${categoryName}: ${filtered.length}`);
    return filtered;
  });

  // Method to update the selected category signal when a user clicks.
  onSelectCategory(categoryName: string | null): void {
    this.selectedCategory.set(categoryName);
  }

  // Method to add items to cart with feedback
  addToCart(product: any): void {
    this.cartService.addToCart(product);
    // You could add a toast notification here or some other feedback
    console.log(`Added ${product.productName} to cart`);
  }

  constructor() {
    // Optional: Log changes to the filtered list for debugging
    effect(() => {
      console.log(`Filtered product count (effect): ${this.filteredProducts().length}`);
    });
  }
}