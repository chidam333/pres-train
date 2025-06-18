import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SearchProduct {
  private http = inject(HttpClient);
  getProductSearchResult(searchData: string, skip: number): Observable<any> {
    return this.http.get(
      'https://dummyjson.com/products/search?q=' + searchData + '&skip=' + skip
    );
  }
  getProductById(id: number): Observable<any> {
    return this.http.get('https://dummyjson.com/products/' + id);
  }
}
