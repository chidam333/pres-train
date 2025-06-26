import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MyCourseList } from './my-course-list';

describe('MyCourseList', () => {
  let component: MyCourseList;
  let fixture: ComponentFixture<MyCourseList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MyCourseList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MyCourseList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
