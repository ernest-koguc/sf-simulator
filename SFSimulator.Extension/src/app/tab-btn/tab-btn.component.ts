import { Component, ElementRef, inject, signal } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronDown, lucideChevronRight } from '@ng-icons/lucide';
import { HlmIconDirective } from '@spartan-ng/helm/icon';

@Component({
  selector: 'tab-btn',
  imports: [NgIcon, HlmIconDirective],
  templateUrl: './tab-btn.component.html',
  styleUrl: './tab-btn.component.css',
  providers: [provideIcons({ lucideChevronRight, lucideChevronDown })],
})
export class TabBtnComponent {
  private host = inject(ElementRef);
  isHovered = signal(false);

  constructor() {
    this.host.nativeElement.addEventListener('mouseenter', () => this.isHovered.set(true));
    this.host.nativeElement.addEventListener('mouseleave', () => this.isHovered.set(false));
  }
}
