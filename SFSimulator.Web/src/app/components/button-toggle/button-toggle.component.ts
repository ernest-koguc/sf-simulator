import { Component, Input } from '@angular/core';
import { Action, Event } from '../../models/schedule';

@Component({
  selector: 'btn-tgl',
  templateUrl: './button-toggle.component.html',
  styleUrls: ['./button-toggle.component.scss']
})
export class ButtonToggleComponent {

  @Input()
  public value?: Action | Event;
  @Input()
  public toggled: boolean = false;
}
