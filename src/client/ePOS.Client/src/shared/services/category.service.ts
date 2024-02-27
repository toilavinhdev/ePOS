import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions';
import { HttpClient } from '@angular/common/http';
import {
  ICategoryViewModel,
  ICreateCategoryRequest,
  IListCategoryRequest,
  IListCategoryResponse,
} from '@app-shared/models/category.models';
import { map, Observable } from 'rxjs';
import { IAPIResponse } from '@app-shared/core/models/common.models';

@Injectable({
  providedIn: 'root',
})
export class CategoryService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('category');
  }

  list(payload: IListCategoryRequest): Observable<IListCategoryResponse> {
    return this.httpClient
      .post<
        IAPIResponse<IListCategoryResponse>
      >(this.getApiUrl('list'), payload)
      .pipe(map((response) => response.data));
  }

  create(payload: ICreateCategoryRequest): Observable<ICategoryViewModel> {
    return this.httpClient
      .post<IAPIResponse<ICategoryViewModel>>(this.getApiUrl('create'), payload)
      .pipe(map((response) => response.data));
  }

  delete(ids: string[]): Observable<any> {
    return this.httpClient.post<IAPIResponse>(this.getApiUrl('delete'), {
      ids,
    });
  }
}
