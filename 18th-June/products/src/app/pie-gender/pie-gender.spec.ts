import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PieGender } from './pie-gender';

describe('PieGender', () => {
  let component: PieGender;
  let fixture: ComponentFixture<PieGender>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PieGender]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PieGender);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
