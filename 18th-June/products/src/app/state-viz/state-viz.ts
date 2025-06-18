import { Component, Input, Signal, signal } from '@angular/core';

@Component({
  selector: 'app-state-viz',
  imports: [],
  templateUrl: './state-viz.html',
  styleUrl: './state-viz.css'
})
export class StateViz {
  @Input() state: Signal<Map<string, number>> = signal(new Map());
}
