import { createFeatureSelector, createSelector } from '@ngrx/store';
import { appStateKey } from '@app-shared/store/app.state';
import { IOrderState } from '@app-shared/store/order/order.reducer';

export const featureOrder = createFeatureSelector<IOrderState>(
  appStateKey.feature_order,
);

export const orderLinesSelector = createSelector(
  featureOrder,
  (state) => state.lines,
);

export const orderSubTotalSelector = createSelector(featureOrder, (state) =>
  state.lines.reduce(
    (acc, cur) =>
      acc + (cur.item.price + (cur.size?.price ?? 0)) * cur.quantity,
    0,
  ),
);
