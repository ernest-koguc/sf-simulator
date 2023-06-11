import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { CustomScheduleComponent } from './layout/custom-schedule/custom-schedule.component';
import { SimulationResultComponent } from './layout/simulation-result/simulation-result.component';
import { SimulatorComponent } from './layout/simulator/simulator.component';
import { TestComponent } from './layout/test/test.component';


const routes: Routes = [
  { path: 'results', component: SimulationResultComponent },
  { path: 'schedules', component: CustomScheduleComponent },
  { path: 'test', component: TestComponent},
  { path: '**', component: SimulatorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
