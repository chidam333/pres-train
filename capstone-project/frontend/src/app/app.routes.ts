import { Routes } from '@angular/router';
import { Auth } from './auth/auth';
import { Home } from './home/home';
import { studentGuard } from './guard/student-guard';
import { InstructorCourse } from './instructor-course/instructor-course';
import { instructorGuard } from './guard/instructor-guard';
import { InstructorStats } from './instructor-stats/instructor-stats';
import { InstructorHelp } from './instructor-help/instructor-help';
import { CreateCourse } from './create-course/create-course';
import { InstructorManage } from './instructor-manage/instructor-manage';
import { CoursePage } from './course-page/course-page';

export const routes: Routes = [
  { path: 'auth', component: Auth },
  { path: '', component: Home, canActivate: [studentGuard] },
  {
    path: 'instructor/course',
    component: InstructorCourse,
    canActivate: [instructorGuard],
  },
  {
    path: 'instructor/stats',
    component: InstructorStats,
    canActivate: [instructorGuard],
  },
  {
    path: 'instructor/help',
    component: InstructorHelp,
    canActivate: [instructorGuard],
  },
  {
    path: 'create-course',
    component: CreateCourse,
    canActivate: [instructorGuard],
  },
  {
    path: 'instructor/manage/:id',
    component: InstructorManage,
    canActivate: [instructorGuard],
  },
  {
    path: 'course/:id',
    component: CoursePage,
    canActivate: [studentGuard],
  },
];
