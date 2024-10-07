import { Component, inject, OnInit } from '@angular/core';
import { AlbumService } from '../../services/album.service';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateAlbumRequest } from '../../models/create-album.request';
import { Album } from '../../models/album';
import { AlbumComponent } from '../../core/components/album/album.component';
import { CONFIGURATION } from '../../core/configuration/config';
import { Helpers } from '../../core/services/helpers';
import { AuthService } from '../../core/services/auth.service';

@Component({
  selector: 'app-my-albums',
  standalone: true,
  imports: [ReactiveFormsModule, AlbumComponent],
  templateUrl: './my-albums.component.html',
  styleUrl: './my-albums.component.scss'
})
export class MyAlbumsComponent implements OnInit {
  albums: Album[] = [];

  currentPage: number = 1;
  totalRecords: number = 0;

  title = new FormControl('', [Validators.required, Validators.maxLength(50)])

  albumService = inject(AlbumService);
  authService = inject(AuthService);
  router = inject(Router);
  route = inject(ActivatedRoute);

  userId!: number;

  constructor() {
    this.route.params.subscribe(params => {
      this.userId = +params['id'];
    });
  }

  ngOnInit(): void {
    this.fetch();
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.fetch();
  }

  totalPages(): number {
    return Helpers.totalPages(this.totalRecords, CONFIGURATION.album.pageSize);
  }

  canCreate(): boolean {
    return this.authService.getUserId() === this.userId;
  }

  fetch() {
    this.albumService.getAlbums(this.currentPage, this.userId).subscribe({
      next: (response) => {
        this.albums = response.list;
        this.totalRecords = response.totalRecords;
      },
      error: (error) => {
        if (error.status === 400) {
          this.router.navigate(['']);
        }
      }
    });
  }

  createAlbum() {
    const data: CreateAlbumRequest = {
      title: this.title.value,
      userId: this.userId
    }

    this.albumService.createAlbum(data).subscribe({
      next: (response) => {
        this.router.navigate(['album', response])
      }
    });
  }
}
