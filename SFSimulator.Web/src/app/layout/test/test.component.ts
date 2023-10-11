import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { finalize } from "rxjs";
import { SftoolsloginComponent } from "../../dialogs/sftools-login-dialog/sftoolslogin.component";
import { greaterThanOrEqualTo, runeDamageBonusValidator, secondWeaponValidator } from "../../helpers/validators";
import { ClassType, DamageRuneType } from "../../models/character";
import { Dungeon, DungeonEnemy } from "../../models/dungeon";
import { DungeonResult } from "../../models/simulation-result";
import { SimulatorService } from "../../services/simulator.service";
import { SFToolsCharacterModel } from "./test";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  public dungeons!: Dungeon[];
  public dungeon = new FormControl<Dungeon | null>(null);
  public enemy = new FormControl<DungeonEnemy | null>(null);
  public iterations = new FormControl<number>(1000, [Validators.required, Validators.min(1), Validators.max(10000000)]);
  public class = ClassType;
  public runeWeaponType = DamageRuneType;
  public isInProgress = false;

  public character = new FormGroup({
    level: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(800)]),
    class: new FormControl<ClassType | null>(null, [Validators.required]),
    strength: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    dexterity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    intelligence: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    constitution: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    luck: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    hasGlovesScroll: new FormControl<boolean>(false, [Validators.required]),
    hasWeaponScroll: new FormControl<boolean>(false, [Validators.required]),
    hasEternityPotion: new FormControl<boolean>(false, [Validators.required]),
    armor: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    gladiatorLevel: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(15)]),
    soloPortal: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(50)]),
    guildPortal: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(50)]),
    firstWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      damageRuneType: new FormControl<DamageRuneType | null>(DamageRuneType.None, [Validators.required]),
      runeBonus: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(1), Validators.max(60)]),
    }),
    secondWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      damageRuneType: new FormControl<DamageRuneType | null>(DamageRuneType.None, [Validators.required]),
      runeBonus: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(1), Validators.max(60)]),
    }, [secondWeaponValidator()]),
    runeBonuses: new FormGroup({
      lightningResistance: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(75)]),
      fireResistance: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(75)]),
      coldResistance: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(75)]),
      healthRune: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(15)])
    })
  });
  public result?: DungeonResult;
  public get form() {
    return this.character.controls;
  }
  public get firstWeapon() {
    return this.character.controls.firstWeapon.controls;
  }
  public get secondWeapon() {
    return this.character.controls.secondWeapon.controls;
  }
  public get runeBonuses() {
    return this.character.controls.runeBonuses.controls;
  }
  public get selectedDungeon() {
    return this.dungeon.valid ? this.dungeon.value : null;
  }
  constructor(private simulatorService: SimulatorService, private dialog: MatDialog) { }

  public loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', enterAnimationDuration: 200, exitAnimationDuration: 200, restoreFocus: false, width: "80%", height: "80%" }).afterClosed().subscribe(data => {
      if (data) {
        this.character.patchValue(data);
      }
    });
  }

  public simulateDungeon() {
    this.character.markAllAsTouched();

    if (this.character.invalid) {
      return;
    }
    if (this.iterations.invalid) {
      return;
    }

    let dungPosition = this.selectedDungeon?.position;

    if (!dungPosition) {
      return;
    }

    let enemyPosition = this.enemy.value?.position;

    if (!enemyPosition) {
      return;
    }

    let char = this.character.getRawValue() as any;
    char = this.normalizeForm(char);

    let iterations = this.iterations.value;
    this.isInProgress = true;
    this.simulatorService.simulateDungeon(dungPosition, enemyPosition, char, iterations!, iterations!)
      .pipe(finalize(() => {
        this.isInProgress = false;
      }))
      .subscribe(v => {
        this.result = v;
        this.result.iterations = iterations!; 
      });
  }

  normalizeForm(character: any) {
    if (character.firstWeapon.damageRuneType === DamageRuneType.None) {
      character.firstWeapon.runeBonus = 0;
    } 
    if (character.secondWeapon.damageRuneType === DamageRuneType.None) {
      character.secondWeapon.runeBonus = 0;
    } 
    if (character.firstWeapon.minDmg == null || character.firstWeapon.maxDmg == null) {
      character.firstWeapon = null;
    }
    if (character.secondWeapon.minDmg == null || character.secondWeapon.maxDmg == null) {
      character.secondWeapon = null;
    }
    return character;
  }
  copy() {
    let form = this.character.getRawValue();
    let character: SFToolsCharacterModel = {
      Class: form.class ?? 1,
      Level: form.level ?? 0,
      Armor: form.armor ?? 0,
      Runes: {
        ResistanceFire: form.runeBonuses.fireResistance ?? 0,
        ResistanceCold: form.runeBonuses.coldResistance ?? 0,
        ResistanceLightning: form.runeBonuses.lightningResistance ?? 0,
        Health: form.runeBonuses.healthRune ?? 0
      },
      Dungeons: {
        Player: form.soloPortal ?? 0,
        Group: form.guildPortal ?? 0
      },
      Fortress: {
        Gladiator: form.gladiatorLevel ?? 0
      },
      Potions: {
        Life: form.hasEternityPotion ? 25 : 0
      },
      Items: {
        Hand: {
          HasEnchantment: form.hasGlovesScroll ?? false
        },
        Wpn1: {
          DamageMin: form.firstWeapon.minDmg ?? 0,
          DamageMax: form.firstWeapon.maxDmg ?? 0,
          HasEnchantment: form.hasWeaponScroll ?? false,
          AttributeTypes: { "2": this.translateRuneType(form.firstWeapon.damageRuneType) },
          Attributes: { "2": form.firstWeapon.runeBonus ?? 0 }
        },
        Wpn2: {
          DamageMin: form.secondWeapon.minDmg ?? 0,
          DamageMax: form.secondWeapon.maxDmg ?? 0,
          HasEnchantment: form.hasWeaponScroll ?? false,
          AttributeTypes: { "2": this.translateRuneType(form.secondWeapon.damageRuneType) },
          Attributes: { "2": form.secondWeapon.runeBonus ?? 0 }
        },
      },
      BlockChance: 25,
      Strength: { Total: form.strength ?? 0 },
      Dexterity: { Total: form.dexterity ?? 0 },
      Intelligence: { Total: form.intelligence ?? 0 },
      Constitution: { Total: form.constitution ?? 0 },
      Luck: { Total: form.luck ?? 0 },
    }
    let valueToClipboard = JSON.stringify(character);
    navigator.clipboard.writeText(valueToClipboard);
  }
  public paste() {
    navigator.clipboard.readText().then((value) => {
      let data: SFToolsCharacterModel;
      try {
        data = JSON.parse(value);
      }
      catch {
        return;
      }

      this.character.setValue({
        level: data.Level,
        class: data.Class as ClassType,
        strength: data.Strength.Total,
        dexterity: data.Dexterity.Total,
        intelligence: data.Intelligence.Total,
        constitution: data.Constitution.Total,
        luck: data.Luck.Total,
        hasGlovesScroll: data.Items.Hand.HasEnchantment,
        hasWeaponScroll: data.Items.Wpn1.HasEnchantment || data.Items.Wpn2.HasEnchantment,
        hasEternityPotion: data.Potions.Life === 25,
        armor: data.Armor,
        gladiatorLevel: data.Fortress.Gladiator,
        soloPortal: data.Dungeons.Player,
        guildPortal: data.Dungeons.Group,
        firstWeapon: {
          minDmg: data.Items.Wpn1.DamageMin,
          maxDmg: data.Items.Wpn1.DamageMax,
          damageRuneType: this.retranslateRuneType(data.Items.Wpn1.AttributeTypes[2]),
          runeBonus: data.Items.Wpn1.Attributes[2]
        },
        secondWeapon: {
          minDmg: data.Items.Wpn2.DamageMin,
          maxDmg: data.Items.Wpn2.DamageMax,
          damageRuneType: this.retranslateRuneType(data.Items.Wpn2.AttributeTypes[2]),
          runeBonus: data.Items.Wpn2.Attributes[2]
        },
        runeBonuses: {
          lightningResistance: data.Runes.ResistanceLightning,
          coldResistance: data.Runes.ResistanceCold,
          fireResistance: data.Runes.ResistanceLightning,
          healthRune: data.Runes.Health
        }
      })
      this.character.updateValueAndValidity();
      this.character.markAllAsTouched();
    });
  }
  private retranslateRuneType(type: number): DamageRuneType {
    switch (type) {
      case 40:
        return DamageRuneType.Fire;
      case 41:
        return DamageRuneType.Cold;
      case 42:
        return DamageRuneType.Lightning;
      default:
        return DamageRuneType.None;
    }
  }

  private translateRuneType(type: DamageRuneType | null) {
  switch (type) {
    case DamageRuneType.Fire:
      return 40;
    case DamageRuneType.Cold:
      return 41;
    case DamageRuneType.Lightning:
      return 42;
    default:
      return 0;
  }
}

  ngOnInit(): void {
    this.firstWeapon.runeBonus.disable();
    this.secondWeapon.runeBonus.disable();
    this.dungeon.valueChanges.subscribe(d => {
      this.enemy.setValue(d?.enemies[0] ?? null);
    });
    this.simulatorService.getDungeonList().subscribe(v => {
      this.dungeons = v;
      this.dungeon.setValue(this.dungeons[0] ?? null);
    });

    this.firstWeapon.damageRuneType.valueChanges.subscribe(v => {
      if (v === DamageRuneType.None) {
        this.firstWeapon.runeBonus.disable();
        this.firstWeapon.runeBonus.setValue(null);
      }
      else {
        this.firstWeapon.runeBonus.enable();
      }
      this.firstWeapon.runeBonus.markAsTouched(); 
      this.firstWeapon.runeBonus.updateValueAndValidity(); 
    });
    this.secondWeapon.damageRuneType.valueChanges.subscribe(v => {
      if (v === DamageRuneType.None) {
        this.secondWeapon.runeBonus.disable();
        this.secondWeapon.runeBonus.setValue(null);
      }
      else {
        this.secondWeapon.runeBonus.enable();
      }
      this.secondWeapon.runeBonus.markAsTouched(); 
      this.secondWeapon.runeBonus.updateValueAndValidity(); 
    });
    this.firstWeapon.minDmg.valueChanges.subscribe(_ => {
      this.firstWeapon.maxDmg.updateValueAndValidity();
    });
    this.secondWeapon.minDmg.valueChanges.subscribe(_ => {
      this.secondWeapon.maxDmg.updateValueAndValidity();
    });
    this.form.class.valueChanges.subscribe(v => {
      if (v === ClassType.Assassin) {
        this.form.secondWeapon.enable();
      }
      else {
        this.form.secondWeapon.disable();
      }
    });
  }
}
