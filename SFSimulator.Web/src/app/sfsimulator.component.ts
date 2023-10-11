import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { environment } from '../environments/environment';
import { PatchNotesDialogComponent } from './dialogs/patch-notes-dialog/patch-notes-dialog.component';
import { UserService } from './services/user.service';


@Component({
  selector: 'sfsimulator',
  templateUrl: './sfsimulator.component.html',
  styleUrls: ['./sfsimulator.component.scss']
})
export class SFSimulatorComponent implements OnInit {
  constructor(private userService: UserService, private matDialog: MatDialog) {
  }

  ngOnInit(): void {
    if (this.userService.hasUserSeenPatchNotes()) {
      return;
    }
    setTimeout(() => this.matDialog.open(PatchNotesDialogComponent, { autoFocus: false, enterAnimationDuration: 400, disableClose: true })
      .afterClosed()
      .subscribe(s =>
      this.userService.updateLastSeenPatchNotes()), 1000);
  }
}
