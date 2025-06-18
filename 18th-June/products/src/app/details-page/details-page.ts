import { Component, inject, OnInit, signal } from '@angular/core';
import { SearchProduct } from '../search-product';
import { ActivatedRoute } from '@angular/router';
import { ProductModel } from '../../model/product.model';

@Component({
  selector: 'app-details-page',
  imports: [],
  templateUrl: './details-page.html',
  styleUrl: './details-page.css',
})
export class DetailsPage implements OnInit {
  private productService = inject(SearchProduct);
  private router = inject(ActivatedRoute);
  product = signal<ProductModel | null>(null);
  error = signal<string | null>(null);
  ngOnInit(): void {
    this.productService
      .getProductById(this.router.snapshot.params['id'])
      .subscribe({
        next: (product) => {
          this.product.set(product);  
        },
        error: (error) => {
          console.error('Error fetching product details:', error);
          this.error.set('Error fetching product details');
        },
      });
  }
}
