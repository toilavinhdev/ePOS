import { IUnitViewModel } from '@app-shared/models/unit.models';
import { IPaginator } from '@app-shared/core/models/common.models';
import { createReducer, on } from '@ngrx/store';
import {
  createUnit,
  createUnitSuccess,
  listUnit,
  listUnitFailed,
  listUnitSuccess,
  updateUnit,
  updateUnitFailed,
  updateUnitSuccess,
} from '@app-shared/store/unit/unit.actions';

export interface IUnitState {
  loadingList: boolean;
  loadingCreateOrUpdate: boolean;
  units: IUnitViewModel[];
  paginator: IPaginator | null;
}

const initialState: IUnitState = {
  loadingList: false,
  loadingCreateOrUpdate: false,
  units: [],
  paginator: null,
};

export const unitReducer = createReducer(
  initialState,
  on(listUnit, (state) => ({
    ...state,
    loadingList: true,
  })),
  on(listUnitSuccess, (state, { data }) => ({
    ...state,
    loadingList: false,
    units: data.records,
    paginator: data.paginator,
  })),
  on(listUnitFailed, (state) => ({
    ...state,
    loadingList: false,
  })),
  on(createUnit, (state) => ({
    ...state,
    loadingCreateOrUpdate: true,
  })),
  on(createUnitSuccess, (state, { data }) => ({
    ...state,
    loadingCreateOrUpdate: false,
    units: [...state.units, data],
  })),
  on(createUnit, (state) => ({
    ...state,
    loadingCreateOrUpdate: false,
  })),
  on(updateUnit, (state) => ({
    ...state,
    loadingCreateOrUpdate: true,
  })),
  on(updateUnitSuccess, (state, { data }) => ({
    ...state,
    loadingCreateOrUpdate: false,
    units: state.units.map((x) => {
      if (x.id === data.id) return { ...data };
      return x;
    }),
  })),
  on(updateUnitFailed, (state) => ({
    ...state,
    loadingCreateOrUpdate: false,
  })),
);
