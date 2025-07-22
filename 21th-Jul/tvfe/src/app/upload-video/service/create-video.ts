import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class CreateVideo {
  apiUrl = environment.apiUrl;

  async createVideo(videoData: FormData, title: string, description: string): Promise<any> {
    try {
      const response = await fetch(`${this.apiUrl}/videos?Title=${title}&Description=${description}`, {
        method: 'POST',
        body: videoData,
      });

      if (!response.ok) {
        const errorText = await response.text();
        return { error: errorText || `HTTP error! status: ${response.status}` };
      }

      return await response.json();
    } catch (error) {
      console.error('Network error:', error);
      return { error: 'Network error: Failed to connect to server' };
    }
  }
}
