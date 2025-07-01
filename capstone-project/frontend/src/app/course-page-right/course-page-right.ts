import { Component, input, WritableSignal } from '@angular/core';
import { Course as CourseModel } from '../model/course.model';

@Component({
  selector: 'app-course-page-right',
  imports: [],
  templateUrl: './course-page-right.html',
  styleUrl: './course-page-right.css'
})
export class CoursePageRight {
  courseDetails = input.required<CourseModel | null>();
  curMaterialId = input.required<WritableSignal<number | null>>();
  updateCurMaterialId(materialId: number) {
    this.curMaterialId().set(materialId);
  }
}
