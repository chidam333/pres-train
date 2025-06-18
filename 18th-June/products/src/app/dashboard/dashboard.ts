import { Component, inject, OnChanges, OnInit, signal } from '@angular/core';
import { UserService } from '../user-service';
import { UserModel } from '../../model/user.model';
import { PieGender } from '../pie-gender/pie-gender';
import { RoleBar } from '../role-bar/role-bar';
import { StateViz } from '../state-viz/state-viz';

@Component({
  selector: 'app-dashboard',
  imports: [PieGender, RoleBar, StateViz],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private userService = inject(UserService);
  male = signal(0);
  female = signal(0);
  roles = signal<Map<string, number>>(new Map());
  users = signal<UserModel[]>([]);
  state = signal<Map<string, number>>(new Map());
  async ngOnInit() {
    this.users.set(await this.userService.getAllUsers());
    this.genderData();
    this.rolesData();
    this.stateData();
  }
  genderData() {
    this.male.set(0);
    this.female.set(0);
    this.users().map((user) =>
      user.gender == 'male'
        ? this.male.set(this.male() + 1)
        : this.female.set(this.female() + 1)
    );
  }
  rolesData() {
    this.roles.set(new Map());
    this.users().map((user) => {
      const role = user.role;
      if (this.roles().has(role)) {
        this.roles.set(new Map(this.roles()).set(role, this.roles().get(role)! + 1));
      } else {
        this.roles.set(new Map(this.roles()).set(role, 1));
      }
    });
  }
  stateData() {
    this.state.set(new Map());
    this.users().map((user) => {
      const state = user.address.state;
      if (this.state().has(state)) {
        this.state.set(new Map(this.state()).set(state, this.state().get(state)! + 1));
      } else {
        this.state.set(new Map(this.state()).set(state, 1));
      }
    });
    console.log('State data:', this.state());
  }
}