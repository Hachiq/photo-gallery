import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { CONFIGURATION } from '../core/configuration/config';
import { CreateAlbumRequest } from '../models/create-album.request';
import { Album } from '../models/album';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AlbumService {
  baseUrl = `${environment.apiUrl}/${CONFIGURATION.album.url}`

  http = inject(HttpClient);

  getAlbums(userId?: number) : Observable<Album[]> {
    if (userId === undefined) {
      return this.http.get<Album[]>(`${this.baseUrl}`);
    }
    return this.http.get<Album[]>(`${this.baseUrl}?userId=${userId}`);
  }

  createAlbum(request: CreateAlbumRequest) {
    return this.http.post(`${this.baseUrl}/create`, request);
  }
}
