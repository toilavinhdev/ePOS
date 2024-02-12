import { Component, Input, OnInit } from '@angular/core';
import { NgForOf } from '@angular/common';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter } from 'rxjs';
import { RippleModule } from 'primeng/ripple';

export interface IMenuItem {
  title: string;
  url: string;
  active?: boolean;
}

@Component({
  selector: 'app-menu',
  standalone: true,
  imports: [NgForOf, RouterLink, RippleModule],
  templateUrl: './menu.component.html',
  styles: ``,
})
export class MenuComponent implements OnInit {
  @Input() items: IMenuItem[] = [];

  constructor(private _router: Router) {}

  ngOnInit() {
    this.items.forEach((item) => {
      item.active = this._router.url === item.url;
    });
    this._router.events
      .pipe(filter((item) => item instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.items.forEach((item) => {
          item.active = event.url === item.url;
        });
      });
  }
}
