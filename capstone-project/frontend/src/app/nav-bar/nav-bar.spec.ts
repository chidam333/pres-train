import { ComponentFixture, TestBed } from '@angular/core/testing';
import { signal } from '@angular/core';
import { provideZonelessChangeDetection } from '@angular/core';

import { NavBar } from './nav-bar';

describe('NavBar Component', () => {
  let component: NavBar;
  let fixture: ComponentFixture<NavBar>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NavBar],
      providers: [
        provideZonelessChangeDetection()
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavBar);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with default values', () => {
    expect(component.isStudent()).toBeUndefined();
    expect(component.isLoginForm()).toBeUndefined();
  });

  it('should toggle role correctly', () => {
    component.isStudent.set(true);
    expect(component.isStudent()).toBe(true);
    
    component.toggleRole();
    expect(component.isStudent()).toBe(false);
    
    component.toggleRole();
    expect(component.isStudent()).toBe(true);
  });

  it('should set login form to true', () => {
    component.setLoginForm();
    expect(component.isLoginForm()).toBe(true);
  });

  it('should set login form to false for signup', () => {
    component.setSignUpForm();
    expect(component.isLoginForm()).toBe(false);
  });

  it('should handle isHome input', () => {
    fixture.componentRef.setInput('isHome', true);
    fixture.detectChanges();
    
    expect(component.isHome()).toBe(true);
  });

  it('should respond to effect when isHome changes', () => {
    spyOn(console, 'log');
    
    fixture.componentRef.setInput('isHome', true);
    fixture.detectChanges();
    
    expect(console.log).toHaveBeenCalledWith({ ishome: true });
  });
});
