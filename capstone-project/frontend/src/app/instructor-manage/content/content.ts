import {
  Component,
  ViewChild,
  ElementRef,
  AfterViewInit,
  inject,
  input,
  signal,
  OnInit,
  computed,
} from '@angular/core';
import { Lesson } from '../../Services/lesson';
import { FormsModule } from '@angular/forms';
import { max } from 'rxjs';
import { LessonListInstructor } from "../../lesson-list-instructor/lesson-list-instructor";
import { Profile } from "../../profile/profile";

@Component({
  selector: 'app-content',
  imports: [FormsModule, LessonListInstructor],
  templateUrl: './content.html',
  styleUrl: './content.css',
})
export class Content implements OnInit {
  lessonService = inject(Lesson);
  courseId = input.required<number>();
  title: string = '';
  description: string = '';
  lessons = signal<any[]>([]);
  sequenceNo = computed(()=>{
    let maxSequenceNo = 0;
    for(const lesson of this.lessons()){
      if (lesson.sequenceNo > maxSequenceNo) {
        maxSequenceNo = lesson.sequenceNo;
      }
    }
    return maxSequenceNo + 1;
  })

  async ngOnInit() {
    const lessons = await this.lessonService.getLessonsByCourseId(this.courseId());
    this.lessons.set(lessons);
    console.log('Lessons fetched:', this.lessons());
  }

  @ViewChild('dialog') dialog!: ElementRef<HTMLDialogElement>;

  openDialog() {
    if (this.dialog?.nativeElement) {
      this.dialog.nativeElement.showModal();
    } else {
      console.error('Dialog element not found');
    }
  }

  closeDialog() {
    console.log('Closing dialog');
    if (this.dialog?.nativeElement) {
      this.dialog.nativeElement.close();
    } else {
      console.error('Dialog element not found');
    }
  }

  async submitForm() {
    console.log('Submitting form with values:');
    if (this.title === "" || this.description === "") {
      alert('Please fill in all fields correctly.');
      return;
    }
    if(typeof this.sequenceNo() !== 'number' || this.sequenceNo() === null) {
      alert('Failed processing the sequence number.');
      return;
    }
    const result = await this.lessonService.addLesson(
      this.courseId().toString(),
      this.title,
      this.description,
      this.sequenceNo()!
    );

    if (result.error) {
      alert('Error adding lesson: ' + result.error);
    } else {
      alert(`Lesson added successfully: ${result.title}`);
      let lessn = await this.lessonService.getLessonsByCourseId(this.courseId());
      if (lessn.error) {
        alert(lessn.error);
      }
      this.lessons.set(lessn);
      this.closeDialog();
    }
  }
}
