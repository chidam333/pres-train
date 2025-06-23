import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructorTopBar } from './instructor-top-bar';

describe('InstructorTopBar', () => {
  let component: InstructorTopBar;
  let fixture: ComponentFixture<InstructorTopBar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructorTopBar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstructorTopBar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
