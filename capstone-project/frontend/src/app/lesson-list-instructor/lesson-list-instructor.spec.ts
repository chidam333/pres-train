import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LessonListInstructor } from './lesson-list-instructor';

describe('LessonListInstructor', () => {
  let component: LessonListInstructor;
  let fixture: ComponentFixture<LessonListInstructor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LessonListInstructor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LessonListInstructor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
