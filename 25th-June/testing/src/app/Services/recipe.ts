import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, catchError, throwError } from 'rxjs';
import { Recipe, RecipeResponse } from '../Models/recipe.model';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private readonly baseUrl = 'https://dummyjson.com/recipes';

  constructor(private http: HttpClient) { }

  getAllRecipes(): Observable<RecipeResponse> {
    return this.http.get<RecipeResponse>(this.baseUrl).pipe(
      catchError(this.handleError)
    );
  }

  getRecipeById(id: number): Observable<Recipe> {
    return this.http.get<Recipe>(`${this.baseUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any): Observable<never> {
    console.error('An error occurred:', error);
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
