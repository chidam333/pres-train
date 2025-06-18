import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoleBar } from './role-bar';

describe('RoleBar', () => {
  let component: RoleBar;
  let fixture: ComponentFixture<RoleBar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RoleBar]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RoleBar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
