import { Injectable } from '@angular/core';
import { StorageService } from '@app-shared/services';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import {
  uploadFile,
  uploadFileFailed,
  uploadFileSuccess,
  uploadMultipleFile,
  uploadMultipleFileFailed,
  uploadMultipleFileSuccess,
} from '@app-shared/store/storage/storage.actions';
import { catchError, map, of, switchMap } from 'rxjs';

@Injectable()
export class StorageEffects {
  constructor(
    private storageService: StorageService,
    private actions: Actions,
  ) {}

  uploadFile$ = createEffect(() =>
    this.actions.pipe(
      ofType(uploadFile),
      switchMap(({ file }) =>
        this.storageService.upload(file).pipe(
          map((url) => uploadFileSuccess({ url: url })),
          catchError((err) => of(uploadFileFailed({ error: err }))),
        ),
      ),
    ),
  );

  uploadMultipleFile$ = createEffect(() =>
    this.actions.pipe(
      ofType(uploadMultipleFile),
      switchMap(({ files }) =>
        this.storageService.uploadMultiple(files).pipe(
          map((urls) => uploadMultipleFileSuccess({ urls: urls })),
          catchError((err) => of(uploadMultipleFileFailed({ error: err }))),
        ),
      ),
    ),
  );
}
