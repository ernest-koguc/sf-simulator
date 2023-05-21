import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
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
import { FormsModule } from '@angular/forms';
import { SimulationConfig } from './components/simulation-config/simulation-config.component';
import { SFSimulatorComponent } from './sfsimulator.component';
import { SimulationResultChartsComponent } from './components/simulation-result-charts/simulation-result-charts.component';
import { CharacterDetailsComponent } from './components/character-details/character-details.component';
import { InfoDialogComponent } from './dialogs/info-dialog/info-dialog.component';
import { ToolBarComponent } from './layout/tool-bar/tool-bar.component';
import { SimulationOptionsDialogComponent } from './dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { ProgressBarComponent } from './dialogs/progress-bar/progress-bar.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CookieService } from 'ngx-cookie-service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { SnackbarComponent } from './snackbars/snackbar/snackbar.component';
import { AppRoutingModule } from './app-routing.module';
import { SimulatorComponent } from './layout/simulator/simulator.component';
import { SimulationResultComponent } from './layout/simulation-result/simulation-result.component';
import { MatTableModule } from '@angular/material/table';
import { MatGridListModule } from '@angular/material/grid-list';
import { RemoveRecordDialogComponent } from './dialogs/remove-record-dialog/remove-record-dialog.component';


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
    RemoveRecordDialogComponent
  ],
  imports: [
    AppRoutingModule,
    BrowserModule,
    HttpClientModule,
    ReactiveFormsModule,
    NgChartsModule,
    BrowserAnimationsModule,
    FormsModule,
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
    MatTableModule,
    MatGridListModule
  ],
  providers: [CookieService, { provide: Window, useValue: window }],
  bootstrap: [SFSimulatorComponent]
})
export class SFSimulatorModule { }
