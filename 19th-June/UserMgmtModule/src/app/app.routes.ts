import { Routes } from '@angular/router';
import { AddUser } from './add-user/add-user';
import { UserList } from './user-list/user-list';

export const routes: Routes = [
    {
        path: "add-user",
        component: AddUser
    },{
        path:"users",
        component: UserList
    }
];
