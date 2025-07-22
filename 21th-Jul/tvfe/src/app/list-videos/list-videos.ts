import { Component, inject, OnInit, signal, WritableSignal, HostListener } from '@angular/core';
import { GetVideos } from './service/get-videos';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-videos',
  imports: [RouterLink, CommonModule],
  templateUrl: './list-videos.html',
  styleUrl: './list-videos.css'
})
export class ListVideos implements OnInit {
  getVideosService = inject(GetVideos);
  videos: WritableSignal<any[]> = signal([]);
  selectedVideo: WritableSignal<any | null> = signal(null);
  isDialogOpen: WritableSignal<boolean> = signal(false);
  
  ngOnInit() {
    this.getVideosService.getVideos().then(videos => {
      if('error' in videos) {
        console.error('Error fetching videos:', videos.error);
        return;
      }
      console.log(videos);
      this.videos.set(videos);
    });
  }

  @HostListener('document:keydown.escape')
  onEscapeKey() {
    if (this.isDialogOpen()) {
      this.closeVideoDialog();
    }
  }

  openVideoDialog(video: any) {
    this.selectedVideo.set(video);
    this.isDialogOpen.set(true);
    document.body.style.overflow = 'hidden'; // Prevent background scrolling
  }

  closeVideoDialog() {
    this.selectedVideo.set(null);
    this.isDialogOpen.set(false);
    document.body.style.overflow = 'auto'; // Restore scrolling
  }

  onDialogBackdropClick(event: Event) {
    if (event.target === event.currentTarget) {
      this.closeVideoDialog();
    }
  }
}
