import { Image } from "./image";

export class Album {
  id!: number;
  title!: string;
  createdAt!: Date;
  userId!: number;
  images!: Image[];
}