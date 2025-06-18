import { Component, inject, OnInit, signal, WritableSignal } from '@angular/core';
import { UserService } from '../user-service';
import { UserModel } from '../../model/user.model';

@Component({
  selector: 'app-filter',
  imports: [],
  templateUrl: './filter.html',
  styleUrl: './filter.css'
})
export class Filter implements OnInit {
  userService = inject(UserService);
  users: WritableSignal<UserModel[]> = signal([]);
  non_editableUsers: WritableSignal<UserModel[]> = signal([]);
  ngOnInit(): void {
    this.userService.getAllUsers().then(users => {
      this.users.set(users);
      this.non_editableUsers.set(users);
    });
  }
  handleInput(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value.toLowerCase();
    this.users.set(this.non_editableUsers().filter(user => user.firstName.toLowerCase().includes(filterValue) || user.lastName.toLowerCase().includes(filterValue)));
  }
}
