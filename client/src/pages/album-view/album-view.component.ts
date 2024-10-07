import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Image } from '../../models/image';
import { ImageService } from '../../services/image.service';
import { AddImageRequest } from '../../models/add-image.request';
import { environment } from '../../environments/environment';
import { CONFIGURATION } from '../../core/configuration/config';
import { Helpers } from '../../core/services/helpers';
import { AuthService } from '../../core/services/auth.service';
import { Album } from '../../models/album';
import { AlbumService } from '../../services/album.service';
import { faAngleLeft, faAngleRight, faThumbsDown, faThumbsUp, faTrashCan } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-album-view',
  standalone: true,
  imports: [FontAwesomeModule, CommonModule],
  templateUrl: './album-view.component.html',
  styleUrl: './album-view.component.scss'
})
export class AlbumViewComponent implements OnInit {
  ilike = faThumbsUp;
  idislike = faThumbsDown;
  itrash = faTrashCan;
  iangleLeft = faAngleLeft;
  iangleRight = faAngleRight;
  
  images: Image[] = [];
  album?: Album;
  baseUrl = environment.apiUrl;

  currentPage: number = 1;
  totalRecords: number = 0;

  selectedFile: File | null = null;
  selectedFileUrl?: string | ArrayBuffer | null;

  selectedImage: Image | null = null;

  imageService = inject(ImageService);
  authService = inject(AuthService);
  albumService = inject(AlbumService);
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

  canUpload() {
    return this.authService.getUserId() === this.album?.userId;
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


  closeFullImage() {
    this.selectedImage = null;
  }

  openFullImage(item: Image) {
    this.selectedImage = item;
  }

  nextImage() {
    if (!this.selectedImage) {
      return;
    }

    const currentIndex = this.images.indexOf(this.selectedImage);
    const nextIndex = (currentIndex + 1) % this.images.length;
    this.selectedImage = this.images[nextIndex];
  }

  previousImage() {
    if (!this.selectedImage) {
      return;
    }

    const currentIndex = this.images.indexOf(this.selectedImage);
    const prevIndex = (currentIndex - 1 + this.images.length) % this.images.length;
    this.selectedImage = this.images[prevIndex];
  }

  onPageChange(page: number) {
    this.currentPage = page;
    this.fetch();
  }

  totalPages(): number {
    return Helpers.totalPages(this.totalRecords, CONFIGURATION.album.pageSize);
  }

  fetch() {
    this.albumService.getAlbum(this.albumId, this.currentPage).subscribe({
      next: (response) => {
        this.album = response.album;
        this.images = response.images.list;
        this.totalRecords = response.images.totalRecords;
      }
    });
  }

  canRate(): boolean {
    return this.authService.isAuthenticated();
  }

  reloadReactions(id: number) {
    this.imageService.getReactions(id).subscribe({
      next: (response) => {
        const image = this.images.find(img => img.id === id);
        if (image) {
          image.likeCount = response.likes;
          image.dislikeCount = response.dislikes;
        }
      }
    });
  }

  onLike(image: Image, $event: MouseEvent) {
    $event.stopPropagation();

    if (this.canRate()) {
      this.imageService.like(image.id).subscribe({
        next: () => {
          this.reloadReactions(image.id);
        }
      });
    }
  }

  onDislike(image: Image, $event: MouseEvent) {
    $event.stopPropagation();

    if (this.canRate()) {
      this.imageService.dislike(image.id).subscribe({
        next: () => {
          this.reloadReactions(image.id);
        }
      });
    }
  }

  canDelete(): boolean {
    return this.authService.isAdmin() || this.album?.userId === this.authService.getUserId();
  }

  onDelete(item: Image, $event: MouseEvent) {
    $event.stopPropagation();

    this.imageService.deleteImage(item.id).subscribe({
      next: () => {
        this.fetch();
      }
    });
  }
}
