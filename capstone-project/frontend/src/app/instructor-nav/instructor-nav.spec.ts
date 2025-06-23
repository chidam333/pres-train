import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructorNav } from './instructor-nav';

describe('InstructorNav', () => {
  let component: InstructorNav;
  let fixture: ComponentFixture<InstructorNav>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructorNav]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstructorNav);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
