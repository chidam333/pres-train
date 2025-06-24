import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Course } from '../../Services/course';

@Component({
  selector: 'app-course-body',
  imports: [RouterLink],
  templateUrl: './course-body.html',
  styleUrl: './course-body.css'
})
export class CourseBody implements OnInit {
  courseService = inject(Course);
  courseList:WritableSignal<{
    thumbnail:string,
    title:string,
    description:string
    id:number
  }[]> = signal([]);
  
  private allCourses: {
    thumbnail:string,
    title:string,
    description:string
    id:number
  }[] = [];
  
  async ngOnInit() {
    const data = await this.courseService.getCourses();
    if(data["error"]){
      alert(data["error"])
    }
    if (data) {
      console.log({data})
      this.allCourses = data;
      this.courseList.set(data);
    }
  }
  filterSearch($event:Event){
    const target = $event.target as HTMLInputElement;
    const searchTerm = target.value.toLowerCase().trim();
    
    if (searchTerm === '') {
      this.courseList.set(this.allCourses);
    } else {
      const filteredCourses = this.allCourses.filter(course => 
        course.title.toLowerCase().includes(searchTerm) ||
        course.description.toLowerCase().includes(searchTerm)
      );
      this.courseList.set(filteredCourses);
    }
  }
}
