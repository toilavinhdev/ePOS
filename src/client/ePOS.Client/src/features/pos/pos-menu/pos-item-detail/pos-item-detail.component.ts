import { Component, OnInit } from '@angular/core';
import {
  IItemSizePriceViewModel,
  IItemViewModel,
} from '@app-shared/models/item.models';
import { addToInvoice, deleteLine, updateLine } from '@app-shared/store/order';
import { defaultImagePath } from '@app-shared/constants';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { SidebarModule } from 'primeng/sidebar';
import { ImageModule } from 'primeng/image';
import { DecimalPipe, JsonPipe, NgForOf, NgIf } from '@angular/common';
import { InputNumberModule } from 'primeng/inputnumber';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { IInvoiceLine } from '@app-shared/models/order.models';

@Component({
  selector: 'app-pos-item-detail',
  standalone: true,
  imports: [
    SidebarModule,
    ImageModule,
    NgForOf,
    DecimalPipe,
    InputNumberModule,
    FormsModule,
    ButtonModule,
    NgIf,
    JsonPipe,
  ],
  templateUrl: './pos-item-detail.component.html',
  styles: ``,
})
export class PosItemDetailComponent extends BaseComponent implements OnInit {
  sidebarVisible = false;
  selectedItem: IItemViewModel | undefined = undefined;
  selectedSize: IItemSizePriceViewModel | undefined = undefined;
  quantity = 1;

  selectedLine: IInvoiceLine | undefined = undefined;

  modeEdit = false;

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {}

  protected readonly defaultImagePath = defaultImagePath;

  selectItem(item: IItemViewModel) {
    this.sidebarVisible = true;
    this.selectedItem = item;
  }

  selectSize(size: IItemSizePriceViewModel) {
    this.selectedSize = this.selectedSize?.id === size.id ? undefined : size;
  }

  selectLine(line: IInvoiceLine) {
    this.quantity = line.quantity;
    this.selectedSize = line.size;
    this.selectedItem = line.item;
    this.sidebarVisible = true;
    this.selectedLine = line;
    this.modeEdit = true;
  }

  onVisibleChange(event: boolean) {
    if (!event) {
      this.selectedItem = undefined;
      this.selectedSize = undefined;
      this.quantity = 1;
      this.modeEdit = false;
    }
    this.sidebarVisible = event;
  }

  onAddToInvoice() {
    if (!this.selectedItem) return;
    this.store.dispatch(
      addToInvoice({
        line: {
          item: this.selectedItem,
          size: this.selectedSize,
          quantity: this.quantity,
        },
      }),
    );
    this.onVisibleChange(false);
  }

  saveLine() {
    if (!this.selectedLine || !this.selectedItem) return;
    this.store.dispatch(
      updateLine({
        line: {
          item: this.selectedItem,
          size: this.selectedSize,
          quantity: this.quantity,
          index: this.selectedLine.index,
        },
      }),
    );
    this.onVisibleChange(false);
  }

  deleteLine() {
    if (!this.selectedLine || !this.selectedItem) return;
    this.store.dispatch(deleteLine({ idx: this.selectedLine.index! }));
    this.onVisibleChange(false);
  }
}
