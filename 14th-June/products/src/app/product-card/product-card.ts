import { Component, Input } from '@angular/core';
import { ProductModel } from '../../model/product.model';

@Component({
  selector: 'app-product-card',
  imports: [],
  templateUrl: './product-card.html',
  styleUrl: './product-card.css'
})
export class ProductCard {
  @Input() products!: ProductModel[];
}
