import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserModel } from '../model/user.model';
import { firstValueFrom } from 'rxjs/internal/firstValueFrom';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private http = inject(HttpClient);
  async getAllUsers(){
    let users: UserModel[] = [];
    let userFound = 30;
    while(userFound==30){
      const data = await firstValueFrom(this.http.get<any>(`https://dummyjson.com/users?skip=${users.length}&limit=30`));
      users.push(...data.users);
      userFound = data.users.length;
    }
    return users;
  }
  async addUser(firstname:string, lastname:string, age:number){
    const newUser = { firstName: firstname, lastName: lastname, age: age };
    const data = await firstValueFrom(this.http.post('https://dummyjson.com/users', newUser));
    console.log('User added:', data);
  }
}