import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { PlayerModel } from '../sfgame/sfgame-parser';
import { Dungeons, Tower } from '../sfgame/SFGameModels';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class DungeonSimulator {
  private httpClient = inject(HttpClient);

  public simulateOpenDungeons(player: PlayerModel, dungeons: Dungeons, tower: Tower) {
    let body = this.createBody(dungeons, player, tower);

    return this.httpClient.post<SimulateDungeonResponse[]>(`${environment.apiUrl}/dungeon/simulate`, body);
  }

  private createBody(dungeons: Dungeons, player: PlayerModel, tower: Tower) {
    return {
      Iterations: 100000,
      WinThreshold: 100000,
      DungeonLevels: {
        ...this.normalizeDungeons(dungeons),
      },
      Player: {
        Class: player.Class,
        Level: player.Level,
        Strength: player.Strength.Total,
        Dexterity: player.Dexterity.Total,
        Intelligence: player.Intelligence.Total,
        Constitution: player.Constitution.Total,
        Luck: player.Luck.Total,
        Health: PlayerModel.getHealth(player, dungeons.SoloPortal),
        Armor: player.Armor,
        FirstWeapon: {
          MinDmg: player.Items.Wpn1.DamageMin,
          MaxDmg: player.Items.Wpn1.DamageMax,
          RuneType: player.Items.Wpn1.RuneType,
          RuneValue: player.Items.Wpn1.RuneValue,
        },
        SecondWeapon: player.Items.Wpn2 ? {
          MinDmg: player.Items.Wpn2.DamageMin,
          MaxDmg: player.Items.Wpn2.DamageMax,
          RuneType: player.Items.Wpn2.RuneType,
          RuneValue: player.Items.Wpn2.RuneValue,
        } : null,
        Reaction: player.Items.Hand.HasEnchantment ? 1 : 0,
        CritMultiplier: 2 + tower.Underworld.Gladiator * 0.11 + (player.Items.Wpn2?.HasEnchantment || player.Items.Wpn1.HasEnchantment ? 0.05 : 0),
        LightningResistance: player.Runes.ResistanceLightning,
        FireResistance: player.Runes.ResistanceFire,
        ColdResistance: player.Runes.ResistanceCold,
        GuildPortal: player.GuildPortal,
      },
      Companions: [
        {
          Class: tower.Companions.Bert.Class,
          Strength: tower.Companions.Bert.Strength.Total,
          Dexterity: tower.Companions.Bert.Dexterity.Total,
          Intelligence: tower.Companions.Bert.Intelligence.Total,
          Constitution: tower.Companions.Bert.Constitution.Total,
          Luck: tower.Companions.Bert.Luck.Total,
          Health: PlayerModel.getHealth(tower.Companions.Bert, dungeons.SoloPortal, 6.1),
          Armor: tower.Companions.Bert.Armor,
          FirstWeapon: {
            MinDmg: tower.Companions.Bert.Items.Wpn1.DamageMin,
            MaxDmg: tower.Companions.Bert.Items.Wpn1.DamageMax,
            RuneType: tower.Companions.Bert.Items.Wpn1.RuneType,
            RuneValue: tower.Companions.Bert.Items.Wpn1.RuneValue,
          },
          Reaction: tower.Companions.Bert.Items.Hand.HasEnchantment ? 1 : 0,
          CritMultiplier: 2 + tower.Underworld.Gladiator * 0.11 + (tower.Companions.Bert.Items.Wpn1.HasEnchantment ? 0.05 : 0),
          LightningResistance: tower.Companions.Bert.Runes.ResistanceLightning,
          FireResistance: tower.Companions.Bert.Runes.ResistanceFire,
          ColdResistance: tower.Companions.Bert.Runes.ResistanceCold
        },
        {
          Class: tower.Companions.Mark.Class,
          Strength: tower.Companions.Mark.Strength.Total,
          Dexterity: tower.Companions.Mark.Dexterity.Total,
          Intelligence: tower.Companions.Mark.Intelligence.Total,
          Constitution: tower.Companions.Mark.Constitution.Total,
          Luck: tower.Companions.Mark.Luck.Total,
          Health: PlayerModel.getHealth(tower.Companions.Mark, dungeons.SoloPortal),
          Armor: tower.Companions.Mark.Armor,
          FirstWeapon: {
            MinDmg: tower.Companions.Mark.Items.Wpn1.DamageMin,
            MaxDmg: tower.Companions.Mark.Items.Wpn1.DamageMax,
            RuneType: tower.Companions.Mark.Items.Wpn1.RuneType,
            RuneValue: tower.Companions.Mark.Items.Wpn1.RuneValue,
          },
          Reaction: tower.Companions.Mark.Items.Hand.HasEnchantment ? 1 : 0,
          CritMultiplier: 2 + tower.Underworld.Gladiator * 0.11 + (tower.Companions.Mark.Items.Wpn1.HasEnchantment ? 0.05 : 0),
          LightningResistance: tower.Companions.Mark.Runes.ResistanceLightning,
          FireResistance: tower.Companions.Mark.Runes.ResistanceFire,
          ColdResistance: tower.Companions.Mark.Runes.ResistanceCold
        },
        {
          Class: tower.Companions.Kunigunde.Class,
          Strength: tower.Companions.Kunigunde.Strength.Total,
          Dexterity: tower.Companions.Kunigunde.Dexterity.Total,
          Intelligence: tower.Companions.Kunigunde.Intelligence.Total,
          Constitution: tower.Companions.Kunigunde.Constitution.Total,
          Luck: tower.Companions.Kunigunde.Luck.Total,
          Health: PlayerModel.getHealth(tower.Companions.Kunigunde, dungeons.SoloPortal),
          Armor: tower.Companions.Kunigunde.Armor,
          FirstWeapon: {
            MinDmg: tower.Companions.Kunigunde.Items.Wpn1.DamageMin,
            MaxDmg: tower.Companions.Kunigunde.Items.Wpn1.DamageMax,
            RuneType: tower.Companions.Kunigunde.Items.Wpn1.RuneType,
            RuneValue: tower.Companions.Kunigunde.Items.Wpn1.RuneValue,
          },
          Reaction: tower.Companions.Kunigunde.Items.Hand.HasEnchantment ? 1 : 0,
          CritMultiplier: 2 + tower.Underworld.Gladiator * 0.11 + (tower.Companions.Kunigunde.Items.Wpn1.HasEnchantment ? 0.05 : 0),
          LightningResistance: tower.Companions.Kunigunde.Runes.ResistanceLightning,
          FireResistance: tower.Companions.Kunigunde.Runes.ResistanceFire,
          ColdResistance: tower.Companions.Kunigunde.Runes.ResistanceCold
        }
      ]
    };
  }

  private normalizeDungeons(dungeons: Dungeons) {
    const light: Record<number, number> = {};
    for (let i = 0; i < dungeons.Light.length; i++) {
      const dung = dungeons.Light[i];
      light[dung.Position] = dung.Current;

    }

    const shadow: Record<number, number> = {};
    for (let i = 0; i < dungeons.Shadow.length; i++) {
      const dung = dungeons.Shadow[i];
      shadow[dung.Position] = dung.Current;

    }

    const dungeonLevels = {
      ...light,
      ...shadow,
      "-1": dungeons.Tower,
      "-2": dungeons.Twister,
      "-3": dungeons.LoopOfIdols,
      "-4": dungeons.Sandstorm,
    };
    return dungeonLevels;
  }
}

export type SimulateDungeonResponse = {
  winRatio: number,
  dungeonMetadata: {
    position: number,
    dungeonName: string,
    enemyName: string,
    withCompanions: boolean,
    class: number,
    experience: number
  }
}
