import { createReducer, on } from '@ngrx/store';
import {
  uploadFile,
  uploadFileFailed,
  uploadFileSuccess,
  uploadMultipleFile,
  uploadMultipleFileFailed,
  uploadMultipleFileSuccess,
} from '@app-shared/store/storage/storage.actions';

export interface IStorageState {
  loading: boolean;
}

const initialState: IStorageState = {
  loading: false,
};

export const storageReducer = createReducer(
  initialState,
  on(uploadFile, (state) => ({
    loading: true,
  })),
  on(uploadFileSuccess, (state) => ({
    loading: false,
  })),
  on(uploadFileFailed, (state) => ({
    loading: false,
  })),
  on(uploadMultipleFile, (state) => ({
    loading: true,
  })),
  on(uploadMultipleFileSuccess, (state) => ({
    loading: false,
  })),
  on(uploadMultipleFileFailed, (state) => ({
    loading: false,
  })),
);
