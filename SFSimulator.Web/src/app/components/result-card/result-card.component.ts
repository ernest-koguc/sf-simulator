import { Component, Input } from '@angular/core';
import { CharacterSnapshot } from '../../models/simulation-snapshot';

@Component({
  selector: 'result-card',
  templateUrl: './result-card.component.html',
  styleUrls: ['./result-card.component.scss']
})
export class ResultCardComponent {
  @Input()
  public snapshot!: CharacterSnapshot
  @Input()
  public date!: string | Date;
}
