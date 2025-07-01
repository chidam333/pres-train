import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CoursePageRight } from './course-page-right';

describe('CoursePageRight', () => {
  let component: CoursePageRight;
  let fixture: ComponentFixture<CoursePageRight>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CoursePageRight]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CoursePageRight);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
