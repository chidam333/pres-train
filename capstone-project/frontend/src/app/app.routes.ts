import { Routes } from '@angular/router';
import { Auth } from './auth/auth';
import { Home } from './home/home';
import { studentGuard } from './guard/student-guard';
import { InstructorCourse } from './instructor-course/instructor-course';
import { instructorGuard } from './guard/instructor-guard';
import { InstructorStats } from './instructor-stats/instructor-stats';
import { InstructorHelp } from './instructor-help/instructor-help';
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
  }
];
