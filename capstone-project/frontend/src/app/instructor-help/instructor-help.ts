import { Component } from '@angular/core';
import { InstructorNav } from "../instructor-nav/instructor-nav";
import { InstructorTopBar } from "../instructor-top-bar/instructor-top-bar";

@Component({
  selector: 'app-instructor-help',
  imports: [InstructorNav, InstructorTopBar],
  templateUrl: './instructor-help.html',
  styleUrl: './instructor-help.css'
})
export class InstructorHelp {

}
