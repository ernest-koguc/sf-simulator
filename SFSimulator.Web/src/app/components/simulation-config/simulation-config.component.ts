import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ConfigurationDialogComponent } from '../../dialogs/configuration-dialog/configuration-dialog.component';
import { SaveNewConfigurationDialogComponent } from '../../dialogs/save-new-configuration-dialog/save-new-configuration-dialog.component';
import { maxExperienceValidator } from '../../helpers/validators';
import { SavedConfiguration, updateSavedConfiguration } from '../../models/configuration';
import { MountType } from '../../models/mount-type';
import { QuestPriority } from '../../models/quest-priority';
import { defaultSchedule, SavedSchedule } from '../../models/schedule';
import { SimulationConfigForm } from '../../models/simulation-configuration';
import { SpinTactic } from '../../models/spin-tactics';
import { DataBaseService } from '../../services/database.service';
import { SnackbarService } from '../../services/snackbar.service';


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
        this.form.schedule.setValue(this.savedSchedules[0]);
      }
    });

    this.databaseService.getAllConfigurations().subscribe(configs => {
      if (configs?.length === 1)
        this.loadFormFromConfiguration(configs[0]);
    })
  }
  @Output() configEmitter = new EventEmitter<SimulationConfigForm>();

  public questPriority = QuestPriority;
  public mountType = MountType;
  public spinTactic = SpinTactic;

  public savedSchedules!: SavedSchedule[];
  public defaultScheduleOption: SavedSchedule = defaultSchedule;
  public savedConfiguration?: SavedConfiguration;

  public get expeditionsEnabled(): boolean {
    return this.form.expeditionsInsteadOfQuests.value!;
  }

  simulationOptions = new FormGroup({
    characterName: new FormControl(''),
    schedule: new FormControl<SavedSchedule>(this.defaultScheduleOption, [Validators.required]),
    questPriority: new FormControl<QuestPriority>(QuestPriority.Experience, [Validators.required]),
    hybridRatio: new FormControl<number>( 0, [Validators.required, Validators.min(0), Validators.max(1)]),
    hybridRatioAfterSwitch: new FormControl<number>( 0, [Validators.required, Validators.min(0), Validators.max(1)]),
    switchPriority: new FormControl(false, [Validators.required]),
    switchLevel: new FormControl<number>(0, [Validators.required, Validators.min(0), Validators.max(800)]),
    priorityAfterSwitch: new FormControl<QuestPriority>(QuestPriority.Gold, [Validators.required]),
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
    mountType: new FormControl<MountType | null>(MountType.Griffin, [Validators.required]),
    scrapbookFillness: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
    xpGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
    xpRuneBonus: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10)]),
    hasExperienceScroll: new FormControl(true),
    hasArenaGoldScroll: new FormControl(true),
    tower: new FormControl(100, [Validators.required, Validators.min(0), Validators.max(100)]),
    goldGuildBonus: new FormControl(200, [Validators.required, Validators.min(0), Validators.max(200)]),
    goldRuneBonus: new FormControl(50, [Validators.required, Validators.min(0), Validators.max(50)]),
    hasGoldScroll: new FormControl(true),
    spinAmount: new FormControl<SpinTactic>(SpinTactic.Max, [Validators.required]),
    dailyGuard: new FormControl(23, [Validators.required, Validators.min(0), Validators.max(24)]),
    simulateDungeon: new FormControl(false, [Validators.required]),
    calendar: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(12)]),
    calendarDay: new FormControl(1, [Validators.required, Validators.min(1), Validators.max(20)]),
    fightsForGold: new FormControl(10, [Validators.required, Validators.min(0), Validators.max(10000)]),
    doWeeklyTasks: new FormControl(true, [Validators.required]),
    drinkExtraWeeklyBeer: new FormControl(true, [Validators.required]),
    expeditionsInsteadOfQuests: new FormControl(true, [Validators.required]),
    expeditionOptions: this.formBuilder.group({
      averageAmountOfChests: [1.5, [Validators.required, Validators.min(0), Validators.max(2)]],
      averageStarExperienceBonus: [1.2, [Validators.required, Validators.min(1), Validators.max(1.35)]],
    }),
    expeditionOptionsAfterSwitch: this.formBuilder.group({
      averageAmountOfChests: [1.5, [Validators.required, Validators.min(0), Validators.max(2)]],
      averageStarExperienceBonus: [1.2, [Validators.required, Validators.min(1), Validators.max(1.35)]],
    }),
  });


  get form() {
    return this.simulationOptions.controls;
  }

  get expeditionOptions() {

    return this.form.expeditionOptions.controls;
  }

  get expeditionOptionsAfterSwitch() {
    return this.form.expeditionOptionsAfterSwitch.controls;
  }

  public loadFormFromConfiguration(configuration: SavedConfiguration) {
    let config = this.normalizeConfiguration(configuration);

    this.simulationOptions.patchValue({ ...config.playstyle, ...config.character }, { emitEvent: true });
    this.simulationOptions.updateValueAndValidity();
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
    this.toggleInputs();
    this.simulationOptions.valueChanges.subscribe(() => {
      this.toggleInputs();

      var form = this.simulationOptions.valid ? this.simulationOptions.getRawValue() : undefined;
      this.configEmitter.emit(form);
    });

    this.form.level.valueChanges.subscribe(() => this.form.experience.updateValueAndValidity())

    this.form.doWeeklyTasks.valueChanges.subscribe(v => {
      if (v === false)
        this.form.drinkExtraWeeklyBeer.disable();
      else
        this.form.drinkExtraWeeklyBeer.enable();
    });

    if (this.simulationOptions.valid)
      this.configEmitter.emit(this.simulationOptions.getRawValue());
  }

  private toggleInputs() {
    if (this.form.questPriority.value != QuestPriority.Hybrid)
      this.form.hybridRatio.disable({emitEvent: false})
    else
      this.form.hybridRatio.enable({ emitEvent: false })

    if (this.form.priorityAfterSwitch.value != QuestPriority.Hybrid || this.form.switchPriority.value == false)
      this.form.hybridRatioAfterSwitch.disable({emitEvent: false})
    else
      this.form.hybridRatioAfterSwitch.enable({ emitEvent: false })

    if (this.form.switchPriority.value == true) {
      this.form.switchLevel.enable({ emitEvent: false })
      this.form.priorityAfterSwitch.enable({ emitEvent: false })
      this.expeditionOptionsAfterSwitch.averageAmountOfChests.enable({ emitEvent: false })
      this.expeditionOptionsAfterSwitch.averageStarExperienceBonus.enable({ emitEvent: false })
    }
    else {
      this.form.switchLevel.disable({ emitEvent: false })
      this.form.priorityAfterSwitch.disable({ emitEvent: false })
      this.expeditionOptionsAfterSwitch.averageAmountOfChests.disable({ emitEvent: false })
      this.expeditionOptionsAfterSwitch.averageStarExperienceBonus.disable({ emitEvent: false })
    }
  }

  private normalizeConfiguration(configuration: SavedConfiguration) {
    let spinTactic = configuration.playstyle.spinAmount;
    if (spinTactic === null || isNaN(Number(spinTactic)))
      configuration.playstyle.spinAmount = SpinTactic.Max;

    let questPriority = configuration.playstyle.questPriority;
    if (questPriority === null || isNaN(Number(questPriority)))
      configuration.playstyle.questPriority = QuestPriority.Experience;

    let priorityAfterSwitch = configuration.playstyle.priorityAfterSwitch;
    if (priorityAfterSwitch === null || isNaN(Number(priorityAfterSwitch)))
      configuration.playstyle.priorityAfterSwitch = QuestPriority.Gold;

    if (configuration.character !== undefined) {
      let mount = configuration.character.mountType;
      if (mount === null || isNaN(Number(mount)))
        configuration.character.mountType = MountType.Griffin;
      }
    return configuration;
  }
}
