import { Routes } from '@angular/router';
import { RegisterComponent } from '../core/components/register/register.component';
import { LoginComponent } from '../core/components/login/login.component';
import { AlbumViewComponent } from '../pages/album-view/album-view.component';
import { MyAlbumsComponent } from '../pages/my-albums/my-albums.component';
import { AlbumsTableComponent } from '../pages/albums-table/albums-table.component';

export const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent },
  { path: 'album/:id', component: AlbumViewComponent },
  { path: 'albums', component: AlbumsTableComponent },
  { path: 'albums/:id', component: MyAlbumsComponent },
  { path: '', component: AlbumsTableComponent },
];
