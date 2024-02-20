import { IPaginator } from '@app-shared/core/models/common.models';

export interface IListCategoryRequest {
  pageIndex: number;
  pageSize: number;
  name?: string;
  sort?: string;
  isActive?: string;
}

export interface ICategoryViewModel {
  id: string;
  name: string;
  isActive: boolean;
  itemCount: number;
}

export interface IListCategoryResponse {
  records: ICategoryViewModel[];
  paginator: IPaginator;
}

export interface ICreateCategoryRequest {
  name: string;
  itemIds?: string[];
}
