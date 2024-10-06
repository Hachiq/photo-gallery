import { Component, inject, Input, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Album } from '../../../models/album';
import { ImageService } from '../../../services/image.service';
import { Image } from '../../../models/image';

@Component({
  selector: 'app-album',
  standalone: true,
  imports: [],
  templateUrl: './album.component.html',
  styleUrl: './album.component.scss'
})
export class AlbumComponent implements OnInit {
  baseUrl = environment.apiUrl;

  @Input() item!: Album;
  @Input() index!: number;
  image?: Image

  imageService = inject(ImageService);

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.imageService.getFirstImage(this.item.id).subscribe({
      next: (response) => {
        this.image = response;
      }
    });
  }
}
