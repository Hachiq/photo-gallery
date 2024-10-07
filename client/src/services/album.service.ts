import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { CONFIGURATION } from '../core/configuration/config';
import { CreateAlbumRequest } from '../models/create-album.request';
import { Album } from '../models/album';
import { Observable } from 'rxjs';
import { Paged } from '../models/paged.response';
import { AlbumViewResponse } from '../models/album-view.response';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  baseUrl = `${environment.apiUrl}/${CONFIGURATION.album.url}`

  http = inject(HttpClient);

  getAlbums(page: number, userId?: number) : Observable<Paged<Album>> {
    if (userId === undefined) {
      return this.http.get<Paged<Album>>(`${this.baseUrl}?page=${page}`);
    }
    return this.http.get<Paged<Album>>(`${this.baseUrl}?page=${page}&userId=${userId}`);
  }

  getAlbum(id: number, page: number) : Observable<AlbumViewResponse> {
    return this.http.get<AlbumViewResponse>(`${this.baseUrl}/${id}?page=${page}`);
  }

  createAlbum(request: CreateAlbumRequest) {
    return this.http.post(`${this.baseUrl}/create`, request);
  }
}
