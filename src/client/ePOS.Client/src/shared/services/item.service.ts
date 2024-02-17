import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions';
import {
  ICreateItemRequest,
  IItemViewModel,
  IListItemRequest,
  IListItemResponse,
} from '@app-shared/models/item.models';
import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { IAPIResponse } from '@app-shared/core/models/common.models';

@Injectable({
  providedIn: 'root',
})
export class ItemService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('item');
  }

  get(id: string): Observable<IItemViewModel> {
    return this.httpClient
      .get<IAPIResponse<IItemViewModel>>('', {
        params: { Id: id },
      })
      .pipe(map((response) => response.data));
  }

  list(payload: IListItemRequest): Observable<IListItemResponse> {
    return this.httpClient
      .post<IAPIResponse<IListItemResponse>>(this.getApiUrl('list'), payload)
      .pipe(map((response) => response.data));
  }

  create(payload: ICreateItemRequest): Observable<IItemViewModel> {
    return this.httpClient
      .post<IAPIResponse<IItemViewModel>>(this.getApiUrl('create'), payload)
      .pipe(map((response) => response.data));
  }
}
