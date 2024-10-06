import { inject, Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { CONFIGURATION } from '../core/configuration/config';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Image } from '../models/image';

@Injectable({
  providedIn: 'root'
})
export class ImageService {
  baseUrl = `${environment.apiUrl}/${CONFIGURATION.image.url}`

  http = inject(HttpClient);

  getImages(albumId: number) : Observable<Image[]> {
    return this.http.get<Image[]>(`${this.baseUrl}?albumId=${albumId}`);
  }
}
