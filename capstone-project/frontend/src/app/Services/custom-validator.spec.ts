import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { CustomValidator } from './custom-validator';

describe('CustomValidator Service', () => {
  let service: CustomValidator;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        CustomValidator
      ]
    });
    service = TestBed.inject(CustomValidator);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
