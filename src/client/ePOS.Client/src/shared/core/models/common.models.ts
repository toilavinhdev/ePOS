export interface IAPIResponse<T = any> {
  success: boolean;
  statusCode: number;
  message?: string;
  data: T;
}

export interface IPaginator {
  pageIndex: number;
  pageSize: number;
  totalRecords: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface IUserClaimsValue {
  exp: number;
  id: string;
  tenantId: string;
  fullName: string;
  email: string;
}
