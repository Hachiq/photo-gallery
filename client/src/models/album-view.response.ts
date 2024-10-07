import { Album } from "./album";
import { Image } from "./image";
import { Paged } from "./paged.response";

export class AlbumViewResponse {
  album!: Album;
  images!: Paged<Image>;
}