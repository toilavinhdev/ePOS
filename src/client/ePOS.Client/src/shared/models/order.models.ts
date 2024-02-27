import {
  IItemSizePriceViewModel,
  IItemViewModel,
} from '@app-shared/models/item.models';

export interface ICreateOrderRequest {
  description?: string;
  orderItems: ICreateOrderItemRequest[];
}

export interface ICreateOrderItemRequest {
  itemId: string;
  itemSizeId?: string;
  quantity: number;
}

export interface IInvoiceLine {
  index?: number;
  item: IItemViewModel;
  size?: IItemSizePriceViewModel;
  quantity: number;
}

export interface IOrderViewModel {
  id: string;
  description?: string;
  subTotal: number;
  totalTax: number;
  total: number;
  orderItems: IOrderItemViewModel[];
}

export interface IOrderItemViewModel {
  itemId: string;
  itemName: string;
  sizeName?: string;
  quantity: string;
  totalLine: string;
}
