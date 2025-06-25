export interface Recipe {
  id: number;
  name: string;
  image: string;
  rating: number;
  cuisine: string;
}

export interface RecipeResponse {
  recipes: Recipe[];
  total: number;
}
