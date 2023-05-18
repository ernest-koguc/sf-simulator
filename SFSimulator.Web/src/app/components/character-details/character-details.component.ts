import { Component, Input, OnInit } from '@angular/core';
import { SimulationResult } from '../../dto/simulation-result';


@Component({
  selector: 'character-details',
  templateUrl: './character-details.component.html',
  styleUrls: ['./character-details.component.scss']
})
export class CharacterDetailsComponent implements OnInit {

  constructor() { }
  @Input()
  public data?: SimulationResult;

  ngOnInit(): void {
  }

}
