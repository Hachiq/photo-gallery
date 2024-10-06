import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Image } from '../../models/image';
import { ImageService } from '../../services/image.service';
import { AddImageRequest } from '../../models/add-image.request';
import { environment } from '../../environments/environment';
import { CONFIGURATION } from '../../core/configuration/config';
import { Helpers } from '../../core/services/helpers';

@Component({
  selector: 'app-album-view',
  standalone: true,
  imports: [],
  templateUrl: './album-view.component.html',
  styleUrl: './album-view.component.scss'
})
export class AlbumViewComponent implements OnInit {
  images: Image[] = [];
  baseUrl = environment.apiUrl;

  currentPage: number = 1;
  totalRecords: number = 0;

  selectedFile: File | null = null;
  selectedFileUrl?: string | ArrayBuffer | null;

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

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files[0]) {
      this.selectedFile = input.files[0];

      const reader = new FileReader();
      reader.onload = (e) => {
        this.selectedFileUrl = e.target?.result;
      };
      reader.readAsDataURL(this.selectedFile);
    }
  }

  clearSelectedFile() {
    this.selectedFile = null;
    this.selectedFileUrl = null;
  }

  save() {
    if (this.selectedFile === null) {
      return;
    }

    const data: AddImageRequest = {
      file: this.selectedFile,
      albumId: this.albumId
    }

    this.imageService.addImage(data).subscribe({
      next: () => {
        this.currentPage = 1;
        this.clearSelectedFile();
        this.fetch();
      }
    });
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.fetch();
  }

  totalPages(): number {
    return Helpers.totalPages(this.totalRecords, CONFIGURATION.album.pageSize);
  }

  fetch() {
    this.imageService.getImages(this.albumId, this.currentPage).subscribe({
      next: (response) => {
        this.images = response.list;
        this.totalRecords = response.totalRecords;
      }
    });
  }
}
