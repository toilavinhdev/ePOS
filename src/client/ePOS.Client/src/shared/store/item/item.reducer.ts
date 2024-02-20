import { IItemViewModel } from '@app-shared/models/item.models';
import { IPaginator } from '@app-shared/core/models/common.models';
import { createReducer, on } from '@ngrx/store';
import {
  createItem,
  createItemSuccess,
  listItem,
  listItemFailed,
  listItemSuccess,
} from '@app-shared/store/item/item.actions';

export interface IItemState {
  loadingList: boolean;
  loadingCreateOrUpdate: boolean;
  items: IItemViewModel[];
  paginator?: IPaginator;
}

const initialState: IItemState = {
  loadingList: false,
  loadingCreateOrUpdate: false,
  items: [],
};

export const itemReducer = createReducer(
  initialState,
  on(listItem, (state) => ({ ...state, loadingList: true })),
  on(listItemSuccess, (state, data) => ({
    ...state,
    loadingList: false,
    items: data.data.records,
    paginator: data.data.paginator,
  })),
  on(listItemFailed, (state) => ({ ...state, loadingList: false })),
  on(createItem, (state) => ({
    ...state,
    loadingCreateOrUpdate: true,
  })),
  on(createItemSuccess, (state, data) => ({
    ...state,
    loadingCreateOrUpdate: false,
  })),
);
