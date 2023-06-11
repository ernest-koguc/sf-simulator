import { Directive, HostListener, Input } from "@angular/core";
import { MatMenuPanel, _MatMenuTriggerBase } from "@angular/material/menu";
import { fromEvent, merge } from "rxjs";

@Directive({
  selector: `[matContextMenuTriggerFor]`,
  host: {
    'class': 'mat-menu-trigger',
  },
  exportAs: 'matContextMenuTrigger',
})
export class MatContextMenuTrigger extends _MatMenuTriggerBase {

  @Input('matContextMenuTriggerFor')
  get menu_again() {
    return this.menu!;
  }
  set menu_again(menu: MatMenuPanel) {
    this.menu = menu;
  }

  @Input('matMenuTriggerFor')
  set ignoredMenu(value: any) { }

  override _handleMousedown(event: MouseEvent): void {
    return super._handleMousedown(new MouseEvent(event.type, Object.assign({}, event, { button: event.button === 0 ? 2 : event.button === 2 ? 0 : event.button })));
  }

  override _handleClick(event: MouseEvent): void { }

  private hostElement: EventTarget | null = null;

  @HostListener('contextmenu', ['$event'])
  _handleContextMenu(event: MouseEvent): void {
    this.hostElement = event.target;
    if (event.shiftKey) return;
    event.preventDefault();
    super._handleClick(event);
  }

  private contextListenerSub = merge(
    fromEvent(document, "contextmenu"),
    fromEvent(document, "click"),
  ).subscribe(event => {
    if (this.menuOpen) {
      if (event.target) {
        const target = event.target as HTMLElement;
        if (target.classList.contains("cdk-overlay-backdrop")) {
          event.preventDefault();
          this.closeMenu();
        } else {
          let inOverlay = false;
          document.querySelectorAll(".cdk-overlay-container").forEach(e => {
            if (e.contains(target))
              inOverlay = true;
          });
          if (inOverlay) {
            if (event.type === "contextmenu") {
              event.preventDefault();
              event.target?.dispatchEvent(new MouseEvent("click", event));
            }
          } else
            if (target !== this.hostElement)
              this.closeMenu();
        }
      }
    }
  });

  override ngOnDestroy() {
    this.contextListenerSub.unsubscribe();
    this.hostElement = null;
    return super.ngOnDestroy();
  }
}
