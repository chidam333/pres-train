import { Component, effect, inject, input, OnInit, signal, WritableSignal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from '../../Services/course';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-landing',
  imports: [FormsModule],
  templateUrl: './landing.html',
  styleUrl: './landing.css'
})
export class Landing implements OnInit {
  private _title = signal<string>('');
  private _description = signal<string>('');
  private _thumbnail = signal<string>('');
  
  get title(): string {
    return this._title();
  }
  
  set title(value: string) {
    this._title.set(value);
  }
  
  get description(): string {
    return this._description();
  }
  
  set description(value: string) {
    this._description.set(value);
  }
  
  get thumbnail(): string {
    return this._thumbnail();
  }
  
  set thumbnail(value: string) {
    this._thumbnail.set(value);
  }

  courseService = inject(Course);
  router = inject(Router);
  courseId = input.required<number>();

  async ngOnInit() {
    const course = await this.courseService.getCourseById(this.courseId());
    if (course.error) {
      alert(course.error);
      this.router.navigate(['instructor/course']);
      return;
    }
    if (course) {
      this._title.set(course.title || '');
      this._description.set(course.description || '');
      this._thumbnail.set(course.thumbnail || '');
      console.log({ course });
    }
  }
  async updateCourse() {
    const response = await this.courseService.updateCourse(
      this.courseId(),
      this.title,
      this.description,
      this.thumbnail
    );
    if (response.error) {
      alert(response.error);
    } else {
      console.log('Course updated successfully:', response);
      alert('Course updated successfully');
    }
  }
}
