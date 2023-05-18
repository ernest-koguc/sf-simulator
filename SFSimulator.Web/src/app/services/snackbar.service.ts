import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SnackbarComponent } from '../snackbars/snackbar/snackbar.component';

@Injectable({
  providedIn: 'root'
})
export class SnackbarService {

  constructor(private snackBar: MatSnackBar) { }

  public createSuccessSnackBar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, { panelClass: ["bg-success", "rounded"], duration: 3000, horizontalPosition: 'left', data: message });
  }

  public createErrorSnackbar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, { panelClass: ["bg-error", "rounded"], duration: 3000, horizontalPosition: 'left', data: message });
  }

  public createInfoSnackbar(message: string) {
    this.snackBar.openFromComponent(SnackbarComponent, { panelClass: ["bg-info", "rounded"], duration: 4000, horizontalPosition: 'left', data: message })
  }
}
