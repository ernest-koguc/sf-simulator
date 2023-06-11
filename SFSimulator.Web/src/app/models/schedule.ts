export type SavedSchedule = {
  timestamp: number;
  name: string;
  scheduleWeeks: ScheduleWeek[];
}

export type ScheduleWeek = {
  scheduleDays: ScheduleDay[];
}
export type ScheduleDay = {
  actions: Action[];
  events: Event[];
};


export type Action = 'BuyGriffin' | 'BuyTiger' | 'SpinMaxTimes' | 'SpinOnce' | 'DrinkAllBeers' | 'DrinkOneBeer' | 'UpgradeTreasury' | 'UpgradeAcademy' | 'UpgradeGoldPit';
export type ActionCell = { action: Action, tooltip: string, toggled: boolean, group?: number };
export const Actions: ActionCell[] =
  [
    { action: 'BuyGriffin', tooltip: 'Change mount to griffin', toggled: false, group: 1 },
    { action: 'BuyTiger', tooltip: 'Change mount to tiger', toggled: false, group: 1 },
    { action: 'SpinMaxTimes', tooltip: 'Spin max times', toggled: false, group: 2 },
    { action: 'SpinOnce', tooltip: 'Spin only for free', toggled: false, group: 2 },
    { action: 'DrinkAllBeers', tooltip: 'Drink all beers', toggled: false, group: 3 },
    { action: 'DrinkOneBeer', tooltip: 'Drink only free beers', toggled: false, group: 3 },
    { action: 'UpgradeTreasury', tooltip: 'Upgrade treasury level by one', toggled: false },
    { action: 'UpgradeAcademy', tooltip: 'Upgrade academy level by one', toggled: false },
    { action: 'UpgradeGoldPit', tooltip: 'Upgrade gold pit level by one', toggled: false }
  ]

export type Event = 'Experience' | 'Gold' | 'Witch' | 'LuckyDay';
export type EventCell = { event: Event, tooltip: string, toggled: boolean };
export const Events: EventCell[] =
  [
    { event: 'Experience', tooltip: "Exceptional XP Event", toggled: false },
    { event: 'Gold', tooltip: 'Glorious Gold Galore', toggled: false },
    { event: 'Witch', tooltip: "Witches' Dance", toggled: false },
    { event: 'LuckyDay', tooltip: 'Lucky Day', toggled: false }
  ];
