import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ListVideos } from './list-videos';

describe('ListVideos', () => {
  let component: ListVideos;
  let fixture: ComponentFixture<ListVideos>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ListVideos]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ListVideos);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
