import { Injectable } from '@angular/core';
import { BaseService } from '@app-shared/core/abtractions/base.service';
import {
  ISignInRequest,
  ISignInResponse,
  ISignUpRequest,
} from '@app-shared/models/user.models';
import { map, Observable } from 'rxjs';
import {
  IAPIResponse,
  IUserClaimsValue,
} from '@app-shared/core/models/common.models';
import { HttpClient } from '@angular/common/http';
import { USER_DATA } from '@app-shared/constants';

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

  signUp(payload: ISignUpRequest): Observable<any> {
    return this.httpClient.post<IAPIResponse>(
      this.getApiUrl('sign-up'),
      payload,
    );
  }

  getUserClaimsValidExp(): IUserClaimsValue | null {
    const data = localStorage.getItem(USER_DATA);
    const userClaims = data ? (JSON.parse(data) as IUserClaimsValue) : null;
    return userClaims && userClaims.exp * 1000 > Date.now() ? userClaims : null;
  }
}
