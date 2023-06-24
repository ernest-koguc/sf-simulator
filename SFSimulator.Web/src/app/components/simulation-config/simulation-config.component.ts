import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfigurationDialogComponent } from '../../dialogs/configuration-dialog/configuration-dialog.component';
import { SaveNewConfigurationDialogComponent } from '../../dialogs/save-new-configuration-dialog/save-new-configuration-dialog.component';
import { defaultSchedule } from '../../layout/custom-schedule/custom-schedule.component';
import { SavedConfiguration } from '../../models/configuration';
import { MountType } from '../../models/mount-type';
import { QuestPriority } from '../../models/quest-priority';
import { SavedSchedule } from '../../models/schedule';
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

  constructor(private snackBar: SnackbarService, private databaseService: DataBaseService, private dialog: MatDialog)
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
  public savedConfiguration?: SavedConfiguration;

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

  loadFormFromConfiguration(configuration: SavedConfiguration) {
    this.simulationOptions.patchValue({ ...configuration.playstyle, ...configuration.character }, { emitEvent: true });
    var schedule = this.savedSchedules.find(v => v.timestamp == configuration.scheduleId) ?? this.savedSchedules.find(v => v.timestamp == 0);

    if (schedule)
      this.simulationOptions.controls.schedule.patchValue(schedule, {emitEvent: true});

    this.savedConfiguration = configuration;
    this.snackBar.createInfoSnackbar("Loaded configuration: " + configuration.name);
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
      this.snackBar.createErrorSnackbar('Error occured while parsing API response');
      return;
    }
  }

  public openConfigurationDialog() {
    this.dialog.open(ConfigurationDialogComponent, { autoFocus: false }).afterClosed().subscribe(configuration => {
      if (!configuration)
        return;

      this.loadFormFromConfiguration(configuration);
    });
  }

  public saveConfiguration(includeCharacter: boolean) {
    if (!this.savedConfiguration)
      return;

    this.savedConfiguration.updateConfiguration(this.simulationOptions.getRawValue(), includeCharacter);
    this.databaseService.saveConfiguration(this.savedConfiguration);
    this.snackBar.createSuccessSnackBar("Configuration saved successfully");
  }

  public saveConfigurationAsNew(includeCharacter: boolean) {
    this.dialog.open(SaveNewConfigurationDialogComponent, { autoFocus: false, data: this.simulationOptions.controls.characterName.value }).afterClosed().subscribe(name => {
      if (!name)
        return;

      var newConfiguration = new SavedConfiguration(name, this.simulationOptions.getRawValue(), includeCharacter);
      this.databaseService.saveConfiguration(newConfiguration);
      this.snackBar.createSuccessSnackBar("Configuration saved successfully");
    })
  }

  private mapToForm(data: any): Partial<SimulationOptionsForm> {
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
