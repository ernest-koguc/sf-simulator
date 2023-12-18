import { Component, OnDestroy, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { environment } from '../../../environments/environment';
import { mapToLowerCase } from '../../helpers/mapper';

@Component({
  selector: 'app-sftoolslogin',
  templateUrl: './sftoolslogin.component.html',
  styleUrls: ['./sftoolslogin.component.scss']
})
export class SftoolsloginComponent implements OnDestroy {
  constructor(public dialogRef: MatDialogRef<SftoolsloginComponent>, private sanitizer: DomSanitizer,
    @Inject(MAT_DIALOG_DATA) private scope: DataScope) {
    let url = environment.sftoolsLogin + `&redirect=${environment.apiUrl}/api/${this.scope}`;
    this.iframeSource = this.sanitizer.bypassSecurityTrustResourceUrl(url);
    window.addEventListener('message', this.eventHandler);
  }

  iframeSource?: SafeResourceUrl;
  eventHandler = (e: MessageEvent) => {
    if (e.origin != environment.apiUrl) {
      return;
    }
    if (e.data.event == 'sftools-close') {
      setTimeout(() => this.dialogRef.close(), 100);
    }

    setTimeout(() => this.dialogRef.close(mapToLowerCase(e.data)), 100);
  }
  ngOnDestroy(): void {
    window.removeEventListener('message', this.eventHandler);
    }
}

export enum DataScope {
  All = 'loadFromEndpoint',
  Dungeon = 'loadDungeonFromEndpoint'
}
