import { Routes } from '@angular/router';
import { HomePage } from './home-page/home-page';
import { AboutPage } from './about-page/about-page';
import { Login } from './login/login';
import { AuthGuard } from './auth-gaurd';
export const routes: Routes = [
    {
        path: 'login',
        component: Login
    },
    {
        path: '',
        redirectTo: 'products',
        pathMatch: 'full'
    },
    {
        path: 'products',
        component: HomePage,
        canActivate: [AuthGuard]
    },
    {
        path: 'about',
        component: AboutPage
    },
    {
        path:'products/:id',
        loadComponent: () => import('./details-page/details-page').then(a => a.DetailsPage),
        canActivate: [AuthGuard]
    }
];
