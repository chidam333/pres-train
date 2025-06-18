import { TestBed } from '@angular/core/testing';

import { SearchProduct } from './search-product';

describe('SearchProduct', () => {
  let service: SearchProduct;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SearchProduct);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
