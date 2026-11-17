import { computed, Injectable, signal } from '@angular/core';

export interface WindowPosition {
  readonly x: number;
  readonly y: number;
}

interface WindowSettings {
  position: WindowPosition | null;
  opacity: number;
}

@Injectable({ providedIn: 'root' })
export class SettingsService {
  private readonly storageKey = 'sf-lab.settings';
  private readonly settings = signal<WindowSettings>(this.load());

  public readonly position = computed(() => this.settings().position);
  public readonly opacity = computed(() => this.settings().opacity);

  public initializePosition(initialPosition: WindowPosition, availableSpace: WindowPosition): void {
    if (this.position() !== null) {
      return;
    }

    this.setPosition({
      x: availableSpace.x > 0 ? initialPosition.x / availableSpace.x : 1,
      y: availableSpace.y > 0 ? initialPosition.y / availableSpace.y : 0,
    });
  }

  public setPosition(position: WindowPosition): void {
    if (!Number.isFinite(position.x) || !Number.isFinite(position.y)) {
      return;
    }

    this.save({
      ...this.settings(),
      position: {
        x: Math.min(1, Math.max(0, position.x)),
        y: Math.min(1, Math.max(0, position.y)),
      },
    });
  }

  public setOpacity(opacity: number): void {
    if (!Number.isFinite(opacity)) {
      return;
    }

    this.save({ ...this.settings(), opacity: Math.min(1, Math.max(0, opacity)) });
  }

  private load(): WindowSettings {
    const settings: WindowSettings = { position: null, opacity: 0.9 };

    try {
      const saved: Partial<WindowSettings> | null =
        JSON.parse(localStorage.getItem(this.storageKey) ?? 'null');

      if (saved?.position && Number.isFinite(saved.position.x) && Number.isFinite(saved.position.y)) {
        settings.position = {
          x: Math.min(1, Math.max(0, saved.position.x)),
          y: Math.min(1, Math.max(0, saved.position.y)),
        };
      }

      if (typeof saved?.opacity === 'number' && Number.isFinite(saved.opacity)) {
        settings.opacity = Math.min(1, Math.max(0, saved.opacity));
      }
    } catch { }

    return settings;
  }

  private save(settings: WindowSettings): void {
    this.settings.set(settings);

    try {
      localStorage.setItem(this.storageKey, JSON.stringify(settings));
    } catch { }
  }
}
