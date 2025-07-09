import { TestBed } from '@angular/core/testing';
import { provideZonelessChangeDetection } from '@angular/core';

import { Notification } from './notification';

describe('Notification Service', () => {
  let service: Notification;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideZonelessChangeDetection(),
        Notification
      ]
    });
    service = TestBed.inject(Notification);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should initialize with null latest message', () => {
    expect(service.latestMessage()).toBeNull();
  });

  it('should update latest message', () => {
    const testMessage = 'Test notification message';
    
    service.latestMessage.set(testMessage);
    
    expect(service.latestMessage()).toBe(testMessage);
  });

  it('should clear latest message', () => {
    service.latestMessage.set('Test message');
    expect(service.latestMessage()).toBe('Test message');
    
    service.latestMessage.set(null);
    expect(service.latestMessage()).toBeNull();
  });

  it('should handle multiple message updates', () => {
    const messages = ['Message 1', 'Message 2', 'Message 3'];
    
    messages.forEach(message => {
      service.latestMessage.set(message);
      expect(service.latestMessage()).toBe(message);
    });
  });

  it('should be a singleton service', () => {
    const anotherInstance = TestBed.inject(Notification);
    expect(service).toBe(anotherInstance);
  });
});
