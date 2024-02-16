import { IPaginator } from '@app-shared/core/models/common.models';

export interface IUnitViewModel {
  id: string;
  name: string;
  isDefault: boolean;
  itemCount: number;
}

export interface IListUnitRequest {
  pageIndex: number;
  pageSize: number;
  name?: string;
  isDefault?: boolean;
  sort?: string;
}

export interface IListUnitResponse {
  records: IUnitViewModel[];
  paginator: IPaginator;
}

export interface ICreateUnitRequest {
  name: string;
}

export interface IUpdateUnitRequest {
  id: string;
  name: string;
}
