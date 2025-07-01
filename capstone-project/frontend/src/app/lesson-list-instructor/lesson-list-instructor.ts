import { Component, effect, inject, input, OnInit, signal } from '@angular/core';
import { MaterialListInstructor } from "../material-list-instructor/material-list-instructor";
import { Lesson } from '../Services/lesson';

@Component({
  selector: 'app-lesson-list-instructor',
  imports: [MaterialListInstructor],
  templateUrl: './lesson-list-instructor.html',
  styleUrl: './lesson-list-instructor.css',
})
export class LessonListInstructor {
  lessons = input.required<any[]>();
  refreshTrigger = signal<number>(0);
  lessonService = inject(Lesson);
  constructor() {

  }

  async updateLesson(lessonId: number, title:string, description:string, $event: Event, sequenceNo: number, courseId: number) {
    $event.preventDefault();
    $event.stopPropagation();
    console.log('Updating lesson with ID:', lessonId, 'Title:', title, 'Description:', description, 'Sequence No:', sequenceNo, 'Course ID:', courseId);
    const response = await this.lessonService.updateLesson(lessonId, title, description, courseId, sequenceNo);
    if ('error' in response) {
      alert('Failed to update lesson: ' + response.error);
    } else {
      console.log('Lesson updated successfully:', response);
      // this.refreshTrigger.set(this.refreshTrigger() + 1);
    }
  }
}
