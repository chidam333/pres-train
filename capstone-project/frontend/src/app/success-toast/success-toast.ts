import { Component, input, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-success-toast',
  imports: [],
  templateUrl: './success-toast.html',
  styleUrl: './success-toast.css'
})
export class SuccessToast implements OnInit {
  title = input();
  message = input.required<string[]>();
  isVisible = signal(true);

  ngOnInit() {
    setTimeout(() => {
      this.isVisible.set(false);
    }, 3000);
  }
}
