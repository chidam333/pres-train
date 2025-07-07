import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Home } from './home';  // adjust path as needed

describe('Home Component', () => {
  let component: Home;
  let fixture: ComponentFixture<Home>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [ReactiveFormsModule,Home],
    }).compileComponents();

    fixture = TestBed.createComponent(Home);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });


  it('  fields as invalid if empty', () => {
    component.formGroup.setValue({
      name: '',
      email: '',
      contact: '',
      amount: ''
    });
    expect(component.formGroup.valid).toBeFalse();
  });

  it('  contact invalid if not 10-digit Indian number', () => {
    component.formGroup.get('contact')?.setValue('54321');
    expect(component.formGroup.get('contact')?.valid).toBeFalse();
  });


  it('should validate form with correct data', () => {
    component.formGroup.setValue({
      name: 'chidam',
      email: 'chidam@gmail.com',
      contact: '8765432109',
      amount: 150
    });
    expect(component.formGroup.valid).toBeTrue();
  });


it('should open Razorpay when form is valid', () => {
  const openSpy = jasmine.createSpy('open');
  (window as any).Razorpay = function () {
    return {
      open: openSpy
    };
  };

  component.formGroup.setValue({
    name: 'Rajesh',
    email: 'rajesh.kumar@gmail.com',
    contact: '7123456789',
    amount: 500
  });

  component.pay();

  expect(openSpy).toHaveBeenCalled();
});

  it('should create component', () => {
    expect(component).toBeTruthy();
  });

  it('  email invalid for invalid email format', () => {
    component.formGroup.get('email')?.setValue('wrong.email.format');
    expect(component.formGroup.get('email')?.valid).toBeFalse();
  });

  it('  amount invalid for zero or negative values', () => {
    component.formGroup.get('amount')?.setValue(-25);
    expect(component.formGroup.get('amount')?.valid).toBeFalse();
  });

});
