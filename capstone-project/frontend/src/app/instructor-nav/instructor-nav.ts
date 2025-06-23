import { Component, input } from '@angular/core';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-instructor-nav',
  imports: [RouterLink],
  templateUrl: './instructor-nav.html',
  styleUrl: './instructor-nav.css'
})
export class InstructorNav {
  activeLink = input.required();
}
