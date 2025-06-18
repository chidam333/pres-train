import { Component, Input, OnChanges, OnInit } from '@angular/core';

@Component({
  selector: 'app-role-bar',
  imports: [],
  templateUrl: './role-bar.html',
  styleUrl: './role-bar.css'
})
export class RoleBar{
  @Input() roles: Map<string, number> = new Map();
}
