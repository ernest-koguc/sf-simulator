import { Component, Input, OnInit } from '@angular/core';


@Component({
  selector: 'character-details',
  templateUrl: './character-details.component.html',
  styleUrls: ['./character-details.component.scss']
})
export class CharacterDetailsComponent implements OnInit {

  constructor() { }
  @Input()
  public daysPassed?: number | null;
  @Input()
  public level?: number | null;
  @Input()
  public baseStats?: number | null;
  @Input()
  public experience?: number | null;
  @Input()
  public prevLevel?: number | null;
  @Input()
  public prevBaseStats?: number | null;
  @Input()
  public prevExperience?: number | null;

  show(): boolean {
    if (this.level || this.baseStats || this.experience)
      return true;

    return false;
  }

  public updateDetails() {

  }

  ngOnInit(): void {
  }

}
