import { Component } from '@angular/core';
import { InstructorNav } from "../instructor-nav/instructor-nav";
import { InstructorTopBar } from "../instructor-top-bar/instructor-top-bar";
import { CourseBody } from "./course-body/course-body";

@Component({
  selector: 'app-instructor-course',
  imports: [InstructorNav, InstructorTopBar, CourseBody],
  templateUrl: './instructor-course.html',
  styleUrl: './instructor-course.css'
})
export class InstructorCourse {

}
