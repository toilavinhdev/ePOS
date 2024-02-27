import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { itemListSelector } from '@app-shared/store/item';
import { Observable, takeUntil } from 'rxjs';
import { IItemViewModel } from '@app-shared/models/item.models';
import {
  AsyncPipe,
  DecimalPipe,
  JsonPipe,
  NgForOf,
  NgIf,
} from '@angular/common';
import { ImageModule } from 'primeng/image';
import { SidebarModule } from 'primeng/sidebar';
import { ButtonModule } from 'primeng/button';
import { InputNumberModule } from 'primeng/inputnumber';
import { FormsModule } from '@angular/forms';
import { PosItemDetailComponent } from '@app-features/pos/pos-menu/pos-item-detail/pos-item-detail.component';
import { defaultImagePath } from '@app-shared/constants';

@Component({
  selector: 'app-pos-menu-item',
  standalone: true,
  imports: [
    AsyncPipe,
    JsonPipe,
    ImageModule,
    SidebarModule,
    ButtonModule,
    DecimalPipe,
    NgIf,
    NgForOf,
    InputNumberModule,
    FormsModule,
    PosItemDetailComponent,
  ],
  templateUrl: './pos-menu-item.component.html',
  styles: ``,
})
export class PosMenuItemComponent extends BaseComponent implements OnInit {
  items$!: Observable<IItemViewModel[]>;

  constructor(private store: Store) {
    super();
  }

  ngOnInit() {
    this.items$ = this.store
      .select(itemListSelector)
      .pipe(takeUntil(this.destroy$));
  }

  protected readonly defaultImagePath = defaultImagePath;
}
