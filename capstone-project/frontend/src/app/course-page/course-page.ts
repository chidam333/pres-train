import {
  Component,
  inject,
  OnInit,
  signal,
  WritableSignal,
} from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Course } from '../Services/course';
import { Course as CourseModel } from '../model/course.model';
import { CoursePageLeft } from '../course-page-left/course-page-left';
import { CoursePageRight } from '../course-page-right/course-page-right';
import { Profile } from '../profile/profile';
import { Material } from '../Services/material';

@Component({
  selector: 'app-course-page',
  imports: [CoursePageLeft, CoursePageRight, RouterLink, Profile],
  templateUrl: './course-page.html',
  styleUrl: './course-page.css',
})
export class CoursePage implements OnInit {
  activatedRoute = inject(ActivatedRoute);
  courseDetails: WritableSignal<CourseModel | null> = signal(null);
  courseId = parseInt(this.activatedRoute.snapshot.paramMap.get('id')!);
  courseService = inject(Course);
  materialService = inject(Material);
  curMaterialId = signal<number | null>(null);
  async ngOnInit() {
    const response = await this.courseService.getCourseDetailsById(
      this.courseId
    );
    if ('error' in response) {
      console.error(response.error);
      alert('Failed to fetch course details. Please check console.');
      return;
    } else {
      const courseLessons = response.lessons['$values'] || [];
      let materials = [];
      if (courseLessons.length > 0) {
        const materialsResponse =
          await this.materialService.getMaterialsByLessonId(
            courseLessons[0].id
          );
        if ('error' in materialsResponse) {
          console.error(materialsResponse.error);
          alert(
            'Failed to fetch materials for the first lesson. Please check console.'
          );
          return;
        } else {
          materials = materialsResponse;
        }
      }
      const courseDetails = new CourseModel(
        response.id,
        response.title,
        response.description,
        response.createdById,
        new Date(response.createdAt),
        response.thumbnail,
        response.lessons,
        response.createdBy
      );
      console.log({courseDetails,response})
      this.courseDetails.set(courseDetails);
      if(materials.length > 0) {
        this.curMaterialId.set(materials[0].id);
      }
    }
  }
}
