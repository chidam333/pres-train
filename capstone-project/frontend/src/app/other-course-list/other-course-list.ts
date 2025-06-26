import { Component, effect, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { Course } from '../Services/course';
import { Enrollment } from '../Services/enrollment';

@Component({
  selector: 'app-other-course-list',
  imports: [],
  templateUrl: './other-course-list.html',
  styleUrl: './other-course-list.css'
})
export class OtherCourseList implements OnInit {
  refreshSignal = input.required<WritableSignal<number>>();
  courseService = inject(Course)
  enrollmentService = inject(Enrollment);
  courses = signal<any[]>([]);

  async ngOnInit() {
    this.fetchCourses();
  }

  constructor(){
    effect(()=>{
      this.refreshSignal()();
      this.fetchCourses();
    })
  }

  async fetchCourses(){
    const response = await this.courseService.getOtherCourses();
    if('error' in response) {
      console.error(response.error);
    }
    else{
      this.courses.set(response);
    }
  }

  async enrollCourse(courseId: number) {
    const response = await this.enrollmentService.enrollCourse(courseId);
    if('error' in response) {
      console.error(response.error);
    }
    else {
      console.log('Enrollment successful:', response);
      this.refreshSignal().update((val) => val + 1);
    }
  }
}
