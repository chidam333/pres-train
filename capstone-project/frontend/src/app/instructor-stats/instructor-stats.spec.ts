import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructorStats } from './instructor-stats';

describe('InstructorStats', () => {
  let component: InstructorStats;
  let fixture: ComponentFixture<InstructorStats>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructorStats]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstructorStats);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
