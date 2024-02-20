import { IOptionAttributeViewModel } from '@app-shared/models/option-attribute.models';
import { IToppingViewModel } from '@app-shared/models/topping.models';
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
  optionAttributes?: IOptionAttributeViewModel[];
  toppings?: IToppingViewModel[];
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
  toppingIds?: string[];
  optionAttributeIds?: string[];
  sizePrices?: IItemSizePriceViewModel[];
}
