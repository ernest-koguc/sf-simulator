import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, ObservableInput, retry, throwError } from 'rxjs';
import { environment } from '../../environments/environment';
import { SimulationType } from '../dialogs/simulation-options-dialog/simulation-options-dialog.component';
import { SimulationOptionsForm } from '../models/simulation-options';
import { SimulationResult } from '../models/simulation-result';
import { SnackbarService } from './snackbar.service';


@Injectable({
  providedIn: 'root'
})
export class SimulatorService {

  constructor(private httpClient: HttpClient, private snackbarService: SnackbarService) { }

  simulate(simulationType: SimulationType, simulationOptions: SimulationOptionsForm): Observable<SimulationResult>  {
    if (simulationType.simulationType == 'Days')
      return this.simulateDays(simulationOptions, simulationType.simulateUntil);

    return this.simulateLevels(simulationOptions, simulationType.simulateUntil);
  }

  simulateDays(simulationOptions: SimulationOptionsForm, days: number): Observable<SimulationResult> {

    var url = environment.apiUrl + '/api/simulateUntilDays';

    var options = {
      params: new HttpParams().set("days", days)
    };

    var result = this.httpClient.post<SimulationResult>(url, simulationOptions, options).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }
  simulateLevels(simulationOptions: SimulationOptionsForm, level: number): Observable<SimulationResult> {

    var url = environment.apiUrl + '/api/simulateUntilLevel';

    var options = {
      params: new HttpParams().set("level", level)
    };

    var result = this.httpClient.post<SimulationResult>(url, simulationOptions, options).pipe(retry(3), catchError(e => this.handleError(e, this.snackbarService)));

    return result;
  }

  private handleError(error: HttpErrorResponse, snackbarService: SnackbarService): ObservableInput<any> {
    snackbarService.createErrorSnackbar('Something bad happened with internal API. Please try again.')
    return throwError(() => new Error('Something bad happened; please try again later.'));
  }
}
