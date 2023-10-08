import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { CustomScheduleInfoDialogComponent } from '../../dialogs/custom-schedule-info-dialog/custom-schedule-info-dialog.component';
import { Action, Event, Actions, Events, ScheduleDay, SavedSchedule, ActionCell, defaultSchedule } from '../../models/schedule';
import { DataBaseService } from '../../services/database.service';
import { SnackbarService } from '../../services/snackbar.service';

@Component({
  selector: 'custom-schedule',
  templateUrl: './custom-schedule.component.html',
  styleUrls: ['./custom-schedule.component.scss']
})
export class CustomScheduleComponent {
  constructor(private dataBaseService: DataBaseService, private snackbarService: SnackbarService, private dialog: MatDialog) {
    this.syncScheduleList();
  }
  public savedSchedules!: SavedSchedule[];
  public schedule?: SavedSchedule;
  public actions = Actions;
  public events = Events;
  public saveDisabled(): boolean {
    return this.schedule?.timestamp == 0 || !this.schedule?.name;
  }

  public openInfoDialog() {
    this.dialog.open(CustomScheduleInfoDialogComponent, { autoFocus: false, enterAnimationDuration: 400 });
  }

  //Schedule
  public syncScheduleList() {
    this.dataBaseService.getAllSchedules().subscribe(v => {
      var defaultConfig: SavedSchedule = defaultSchedule;
      this.savedSchedules = [defaultConfig];
      if (v) {
        this.savedSchedules.push(...v);
      }
    });
  }

  public saveSchedule() {
    if (this.schedule)
      this.dataBaseService.saveSchedule(this.schedule);

    this.syncScheduleList();
    this.snackbarService.createSuccessSnackBar('Schedule saved!');
  }

  public changeSchedule(schedule: SavedSchedule) {
    var scheduleCopy = JSON.parse(JSON.stringify(schedule))
    this.schedule = scheduleCopy;
  }

  public createSchedule(schedule?: SavedSchedule) {
    this.schedule = new SavedSchedule('', Date.now(), schedule ? schedule.scheduleWeeks : []);
  }

  public deleteSchedule() {
    if (this.schedule?.timestamp == 0)
      return;

    if (this.schedule)
      this.dataBaseService.removeSchedule(this.schedule);

    this.syncScheduleList();
    this.schedule = undefined;
    this.snackbarService.createInfoSnackbar('Schedule deleted!');
  }

  //Weeks
  public addNewWeek(direction: 'append' | 'push') {
    if (!this.schedule)
      return;

    var newWeek = { scheduleDays: [{ actions: [], events: [] }, { actions: [], events: [] }, { actions: [], events: [] }, { actions: [], events: [] }, { actions: [], events: [] }, { actions: [], events: [] }, { actions: [], events: [] }] };
    if (direction == 'push')
      this.schedule.scheduleWeeks!.push(newWeek);
    else
      this.schedule.scheduleWeeks!.unshift(newWeek);
  }

  public removeWeek(week: any) {
    if (!this.schedule)
      return;

    this.schedule.scheduleWeeks = this.schedule.scheduleWeeks!.filter(w => w != week);
  }

  public clearAll() {
    if (!this.schedule)
      return;

    this.schedule.scheduleWeeks = [];
  }
  public setDefault() {
    if (!this.schedule)
      return;

    this.schedule = defaultSchedule;
  }

  //Actions
  public addAction(day: ScheduleDay) {
    var toggledActions = this.actions.filter(v => v.toggled);

    for (let actionInfo of toggledActions) {
      if (!day.actions.find(a => a == actionInfo.action))
        day.actions.push(actionInfo.action);
    }
    for (let actionInfo of toggledActions.filter(v => v.group)) {
      var actionsToRemove = this.actions.filter(v => v.group == actionInfo.group && v.action != actionInfo.action);
      for (let actionToRemove of actionsToRemove) {
        day.actions = day.actions.filter(v => v != actionToRemove.action);
      }
    }

    day.actions.sort();
  }

  public removeAction(e: any, day: ScheduleDay, action: Action) {
    e.preventDefault();
    if (e.which != 2)
      return;

    var index = day.actions.findIndex(a => a == action);
    if (index >= 0)
      day.actions.splice(index, 1);
  }

  public removeAllActions(day: ScheduleDay) {
    day.actions = [];
  }

  public toggleAction(actionInfo: ActionCell) {
    actionInfo.toggled = !actionInfo.toggled;

    if (!actionInfo.toggled)
      return;

    //this.actions.find(a => a.action == actionInfo.action)!.toggled = false;

    if (actionInfo.action == 'BuyGriffin') 
      this.actions.find(a => a.action == 'BuyTiger')!.toggled = false;
    
    if (actionInfo.action == 'BuyTiger')
      this.actions.find(a => a.action == 'BuyGriffin')!.toggled = false;

    if (actionInfo.action == 'SpinMaxTimes')
      this.actions.find(a => a.action == 'SpinOnce')!.toggled = false;

    if (actionInfo.action == 'SpinOnce')
      this.actions.find(a => a.action == 'SpinMaxTimes')!.toggled = false;

    if (actionInfo.action == 'DrinkAllBeers')
      this.actions.find(a => a.action == 'DrinkOneBeer')!.toggled = false;

    if (actionInfo.action == 'DrinkOneBeer')
      this.actions.find(a => a.action == 'DrinkAllBeers')!.toggled = false;
  }

  //Events
  public addEvent(day: ScheduleDay) {
    for (let eventInfo of this.events.filter(e => e.toggled)) {
      if (!day.events.find(e => e == eventInfo.event))
        day.events.push(eventInfo.event);
    }
    day.events.sort();
  }

  public removeEvent(e: any, day: ScheduleDay, event: Event) {
    e.preventDefault();
    if (e.which != 2)
      return;

    var index = day.events.findIndex(e => e == event);
    if (index >= 0)
      day.events.splice(index, 1);
  }

  public removeAllEvents(day: ScheduleDay) {
    day.events = [];
  }

  //Context Menu

  public getSquareSize(number: number) {
    var size = 1;
    while (number > size * size) {
      size++;
    }
    return size;
  }
  
  public daysOfWeek = ['Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday', 'Sunday']
}

