import { Component,  OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfigurationDialogComponent } from '../../dialogs/configuration-dialog/configuration-dialog.component';
import { SaveNewConfigurationDialogComponent } from '../../dialogs/save-new-configuration-dialog/save-new-configuration-dialog.component';
import { booleanValidator, maxExperienceValidator, numberValidator } from '../../helpers/validators';
import { SavedConfiguration, updateSavedConfiguration } from '../../models/configuration';
import { MountType } from '../../models/mount-type';
import { QuestPriority } from '../../models/quest-priority';
import { defaultSchedule, SavedSchedule } from '../../models/schedule';
import { SpinTactic } from '../../models/spin-tactics';
import { DataBaseService } from '../../services/database.service';
import { SnackbarService } from '../../services/snackbar.service';
import { FlatSimulationConfig, SimulationConfigForm } from 'src/app/models/simulation-configuration';
import typia from 'typia';


@Component({
  selector: 'simulation-config',
  templateUrl: './simulation-config.component.html',
  styleUrls: ['./simulation-config.component.scss']
})
export class SimulationConfig implements OnInit {

  constructor(private snackBar: SnackbarService, private databaseService: DataBaseService, private dialog: MatDialog,
  private formBuilder: FormBuilder)
  {
    this.databaseService.getAllSchedules().subscribe(v => {
      var defaultConfig: SavedSchedule =  defaultSchedule;
      this.savedSchedules = [defaultConfig];
      if (v) {
        this.savedSchedules.push(...v);
        this.playstyle.schedule.setValue(this.savedSchedules[0]);
      }
    });

    this.databaseService.getAllConfigurations().subscribe(configs => {
      if (configs?.length === 1)
        this.loadConfiguration(configs[0]);
    })
  }

  public getSimulationOptions() {
    this.simulationOptions.markAllAsTouched();
    if (this.simulationOptions.valid) {
      let form = this.simulationOptions.getRawValue();
      if (form.playstyle.expeditionsInsteadOfQuests === true) {
        form.playstyle.questOptions = undefined as any;
        form.playstyle.questOptionsAfterSwitch = undefined as any;
      }
      else {
        form.playstyle.expeditionOptions = undefined as any;
        form.playstyle.expeditionOptionsAfterSwitch = undefined as any;
      }

      if (form.playstyle.switchPriority === false) {
        form.playstyle.questOptionsAfterSwitch = undefined as any;
        form.playstyle.expeditionOptionsAfterSwitch = undefined as any;
      }
      return form;
    }

    let tabsState = [this.accountInvalid, this.bonusesInvalid, this.playstyleInvalid];
    if (tabsState.filter(v => v).length == 1 || !tabsState[this.selectedIndex]) {
      this.selectedIndex = tabsState.indexOf(true);
    }

    return;
  }

  public selectedIndex!: number;
  public questPriority = QuestPriority;
  public mountTypeEnum = MountType;
  public spinTactic = SpinTactic;

  public savedSchedules!: SavedSchedule[];
  public defaultScheduleOption: SavedSchedule = defaultSchedule;
  public savedConfiguration?: SavedConfiguration;

  public get expeditionsEnabled(): boolean {
    return this.playstyle.expeditionsInsteadOfQuests.value!;
  }

  // TODO: Change form saving to just include everything in the form and fuck the consequences

  simulationOptions = new FormGroup({

    // Account Tab
    account: this.formBuilder.group({
      characterName: new FormControl(''),
      level: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(800)]),
      baseStat: new FormControl< number | null>(null, [Validators.required, Validators.min(0), Validators.max(10_000_000)]),
      experience: new FormControl<number | null>(null, [maxExperienceValidator(), Validators.required, Validators.min(0)]),
      goldPitLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]),
      academyLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(20)]),
      hydraHeads: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(20)]),
      gemMineLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(100)]),
      treasuryLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(45)]),
      mountType: new FormControl<MountType | null>(null, [Validators.required, numberValidator()])
    }),

    // Bonuses Tab
    bonuses: this.formBuilder.group({
      scrapbookFillness: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
      xpGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
      xpRuneBonus: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10)]),
      hasExperienceScroll: new FormControl<boolean>(true, { nonNullable: true, validators: [booleanValidator()]}),
      tower: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
      goldGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
      goldRuneBonus: new FormControl(50, [Validators.required, Validators.min(0), Validators.max(50)]),
      hasGoldScroll: new FormControl<boolean>(true, { nonNullable: true, validators: [booleanValidator()]}),
      hasArenaGoldScroll: new FormControl<boolean>(true, { nonNullable: true, validators: [booleanValidator()]}),
    }),


    // Playstyle Tab
    playstyle: this.formBuilder.group({
      drinkExtraWeeklyBeer: new FormControl(true, { nonNullable: true, validators: [Validators.required]}),
      drinkBeerOneByOne: new FormControl(true, { nonNullable: true, validators:[Validators.required]}),
      skipCalendar: new FormControl(true, { nonNullable: true, validators: [Validators.required]}),
      doWeeklyTasks: new FormControl(true, { nonNullable: true, validators: [Validators.required]}),
      simulateDungeon: new FormControl(false, { nonNullable: true, validators: [Validators.required]}),

      spinAmount: new FormControl<SpinTactic>(SpinTactic.Max, [Validators.required, numberValidator()]),
      dailyGuard: new FormControl(23, [Validators.required, Validators.min(0), Validators.max(24)]),
      calendar: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(12)]),
      calendarDay: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(20)]),
      fightsForGold: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10000)]),
      schedule: new FormControl<SavedSchedule>(this.defaultScheduleOption, [Validators.required]),

      dailyThirst: new FormControl<number>(320, [Validators.required, Validators.min(0), Validators.max(320)]),
      switchPriority: new FormControl<boolean>(false, { nonNullable: true, validators: [Validators.required] }),
      switchLevel: new FormControl<number>(0, [Validators.required, Validators.min(0), Validators.max(800)]),
      expeditionsInsteadOfQuests: new FormControl<boolean>(true, { nonNullable: true, validators: [Validators.required] }),
      expeditionOptions: this.formBuilder.group({
        averageAmountOfChests: [1.5, [Validators.required, Validators.min(0), Validators.max(2)]],
        averageStarExperienceBonus: [1.2, [Validators.required, Validators.min(1), Validators.max(1.35)]],
      }),
      expeditionOptionsAfterSwitch: this.formBuilder.group({
        averageAmountOfChests: [1.5, [Validators.required, Validators.min(0), Validators.max(2)]],
        averageStarExperienceBonus: [1.2, [Validators.required, Validators.min(1), Validators.max(1.35)]],
      }),
      questOptions: this.formBuilder.group({
        priority: [QuestPriority.Experience, [Validators.required, numberValidator()]],
        hybridRatio: [0, [Validators.required, Validators.min(0), Validators.max(1)]],
      }),
      questOptionsAfterSwitch: this.formBuilder.group({
        priority: [QuestPriority.Experience, [Validators.required, numberValidator()]],
        hybridRatio: [0, [Validators.required, Validators.min(0), Validators.max(1)]],
      }),
    }),
  });


  get form() {
    return this.simulationOptions.controls;
  }

  get account() {
    return this.form.account.controls;
  }

  get bonuses() {
    return this.form.bonuses.controls;
  }

  get playstyle() {
    return this.form.playstyle.controls;
  }

  get expeditionOptions() {

    return this.playstyle.expeditionOptions.controls;
  }

  get expeditionOptionsAfterSwitch() {
    return this.playstyle.expeditionOptionsAfterSwitch.controls;
  }

  get questOptions() {
    return this.playstyle.questOptions.controls;
  }

  get questOptionsAfterSwitch() {
    return this.playstyle.questOptionsAfterSwitch.controls;
  }

  get accountInvalid() {
    return this.hasInvalidForms(this.form.account);
  }

  get bonusesInvalid() {
    return this.hasInvalidForms(this.form.bonuses);
  }

  get playstyleInvalid() {
    return this.hasInvalidForms(this.form.playstyle);
  }

  public loadEndpoint(data: FlatSimulationConfig) {
    if (data === undefined || data === null) {
      console.error("Data is in unexpected format!");
      return;
    }

    let form = data as any;
    let validation = typia.validate<DeepPartial<FlatSimulationConfig>>(form);
    validation.errors.forEach((error) => {
      let start = '$input.'.length;
      let path = error.path.slice(start, error.path.length).split('.');
      let last = path.pop()!;
      delete path.reduce((o, k) => o[k] || {}, form)[last];
    });

    let paths = this.getControlsPath(this.simulationOptions);

    for (let path of paths) {
      let lastProp = path.split('.').pop();
      if (lastProp !== undefined && Object.keys(form).includes(lastProp)) {
        this.simulationOptions.get(path)?.patchValue(form[lastProp], { emitEvent: true });
      }
    }

    this.savedConfiguration = undefined;
    setTimeout(() => this.snackBar.createSuccessSnackBar("Successfully logged in!"), 100);
  }

  public loadConfiguration(configuration: SavedConfiguration) {
    let form = configuration.form as any;
    let validation = typia.validate<DeepPartial<SimulationConfigForm>>(form);

    if (form === undefined) {
      let errorMessage = 'Data is in unexpected format, most likely saved configuration is outdated!';
      console.error(errorMessage);
      this.snackBar.createErrorSnackbar(errorMessage);
      return;
    }

    validation.errors.forEach((error) => {
      let start = '$input.'.length;
      let path = error.path.slice(start, error.path.length).split('.');
      let last = path.pop()!;
      delete path.reduce((o, k) => o[k] || {}, form)[last];
    });

    this.simulationOptions.patchValue({ ...form }, { emitEvent: true });
    var schedule = this.savedSchedules.find(v => v.timestamp == configuration.scheduleId) ?? this.savedSchedules.find(v => v.timestamp == 0);

    if (schedule)
      this.playstyle.schedule.patchValue(schedule, {emitEvent: true});

    this.savedConfiguration = configuration;
    this.snackBar.createInfoSnackbar("Loaded configuration: " + configuration.name);
    this.resetInvalidForms(this.simulationOptions);
  }

  public openConfigurationDialog() {
    this.dialog.open(ConfigurationDialogComponent, { autoFocus: false }).afterClosed().subscribe(configuration => {
      if (!configuration)
        return;

      this.loadConfiguration(configuration);
    });
  }

  public saveConfiguration() {
    if (!this.savedConfiguration)
      return;

    updateSavedConfiguration(this.savedConfiguration, this.simulationOptions.getRawValue());
    this.databaseService.saveConfiguration(this.savedConfiguration);
    this.snackBar.createSuccessSnackBar("Configuration saved successfully");
  }

  public saveConfigurationAsNew() {
    this.dialog.open(SaveNewConfigurationDialogComponent, { autoFocus: false, data: this.account.characterName.value }).afterClosed().subscribe(name => {
      if (!name)
        return;

      var newConfiguration = new SavedConfiguration(name, this.simulationOptions.getRawValue());
      this.savedConfiguration = newConfiguration;
      this.databaseService.saveConfiguration(newConfiguration);
      this.snackBar.createSuccessSnackBar("Configuration saved successfully");
    });
  }

  ngOnInit(): void {
    this.toggleInputs();
    this.simulationOptions.valueChanges.subscribe(() => {
      this.toggleInputs();

    });

    this.account.level.valueChanges.subscribe(() => this.account.experience.updateValueAndValidity())

    this.playstyle.doWeeklyTasks.valueChanges.subscribe(v => {
      if (v === false) {

        this.playstyle.drinkExtraWeeklyBeer.disable();
        this.playstyle.drinkExtraWeeklyBeer.setValue(false);
      }
      else
        this.playstyle.drinkExtraWeeklyBeer.enable();
    });
  }

  private toggleInputs() {
    let switchPriority = this.playstyle.switchPriority.value === true;
    this.toggleControl(this.playstyle.switchLevel, switchPriority);
    this.toggleControl(this.questOptionsAfterSwitch.priority, switchPriority);
    this.toggleControl(this.playstyle.expeditionOptionsAfterSwitch, switchPriority);

    let enableExpeditions = this.expeditionsEnabled;
    this.toggleControl(this.playstyle.expeditionOptions, enableExpeditions);
    this.toggleControl(this.playstyle.questOptions, !enableExpeditions);

    let toggleHybridRatio = this.questOptions.priority.value === QuestPriority.Hybrid;
    this.toggleControl(this.questOptions.hybridRatio, toggleHybridRatio);

    let toggleHybridRatioAfterSwitch = this.questOptionsAfterSwitch.priority.value === QuestPriority.Hybrid && this.playstyle.switchPriority.value == true;
    this.toggleControl(this.questOptionsAfterSwitch.hybridRatio, toggleHybridRatioAfterSwitch);
  }

  private toggleControl(control: AbstractControl, enable: boolean) {
    if (enable)
      control.enable({ emitEvent: false });
    else
      control.disable({ emitEvent: false });
  }

  private resetInvalidForms(formGroup: FormGroup) {
    for (var controlName in formGroup.controls)
    {
      let control = formGroup.get(controlName);
      if (control instanceof FormGroup) {
        this.resetInvalidForms(control);
        continue;
      }
      if (control?.invalid === true)
        control?.reset(undefined, { emitEvent: true });
    }
  }

  private hasInvalidForms(formGroup: FormGroup): boolean {
    for (var controlName in formGroup.controls) {
      let control = formGroup.get(controlName);
      if (control instanceof FormGroup) {
        if (this.hasInvalidForms(control))
          return true;
        continue;
      }
      if (control?.invalid && control?.touched)
        return true;
    }
    return false;
  }

  private getControlsPath(formGroup: FormGroup, path?: string): string[] {
    let paths: string[] = [];
    let currPath = path !== undefined ? path + '.' : '';
    for (let controlName in formGroup.controls) {
      let control = formGroup.get(controlName);
      if (control instanceof FormGroup) {
        paths.push(...this.getControlsPath(control, currPath + controlName));
      }
      else if (control !== null) {
        paths.push(currPath + controlName)
      }
    }

    return paths;
  }
}

export type DeepPartial<T> = T extends object ? {
    [P in keyof T]?: DeepPartial<T[P]>;
} : T;
