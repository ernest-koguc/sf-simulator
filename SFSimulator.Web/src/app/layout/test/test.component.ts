import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { finalize } from "rxjs";
import { SftoolsloginComponent, DataScope } from "../../dialogs/sftools-login-dialog/sftoolslogin.component";
import { greaterThanOrEqualTo, runeDamageBonusValidator } from "../../helpers/validators";
import { Character, ClassType, Companion, RuneType } from "../../models/character";
import { Dungeon, DungeonEnemy, DungeonType } from "../../models/dungeon";
import { DungeonResult } from "../../models/simulation-result";
import { SimulatorService } from "../../services/simulator.service";
import { SFToolsCharacterModel } from "./test";
import { Nullable } from "src/app/helpers/nullable";

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.scss']
})
export class TestComponent implements OnInit {

  public dungeons!: Dungeon[];
  public dungeon = new FormControl<Dungeon | null>(null);
  public DungeonType = DungeonType;
  public enemy = new FormControl<DungeonEnemy | null>(null);
  public iterations = new FormControl<number>(1000, [Validators.required, Validators.min(1), Validators.max(10000000)]);
  public class = ClassType;
  public runeWeaponType = RuneType;
  public isInProgress = false;

  public bert = new FormGroup({
    class: new FormControl<ClassType | null>({ value: ClassType.Bert, disabled: true}, [Validators.required]),
    strength: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    dexterity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    intelligence: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    constitution: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    luck: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    reaction: new FormControl<boolean>(false, [Validators.required]),
    hasWeaponScroll: new FormControl<boolean>(false, [Validators.required]),
    armor: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    lightningResistance: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(75)]),
    fireResistance: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(75)]),
    coldResistance: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(75)]),
    healthRune: new FormControl<number | null>(null, [Validators.required, Validators.min(0), Validators.max(15)]),
    firstWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      runeType: new FormControl<RuneType | null>(RuneType.None, [Validators.required]),
      runeValue: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(0), Validators.max(60)]),
    }),
  });
  public get bertWeapon() {
    return this.bertControls.firstWeapon.controls;
  }
  public get bertControls() {
    return this.bert.controls;
  }
  public kunigunde = new FormGroup({
    class: new FormControl<ClassType | null>({ value: ClassType.Scout, disabled: true}, [Validators.required]),
    strength: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    dexterity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    intelligence: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    constitution: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    luck: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    reaction: new FormControl<boolean>(false, [Validators.required]),
    hasWeaponScroll: new FormControl<boolean>(false, [Validators.required]),
    armor: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    lightningResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    fireResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    coldResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    healthRune: new FormControl<number | null>(null, [Validators.min(0), Validators.max(15)]),
    firstWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      runeType: new FormControl<RuneType | null>(RuneType.None, [Validators.required]),
      runeValue: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(0), Validators.max(60)]),
    }),
  });
  public get kunigundeWeapon() {
    return this.kunigundeControls.firstWeapon.controls;
  }
  public get kunigundeControls() {
    return this.kunigunde.controls;
  }
  public marc = new FormGroup({
    class: new FormControl<ClassType | null>({ value: ClassType.Mage, disabled: true}, [Validators.required]),
    strength: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    dexterity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    intelligence: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    constitution: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    luck: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    reaction: new FormControl<boolean>(false, [Validators.required]),
    hasWeaponScroll: new FormControl<boolean>(false, [Validators.required]),
    armor: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    lightningResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    fireResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    coldResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    healthRune: new FormControl<number | null>(null, [Validators.min(0), Validators.max(15)]),
    firstWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      runeType: new FormControl<RuneType | null>(RuneType.None, [Validators.required]),
      runeValue: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(0), Validators.max(60)]),
    }),
  });
  public get marcWeapon() {
    return this.marcControls.firstWeapon.controls;
  }
  public get marcControls() {
    return this.marc.controls;
  }
  public character = new FormGroup({
    level: new FormControl<number | null>(null, [Validators.required, Validators.min(1), Validators.max(800)]),
    class: new FormControl<ClassType | null>(null, [Validators.required]),
    strength: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    dexterity: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    intelligence: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    constitution: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    luck: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    reaction: new FormControl<boolean>(false, [Validators.required]),
    hasWeaponScroll: new FormControl<boolean>(false, [Validators.required]),
    hasEternityPotion: new FormControl<boolean>(false, [Validators.required]),
    armor: new FormControl<number | null>(null, [Validators.required, Validators.min(0)]),
    gladiatorLevel: new FormControl<number | null>(null, [Validators.min(0), Validators.max(15)]),
    soloPortal: new FormControl<number | null>(null, [Validators.min(0), Validators.max(50)]),
    guildPortal: new FormControl<number | null>(null, [Validators.min(0), Validators.max(50)]),
    lightningResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    fireResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    coldResistance: new FormControl<number | null>(null, [Validators.min(0), Validators.max(75)]),
    healthRune: new FormControl<number | null>(null, [Validators.min(0), Validators.max(15)]),
    firstWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      runeType: new FormControl<RuneType | null>(RuneType.None, [Validators.required]),
      runeValue: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(0), Validators.max(60)]),
    }),
    secondWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000)]),
      maxDmg: new FormControl<number | null>(null, [Validators.min(0), Validators.max(2000000), greaterThanOrEqualTo('minDmg', 'Minimum Damage')]),
      runeType: new FormControl<RuneType | null>(RuneType.None, [Validators.required]),
      runeValue: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(0), Validators.max(60)]),
    }),
  });
  public result?: DungeonResult;
  public get characterControls() {
    return this.character.controls;
  }
  public get firstWeapon() {
    return this.character.controls.firstWeapon.controls;
  }
  public get secondWeapon() {
    return this.character.controls.secondWeapon.controls;
  }
  public get selectedDungeon() {
    return this.dungeon.valid ? this.dungeon.value : null;
  }
  constructor(private simulatorService: SimulatorService, private dialog: MatDialog) { }

  public loginThroughSFTools() {
    this.dialog.open(SftoolsloginComponent, { autoFocus: 'dialog', enterAnimationDuration: 200, exitAnimationDuration: 200, restoreFocus: false, width: "80%", height: "80%", data: DataScope.Dungeon }).afterClosed().subscribe(data => {
      if (data) {
        data.reaction = data.reaction === 1;
        this.character.patchValue(data);

        data.companions[0].class = ClassType.Bert;
        data.companions[0].reaction = data.companions[0].reaction === 1;
        this.bert.patchValue(data.companions[0]);

        data.companions[1].reaction = data.companions[1].reaction === 1;
        this.marc.patchValue(data.companions[1]);

        data.companions[2].reaction = data.companions[2].reaction === 1;
        this.kunigunde.patchValue(data.companions[2]);
      }
    });
  }

  public simulateDungeon() {
    this.character.markAllAsTouched();

    if (this.character.invalid) {
      return;
    }

    let dungeon = this.selectedDungeon;

    if (!dungeon?.position) {
      return;
    }

    if ((dungeon.type === DungeonType.Tower || dungeon.type === DungeonType.ShadowWorld) && (this.bert.invalid || this.kunigunde.invalid || this.marc.invalid || this.iterations.invalid)) {
      return;
    }

    let enemyPosition = this.enemy.value?.position;

    if (!enemyPosition) {
      return;
    }

    let char = this.normalizeCharacter(this.character.getRawValue());
    let companions: Companion[] = [];
    if (dungeon.type === DungeonType.Tower || dungeon.type === DungeonType.ShadowWorld) {
      companions.push(this.normalizeCompanion(this.bert.getRawValue()));
      companions.push(this.normalizeCompanion(this.marc.getRawValue()));
      companions.push(this.normalizeCompanion(this.kunigunde.getRawValue()));
    }

    let iterations = this.iterations.value;
    this.isInProgress = true;
    this.simulatorService.simulateDungeon(dungeon.position, enemyPosition, char, companions, iterations!, iterations!)
      .pipe(finalize(() => {
        this.isInProgress = false;
      }))
      .subscribe(v => {
        this.result = v;
        this.result.iterations = iterations!;
      });
  }

  public normalizeCharacter(character: Nullable<Character>): Character {
    let char: Character = {
      level: character.level!,
      class: character.class!,
      strength: character.strength!,
      dexterity: character.dexterity!,
      intelligence: character.intelligence!,
      constitution: character.constitution!,
      luck: character.luck!,
      armor: character.armor!,
      firstWeapon: character.firstWeapon,
      secondWeapon: character.secondWeapon,
      lightningResistance: character.lightningResistance!,
      fireResistance: character.fireResistance ?? 0,
      coldResistance: character.coldResistance ?? 0,
      healthRune: character.healthRune ?? 0,
      reaction: character.reaction ? 1 : 0,
      hasWeaponScroll: character.hasWeaponScroll!,
      hasEternityPotion: character.hasEternityPotion!,
      gladiatorLevel: character.gladiatorLevel ?? 0,
      soloPortal: character.soloPortal ?? 0,
      guildPortal: character.guildPortal ?? 0,
    }
    if (char.firstWeapon?.runeType === RuneType.None) {
      char.firstWeapon.runeValue = 0;
    }
    if (char.secondWeapon?.runeType === RuneType.None) {
      char.secondWeapon.runeValue = 0;
    }
    if (char.class !== ClassType.Assassin)
    {
      char.secondWeapon = null;
    }
    if (char.firstWeapon?.minDmg == null || char.firstWeapon?.maxDmg == null) {
      char.firstWeapon = null;
    }
    if (char.secondWeapon?.minDmg == null || char.secondWeapon?.maxDmg == null) {
      char.secondWeapon = null;
    }
    return char;
  }
  normalizeCompanion(companion: Nullable<Companion>): Companion {
    let comp: Companion = {
      class: companion.class!,
      strength: companion.strength!,
      dexterity: companion.dexterity!,
      intelligence: companion.intelligence!,
      constitution: companion.constitution!,
      luck: companion.luck!,
      armor: companion.armor!,
      firstWeapon: companion.firstWeapon,
      lightningResistance: companion.lightningResistance!,
      fireResistance: companion.fireResistance ?? 0,
      coldResistance: companion.coldResistance ?? 0,
      healthRune: companion.healthRune ?? 0,
      reaction: companion.reaction ? 1 : 0,
      hasWeaponScroll: companion.hasWeaponScroll!,
    }
    if (comp.firstWeapon?.runeType === RuneType.None) {
      comp.firstWeapon.runeValue = 0;
    }
    if (comp.firstWeapon?.minDmg == null || comp.firstWeapon?.maxDmg == null) {
      comp.firstWeapon = null;
    }
    return comp;
  }
  copy() {
    let form = this.character.getRawValue();
    let character: SFToolsCharacterModel = {
      Class: form.class ?? 1,
      Level: form.level ?? 0,
      Armor: form.armor ?? 0,
      Runes: {
        ResistanceFire: form.fireResistance ?? 0,
        ResistanceCold: form.coldResistance ?? 0,
        ResistanceLightning: form.lightningResistance ?? 0,
        Health: form.healthRune ?? 0
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
          HasEnchantment: form.reaction ?? false
        },
        Wpn1: {
          DamageMin: form.firstWeapon.minDmg ?? 0,
          DamageMax: form.firstWeapon.maxDmg ?? 0,
          HasEnchantment: form.hasWeaponScroll ?? false,
          AttributeTypes: { "2": this.translateRuneType(form.firstWeapon.runeType ) },
          Attributes: { "2": form.firstWeapon.runeValue ?? 0 }
        },
        Wpn2: {
          DamageMin: form.secondWeapon.minDmg ?? 0,
          DamageMax: form.secondWeapon.maxDmg ?? 0,
          HasEnchantment: form.hasWeaponScroll ?? false,
          AttributeTypes: { "2": this.translateRuneType(form.secondWeapon.runeType) },
          Attributes: { "2": form.secondWeapon.runeValue?? 0 }
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
        reaction: data.Items.Hand.HasEnchantment,
        hasWeaponScroll: data.Items.Wpn1.HasEnchantment || data.Items.Wpn2.HasEnchantment,
        hasEternityPotion: data.Potions.Life === 25,
        armor: data.Armor,
        gladiatorLevel: data.Fortress.Gladiator,
        soloPortal: data.Dungeons.Player,
        guildPortal: data.Dungeons.Group,
        firstWeapon: {
          minDmg: data.Items.Wpn1.DamageMin,
          maxDmg: data.Items.Wpn1.DamageMax,
          runeType: this.retranslateRuneType(data.Items.Wpn1.AttributeTypes[2]),
          runeValue: data.Items.Wpn1.Attributes[2]
        },
        secondWeapon: {
          minDmg: data.Items.Wpn2.DamageMin,
          maxDmg: data.Items.Wpn2.DamageMax,
          runeType: this.retranslateRuneType(data.Items.Wpn2.AttributeTypes[2]),
          runeValue: data.Items.Wpn2.Attributes[2]
        },
        lightningResistance: data.Runes.ResistanceLightning,
        coldResistance: data.Runes.ResistanceCold,
        fireResistance: data.Runes.ResistanceLightning,
        healthRune: data.Runes.Health
      })
      this.character.updateValueAndValidity();
      this.character.markAllAsTouched();
    });
  }
  private retranslateRuneType(type: number): RuneType {
    switch (type) {
      case 40:
        return RuneType.Fire;
      case 41:
        return RuneType.Cold;
      case 42:
        return RuneType.Lightning;
      default:
        return RuneType.None;
    }
  }

  private translateRuneType(type: RuneType | null) {
  switch (type) {
    case RuneType.Fire:
      return 40;
    case RuneType.Cold:
      return 41;
    case RuneType.Lightning:
      return 42;
    default:
      return 0;
  }
}

  ngOnInit(): void {
    this.dungeon.valueChanges.subscribe(d => {
      this.enemy.setValue(d?.enemies[0] ?? null);
    });
    this.simulatorService.getDungeonList().subscribe(v => {
      this.dungeons = v;
      this.dungeon.setValue(this.dungeons[0] ?? null);
    });

    let weaponControls = [this.firstWeapon, this.secondWeapon, this.bertWeapon, this.marcWeapon, this.kunigundeWeapon]
    for (let weaponControl of weaponControls) {
      weaponControl.runeValue.disable();
      weaponControl.minDmg.valueChanges.subscribe(_ => weaponControl.maxDmg.updateValueAndValidity());
      weaponControl.runeType.valueChanges.subscribe(v => {
        if (v === RuneType.None) {
          weaponControl.runeValue.disable();
          weaponControl.runeValue.setValue(null);
        }
        else {
          weaponControl.runeValue.enable();
        }
        this.firstWeapon.runeValue.markAsTouched();
        this.firstWeapon.runeValue.updateValueAndValidity();
      });
    }

    this.characterControls.class.valueChanges.subscribe(v => {
      if (v === ClassType.Assassin) {
        this.characterControls.secondWeapon.enable();
      }
      else {
        this.characterControls.secondWeapon.disable();
      }
    });
  }
}
