import { Component, signal, WritableSignal } from '@angular/core';
import { Landing } from "./landing/landing";
import { Content } from "./content/content";

@Component({
  selector: 'app-instructor-manage',
  imports: [Landing, Content],
  templateUrl: './instructor-manage.html',
  styleUrl: './instructor-manage.css'
})
export class InstructorManage {
  activeTab: WritableSignal<number> = signal(0);
}
