import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Album } from '../../../models/album';
import { ImageService } from '../../../services/image.service';
import { Image } from '../../../models/image';
import { RouterModule } from '@angular/router';
import { EmptyCollectionResponse } from '../../../models/empty-collection.response';
import { faTrashCan } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { AuthService } from '../../services/auth.service';
import { AlbumService } from '../../../services/album.service';

@Component({
  selector: 'app-album',
  standalone: true,
  imports: [RouterModule, FontAwesomeModule],
  templateUrl: './album.component.html',
  styleUrl: './album.component.scss'
})
export class AlbumComponent implements OnInit {
  itrash = faTrashCan;

  baseUrl = environment.apiUrl;

  @Output() albumDeleted = new EventEmitter();
  @Input() item!: Album;
  @Input() index!: number;
  image?: Image

  imageService = inject(ImageService);
  albumService = inject(AlbumService);
  authService = inject(AuthService);

  ngOnInit(): void {
    this.fetch();
  }

  fetch() {
    this.imageService.getFirstImage(this.item.id).subscribe({
      next: (response) => {
        if (this.isEmptyCollection(response)) {
          return;
        } else {
          this.image = response;
        }
      }
    });
  }

  isEmptyCollection(response: Image | EmptyCollectionResponse): response is EmptyCollectionResponse {
    return (response as EmptyCollectionResponse).isEmpty !== undefined;
  }

  canDelete(): boolean {
    return this.authService.isAdmin() || this.item.userId === this.authService.getUserId();
  }

  onDelete($event: MouseEvent) {
    $event.stopPropagation();
    this.albumService.deleteAlbum(this.item.id).subscribe({
      next: () => {
        this.albumDeleted.emit();
      }
    });
  }
}
