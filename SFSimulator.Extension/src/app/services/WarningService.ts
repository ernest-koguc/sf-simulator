import { computed, inject, Injectable } from '@angular/core';
import { SessionManager } from './SessionManager';
import { ScrollType } from '../sfgame/SFGameModels';

@Injectable({
  providedIn: 'root'
})
export class WarningService {
  private sessionManager = inject(SessionManager);

  public dungeonWarnings = computed(() => {
    const current = this.sessionManager.current();
    if (!current || !current.ownPlayerSave) {
      return [];
    }
    const items = current.ownItems;
    const ownSave = current.ownPlayerSave;

    const warnings = [];

    const witch = current.witch;
    const isCritScrollUnlocked = witch?.Scrolls.includes(ScrollType.Crit) && ownSave.Level >= 66;
    const isReactionScrollUnlocked = witch?.Scrolls.includes(ScrollType.Reaction) && ownSave.Level >= 66;

    const missingScrolls = {
      Player: [] as string[],
      Bert: [] as string[],
      Mark: [] as string[],
      Kunigunde: [] as string[],
    }

    const companionsItems = current.companionItems;

    for (const key of ['Player', 'Bert', 'Mark', 'Kunigunde']) {
      const equipment =
        key == 'Player' ? items
          : key == 'Bert' ? companionsItems?.Bert
            : key == 'Mark' ? companionsItems?.Mark
              : companionsItems?.Kunigunde;

      let weapon = isCritScrollUnlocked && !equipment?.Wpn1?.HasEnchantment && !equipment?.Wpn2?.HasEnchantment
      let gloves = isReactionScrollUnlocked === true && !equipment?.Hand?.HasEnchantment;
      if (!weapon && !gloves)
        continue;

      let warning = `${key} is missing `
      if (weapon && gloves)
        warning += 'Weapon and Gloves'
      else if (weapon)
        warning += 'Weapon'
      else if (gloves)
        warning += 'Gloves'

      warning += ' scroll'
      warnings.push(warning);
    }

    return warnings;
  })
}
