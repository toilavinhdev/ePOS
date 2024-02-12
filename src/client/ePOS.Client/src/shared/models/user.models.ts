export interface ISignInRequest {
  email: string;
  password: string;
}

export interface ISignInResponse {
  accessToken: string;
  refreshToken: string;
}

export interface ISignUpRequest {
  fullName: string;
  tenantName: string;
  email: string;
  password: string;
}
