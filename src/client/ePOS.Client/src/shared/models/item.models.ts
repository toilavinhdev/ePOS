import { IPaginator } from '@app-shared/core/models/common.models';

export interface IItemViewModel {
  id: string;
  sku: string;
  name: string;
  isActive: boolean;
  price?: boolean;
  sizePrices?: IItemSizePriceViewModel[];
  unitId: string;
  unitName: string;
  images?: IItemImageViewModel[];
  createdAt: Date;
}

export interface IItemSizePriceViewModel {
  name: string;
  price: number;
}

export interface IItemImageViewModel {
  url: string;
  sortIndex: string;
}

export interface IListItemRequest {
  pageIndex: number;
  pageSize: number;
  name?: string;
  categoryId?: string;
  isActive?: boolean;
  sort?: string;
}

export interface IListItemResponse {
  records: IItemViewModel[];
  paginator: IPaginator;
}

export interface ICreateItemRequest {
  name: string;
  sku: string;
  price?: number;
  unitId: number;
  imagesUrls?: string[];
  sizePrices?: IItemSizePriceViewModel[];
}
