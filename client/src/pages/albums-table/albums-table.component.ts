import { Component, inject, OnInit } from '@angular/core';
import { AlbumService } from '../../services/album.service';
import { Album } from '../../models/album';
import { AlbumComponent } from './album/album.component';

@Component({
  selector: 'app-albums-table',
  standalone: true,
  imports: [AlbumComponent],
  templateUrl: './albums-table.component.html',
  styleUrl: './albums-table.component.scss'
})
export class AlbumsTableComponent implements OnInit {
  albums: Album[] = [];

  albumService = inject(AlbumService);

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.albumService.getAlbums().subscribe({
      next: (response) => {
        this.albums = response;
      }
    })
  }
}
