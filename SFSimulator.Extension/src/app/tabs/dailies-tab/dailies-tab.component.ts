import { Component, computed, inject } from '@angular/core';
import { SessionManager } from '../../services/SessionManager';
import { EventType } from '../../sfgame/SFGameModels';
import { HlmTableImports } from '@spartan-ng/helm/table';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { HlmIconDirective } from '@spartan-ng/helm/icon';
import { lucideCircleCheck, lucideCircleMinus } from '@ng-icons/lucide';

@Component({
  selector: 'dailies-tab',
  imports: [HlmTableImports, HlmIconDirective, NgIcon],
  providers: [provideIcons({ lucideCircleCheck, lucideCircleMinus })],
  templateUrl: './dailies-tab.component.html',
  styleUrl: './dailies-tab.component.css'
})
export class DailiesTabComponent {
  private sessionManager = inject(SessionManager);

  public dailies = computed(() => {
    const current = this.sessionManager.current();
    const ownSave = current?.ownPlayerSave;
    const events = current?.activeEvents;
    const pets = current?.pets;
    const dailyTasksRewards = current?.dailyTasksRewards;
    const dailyTasks = current?.dailyTasks;
    const tower = current?.tower;
    const toiletState = current?.toiletState;
    const guild = current?.guild;
    const portalProgress = current?.portalProgress;
    if (!current || !ownSave || events === null || events === undefined || !dailyTasksRewards || !dailyTasks) {
      return [];
    }

    let dailies = structuredClone(this.DailyTasks);

    const maxThirst = ownSave.MaxBeers * 20 + 100;
    const usedThirst = 100 + (ownSave.UsedBeers * 20) - (ownSave.ThirstLeft / 60);

    dailies[TaskType.DoExpeditions].Completed = maxThirst == usedThirst;
    dailies[TaskType.DoExpeditions].Progress = `${usedThirst}/${maxThirst}`;

    if (ownSave.Level < 100 || !toiletState) {
      delete dailies[TaskType.SacrificeItemInToilet];
    } else {
      const maxAmountOfSacrifices = events.find(e => e.Id === EventType.TidyToiletTime) ? 2 : 1;
      const sacrificesDone = maxAmountOfSacrifices - toiletState.AmountOfItemsToSacrifice;
      dailies[TaskType.SacrificeItemInToilet].Completed = sacrificesDone === maxAmountOfSacrifices;
      dailies[TaskType.SacrificeItemInToilet].Progress = `${sacrificesDone}/${maxAmountOfSacrifices}`;
    }

    if (ownSave.Level < 75 || !pets) {
      delete dailies[TaskType.PetFights];
    } else {
      const petFights = [pets.ShadowArenaFought, pets.LightArenaFought, pets.EarthArenaFought, pets.FireArenaFought, pets.WaterArenaFought].filter(f => f).length;
      dailies[TaskType.PetFights].Completed = petFights === 5;
      dailies[TaskType.PetFights].Progress = `${petFights}/5`;
    }

    const isLuckyDayEvent = events.find(e => e.Id === EventType.LuckyDay) !== undefined;

    if (ownSave.Level <= 120) {
      delete dailies[TaskType.DiceGame];
    } else {
      const maxDice = isLuckyDayEvent ? 20 : 10;
      dailies[TaskType.DiceGame].Completed = ownSave.DiceGamesRemaining === 0;
      dailies[TaskType.DiceGame].Progress = `${maxDice - ownSave.DiceGamesRemaining}/${maxDice}`;
    }

    dailies[TaskType.ArenaFights].Completed = ownSave.ArenaFightsToday === 10;
    dailies[TaskType.ArenaFights].Progress = `${ownSave.ArenaFightsToday}/10`;

    // TODO: handle the case when Adventuromatic is being upgraded
    if (ownSave.Level < 125 || !tower) {
      delete dailies[TaskType.CollectAdventuromatic];
    } else {
      dailies[TaskType.CollectAdventuromatic].Completed = tower.Underworld.TimeMachineThirst === 0;
    }

    const maxSpins = isLuckyDayEvent ? 40 : 20;
    dailies[TaskType.SpinWheelOfFortune].Completed = ownSave.WheelSpinsToday === maxSpins;
    dailies[TaskType.SpinWheelOfFortune].Progress = `${ownSave.WheelSpinsToday}/${maxSpins}`;

    // SOLO PORTAL
    if (ownSave.Level < 99 || !portalProgress || portalProgress.Finished === 50) {
      delete dailies[TaskType.FightSoloPortal];
    } else {
      dailies[TaskType.FightSoloPortal].Completed = !portalProgress.CanAttack;
    }

    if (!guild || guild.PetMaxLevel <= 0) {
      delete dailies[TaskType.FightHydra];
    } else {
      dailies[TaskType.FightHydra].Completed = !ownSave.Group.CanAttackHydra;
    }

    // if (ownSave.Level < 99 || !guild || guild.Portal === 50) {
    //   delete dailies[TaskType.FightGuildPortal];
    // } else {
    //   dailies[TaskType.FightGuildPortal].Completed = !guild.CanAttackPortal;
    // }

    const dailyTasksPointsRequired = dailyTasksRewards.reduce((a, b) => a.PointsRequired > b.PointsRequired ? a : b).PointsRequired;
    const currentPoints = dailyTasks.filter(v => v.Completed).reduce((a, b) => a + b.Points, 0);
    const allTasksClaimed = dailyTasksRewards.every(r => r.Claimed);

    dailies[TaskType.CompleteDailyTasks].Completed = currentPoints >= dailyTasksPointsRequired && allTasksClaimed;
    dailies[TaskType.CompleteDailyTasks].Progress = `${currentPoints}/${dailyTasksPointsRequired}`;

    return dailies.filter(_ => true).sort((a, b) => (a.Completed === b.Completed) ? 0 : a.Completed ? 1 : -1);
  });

  private DailyTasks: DailyTask[] = Object.assign([], {
    [TaskType.DoExpeditions]: { Completed: false, Information: 'Complete expeditions', Progress: '' },
    [TaskType.SacrificeItemInToilet]: { Completed: false, Information: 'Sacrifice item(s) in the toilet', Progress: '' },
    [TaskType.PetFights]: { Completed: false, Information: 'Fight in pet arena', Progress: '' },
    [TaskType.DiceGame]: { Completed: false, Information: 'Play game of dice', Progress: '' },
    [TaskType.ArenaFights]: { Completed: false, Information: 'Fight in the arena', Progress: '' },
    [TaskType.CollectAdventuromatic]: { Completed: false, Information: 'Collect TFA from Adventuromatic', Progress: '' },
    [TaskType.SpinWheelOfFortune]: { Completed: false, Information: 'Spin Abawuwu Wheel of Fortune', Progress: '' },
    [TaskType.FightSoloPortal]: { Completed: false, Information: 'Fight in Solo Portal', Progress: '' },
    //[TaskType.FightGuildPortal]: { Completed: false, Information: 'Fight in Guild Portal', Progress: '' },
    [TaskType.FightHydra]: { Completed: false, Information: 'Fight Hydra', Progress: '' },
    [TaskType.CompleteDailyTasks]: { Completed: false, Information: 'Complete Daily Tasks', Progress: '' },
  });
}

interface DailyTask {
  Completed: boolean;
  Information: string;
  Progress: string;
}

enum TaskType {
  DoExpeditions,
  SacrificeItemInToilet,
  PetFights,
  DiceGame,
  ArenaFights,
  CollectAdventuromatic,
  SpinWheelOfFortune,
  FightSoloPortal,
  //FightGuildPortal,
  FightHydra,
  CompleteDailyTasks,
}

// - Dailies:
//   - Toilet
//   - Portal
//   - Adventuromatic
//   - Feed Pets (maybe? - this might be hard to implement)
