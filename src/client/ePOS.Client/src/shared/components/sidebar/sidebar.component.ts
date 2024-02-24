import { Component, OnInit } from '@angular/core';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { CommonModule, NgTemplateOutlet } from '@angular/common';
import { filter } from 'rxjs';
import { BaseComponent } from '@app-shared/core/abtractions';

interface ISidebarItem {
  faIcon: string;
  title: string;
  url: string;
  selected: boolean;
  hidden?: boolean;
}

const items: ISidebarItem[] = [
  {
    faIcon: 'fa-solid fa-chart-pie',
    title: 'Báo cáo',
    url: '/report',
    selected: false,
  },
  {
    faIcon: 'fa-solid fa-layer-group',
    title: 'Quản lý',
    url: '/management',
    selected: false,
  },
  {
    faIcon: 'fa-regular fa-cup-togo',
    title: 'Sản phẩm',
    url: '/library',
    selected: false,
  },
  {
    faIcon: '',
    title: 'Hồ sơ cá nhân',
    url: '/profile',
    selected: false,
    hidden: true,
  },
  {
    faIcon: 'fa-solid fa-store',
    title: 'POS',
    url: '/pos',
    selected: false,
  },
];

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, NgTemplateOutlet, CommonModule],
  templateUrl: './sidebar.component.html',
})
export class SidebarComponent extends BaseComponent implements OnInit {
  currentTitle!: string;
  items = items;

  constructor(private _router: Router) {
    super();
  }

  ngOnInit() {
    this.trackItemSelected();
  }

  private trackItemSelected() {
    this.items.forEach((item) => {
      item.selected = this._router.url.includes(item.url);
      if (item.selected) this.currentTitle = item.title;
    });
    this._router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event: any) => {
        this.items.forEach((item) => {
          item.selected = item.url.split('/')[1] === event.url.split('/')[1];
          if (item.selected) this.currentTitle = item.title;
        });
      });
  }
}
