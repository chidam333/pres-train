import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable(
)
export class RecipeService {
  private http = inject(HttpClient);
  private readonly baseUrl = 'http://dummysadasjson.com/recipes';

  getAllRecipes(){
    return this.http.get(this.baseUrl);
  }

  getRecipesByLimit(limit: number = 10){
    return this.http.get(`${this.baseUrl}?limit=${limit}`);
  }

  searchRecipes(query: string){
    return this.http.get(`${this.baseUrl}/search?q=${query}`);
  }
}
