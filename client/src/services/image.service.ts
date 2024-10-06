import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { CONFIGURATION } from '../core/configuration/config';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Image } from '../models/image';
import { AddImageRequest } from '../models/add-image.request';
import { Paged } from '../models/paged.response';
import { EmptyCollectionResponse } from '../models/empty-collection.response';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  baseUrl = `${environment.apiUrl}/${CONFIGURATION.image.url}`

  http = inject(HttpClient);

  getImages(albumId: number, page: number) : Observable<Paged<Image>> {
    return this.http.get<Paged<Image>>(`${this.baseUrl}?albumId=${albumId}&page=${page}`);
  }

  addImage(request: AddImageRequest) {
    const formData = new FormData();
    formData.append('File', request.file);
    formData.append('AlbumId', request.albumId.toString());

    return this.http.post(`${this.baseUrl}/add`, formData);
  }

  getFirstImage(albumId: number) : Observable<Image | EmptyCollectionResponse> {
    return this.http.get<Image | EmptyCollectionResponse>(`${this.baseUrl}/first?albumId=${albumId}`);
  }
}
