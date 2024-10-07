import { Component, inject, Input, OnInit } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Album } from '../../../models/album';
import { ImageService } from '../../../services/image.service';
import { Image } from '../../../models/image';
import { RouterModule } from '@angular/router';
import { EmptyCollectionResponse } from '../../../models/empty-collection.response';
import { faTrashCan } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';

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
    return true;
  }

  onDelete($event: MouseEvent) {
    $event.stopPropagation();
    console.log('deleted')
  }
}
