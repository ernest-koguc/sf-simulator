import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TooltipPosition } from '@angular/material/tooltip';
import { defaultSchedule } from '../../layout/custom-schedule/custom-schedule.component';
import { MountType } from '../../models/mount-type';
import { QuestPriority } from '../../models/quest-priority';
import { SavedSchedule, ScheduleWeek } from '../../models/schedule';
import { SimulationOptionsForm } from '../../models/simulation-options';
import { SpinTactic, spinTactics } from '../../models/spin-tactics';
import { DataBaseService } from '../../services/database.service';
import { SnackbarService } from '../../services/snackbar.service';


@Component({
  selector: 'simulation-config',
  templateUrl: './simulation-config.component.html',
  styleUrls: ['./simulation-config.component.scss']
})
export class SimulationConfig implements OnInit {

  constructor(private snackBar: SnackbarService, private databaseService: DataBaseService)
  {
    this.databaseService.getAllSchedules().subscribe(v => {
      var defaultConfig: SavedSchedule =  defaultSchedule;
      this.savedSchedules = [defaultConfig];
      if (v) {
        this.savedSchedules.push(...v);
        this.simulationOptions.controls.schedule.setValue(this.savedSchedules[0]);
      }
    });
    
  }
  @Output() configEmitter = new EventEmitter<SimulationOptionsForm>();

  public questPriority = ['Gold', 'Experience', 'Hybrid'];
  public mountType = ['None', 'Pig', 'Horse', 'Tiger', 'Griffin'];
  public savedSchedules!: SavedSchedule[];
  public defaultScheduleOption: SavedSchedule = defaultSchedule;
  public spinTactics = spinTactics;

  simulationOptions = new FormGroup({
    characterName: new FormControl(''),
    schedule: new FormControl<SavedSchedule>(this.defaultScheduleOption, [Validators.required]),
    questPriority: new FormControl<QuestPriority>('Experience', [Validators.required]),
    hybridRatio: new FormControl({ value: 0, disabled: true }, [Validators.required, Validators.min(0), Validators.max(1)]),
    switchPriority: new FormControl(false, [Validators.required]),
    switchLevel: new FormControl({ value: 0, disabled: true}, [Validators.required, Validators.min(0), Validators.max(700)]),
    priorityAfterSwitch: new FormControl<QuestPriority>({ value: 'Gold', disabled: true }, [Validators.required]),
    drinkBeerOneByOne: new FormControl(true, [Validators.required]),
    dailyThirst: new FormControl(320, [Validators.required, Validators.min(0), Validators.max(320)]),
    skipCalendar: new FormControl(true, [Validators.required]),
    level: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(700)]),
    baseStat: new FormControl(0, [Validators.required, Validators.min(0)]),
    experience: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(1499999999)]),
    goldPitLevel: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(100)]),
    academyLevel: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(20)]),
    hydraHeads: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(20)]),
    gemMineLevel: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(100)]),
    treasuryLevel: new FormControl(0, [Validators.required, Validators.min(0), Validators.max(45)]),
    mountType: new FormControl<MountType>('Griffin', [Validators.required]),
    scrapbookFillness: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
    xpGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
    xpRuneBonus: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10)]),
    hasExperienceScroll: new FormControl(true),
    tower: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
    goldGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
    goldRuneBonus: new FormControl(50, [Validators.required, Validators.min(0), Validators.max(50)]),
    hasGoldScroll: new FormControl(true),
    spinAmount: new FormControl<SpinTactic>('Max', [Validators.required]),
    dailyGuard: new FormControl(23, [Validators.required, Validators.min(0), Validators.max(24)])
  });

  toggleInputs() {
    if (this.simulationOptions.controls.questPriority.value != 'Hybrid' && this.simulationOptions.controls.switchPriority.value == false)
      this.simulationOptions.controls.hybridRatio.disable({emitEvent: false})
    else if (this.simulationOptions.controls.questPriority.value != 'Hybrid' && this.simulationOptions.controls.priorityAfterSwitch.value != 'Hybrid')
      this.simulationOptions.controls.hybridRatio.disable({ emitEvent: false })
    else
      this.simulationOptions.controls.hybridRatio.enable({ emitEvent: false })

    if (this.simulationOptions.controls.switchPriority.value == true) {
      this.simulationOptions.controls.switchLevel.enable({ emitEvent: false })
      this.simulationOptions.controls.priorityAfterSwitch.enable({ emitEvent: false })
    }
    else {
      this.simulationOptions.controls.switchLevel.disable({ emitEvent: false })
      this.simulationOptions.controls.priorityAfterSwitch.disable({ emitEvent: false })
    }
  }


  loadForm(data: any): void {
    if (data.error) {
      this.snackBar.createErrorSnackbar(data.error);
      return;
    }
    try {
      var form = this.mapToForm(data);
      this.simulationOptions.patchValue(form, { emitEvent: true });
      setTimeout(() => this.snackBar.createSuccessSnackBar("Successfully logged in!"), 200);
      return;
    }
    catch {
      this.snackBar.createErrorSnackbar('Error with parsing API response');
      return;
    }
  }

  mapToForm(data: any): Partial<SimulationOptionsForm> {
    var key, keys = Object.keys(data);
    var n = keys.length;
    var mappedData: any = {};
    while (n--) {
      key = keys[n][0].toLowerCase();
      var lowerCaseName = key + keys[n].substring(1);
      mappedData[lowerCaseName] = data[keys[n]];
    }
    return mappedData;
  }

  ngOnInit(): void {
    this.simulationOptions.valueChanges.subscribe(_ => {
      this.toggleInputs();

      var form = this.simulationOptions.valid ? this.simulationOptions.getRawValue() : undefined;
      this.configEmitter.emit(form);
    });

    if (this.simulationOptions.valid)
      this.configEmitter.emit(this.simulationOptions.getRawValue());
  }
}
