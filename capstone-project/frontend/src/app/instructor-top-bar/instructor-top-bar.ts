import { Component, input } from '@angular/core';
import { Profile } from "../profile/profile";

@Component({
  selector: 'app-instructor-top-bar',
  imports: [Profile],
  templateUrl: './instructor-top-bar.html',
  styleUrl: './instructor-top-bar.css'
})
export class InstructorTopBar {
  title = input.required<string>();
}
