import { ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherCourseList } from './other-course-list';

describe('OtherCourseList', () => {
  let component: OtherCourseList;
  let fixture: ComponentFixture<OtherCourseList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [OtherCourseList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(OtherCourseList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
