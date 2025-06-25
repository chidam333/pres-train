import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MaterialListInstructor } from './material-list-instructor';

describe('MaterialListInstructor', () => {
  let component: MaterialListInstructor;
  let fixture: ComponentFixture<MaterialListInstructor>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MaterialListInstructor]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MaterialListInstructor);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
