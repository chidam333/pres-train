import { ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructorHelp } from './instructor-help';

describe('InstructorHelp', () => {
  let component: InstructorHelp;
  let fixture: ComponentFixture<InstructorHelp>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [InstructorHelp]
    })
    .compileComponents();

    fixture = TestBed.createComponent(InstructorHelp);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
