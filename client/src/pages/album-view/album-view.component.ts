import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Image } from '../../models/image';
import { ImageService } from '../../services/image.service';

@Component({
  selector: 'app-album-view',
  standalone: true,
  imports: [],
  templateUrl: './album-view.component.html',
  styleUrl: './album-view.component.scss'
})
export class AlbumViewComponent implements OnInit {
  images: Image[] = [];

  imageService = inject(ImageService);
  route = inject(ActivatedRoute);

  albumId!: number;

  constructor() {
    this.route.params.subscribe(params => {
      this.albumId = +params['id'];
    });
  }

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.imageService.getImages(this.albumId).subscribe({
      next: (response) => {
        this.images = response;
      }
    });
  }
}
