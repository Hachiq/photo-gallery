import { Component, inject, OnInit } from '@angular/core';
import { AlbumService } from '../../services/album.service';
import { Album } from '../../models/album';
import { AlbumComponent } from './album/album.component';
import { CONFIGURATION } from '../../core/configuration/config';
import { Helpers } from '../../services/helpers';

@Component({
  selector: 'app-albums-table',
  standalone: true,
  imports: [AlbumComponent],
  templateUrl: './albums-table.component.html',
  styleUrl: './albums-table.component.scss'
})
export class AlbumsTableComponent implements OnInit {
  albums: Album[] = [];

  currentPage: number = 1;
  totalRecords: number = 0;

  albumService = inject(AlbumService);

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

  fetch() {
    this.albumService.getAlbums(this.currentPage).subscribe({
      next: (response) => {
        this.albums = response.list;
        this.totalRecords = response.totalRecords;
      }
    })
  }
}
