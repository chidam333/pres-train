import { Routes } from '@angular/router';
import { ListVideos } from './list-videos/list-videos';
import { UploadVideo } from './upload-video/upload-video';

export const routes: Routes = [
    {
        path: '',
        component: ListVideos
    },
    {
        path:"upload",
        component: UploadVideo
    }
];
