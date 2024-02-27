import { ICategoryViewModel } from '@app-shared/models/category.models';
import { IPaginator } from '@app-shared/core/models/common.models';
import { createReducer, on } from '@ngrx/store';
import {
  createCategory,
  createCategoryFailed,
  createCategorySuccess,
  listCategory,
  listCategoryFailed,
  listCategorySuccess,
} from '@app-shared/store/category/category.actions';

export interface ICategoryState {
  loadingList: boolean;
  loadingCreateOrUpdate: boolean;
  categories: ICategoryViewModel[];
  totalRecords: number;
  paginator?: IPaginator;
}

const initialState: ICategoryState = {
  loadingList: false,
  loadingCreateOrUpdate: false,
  categories: [],
  totalRecords: 0,
};

export const categoryReducer = createReducer(
  initialState,
  on(listCategory, (state) => ({ ...state, loadingList: true })),
  on(listCategorySuccess, (state, { response }) => ({
    ...state,
    loadingList: false,
    categories: response.records,
    paginator: response.paginator,
    totalRecords: response.totalRecords,
  })),
  on(listCategoryFailed, (state) => ({ ...state, loadingList: false })),
  on(createCategory, (state) => ({ ...state, loadingCreateOrUpdate: true })),
  on(createCategorySuccess, (state, { data }) => ({
    ...state,
    loadingCreateOrUpdate: false,
  })),
  on(createCategoryFailed, (state) => ({
    ...state,
    loadingCreateOrUpdate: false,
  })),
);
