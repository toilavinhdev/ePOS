import { Component, OnInit } from '@angular/core';
import { InputTextModule } from 'primeng/inputtext';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { debounceTime, Observable, Subject, takeUntil } from 'rxjs';
import { ITenantViewModel } from '@app-shared/models/tenant.models';
import { tenantSelector } from '@app-shared/store/tenant';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PosMenuCategoryComponent } from '@app-features/pos/pos-menu/pos-menu-category/pos-menu-category.component';
import { PosMenuItemComponent } from '@app-features/pos/pos-menu/pos-menu-item/pos-menu-item.component';
import { itemListSelector, listItem } from '@app-shared/store/item';
import { IItemViewModel } from '@app-shared/models/item.models';
import { listCategory } from '@app-shared/store/category';

@Component({
  selector: 'app-pos-menu',
  standalone: true,
  imports: [
    InputTextModule,
    AsyncPipe,
    PosMenuCategoryComponent,
    PosMenuItemComponent,
    FormsModule,
  ],
  templateUrl: './pos-menu.component.html',
  styles: ``,
})
export class PosMenuComponent extends BaseComponent implements OnInit {
  tenant$!: Observable<ITenantViewModel | undefined>;

  itemNameSubject$ = new Subject<string>();

  item$!: Observable<IItemViewModel[]>;

  itemName = '';
  pageIndex = 1;
  pageSize = 15;
  categoryId: string | undefined = undefined;

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {
    this.setSelector();
    this.itemNameSubject$
      .pipe(takeUntil(this.destroy$), debounceTime(300))
      .subscribe(() => this.loadMenu());
    this.loadMenu();
  }

  onSearchChange(event: string) {
    this.itemNameSubject$.next(event);
  }

  onCategoryIdChange(categoryId: string | undefined) {
    this.categoryId = categoryId;
    this.loadMenu();
  }

  loadMenu() {
    this.store.dispatch(
      listItem({
        payload: {
          pageIndex: this.pageIndex,
          pageSize: this.pageSize,
          name: this.itemName,
          categoryId: this.categoryId,
          isActive: true,
        },
      }),
    );
    this.store.dispatch(
      listCategory({
        payload: {
          pageIndex: 1,
          pageSize: 20,
          isActive: true,
          itemName: this.itemName,
        },
      }),
    );
  }

  private setSelector() {
    this.tenant$ = this.store
      .select(tenantSelector)
      .pipe(takeUntil(this.destroy$));
    this.item$ = this.store
      .select(itemListSelector)
      .pipe(takeUntil(this.destroy$));
  }
}
