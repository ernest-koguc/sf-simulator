import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
import { mapToLowerCase } from '../../helpers/mapper';

@Component({
  selector: 'app-sftoolslogin',
  templateUrl: './sftoolslogin.component.html',
  styleUrls: ['./sftoolslogin.component.scss']
})
export class SftoolsloginComponent implements OnDestroy {
  constructor(public dialogRef: MatDialogRef<SftoolsloginComponent>, private sanitizer: DomSanitizer) {

    this.iframeSource = this.sanitizer.bypassSecurityTrustResourceUrl(environment.sftoolsLogin);
    window.addEventListener('message', this.eventHandler);
  }

  iframeSource?: SafeResourceUrl;
  eventHandler = (e: MessageEvent) => {
    if (e.origin == environment.apiUrl) {
      setTimeout(() => this.dialogRef.close(mapToLowerCase(e.data)), 100);
    }
    else if (e.data.event == 'sftools-close') {
      setTimeout(() => this.dialogRef.close(), 100);
    }
  }
  ngOnDestroy(): void {
    window.removeEventListener('message', this.eventHandler);
    }
}
