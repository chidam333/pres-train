import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailsPage } from './details-page';

describe('DetailsPage', () => {
  let component: DetailsPage;
  let fixture: ComponentFixture<DetailsPage>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetailsPage]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetailsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
