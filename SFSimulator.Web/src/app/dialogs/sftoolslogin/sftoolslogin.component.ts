import { Component, OnDestroy } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-sftoolslogin',
  templateUrl: './sftoolslogin.component.html',
  styleUrls: ['./sftoolslogin.component.scss']
})
export class SftoolsloginComponent implements OnDestroy {
  constructor(public dialogRef: MatDialogRef<SftoolsloginComponent>, private sanitizer: DomSanitizer) {

    this.iframeSource = this.sanitizer.bypassSecurityTrustResourceUrl(environment.sftoolsLogin);
    setTimeout(() => this.hide = undefined, 500);
    window.addEventListener('message', this.eventHandler);
  }

  iframeSource?: SafeResourceUrl;
  hide?: string = 'hidden';
  eventHandler = (e: MessageEvent) => {
    if (e.origin == environment.apiUrl) {
      setTimeout(() => this.dialogRef.close(e.data), 200);
    }
    else if (e.data.event == 'sftools-close')
      setTimeout(() => this.dialogRef.close(),200);
  }
  ngOnDestroy(): void {
    window.removeEventListener('message', this.eventHandler);
    }
}
