import { Component, Input } from '@angular/core';
import { ngIfHorizontalSlide, ngIfVerticalSlide } from '../../animation/slide-animation';
import { SimulationResult } from '../../models/simulation-result';
import { CharacterSnapshot, SavedSimulationSnapshot } from '../../models/simulation-snapshot';
import { DataBaseService } from '../../services/database.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'simulation-result',
  templateUrl: './simulation-result.component.html',
  styleUrls: ['./simulation-result.component.scss'],
  animations: [ngIfVerticalSlide, ngIfHorizontalSlide]
})
export class SimulationResultComponent {
  @Input()
  public set data(value: SimulationResultComponentData | undefined) {
    this.isSaved = false;
    this._data = value;
  }
  @Input()
  public loading: boolean = false;
  @Input()
  public isSaved: boolean = false;
  public _data?: SimulationResultComponentData
  public get from() {
    return new Date(Date.now()).toLocaleDateString('en-gb');
  }
  public get to() {
    let date = new Date(Date.now());
    date.setDate(date.getDate() + this._data!.simulationResult.days)
    return date.toLocaleDateString('en-gb');
  }
  public get characterAfter(): CharacterSnapshot {
    let characterAfter = {
      level: this._data!.simulationResult.level,
      baseStat: this._data!.simulationResult.baseStat,
      experience: this._data!.simulationResult.experience
    }
    return characterAfter;
  }
  constructor(private dataBaseService: DataBaseService, private snackbarService: SnackbarService) { }

  public saveResult() {
    this.isSaved = true;
    if (this._data) {
      var snapshot = new SavedSimulationSnapshot(this._data.simulationResult, this._data.characterBefore, this._data.characterName);
      this.dataBaseService.saveSimulationSnapshot(snapshot);
      this.snackbarService.createSuccessSnackBar('Simulation result has been saved')
    }
  }
}

export type SimulationResultComponentData = {
  simulationResult: SimulationResult,
  characterBefore: CharacterSnapshot,
  characterName: string | null
}
