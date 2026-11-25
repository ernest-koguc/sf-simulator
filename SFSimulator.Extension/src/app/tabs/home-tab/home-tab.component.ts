import { Component } from '@angular/core';
import { HlmButtonImports } from '@spartan-ng/helm/button';
import { HlmTypographyImports } from '@spartan-ng/helm/typography';

@Component({
  imports: [HlmTypographyImports, HlmButtonImports],
  selector: 'home-tab',
  styleUrl: './home-tab.component.css',
  templateUrl: './home-tab.component.html',
})
export class HomeTabComponent {
}
