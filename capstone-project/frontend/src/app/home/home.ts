import { Component, inject, signal, WritableSignal, computed, effect } from '@angular/core';
import { NavBar } from "../nav-bar/nav-bar";
import { AuthFetch } from '../auth/auth-service/auth-fetch';
import { Profile } from "../profile/profile";
import { MyCourseList } from "../my-course-list/my-course-list";
import { OtherCourseList } from "../other-course-list/other-course-list";
import { SuccessToast } from "../success-toast/success-toast";
import { Notification as NotificationHub } from '../Services/notification';

@Component({
  selector: 'app-home',
  imports: [NavBar, Profile, MyCourseList, OtherCourseList, SuccessToast],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  authFetch = inject(AuthFetch);
  notification = inject(NotificationHub);
  refreshSignal = signal(0);
  uniqueName = this.authFetch.getUniqueName();
  
  toastMessages = signal<string[]>([]);

  constructor() {
    effect(() => {
      const latestMessage = this.notification.latestMessage();
      if (latestMessage) {
        this.toastMessages.set([latestMessage]);
        setTimeout(() => {
          this.toastMessages.set([]);
          this.notification.latestMessage.set(null);
        }, 10000);
      }
    });
  }
}
