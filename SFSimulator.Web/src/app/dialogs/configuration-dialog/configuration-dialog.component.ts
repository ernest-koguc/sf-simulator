import { Component, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { SavedConfiguration } from '../../models/configuration';
import { DataBaseService } from '../../services/database.service';

@Component({
  selector: 'app-configuration-dialog',
  templateUrl: './configuration-dialog.component.html',
  styleUrls: ['./configuration-dialog.component.scss']
})
export class ConfigurationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<ConfigurationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: SavedConfiguration[],
    private dataBaseService: DataBaseService
  ) {
    this.syncList();
  }

  public configurationList: SavedConfiguration[] = [];
  public selectedConfig?: SavedConfiguration;

  public selectConfig(configuration: SavedConfiguration) {
    if (this.selectedConfig === configuration) {
      this.selectedConfig = undefined;
      return;
    }

    this.selectedConfig = configuration;
  }

  public deleteConfiguration(configuration: SavedConfiguration) {
    this.dataBaseService.removeConfiguration(configuration);

    if (this.selectedConfig === configuration) {
      this.selectedConfig = undefined;
    }

    this.syncList();
  }

  private syncList() {
    this.dataBaseService.getAllConfigurations().subscribe(v => {
      if (v)
        this.configurationList = v;
    });
  }
}
