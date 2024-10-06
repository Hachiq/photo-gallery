export class Helpers {
  static totalPages(totalRecords: number, pageSize: number): number {
    return Math.ceil(totalRecords / pageSize);
  }

  static decodeJwt(token: string) {
    if (!token) {
      return null;
    }

    const stringSplit = token.split('.');

    const tokenObject: any = {};
    tokenObject.raw = tokenObject;
    tokenObject.header = JSON.parse(window.atob(stringSplit[0]));
    tokenObject.payload = JSON.parse(window.atob(stringSplit[1]));
    return tokenObject;
  }
}