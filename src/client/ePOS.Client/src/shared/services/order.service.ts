import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions';
import { HttpClient } from '@angular/common/http';
import {
  ICreateOrderRequest,
  IOrderViewModel,
} from '@app-shared/models/order.models';
import { map, Observable, takeUntil } from 'rxjs';
import { IAPIResponse } from '@app-shared/core/models/common.models';

@Injectable({
  providedIn: 'root',
})
export class OrderService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('order');
  }

  checkout(payload: ICreateOrderRequest): Observable<IOrderViewModel> {
    return this.httpClient
      .post<IAPIResponse<IOrderViewModel>>(this.getApiUrl('checkout'), payload)
      .pipe(map((response) => response.data));
  }
}
