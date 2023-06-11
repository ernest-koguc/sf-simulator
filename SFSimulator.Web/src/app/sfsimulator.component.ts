import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from '../environments/environment';
import { PatchNotesDialogComponent } from './dialogs/patch-notes-dialog/patch-notes-dialog.component';
import { DataBaseService } from './services/database.service';


@Component({
  selector: 'sfsimulator',
  templateUrl: './sfsimulator.component.html',
  styleUrls: ['./sfsimulator.component.scss']
})
export class SFSimulatorComponent implements OnInit {
  constructor(private dataBaseService: DataBaseService, private matDialog: MatDialog) {
  
  }

  ngOnInit(): void {
    var userData = this.dataBaseService.getUserData();

    if (!userData.lastSeenPatchNotes || environment.currentVersion > userData.lastSeenPatchNotes) {
      setTimeout(() => this.matDialog.open(PatchNotesDialogComponent, { autoFocus: false, enterAnimationDuration: 400, disableClose: true }).afterClosed().subscribe(s =>
        this.dataBaseService.updateUserData({ lastSeenPatchNotes: environment.currentVersion })), 1000);
    }
  }
}
