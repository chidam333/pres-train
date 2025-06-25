import { Recipe, RecipeResponse } from './recipe.model';

describe('Recipe Model', () => {
  it('should create a recipe object', () => {
    const recipe: Recipe = {
      id: 1,
      name: 'Test Recipe',
      image: 'test.jpg',
      rating: 4.5,
      cuisine: 'Italian'
    };

    expect(recipe).toBeTruthy();
    expect(recipe.id).toBe(1);
    expect(recipe.name).toBe('Test Recipe');
    expect(recipe.cuisine).toBe('Italian');
  });

  it('should create a recipe response object', () => {
    const response: RecipeResponse = {
      recipes: [],
      total: 0
    };

    expect(response).toBeTruthy();
    expect(response.recipes).toEqual([]);
    expect(response.total).toBe(0);
  });
});
