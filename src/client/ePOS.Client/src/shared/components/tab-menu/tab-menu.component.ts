import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

export interface ITabMenuItem {
  id?: string;
  title: string;
  active?: boolean;
}

@Component({
  selector: 'app-tab-menu',
  standalone: true,
  imports: [],
  templateUrl: './tab-menu.component.html',
  styles: ``,
})
export class TabMenuComponent implements OnInit {
  @Input() items: ITabMenuItem[] = [];
  @Output() activeChange = new EventEmitter<string>();

  ngOnInit() {
    this.items.forEach((item, idx) => {
      item.active = idx === 0;
    });
    this.activeChange.emit(this.items[0].id);
  }

  onActive(item: ITabMenuItem) {
    this.items.forEach((item) => (item.active = false));
    item.active = true;
    this.activeChange.emit(item.id);
  }
}
