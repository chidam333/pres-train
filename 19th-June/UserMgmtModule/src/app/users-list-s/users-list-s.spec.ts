import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UsersListS } from './users-list-s';

describe('UsersListS', () => {
  let component: UsersListS;
  let fixture: ComponentFixture<UsersListS>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UsersListS]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UsersListS);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
