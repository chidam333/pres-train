import { Component } from '@angular/core';
import { CitySearch } from '../city-search/city-search';

@Component({
  selector: 'app-weather-dashboard',
  imports: [CitySearch],
  templateUrl: './weather-dashboard.html',
  styleUrl: './weather-dashboard.css'
})
export class WeatherDashboard {

}
