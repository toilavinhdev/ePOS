import { createReducer, on } from '@ngrx/store';
import {
  addToInvoice,
  deleteAllLine,
  deleteLine,
  updateLine,
} from '@app-shared/store/order/order.actions';
import { IInvoiceLine } from '@app-shared/models/order.models';

export interface IOrderState {
  lines: IInvoiceLine[];
}

const initialState: IOrderState = {
  lines: [],
};

export const orderReducer = createReducer(
  initialState,
  on(addToInvoice, (state, { line }) => ({
    ...state,
    lines: handlerAddToCart(state.lines, line),
  })),
  on(updateLine, (state, { line }) => ({
    ...state,
    lines: state.lines.map((x) => (x.index === line.index ? line : x)),
  })),
  on(deleteLine, (state, { idx }) => ({
    ...state,
    lines: state.lines.filter((x) => x.index !== idx),
  })),
  on(deleteAllLine, (state) => ({
    ...state,
    lines: [],
  })),
);

function handlerAddToCart(
  invoice: IInvoiceLine[],
  newLine: IInvoiceLine,
): IInvoiceLine[] {
  let items = invoice.filter((x) => x.item.id === newLine.item.id);

  if (
    items.length === 0 ||
    !items.some((x) => x.size?.id === newLine.size?.id)
  ) {
    return [...invoice, { ...newLine, index: invoice.length + 1 }];
  } else {
    return invoice.map((line) => {
      if (
        (!newLine.size && !line.size && newLine.item.id === line.item.id) ||
        (newLine.size && line.size && newLine.size?.id === line.size?.id)
      ) {
        return { ...line, quantity: line.quantity + newLine.quantity };
      }
      return line;
    });
  }
}
