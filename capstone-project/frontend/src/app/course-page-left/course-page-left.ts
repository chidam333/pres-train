import { Component, effect, inject, input, signal } from '@angular/core';
import { Course as CourseModel } from '../model/course.model';
import { DomSanitizer } from '@angular/platform-browser';
import { Material } from '../Services/material';
import { DatePipe } from '@angular/common';
import { environment } from '../../environments/environment';


@Component({
  selector: 'app-course-page-left',
  imports: [DatePipe],
  templateUrl: './course-page-left.html',
  styleUrl: './course-page-left.css'
})
export class CoursePageLeft {
  courseDetails = input.required<CourseModel | null>();
  curMaterialId = input.required<number | null>();
  sanitizer = inject(DomSanitizer);
  materialService = inject(Material); 
  materialDetail = signal<any>(null);
  apiUrl = environment.apiUrl;
  
  constructor(){
    effect(async () => {
      const curMaterialId = this.curMaterialId();
      if (!curMaterialId) {
        this.materialDetail.set(null);
        return;
      }

      const materialResponse = await this.materialService.getMaterialByMaterialId(curMaterialId);
      if ('error' in materialResponse) {
        console.error(materialResponse.error);
        this.materialDetail.set(null);
        return;
      }
      console.log('Material response:', materialResponse);
      this.materialDetail.set(materialResponse);
    });
  }
  
  trustUrl(url: string) {
    return this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }
}
