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
  @Input() urls: string[] | undefined;
  @Input() count = 5;
  @Input() itemStyleClass = 'h-[100px] w-[100px]';
  @Output() fileChange = new EventEmitter<File[]>();
  files!: File[];
  @ViewChildren(UploadImageComponent)
  uploadImageComponents?: QueryList<UploadImageComponent>;

  getFiles() {
    return this.uploadImageComponents
      ? this.uploadImageComponents.filter((x) => x.file).map((x) => x.file)
      : [];
  }

  getBothFileAndSrc(): (File | string)[] | undefined {
    return this.uploadImageComponents
      ? this.uploadImageComponents.map((x) => x.src ?? x.file)
      : undefined;
  }

  onFileChange() {
    this.fileChange.emit(this.getFiles());
  }

  reset() {
    if (this.uploadImageComponents)
      this.uploadImageComponents.forEach((x) => x.reset());
  }
}
