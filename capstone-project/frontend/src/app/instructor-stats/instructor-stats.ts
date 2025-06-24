import { Component } from '@angular/core';
import { InstructorNav } from "../instructor-nav/instructor-nav";
import { InstructorTopBar } from "../instructor-top-bar/instructor-top-bar";

@Component({
  selector: 'app-instructor-stats',
  imports: [InstructorNav, InstructorTopBar],
  templateUrl: './instructor-stats.html',
  styleUrl: './instructor-stats.css'
})
export class InstructorStats {

}
