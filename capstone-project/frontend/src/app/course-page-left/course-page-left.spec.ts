import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoursePageLeft } from './course-page-left';

describe('CoursePageLeft', () => {
  let component: CoursePageLeft;
  let fixture: ComponentFixture<CoursePageLeft>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CoursePageLeft]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoursePageLeft);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
