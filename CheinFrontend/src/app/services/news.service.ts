import { Injectable, signal, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { News, NewsCreate, NewsUpdate } from '../models/news.model';
import { toObservable } from '@angular/core/rxjs-interop';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class NewsService {
  private news = signal<News[]>([]);
  private http = inject(HttpClient);
  private baseUrl = 'http://localhost:5127/api/News';

  news$ = toObservable(this.news);

  constructor() {
    this.loadNews();
  }

  loadNews() {
    this.http.get<News[]>(this.baseUrl).subscribe(data => this.news.set(data));
  }

  create(newsData: NewsCreate): Observable<News> {
    return this.http.post<News>(this.baseUrl, newsData);
  }

  update(id: number, newsData: NewsUpdate): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, newsData);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }

  exportCsv() {
    return this.http.get(`${this.baseUrl}/export/csv`, { responseType: 'blob' });
  }

  exportExcel() {
    return this.http.get(`${this.baseUrl}/export/excel`, { responseType: 'blob' });
  }
}