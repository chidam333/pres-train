import { Routes } from '@angular/router';
import { Auth } from './auth/auth';
import { Home } from './home/home';
export const routes: Routes = [
    { path: 'auth', component: Auth },
    { path: '', component: Home }
];
