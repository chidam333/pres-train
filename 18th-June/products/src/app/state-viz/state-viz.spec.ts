import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StateViz } from './state-viz';

describe('StateViz', () => {
  let component: StateViz;
  let fixture: ComponentFixture<StateViz>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StateViz]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StateViz);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
