import { Component, OnInit } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { MatButtonToggleAppearance } from "@angular/material/button-toggle";
import { finalize } from "rxjs";
import { greaterThan, runeDamageBonusValidator, secondWeaponValidator } from "../../helpers/validators";
import { ClassType, DamageRuneType } from "../../models/character";
import { Dungeon, DungeonEnemy } from "../../models/dungeon";
import { DungeonResult } from "../../models/simulation-result";
import { SimulatorService } from "../../services/simulator.service";

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
      minDmg: new FormControl<number | null>(null, [Validators.required]),
      maxDmg: new FormControl<number | null>(null, [Validators.required, greaterThan('minDmg', 'Minimum Damage')]),
      damageRuneType: new FormControl<DamageRuneType | null>(DamageRuneType.None, [Validators.required]),
      runeBonus: new FormControl<number | null>(null, [runeDamageBonusValidator(), Validators.min(1), Validators.max(60)]),
    }),
    secondWeapon: new FormGroup({
      minDmg: new FormControl<number | null>(null),
      maxDmg: new FormControl<number | null>(null, [Validators.required, greaterThan('minDmg', 'Minimum Damage')]),
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
  constructor(private simulatorService: SimulatorService) { }

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
