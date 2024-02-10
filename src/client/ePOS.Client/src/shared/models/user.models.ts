export interface ISignInRequest {
  email: string;
  password: string;
}

export interface ISignInResponse {
  accessToken: string;
  refreshToken: string;
}
