import { Injectable } from '@angular/core';
import { db } from '../db';
import { Equipment } from '../sfgame/SFGameModels';
import { ByteParser } from '../sfgame/parsers/ByteParser';

@Injectable({
  providedIn: 'root'
})
export class EquipmentService {
  private database = db;
  public async saveEquipment(playerEquipment: Equipment, playerSave: number[], playerName: string, server: string) {

    let parser = new ByteParser(playerSave);
    let classType = parser.atLong(20);

    let equipmentGathering = {
      UniquePlayerId: `${server}-${playerName}`,
      Class: classType,
      Items: [
        playerEquipment.Head,
        playerEquipment.Body,
        playerEquipment.Hand,
        playerEquipment.Feet,
        playerEquipment.Neck,
        playerEquipment.Belt,
        playerEquipment.Ring,
        playerEquipment.Misc,
        playerEquipment.Wpn1,
        playerEquipment.Wpn2
      ].filter(i => i !== null && i.ItemQuality != 0).map(i => {
        return {
          Strength: i!.BaseStrength,
          Dexterity: i!.BaseDexterity,
          Intelligence: i!.BaseIntelligence,
          Constitution: i!.BaseConstitution,
          Luck: i!.BaseLuck,
          ItemQuality: i!.ItemQuality,
          PicIndex: i!.PicIndex,
          Armor: i!.Armor,
          MinDmg: i!.DamageMin,
          MaxDmg: i!.DamageMax,
          Type: i!.Type,
        }
      })
    }

    const previous = await this.database.EquipmentGathering.filter(e => e.UniquePlayerId === equipmentGathering.UniquePlayerId).first();
    await this.database.EquipmentGathering.put({ Id: previous?.Id, ...equipmentGathering });
  }
}
