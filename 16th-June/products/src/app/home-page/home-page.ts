import { Component, signal, WritableSignal } from '@angular/core';
import { SearchBar } from '../search-bar/search-bar';
import { ProductModel } from '../../model/product.model';

@Component({
  selector: 'app-home-page',
  imports: [SearchBar],
  templateUrl: './home-page.html',
  styleUrl: './home-page.css'
})
export class HomePage {
  loading: WritableSignal<boolean> = signal(false);
  products: WritableSignal<ProductModel[]> = signal([]);
}
