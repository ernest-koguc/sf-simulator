import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-save-new-configuration-dialog',
  templateUrl: './save-new-configuration-dialog.component.html',
  styleUrls: ['./save-new-configuration-dialog.component.scss']
})
export class SaveNewConfigurationDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public name: string | null) { }
}
