import { Component } from '@angular/core';

@Component({
  selector: 'app-user-details',
  imports: [],
  templateUrl: './user-details.html',
  styleUrl: './user-details.css'
})
export class UserDetails {
  users =[{
    name:"ram",likes:0,dislikes:0
  },{
    name:"shyam",likes:0,dislikes:0
  },{
    name:"ganesh",likes:0,dislikes:0
  },{
    name:"sita",likes:0,dislikes:0
  }]
  likeUser(user: any) {
    user.likes++;
  }
  dislikeUser(user: any) {
    user.dislikes++;
  }
}
