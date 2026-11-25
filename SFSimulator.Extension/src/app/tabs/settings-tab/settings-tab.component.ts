import { Component } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideConstruction } from '@ng-icons/lucide';
import { HlmAlertImports } from '@spartan-ng/helm/alert';

@Component({
  imports: [HlmAlertImports, NgIcon],
  providers: [provideIcons({ lucideConstruction })],
  selector: 'settings-tab',
  styleUrl: './settings-tab.component.css',
  templateUrl: './settings-tab.component.html',
})
export class SettingsTabComponent {
}
