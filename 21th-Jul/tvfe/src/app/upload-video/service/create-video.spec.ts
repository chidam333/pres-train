import { TestBed } from '@angular/core/testing';

import { CreateVideo } from './create-video';

describe('CreateVideo', () => {
  let service: CreateVideo;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CreateVideo);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
