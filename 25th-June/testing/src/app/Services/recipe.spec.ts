import { TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { provideHttpClient } from '@angular/common/http';
import { RecipeService } from './recipe';

describe('RecipeService', () => {
  let service: RecipeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });
    service = TestBed.inject(RecipeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all recipes', () => {
    const mockResponse = { recipes: [
      { id: 1, name: 'Recipe 1', image: 'image1.jpg', rating: 4.5, cuisine: 'Italian' },
      { id: 2, name: 'Recipe 2', image: 'image2.jpg', rating: 4.0, cuisine: 'Mexican' }
    ], total: 2 };

    service.getAllRecipes().subscribe(response => {
      console.log(response);
      expect(response).toEqual(mockResponse);
    });

    const req = httpMock.expectOne('https://dummyjson.com/recipes');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse);
  });

  it('should get recipe by id', () => {
    const mockRecipe = { 
      id: 1, 
      name: 'Test Recipe',
      image: 'test.jpg',
      rating: 4.5,
      cuisine: 'Italian'
    };

    service.getRecipeById(1).subscribe(recipe => {
      expect(recipe.id).toBe(1);
      expect(recipe.name).toBe('Test Recipe');
    });

    const req = httpMock.expectOne('https://dummyjson.com/recipes/1');
    expect(req.request.method).toBe('GET');
    req.flush(mockRecipe);
  });

  afterEach(() => {
    httpMock.verify();
  });
});
