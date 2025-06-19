import { Component, Input } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { UserModel } from '../user-model';
import { AsyncPipe } from '@angular/common';

@Component({
  selector: 'app-users-list-s',
  imports: [AsyncPipe],
  templateUrl: './users-list-s.html',
  styleUrl: './users-list-s.css'
})
export class UsersListS {
  @Input() users: BehaviorSubject<UserModel[]> = new BehaviorSubject<UserModel[]>([]);
}
