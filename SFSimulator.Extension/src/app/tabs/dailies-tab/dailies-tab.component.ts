import { Component, computed, inject, ChangeDetectionStrategy } from '@angular/core';
import { SessionManager } from '../../services/SessionManager';
import { EventType } from '../../sfgame/SFGameModels';
import { HlmTableImports } from '@spartan-ng/helm/table';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideCircleCheck, lucideCircleMinus } from '@ng-icons/lucide';

@Component({
  selector: 'dailies-tab',
  imports: [HlmTableImports, NgIcon],
  providers: [provideIcons({ lucideCircleCheck, lucideCircleMinus })],
  templateUrl: './dailies-tab.component.html',
  changeDetection: ChangeDetectionStrategy.Eager,
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
    const characterStatus = current?.characterStatus;
    if (!current || !ownSave || events === null || events === undefined || !dailyTasksRewards || !dailyTasks) {
      return [];
    }

    let dailies = structuredClone(this.DailyTasks);

    if (characterStatus) {
      const maxThirst = characterStatus.MaxBeers * 20 + 100;
      const usedThirst = 100 + (characterStatus.UsedBeers * 20) - (characterStatus.ThirstLeft / 60);
      dailies[TaskType.DoExpeditions].Completed = maxThirst === usedThirst;
      dailies[TaskType.DoExpeditions].Progress = `${usedThirst}/${maxThirst}`;
    } else {
      delete dailies[TaskType.DoExpeditions];
    }

    if (ownSave.Level < 100 || !toiletState) {
      delete dailies[TaskType.SacrificeItemInToilet];
    } else {
      const maxAmountOfSacrifices = events.includes(EventType.TidyToiletTime) ? 2 : 1;
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

    const isLuckyDayEvent = events.includes(EventType.LuckyDay);

    if (ownSave.Level <= 120 || current.diceGamesRemaining === null) {
      delete dailies[TaskType.DiceGame];
    } else {
      const maxDice = isLuckyDayEvent ? 20 : 10;
      dailies[TaskType.DiceGame].Completed = current.diceGamesRemaining === 0;
      dailies[TaskType.DiceGame].Progress = `${maxDice - current.diceGamesRemaining}/${maxDice}`;
    }

    if (current.arenaFightsToday === null) {
      delete dailies[TaskType.ArenaFights];
    } else {
      dailies[TaskType.ArenaFights].Completed = current.arenaFightsToday >= 10;
      dailies[TaskType.ArenaFights].Progress = `${current.arenaFightsToday}/10`;
    }

    // TODO: handle the case when Adventuromatic is being upgraded
    if (ownSave.Level < 125 || !tower) {
      delete dailies[TaskType.CollectAdventuromatic];
    } else {
      dailies[TaskType.CollectAdventuromatic].Completed = tower.Underworld.TimeMachineThirst === 0;
    }

    const maxSpins = isLuckyDayEvent ? 40 : 20;
    if (current.wheelSpinsToday === null) {
      delete dailies[TaskType.SpinWheelOfFortune];
    } else {
      dailies[TaskType.SpinWheelOfFortune].Completed = current.wheelSpinsToday >= maxSpins;
      dailies[TaskType.SpinWheelOfFortune].Progress = `${current.wheelSpinsToday}/${maxSpins}`;
    }

    // SOLO PORTAL
    if (ownSave.Level < 99 || !portalProgress || portalProgress.Finished === 50) {
      delete dailies[TaskType.FightSoloPortal];
    } else {
      dailies[TaskType.FightSoloPortal].Completed = !portalProgress.CanAttack;
    }

    if (!guild || guild.PetMaxLevel <= 0 || current.canAttackHydra === null) {
      delete dailies[TaskType.FightHydra];
    } else {
      dailies[TaskType.FightHydra].Completed = !current.canAttackHydra;
    }

    const dailyTasksPointsRequired = Math.max(0, ...dailyTasksRewards.map(reward => reward.PointsRequired));
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
  FightHydra,
  CompleteDailyTasks,
}

// - Dailies:
//   - Toilet
//   - Portal
//   - Adventuromatic
//   - Feed Pets (maybe? - this might be hard to implement)
