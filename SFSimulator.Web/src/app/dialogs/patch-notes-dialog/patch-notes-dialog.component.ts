import { Component } from '@angular/core';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-patch-notes-dialog',
  templateUrl: './patch-notes-dialog.component.html',
  styleUrls: ['./patch-notes-dialog.component.scss']
})
export class PatchNotesDialogComponent {
  public version = environment.currentVersion;
}
