import { Component, inject, signal, WritableSignal } from '@angular/core';
import { NavBar } from "../nav-bar/nav-bar";
import { AuthFetch } from '../auth/auth-service/auth-fetch';
import { Profile } from "../profile/profile";
import { MyCourseList } from "../my-course-list/my-course-list";
import { OtherCourseList } from "../other-course-list/other-course-list";

@Component({
  selector: 'app-home',
  imports: [NavBar, Profile, MyCourseList, OtherCourseList],
  templateUrl: './home.html',
  styleUrl: './home.css'
})
export class Home {
  authFetch = inject(AuthFetch);
  refreshSignal = signal(0);
  uniqueName = this.authFetch.getUniqueName();
}
