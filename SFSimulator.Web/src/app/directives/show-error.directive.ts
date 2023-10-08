import { Directive, ElementRef, Inject, Input, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ErrorService } from '../services/error.service';
@Directive({
  selector: 'mat-error[showError]',
})
export class ShowErrorDirective implements OnInit {
  @Input() control?: FormControl;

  constructor(private errorService: ErrorService, private host: ElementRef) {
  }

  ngOnInit(): void {
    this.handleErrorMessage();
    this.control?.statusChanges.subscribe(() => this.handleErrorMessage());
  }

  private handleErrorMessage() {
    if (!this.control?.errors) {
      this.setErrorMessage('');
      return;
    }

    let error = this.control.errors;
    let message = this.errorService.getErrorMessage(error);
    this.setErrorMessage(message);
  }

  private setErrorMessage(message?: string) {
    this.host.nativeElement.innerHTML = message;
  }
}
