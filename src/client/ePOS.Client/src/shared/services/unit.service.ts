import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import {
  ICreateUnitRequest,
  IListUnitRequest,
  IListUnitResponse,
  IUnitViewModel,
  IUpdateUnitRequest,
} from '@app-shared/models/unit.models';
import { IAPIResponse } from '@app-shared/core/models/common.models';

@Injectable({
  providedIn: 'root',
})
export class UnitService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('unit');
  }

  list(payload: IListUnitRequest): Observable<IListUnitResponse> {
    return this.httpClient
      .post<IAPIResponse<IListUnitResponse>>(this.getApiUrl('list'), payload)
      .pipe(map((response) => response.data));
  }

  create(payload: ICreateUnitRequest): Observable<IUnitViewModel> {
    return this.httpClient
      .post<IAPIResponse<IUnitViewModel>>(this.getApiUrl('create'), payload)
      .pipe(map((response) => response.data));
  }

  update(payload: IUpdateUnitRequest): Observable<IUnitViewModel> {
    return this.httpClient
      .put<IAPIResponse<IUnitViewModel>>(this.getApiUrl('update'), payload)
      .pipe(map((response) => response.data));
  }
}
