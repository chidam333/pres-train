import { TestBed } from '@angular/core/testing';

import { GetVideos } from './get-videos';

describe('GetVideos', () => {
  let service: GetVideos;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(GetVideos);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
