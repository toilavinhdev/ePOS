import {
  Component,
  EventEmitter,
  Input,
  Output,
  QueryList,
  ViewChildren,
} from '@angular/core';
import { NgForOf } from '@angular/common';
import { UploadImageComponent } from '@app-shared/components/upload-image/upload-image.component';

@Component({
  selector: 'app-upload-multiple-image',
  standalone: true,
  imports: [NgForOf, UploadImageComponent],
  templateUrl: './upload-multiple-image.component.html',
  styles: ``,
})
export class UploadMultipleImageComponent {
  @Input() count = 5;
  @Input() itemStyleClass = 'h-[100px] w-[100px]';
  @Output() fileChange = new EventEmitter<File[]>();
  files!: File[];
  @ViewChildren(UploadImageComponent)
  uploadImageComponents!: QueryList<UploadImageComponent>;

  getFiles() {
    return this.uploadImageComponents.filter((x) => x.file).map((x) => x.file);
  }

  onFileChange() {
    this.fileChange.emit(this.getFiles());
  }
}
