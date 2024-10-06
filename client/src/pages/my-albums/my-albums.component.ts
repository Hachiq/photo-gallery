import { Component, inject, OnInit } from '@angular/core';
import { AlbumService } from '../../services/album.service';
import { FormControl, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { CreateAlbumRequest } from '../../models/create-album.request';
import { Album } from '../../models/album';
import { AlbumComponent } from '../albums-table/album/album.component';

@Component({
  selector: 'app-my-albums',
  standalone: true,
  imports: [ReactiveFormsModule, AlbumComponent],
  templateUrl: './my-albums.component.html',
  styleUrl: './my-albums.component.scss'
})
export class MyAlbumsComponent implements OnInit {
  albums: Album[] = [];

  title = new FormControl('', [Validators.required, Validators.maxLength(50)])

  albumService = inject(AlbumService);
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

  fetch() {
    this.albumService.getAlbums(this.userId).subscribe({
      next: (response) => {
        this.albums = response;
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
