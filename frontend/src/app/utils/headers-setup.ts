import { HttpHeaders } from '@angular/common/http';

export let httpHeaders: HttpHeaders = {} as HttpHeaders;

export function setupHeader(accessToken: string): HttpHeaders {
  console.log(accessToken);
  httpHeaders = new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + accessToken,
  });
  console.log(httpHeaders);
  return httpHeaders;
}
