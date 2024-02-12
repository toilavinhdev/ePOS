import { Component, EventEmitter, Input, Output } from '@angular/core';
import { defaultImagePath } from '@app-shared/constants';

@Component({
  selector: 'app-upload-image',
  standalone: true,
  imports: [],
  templateUrl: './upload-image.component.html',
  styles: ``,
})
export class UploadImageComponent {
  @Input() styleClass!: string;
  @Input() src!: string;
  @Output() fileChange = new EventEmitter<File>();
  blobSrc!: string;
  file!: File;

  get source() {
    return this.blobSrc ? this.blobSrc : this.src ? this.src : defaultImagePath;
  }

  onInput(event: any) {
    this.file = event.target.files[0];
    this.blobSrc = URL.createObjectURL(
      new Blob([this.file], { type: this.file.type }),
    );
    this.fileChange.emit(this.file);
  }
}
