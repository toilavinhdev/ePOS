import { Injectable } from '@angular/core';
import { map, Observable, of } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { BaseService } from '@app-shared/core/abtractions';
import { IAPIResponse } from '@app-shared/core/models/common.models';

@Injectable({
  providedIn: 'root',
})
export class StorageService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('storage');
  }

  upload(file: File): Observable<string> {
    let formData = new FormData();
    formData.append('file', file);
    return this.httpClient
      .post<IAPIResponse<string>>(this.getApiUrl('upload'), formData)
      .pipe(map((response) => response.data));
  }

  uploadMultiple(files: File[]): Observable<string[]> {
    let formData = new FormData();
    files.forEach((file, idx) => {
      formData.append('files', file);
    });
    return this.httpClient
      .post<IAPIResponse<string[]>>(this.getApiUrl('upload-multiple'), formData)
      .pipe(map((response) => response.data));
  }
}
