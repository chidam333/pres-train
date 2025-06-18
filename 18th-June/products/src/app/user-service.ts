import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserModel } from '../model/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  async getAllUsers(){
    let users: UserModel[] = [];
    let userFound = 30;
    while(userFound==30){
      const data = await this.http.get<any>(`https://dummyjson.com/users?skip=${users.length}&limit=30`).toPromise();
      users.push(...data.users);
      userFound = data.users.length;
    }
    return users;
  }
}