import { Component, inject, signal, WritableSignal } from '@angular/core';
import { Landing } from "./landing/landing";
import { Content } from "./content/content";
import { ActivatedRoute, Router } from '@angular/router';
import { Profile } from "../profile/profile";
import { Course } from '../Services/course';

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
  courseService = inject(Course);
  router = inject(Router);
  async deleteCourse() {
    const response = await this.courseService.deleteCourse(this.courseId);
    if ('error' in response) {
      alert('Failed to delete course: ' + response.error);
    } else {
      console.log('Course deleted successfully:', response);
      this.router.navigate(['/instructor/course']);
    }
  }
}
