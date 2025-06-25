import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Recipe } from '../../Models/recipe.model';
import { RecipeService } from '../../Services/recipe';

@Component({
  selector: 'app-recipe',
  imports: [CommonModule],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class RecipeComponent implements OnInit {
  recipes: Recipe[] = [];
  loading: boolean = false;

  constructor(private recipeService: RecipeService) {}

  ngOnInit(): void {
    this.loadRecipes();
  }

  loadRecipes(): void {
    this.loading = true;
    this.recipeService.getAllRecipes().subscribe({
      next: (response) => {
        this.recipes = response.recipes;
        this.loading = false;
      },
      error: () => {
        this.loading = false;
      }
    });
  }
}
