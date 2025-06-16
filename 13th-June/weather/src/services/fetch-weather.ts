import { Injectable, signal, effect } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class WeatherService {
  latitude = signal<number | null>(null);
  longitude = signal<number | null>(null);
  city = signal<string>('');
  weather = signal<any | null>(null);
  error = signal<string>('');

  constructor(private http: HttpClient) {
  }


  fetchLatLon(city: string): void {
    const url = `https://nominatim.openstreetmap.org/search?q=${city}&format=json`;
    this.http.get<any>(url).subscribe({
      next: data => {
        if (data && data.length > 0) {
          const location = data[0];
          this.latitude.set(location.lat);
          this.longitude.set(location.lon);
          this.city.set(location.display_name);
        } else {
          this.error.set('City not found');
        }
      },
      error: err => {
        console.error('Error fetching weather:', err);
        this.error.set('Error fetching data');
      }
    });
  }
  fetchWeather(): void {
    const lat = this.latitude();
    const lon = this.longitude();
    if (lat && lon) {
      const url = `https://api.open-meteo.com/v1/forecast?latitude=${lat}&longitude=${lon}&current_weather=true`;
      this.http.get<any>(url).subscribe({
        next: data => {
          this.weather.set(data.current_weather);
          this.error.set('');
        },
        error: err => {
          console.error('Error fetching weather:', err);
          this.error.set('Error fetching weather data');
        }
      });
    } else {
      this.error.set('Latitude and Longitude are not set');
    }
  }
}