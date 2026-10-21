import { Component, ElementRef, inject, signal, ChangeDetectionStrategy } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { lucideChevronDown, lucideChevronRight } from '@ng-icons/lucide';

@Component({
  selector: 'tab-btn',
  imports: [NgIcon],
  templateUrl: './tab-btn.component.html',
  styleUrl: './tab-btn.component.css',
  changeDetection: ChangeDetectionStrategy.Eager,
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
