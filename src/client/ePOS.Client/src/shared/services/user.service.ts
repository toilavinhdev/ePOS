import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions/base.service';
import {
  ISignInRequest,
  ISignInResponse,
} from '@app-shared/models/user.models';
import { map, Observable } from 'rxjs';
import { IAPIResponse } from '@app-shared/core/models/common.models';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root',
})
export class UserService extends BaseService {
  constructor(private httpClient: HttpClient) {
    super();
    this.setEndpoint('user');
  }

  signIn(payload: ISignInRequest): Observable<ISignInResponse> {
    return this.httpClient
      .post<IAPIResponse<ISignInResponse>>(this.getApiUrl('sign-in'), payload)
      .pipe(map((response) => response.data));
  }
}
