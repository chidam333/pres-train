import { Component, effect, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { Course } from '../Services/course';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-my-course-list',
  imports: [RouterLink],
  templateUrl: './my-course-list.html',
  styleUrl: './my-course-list.css',
})
export class MyCourseList implements OnInit {
  refreshSignal = input.required<WritableSignal<number>>();
  courses = signal<any[]>([]);
  courseService = inject(Course);
  async ngOnInit() {
    await this.fetchCourses();
  }
  constructor(){
    effect(()=>{
      this.refreshSignal()();
      this.fetchCourses();
    })
  }
  async fetchCourses() {
    const response = await this.courseService.getMyCourses();
    if ('error' in response) {
      console.error(response.error);
    } else {
      console.log('My courses fetched successfully:', response);
      this.courses.set(response);
    }
  }
  async unenrollCourse(courseId: number, $event: Event) {
    $event.stopPropagation(); 
    $event.preventDefault()
    try{
    const response = await this.courseService.unenrollCourse(courseId);
    if ('error' in response) {
      console.error(response.error);
    } else {
      console.log('Unenrollment successful:', response);
      this.refreshSignal().update((val) => val + 1);
    }
  }catch(ex){
    console.error('An error occurred while unenrolling:', ex);
  }
  }
}
