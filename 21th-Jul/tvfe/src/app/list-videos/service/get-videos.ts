import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
@Injectable({
  providedIn: 'root'
})
export class GetVideos {
  constructor() {
    console.log({environment})
  }
  private apiUrl = environment.apiUrl;
  async getVideos(){
    const response = await fetch(`${this.apiUrl}/videos`);
    if (!response.ok) {
      return {error: response.text() || 'Failed to fetch videos'};
    }
    return response.json();
  }
}
