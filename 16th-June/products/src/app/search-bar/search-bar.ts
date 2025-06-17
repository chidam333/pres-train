import {
  Component,
  Input,
  OnInit,
  signal,
  WritableSignal,
  inject,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SearchProduct } from '../search-product';
import {
  debounceTime,
  distinctUntilChanged,
  Subject,
  switchMap,
  tap,
} from 'rxjs';
import { ProductModel } from '../../model/product.model';

@Component({
  selector: 'app-search-bar',
  imports: [FormsModule],
  templateUrl: './search-bar.html',
  styleUrl: './search-bar.css',
})
export class SearchBar implements OnInit {
  query: WritableSignal<string> = signal('');
  @Input() loading: WritableSignal<boolean> = signal(false);
  @Input() products: WritableSignal<ProductModel[]> = signal([]);
  searchSubject = new Subject<string>();
  private searchProduct = inject(SearchProduct);
  ngOnInit() {
    this.searchSubject
      .pipe(
        debounceTime(400),
        distinctUntilChanged(),
        tap(() => this.loading.set(true)),
        switchMap((query) =>
          this.searchProduct.getProductSearchResult(query, 0)
        ),
        tap(() => this.loading.set(false))
      )
      .subscribe({
        next: (data: any) => {
          this.products.set(data.products);
        },
      });
  }
  handleSearch() {
    console.log('Search query:', this.query());
    this.searchSubject.next(this.query());
  }
}
