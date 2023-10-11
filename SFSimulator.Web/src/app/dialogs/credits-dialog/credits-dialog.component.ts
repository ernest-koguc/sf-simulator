import { Component } from '@angular/core';
import { SnackbarService } from '../../services/snackbar.service';
import { UserService } from '../../services/user.service';

@Component({
  selector: 'app-credits-dialog',
  templateUrl: './credits-dialog.component.html',
  styleUrls: ['./credits-dialog.component.scss']
})
export class CreditsDialogComponent {
  constructor(private userService: UserService, private snackbarService: SnackbarService) { }

  private counter = 0;
  
  public increment() {
    if (this.counter === 4) {

      let isAdvancedModeEnabled = this.userService.isAdvancedModeEnabled();
      if (!isAdvancedModeEnabled) {

        this.snackbarService.createInfoSnackbar('Advanced mode enabled!');
        this.userService.enableAdvancedMode();
      }
      else {
        this.snackbarService.createInfoSnackbar('Already in advanced mode!');
      }

      return;
    }
    this.counter++;
  }
}
