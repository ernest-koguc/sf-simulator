import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TooltipPosition } from '@angular/material/tooltip';
import { MountType } from '../../dto/mount-type';
import { QuestPriority } from '../../dto/quest-priority';
import { SimulationOptionsForm } from '../../dto/simulation-options';


@Component({
  selector: 'simulation-config',
  templateUrl: './simulation-config.component.html',
  styleUrls: ['./simulation-config.component.scss']
})
export class SimulationConfig implements OnInit {

  constructor() { }

  @Output() configEmitter = new EventEmitter<SimulationOptionsForm>();

  public questPriority = ['Gold', 'Experience', 'Hybrid'];
  public mountType = ['None', 'Pig', 'Horse', 'Tiger', 'Griffin'];

  simulationOptions = new FormGroup({
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
      hasGoldScroll: new FormControl(true)
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

  ngOnInit(): void {
    this.simulationOptions.valueChanges.subscribe(_ => {
      this.toggleInputs();

      if (this.simulationOptions.valid)
        this.configEmitter.emit(this.simulationOptions.getRawValue())
      else
        this.configEmitter.emit(undefined);
    });

    if (this.simulationOptions.valid)
      this.configEmitter.emit(this.simulationOptions.getRawValue());
  }
  loadForm(formData: any) {
    this.simulationOptions.patchValue(formData, { emitEvent: true });
  }

  public position: TooltipPosition = "right";
  public goldPitTooltip: string = "Goldpit lvl is raised by 1 every 15 days";
}
