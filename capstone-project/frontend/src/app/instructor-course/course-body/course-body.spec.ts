import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseBody } from './course-body';

describe('CourseBody', () => {
  let component: CourseBody;
  let fixture: ComponentFixture<CourseBody>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CourseBody]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CourseBody);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
