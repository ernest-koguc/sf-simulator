import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from '../../../environments/environment';
import { InfoDialogComponent } from '../../dialogs/info-dialog/info-dialog.component';
import { PatchNotesDialogComponent } from '../../dialogs/patch-notes-dialog/patch-notes-dialog.component';

@Component({
  selector: 'tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.scss']
})
export class ToolBarComponent {
  constructor(
    private dialog: MatDialog
  ) {}

  production = environment.production;
  
  public openInfoDialog() {
    this.dialog.open(InfoDialogComponent, { autoFocus: 'dialog', enterAnimationDuration: 400 });
  }

  public openPatchNotesDialog() {
    this.dialog.open(PatchNotesDialogComponent, { autoFocus: false, enterAnimationDuration: 400 })
  }
}
