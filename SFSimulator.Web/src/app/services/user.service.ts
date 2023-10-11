import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { DataBaseService } from './database.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private databaseService: DataBaseService) { }

  public hasUserSeenPatchNotes(): boolean {
    let userData = this.databaseService.getUserData();
    return userData.lastSeenPatchNotes === environment.currentVersion; 
  }

  public updateLastSeenPatchNotes() {
    let version = environment.currentVersion;
    let userData = this.databaseService.getUserData();
    userData.lastSeenPatchNotes = version;
    this.databaseService.saveUserData(userData);
  }

  public isAdvancedModeEnabled(): boolean {
    return this.databaseService.getUserData().isAdvancedModeEnabled ?? false;
  }
  public enableAdvancedMode() {
    let userData = this.databaseService.getUserData();
    userData.isAdvancedModeEnabled = true;
    this.databaseService.saveUserData(userData);
  }
}
