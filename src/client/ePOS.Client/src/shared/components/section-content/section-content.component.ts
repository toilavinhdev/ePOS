import { Component, Input, TemplateRef } from '@angular/core';
import { NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'app-section-content',
  standalone: true,
  imports: [NgTemplateOutlet],
  templateUrl: './section-content.component.html',
  styles: ``,
})
export class SectionContentComponent {
  @Input() title: string = '';
  @Input() customTitle?: TemplateRef<any>;
  @Input() loading: boolean = false;
}
