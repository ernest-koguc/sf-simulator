import { Injectable } from '@angular/core';
import { db } from '../db';
import { Class, Equipment } from '../sfgame/SFGameModels';

@Injectable({ providedIn: 'root' })
export class EquipmentService {
  public async saveEquipment(playerEquipment: Equipment, classType: Class, playerName: string, server: string) {
    const record = {
      UniquePlayerId: `${server}-${playerName}`,
      Class: classType,
      Items: Object.values(playerEquipment)
        .filter((item): item is Equipment['Wpn1'] => item !== null && item.ItemQuality !== 0)
        .map(item => ({
          ItemQuality: item.ItemQuality,
          Armor: item.Armor,
          MinDmg: item.DamageMin,
          MaxDmg: item.DamageMax,
          Type: item.Type,
        })),
    };
    const previous = await db.EquipmentGathering
      .filter(entry => entry.UniquePlayerId === record.UniquePlayerId).first();
    await db.EquipmentGathering.put({ Id: previous?.Id, ...record });
  }
}
