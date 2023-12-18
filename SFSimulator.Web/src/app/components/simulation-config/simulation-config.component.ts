import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfigurationDialogComponent } from '../../dialogs/configuration-dialog/configuration-dialog.component';
import { SaveNewConfigurationDialogComponent } from '../../dialogs/save-new-configuration-dialog/save-new-configuration-dialog.component';
import { maxExperienceValidator } from '../../helpers/validators';
import { SavedConfiguration, updateSavedConfiguration } from '../../models/configuration';
import { MountType } from '../../models/mount-type';
import { QuestPriority } from '../../models/quest-priority';
import { defaultSchedule, SavedSchedule } from '../../models/schedule';
import { SimulationConfigForm } from '../../models/simulation-configuration';
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
        this.form.schedule.setValue(this.savedSchedules[0]);
      }
    });

    this.databaseService.getAllConfigurations().subscribe(configs => {
      if (configs?.length === 1)
        this.loadFormFromConfiguration(configs[0]);
    })
  }
  @Output() configEmitter = new EventEmitter<SimulationConfigForm>();

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
    hybridRatio: new FormControl<number>({ value: 0, disabled: true }, [Validators.required, Validators.min(0), Validators.max(1)]),
    switchPriority: new FormControl(false, [Validators.required]),
    switchLevel: new FormControl<number>({ value: 0, disabled: true }, [Validators.required, Validators.min(0), Validators.max(800)]),
    priorityAfterSwitch: new FormControl<QuestPriority>({ value: 'Gold', disabled: true }, [Validators.required]),
    drinkBeerOneByOne: new FormControl(true, [Validators.required]),
    dailyThirst: new FormControl<number>(320, [Validators.required, Validators.min(0), Validators.max(320)]),
    skipCalendar: new FormControl(true, [Validators.required]),
    level: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(800)]),
    baseStat: new FormControl< number | null>(null, [Validators.required, Validators.min(0), Validators.max(10_000_000)]),
    experience: new FormControl<number | null>(null, [maxExperienceValidator(), Validators.required, Validators.min(0)]),
    goldPitLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]),
    academyLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(20)]),
    hydraHeads: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(20)]),
    gemMineLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]),
    treasuryLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(45)]),
    mountType: new FormControl<MountType>('Griffin', [Validators.required]),
    scrapbookFillness: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
    xpGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
    xpRuneBonus: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10)]),
    hasExperienceScroll: new FormControl(true),
    hasArenaGoldScroll: new FormControl(true),
    tower: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
    goldGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
    goldRuneBonus: new FormControl(50, [Validators.required, Validators.min(0), Validators.max(50)]),
    hasGoldScroll: new FormControl(true),
    spinAmount: new FormControl<SpinTactic>('Max', [Validators.required]),
    dailyGuard: new FormControl(23, [Validators.required, Validators.min(0), Validators.max(24)]),
    simulateDungeon: new FormControl(false, [Validators.required]),
    calendar: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(12)]),
    calendarDay: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(20)]),
    fightsForGold: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10000)])
  });

  get form() {
    return this.simulationOptions.controls;
  }

  public loadFormFromConfiguration(configuration: SavedConfiguration) {
    this.simulationOptions.patchValue({ ...configuration.playstyle, ...configuration.character }, { emitEvent: true });
    var schedule = this.savedSchedules.find(v => v.timestamp == configuration.scheduleId) ?? this.savedSchedules.find(v => v.timestamp == 0);

    if (schedule)
      this.form.schedule.patchValue(schedule, {emitEvent: true});

    this.savedConfiguration = configuration;
    this.snackBar.createInfoSnackbar("Loaded configuration: " + configuration.name);
  }

  public loadForm(data: any): void {
    if (data.error) {
      this.snackBar.createErrorSnackbar(data.error);
      return;
    }

    try {
      this.simulationOptions.patchValue(data, { emitEvent: true });
      this.updateConfiguration();
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
  public updateConfiguration() {
    this.databaseService.getAllConfigurations().subscribe(v => {
      if (v == undefined || this.form.characterName.value == null)
        return;

      let configuration = v.find(v => v.name == this.form.characterName.value);
      this.savedConfiguration = configuration;
    });
  }

  public saveConfiguration(includeCharacter: boolean) {
    if (!this.savedConfiguration)
      return;

    updateSavedConfiguration(this.savedConfiguration, this.simulationOptions.getRawValue(), includeCharacter);
    this.databaseService.saveConfiguration(this.savedConfiguration);
    this.snackBar.createSuccessSnackBar("Configuration saved successfully");
  }

  public saveConfigurationAsNew(includeCharacter: boolean) {
    this.dialog.open(SaveNewConfigurationDialogComponent, { autoFocus: false, data: this.form.characterName.value }).afterClosed().subscribe(name => {
      if (!name)
        return;

      var newConfiguration = new SavedConfiguration(name, this.simulationOptions.getRawValue(), includeCharacter);
      this.savedConfiguration = newConfiguration;
      this.databaseService.saveConfiguration(newConfiguration);
      this.snackBar.createSuccessSnackBar("Configuration saved successfully");
    });
  }


  ngOnInit(): void {
    this.simulationOptions.valueChanges.subscribe(() => {
      this.toggleInputs();

      var form = this.simulationOptions.valid ? this.simulationOptions.getRawValue() : undefined;
      this.configEmitter.emit(form);
    });

    this.form.level.valueChanges.subscribe(() => this.form.experience.updateValueAndValidity())

    if (this.simulationOptions.valid)
      this.configEmitter.emit(this.simulationOptions.getRawValue());
  }

  private toggleInputs() {
    if (this.form.questPriority.value != 'Hybrid' && this.form.switchPriority.value == false)
      this.form.hybridRatio.disable({emitEvent: false})
    else if (this.form.questPriority.value != 'Hybrid' && this.simulationOptions.controls.priorityAfterSwitch.value != 'Hybrid')
      this.form.hybridRatio.disable({ emitEvent: false })
    else
      this.form.hybridRatio.enable({ emitEvent: false })

    if (this.form.switchPriority.value == true) {
      this.form.switchLevel.enable({ emitEvent: false })
      this.form.priorityAfterSwitch.enable({ emitEvent: false })
    }
    else {
      this.form.switchLevel.disable({ emitEvent: false })
      this.form.priorityAfterSwitch.disable({ emitEvent: false })
    }
  }
}
