import { HttpClientModule } from '@angular/common/http';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ReactiveFormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { MatSliderModule } from '@angular/material/slider';
import { MatTabsModule } from '@angular/material/tabs';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { MatCardModule } from '@angular/material/card';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatIconModule } from '@angular/material/icon';
import { MatRippleModule } from '@angular/material/core';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatDialogModule } from '@angular/material/dialog';
import { MatRadioModule } from '@angular/material/radio';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatMenuModule } from '@angular/material/menu';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { FormsModule } from '@angular/forms';
import { SimulationConfig } from './components/simulation-config/simulation-config.component';
import { SFSimulatorComponent } from './sfsimulator.component';
import { SimulationResultChartsComponent } from './components/simulation-result-charts/simulation-result-charts.component';
import { CharacterDetailsComponent } from './components/character-details/character-details.component';
import { InfoDialogComponent } from './dialogs/info-dialog/info-dialog.component';
import { ToolBarComponent } from './layout/tool-bar/tool-bar.component';
import { SimulationOptionsDialogComponent } from './dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { ProgressBarComponent } from './dialogs/progress-bar-dialog/progress-bar.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CookieService } from 'ngx-cookie-service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SnackbarComponent } from './snackbars/snackbar/snackbar.component';
import { AppRoutingModule } from './app-routing.module';
import { SimulatorComponent } from './layout/simulator/simulator.component';
import { SimulationResultComponent } from './layout/simulation-result/simulation-result.component';
import { RemoveRecordDialogComponent } from './dialogs/remove-record-dialog/remove-record-dialog.component';
import { TestComponent } from './layout/test/test.component';
import { TableModule } from 'primeng/table';
import { ButtonModule } from 'primeng/button';
import { PatchNotesDialogComponent } from './dialogs/patch-notes-dialog/patch-notes-dialog.component';
import { MatContextMenuTrigger } from './directives/mat-context-menu-trigger-for.directive';
import { ButtonToggleComponent } from './components/button-toggle/button-toggle.component';
import { CustomScheduleComponent } from './layout/custom-schedule/custom-schedule.component';
import { CustomScheduleInfoDialogComponent } from './dialogs/custom-schedule-info-dialog/custom-schedule-info-dialog.component';
import { ConfigurationDialogComponent } from './dialogs/configuration-dialog/configuration-dialog.component';
import { SaveNewConfigurationDialogComponent } from './dialogs/save-new-configuration-dialog/save-new-configuration-dialog.component';
import { CreditsDialogComponent } from './dialogs/credits-dialog/credits-dialog.component';

@NgModule({
  declarations: [
    SimulationConfig,
    SFSimulatorComponent,
    SimulationResultChartsComponent,
    CharacterDetailsComponent,
    InfoDialogComponent,
    SimulationOptionsDialogComponent,
    ToolBarComponent,
    ProgressBarComponent,
    SnackbarComponent,
    SimulatorComponent,
    SimulationResultComponent,
    RemoveRecordDialogComponent,
    TestComponent,
    PatchNotesDialogComponent,
    MatContextMenuTrigger,
    ButtonToggleComponent,
    CustomScheduleComponent,
    CustomScheduleInfoDialogComponent,
    ConfigurationDialogComponent,
    SaveNewConfigurationDialogComponent,
    CreditsDialogComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgChartsModule,
    BrowserAnimationsModule,
    FormsModule,
    TableModule,
    ButtonModule,
    MatSliderModule,
    MatSlideToggleModule,
    MatTabsModule,
    MatCardModule,
    MatProgressBarModule,
    MatIconModule,
    MatRippleModule,
    MatTooltipModule,
    MatRadioModule,
    MatToolbarModule,
    MatButtonModule,
    MatDialogModule,
    MatInputModule,
    MatSelectModule,
    MatProgressSpinnerModule,
    MatSnackBarModule,
    MatButtonToggleModule,
    MatMenuModule,
    MatAutocompleteModule
  ],
  providers: [CookieService, { provide: Window, useValue: window }],
  bootstrap: [SFSimulatorComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class SFSimulatorModule { }
