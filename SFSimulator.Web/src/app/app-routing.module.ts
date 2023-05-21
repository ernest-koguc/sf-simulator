import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { SimulationResultComponent } from './layout/simulation-result/simulation-result.component';
import { SimulatorComponent } from './layout/simulator/simulator.component';


const routes: Routes = [
  { path: 'results', component: SimulationResultComponent },
  { path: '**', component: SimulatorComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
