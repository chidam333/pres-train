
import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Color, ColorCreate, ColorUpdate } from '../models/color.model';
import { toObservable } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ColorService {
  private colors = signal<Color[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5127/api/Colors';

  colors$ = toObservable(this.colors);

  constructor() {
    this.loadColors();
  }

  loadColors() {
    this.http.get<Color[]>(this.baseUrl).subscribe(data => this.colors.set(data));
  }

  create(colorData: ColorCreate): Observable<Color> {
    return this.http.post<Color>(this.baseUrl, colorData);
  }

  update(id: number, colorData: ColorUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, colorData);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
