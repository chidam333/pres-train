import { Component, input, signal, effect } from '@angular/core';

@Component({
  selector: 'app-success-toast',
  imports: [],
  templateUrl: './success-toast.html',
  styleUrl: './success-toast.css'
})
export class SuccessToast {
  title = input();
  message = input.required<string[]>();
  isVisible = signal(true);
  private timeoutId: number | null = null;

  constructor() {
    effect(() => {
      const messages = this.message();
      if (messages && messages.length > 0) {
        if (this.timeoutId) {
          clearTimeout(this.timeoutId);
        }
        this.isVisible.set(true);
        this.timeoutId = setTimeout(() => {
          this.isVisible.set(false);
          this.timeoutId = null;
        }, 4000);
      }
    });
  }
}
