import { Component } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { HlmAlertImports } from '@spartan-ng/helm/alert';
import { lucideConstruction } from '@ng-icons/lucide';

@Component({
  imports: [HlmAlertImports, NgIcon],
  providers: [provideIcons({ lucideConstruction })],
  selector: 'logs-tab',
  styleUrl: './logs-tab.component.css',
  templateUrl: './logs-tab.component.html',
})
export class LogsTabComponent {
}
