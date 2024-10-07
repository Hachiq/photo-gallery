import { Routes } from '@angular/router';
import { RegisterComponent } from '../core/components/register/register.component';
import { LoginComponent } from '../core/components/login/login.component';
import { AlbumViewComponent } from '../pages/album-view/album-view.component';
import { MyAlbumsComponent } from '../pages/my-albums/my-albums.component';
import { AlbumsTableComponent } from '../pages/albums-table/albums-table.component';
import { AuthGuard, UnauthorizedGuard } from '../core/guards/auth.guard';
import { UserGuard } from '../core/guards/user.guard';

export const routes: Routes = [
  { path: 'register', component: RegisterComponent, canActivate: [UnauthorizedGuard] },
  { path: 'login', component: LoginComponent, canActivate: [UnauthorizedGuard] },
  { path: 'album/:id', component: AlbumViewComponent },
  { 
    path: 'albums',
    children: [
      { path: 'all', component: AlbumsTableComponent },
      { path: ':id', component: MyAlbumsComponent, canActivate: [AuthGuard, UserGuard] },
      { path: '', component: AlbumsTableComponent }
    ]
  },
  { path: '', component: AlbumsTableComponent },
];
