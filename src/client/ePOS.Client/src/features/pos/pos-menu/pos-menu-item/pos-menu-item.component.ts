import { Component, OnInit } from '@angular/core';
import { BaseComponent } from '@app-shared/core/abtractions';
import { Store } from '@ngrx/store';
import { itemListSelector } from '@app-shared/store/item';
import { Observable, takeUntil } from 'rxjs';
import { IItemViewModel } from '@app-shared/models/item.models';
import { AsyncPipe, JsonPipe } from '@angular/common';
import { ImageModule } from 'primeng/image';
import { defaultImagePath } from '@app-shared/constants';

@Component({
  selector: 'app-pos-menu-item',
  standalone: true,
  imports: [AsyncPipe, JsonPipe, ImageModule],
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
