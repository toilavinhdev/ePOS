import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Observable, take, takeUntil } from 'rxjs';
import {
  ICreateOrderItemRequest,
  ICreateOrderRequest,
  IInvoiceLine,
} from '@app-shared/models/order.models';
import { Store } from '@ngrx/store';
import {
  checkout,
  checkoutSuccess,
  deleteAllLine,
  orderLinesSelector,
  orderSubTotalSelector,
} from '@app-shared/store/order';
import { AsyncPipe, DecimalPipe, JsonPipe, NgIf } from '@angular/common';
import { ImageModule } from 'primeng/image';
import { defaultImagePath } from '@app-shared/constants';
import { PosItemDetailComponent } from '@app-features/pos/pos-menu/pos-item-detail/pos-item-detail.component';
import { ButtonModule } from 'primeng/button';
import { Actions, ofType } from '@ngrx/effects';

@Component({
  selector: 'app-pos-bill',
  standalone: true,
  imports: [
    AsyncPipe,
    JsonPipe,
    ImageModule,
    NgIf,
    DecimalPipe,
    PosItemDetailComponent,
    ButtonModule,
  ],
  templateUrl: './pos-bill.component.html',
  styles: ``,
})
export class PosBillComponent extends BaseComponent implements OnInit {
  lines$!: Observable<IInvoiceLine[]>;
  subTotal$!: Observable<number>;

  constructor(
    private store: Store,
    private action$: Actions,
  ) {
    super();
  }

  ngOnInit() {
    this.lines$ = this.store
      .select(orderLinesSelector)
      .pipe(takeUntil(this.destroy$));
    this.subTotal$ = this.store
      .select(orderSubTotalSelector)
      .pipe(takeUntil(this.destroy$));
    this.action$
      .pipe(ofType(checkoutSuccess))
      .subscribe(() => this.removeAllLine());
  }

  protected readonly defaultImagePath = defaultImagePath;

  removeAllLine() {
    this.store.dispatch(deleteAllLine());
  }

  onCheckout() {
    this.store
      .select(orderLinesSelector)
      .pipe(take(1))
      .subscribe((data) => {
        const orderItems: ICreateOrderItemRequest[] = data.map((x) => ({
          itemId: x.item.id,
          itemSizeId: x.size?.id,
          quantity: x.quantity,
        }));
        const payload: ICreateOrderRequest = { orderItems: orderItems };
        this.store.dispatch(checkout({ payload: payload }));
      });
  }
}
