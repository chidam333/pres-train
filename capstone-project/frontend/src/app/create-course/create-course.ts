import { Component, effect, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Course } from '../Services/course';

@Component({
  selector: 'app-create-course',
  imports: [FormsModule],
  templateUrl: './create-course.html',
  styleUrl: './create-course.css',
})
export class CreateCourse {
  title: string | null = null;
  description: string | null = null;
  thumbnail: string | null = null;
  stepper = signal<number>(0);
  steps = signal<string[]>([
    'Course Title',
    'Course Description',
    'Course Thumbnail',
  ]);
  router = inject(Router);
  courseService = inject(Course);

  constructor() {
    effect(() => {
      console.log('Current Step:', this.stepper());
    });
  }

  exit() {
    console.log('Exiting course creation');
    this.router.navigate(['/instructor/course']);
  }
  async continue(event: Event) {
    event.preventDefault();
    console.log(this.title, this.description, this.thumbnail);

    const currentStep = this.stepper();
    if (currentStep === 0 && !this.title) return;
    if (currentStep === 1 && !this.description) return;
    if (currentStep === 2 && !this.thumbnail) return;

    if (this.stepper() < 3) {
      this.stepper.update((value) => value + 1);
    }
    if (this.stepper() === 3) {
      if (this.title && this.description && this.thumbnail) {
        const result = await this.courseService.createCourse(
          this.title,
          this.description,
          this.thumbnail
        );
        if (result.error) {
          console.error('Error creating course:', result.error);
          alert('Failed to create course: ' + result.error);
          return;
        }
        console.log('Course created successfully:', result);
        alert('Course created successfully!');
        this.router.navigate([`/instructor/manage/${result.id}`]);
      }
    }
  }
}
