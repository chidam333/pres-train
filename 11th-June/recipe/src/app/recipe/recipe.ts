import { Component, inject, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { Recipe, RecipeResponse } from '../models/recipe';
import { RecipeCard } from '../recipe-card/recipe-card';

@Component({
  selector: 'app-recipe',
  standalone: true,
  imports: [RecipeCard],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class RecipeComponent{
  private recipeService = inject(RecipeService);
  
  recipes = signal<Recipe[]>([]);
  loading = signal<boolean>(true);
  error = signal<string | null>(null);

  constructor() {
    this.recipeService.getAllRecipes().subscribe({
      next: (response) => {
        const recipeResponse = response as RecipeResponse;
        console.log('Fetched recipes:', recipeResponse.recipes);
        this.recipes.set(recipeResponse.recipes);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error fetching recipes:', err);
        this.error.set('Failed to load recipes. Please try again later.');
        this.loading.set(false);
      },
      complete: () => {
        console.log('Recipe fetching completed');
      }
    });
  }

  searchRecipesComp(query: string) {
    this.loading.set(true);
    this.error.set(null);

    this.recipeService.searchRecipes(query).subscribe({
      next: (response) => {
        const recipeResponse = response as RecipeResponse;
        console.log('Searched recipes:', recipeResponse.recipes);
        this.recipes.set(recipeResponse.recipes);
        this.loading.set(false);
      },
      error: (err) => {
        console.error('Error searching recipes:', err);
        this.error.set('Failed to search recipes. Please try again later.');
        this.loading.set(false);
      },
      complete: () => {
        console.log('Recipe searching completed');
      }
    });
  }
}
