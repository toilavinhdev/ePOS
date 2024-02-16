import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions';
import { map, Observable } from 'rxjs';
import {
  ITenantViewModel,
  IUpdateTenantRequest,
} from '@app-shared/models/tenant.models';
import { HttpClient } from '@angular/common/http';
import { IAPIResponse } from '@app-shared/core/models/common.models';

@Injectable({
  providedIn: 'root',
})
export class TenantService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('tenant');
  }

  getTenant(): Observable<ITenantViewModel> {
    return this.httpClient
      .get<IAPIResponse<ITenantViewModel>>(this.getApiUrl(''))
      .pipe(map((response) => response.data));
  }

  updateTenant(payload: IUpdateTenantRequest): Observable<ITenantViewModel> {
    return this.httpClient
      .put<IAPIResponse<ITenantViewModel>>(this.getApiUrl('update'), payload)
      .pipe(map((response) => response.data));
  }
}
