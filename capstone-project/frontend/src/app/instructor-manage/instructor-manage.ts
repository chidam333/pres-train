import { Component, inject, signal, WritableSignal } from '@angular/core';
import { Landing } from "./landing/landing";
import { Content } from "./content/content";
import { ActivatedRoute } from '@angular/router';
import { Profile } from "../profile/profile";

@Component({
  selector: 'app-instructor-manage',
  imports: [Landing, Content],
  templateUrl: './instructor-manage.html',
  styleUrl: './instructor-manage.css'
})
export class InstructorManage {
  activeTab: WritableSignal<number> = signal(0);
  activatedRoute = inject(ActivatedRoute);
  courseId = Number(this.activatedRoute.snapshot.params['id']);
}
