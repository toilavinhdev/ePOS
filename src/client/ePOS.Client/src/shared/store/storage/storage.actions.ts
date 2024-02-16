import { createAction, props } from '@ngrx/store';

export const uploadFile = createAction(
  '[Storage] Upload File',
  props<{ file: File }>(),
);

export const uploadFileSuccess = createAction(
  '[Storage] Upload File Success',
  props<{ url: string }>(),
);

export const uploadFileFailed = createAction(
  '[Storage] Upload File Failed',
  props<{ error: string }>(),
);

export const uploadMultipleFile = createAction(
  '[Storage] Upload Multiple File',
  props<{ files: File[] }>(),
);

export const uploadMultipleFileSuccess = createAction(
  '[Storage] Upload Multiple File Success',
  props<{ urls: string[] }>(),
);

export const uploadMultipleFileFailed = createAction(
  '[Storage] Upload Multiple File Failed',
  props<{ error: string }>(),
);
