import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { InfoDialogComponent } from '../../dialogs/info-dialog/info-dialog.component';

@Component({
  selector: 'tool-bar',
  templateUrl: './tool-bar.component.html',
  styleUrls: ['./tool-bar.component.scss']
})
export class ToolBarComponent {
  constructor(
    private dialog: MatDialog
  ) {}

  
  public openInfoDialog() {
    this.dialog.open(InfoDialogComponent, { autoFocus: 'dialog', enterAnimationDuration: 400 });
  }
}
