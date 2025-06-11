import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { RecipeComponent } from './recipe/recipe';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, RecipeComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'recipe';
}
