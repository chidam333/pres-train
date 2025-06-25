import { Component, effect, input, OnInit } from '@angular/core';
import { MaterialListInstructor } from "../material-list-instructor/material-list-instructor";

@Component({
  selector: 'app-lesson-list-instructor',
  imports: [MaterialListInstructor],
  templateUrl: './lesson-list-instructor.html',
  styleUrl: './lesson-list-instructor.css',
})
export class LessonListInstructor {
  lessons = input.required<any[]>();
  constructor() {
    // effect(() => {
    //   console.log('Lessons updated:', this.lessons());
    // });
  }
}
