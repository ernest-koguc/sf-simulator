import { computed, inject, Injectable } from '@angular/core';
import { SessionManager } from './SessionManager';
import { ScrollType } from '../sfgame/SFGameModels';

@Injectable({
  providedIn: 'root'
})
export class WarningService {
  private sessionManager = inject(SessionManager);

  public ownMissingScrolls = computed(() => {
    const current = this.sessionManager.current();
    if (!current) {
      return [];
    }
    const items = current.ownItems;
    const ownSave = current.ownPlayerSave;
    if (!items || !ownSave) {
      return [];
    }

    const warnings = [];

    const witch = current.witch;
    const isCritScrollUnlocked = witch?.Scrolls.find(s => s.Type === ScrollType.Crit)?.Unlocked && ownSave.Level >= 66;
    if (isCritScrollUnlocked === true && !items.Wpn1?.HasEnchantment && !items.Wpn2?.HasEnchantment) {
      warnings.push('Weapon scroll missing!');
    }

    const isReactionScrollUnlocked = witch?.Scrolls.find(s => s.Type === ScrollType.Reaction)?.Unlocked && ownSave.Level >= 66;
    if (isReactionScrollUnlocked === true && !items.Hand?.HasEnchantment) {
      warnings.push('Gloves scroll missing!');
    }

    return warnings;
  })

  public companionsMissingScrolls = computed(() => {
    const current = this.sessionManager.current();
    if (!current) {
      return [];
    }
    const ownSave = current.ownPlayerSave;
    const companionsItems = current.companionItems;

    if (!ownSave || !companionsItems) {
      return [];
    }

    const warnings = [];

    const witch = current.witch;

    const isCritScrollUnlocked = witch?.Scrolls.find(s => s.Type === ScrollType.Crit)?.Unlocked && ownSave?.Level >= 66;
    const isReactionScrollUnlocked = witch?.Scrolls.find(s => s.Type === ScrollType.Reaction)?.Unlocked && ownSave?.Level >= 66;

    if (isCritScrollUnlocked) {
      const bertWeaponScroll = companionsItems.Bert?.Wpn1?.HasEnchantment;
      if (!bertWeaponScroll) {
        warnings.push('Bert weapon scroll missing!');
      }
      const markWeaponScroll = companionsItems.Mark?.Wpn1?.HasEnchantment;
      if (!markWeaponScroll) {
        warnings.push('Mark weapon scroll missing!');
      }
      const kunigundeWeaponScroll = companionsItems.Kunigunde?.Wpn1?.HasEnchantment;
      if (!kunigundeWeaponScroll) {
        warnings.push('Kunigunde weapon scroll missing!');
      }
    }

    if (isReactionScrollUnlocked) {
      const bertGlovesScroll = companionsItems.Bert?.Hand?.HasEnchantment;
      if (!bertGlovesScroll) {
        warnings.push('Bert gloves scroll missing!');
      }
      const markGlovesScroll = companionsItems.Mark?.Hand?.HasEnchantment;
      if (!markGlovesScroll) {
        warnings.push('Mark gloves scroll missing!');
      }
      const kunigundeGlovesScroll = companionsItems.Kunigunde?.Hand?.HasEnchantment;
      if (!kunigundeGlovesScroll) {
        warnings.push('Kunigunde gloves scroll missing!');
      }
    }

    return warnings;

  });
}
