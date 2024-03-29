import { Component, Input } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'check-box',
  templateUrl: './check-box.component.html',
  styleUrls: ['./check-box.component.scss'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    multi: true,
    useExisting: CheckBoxComponent
  }]
})
export class CheckBoxComponent implements ControlValueAccessor {


  @Input()
  public isDisabled = false;
  public _isChecked = false;
  public onChange = (value: any) => { };
  public onTouched = () => { };

  public getCheckboxClass() {
    let elementClass = this.isChecked === true ? 'checked' : 'unchecked';
    if (this.isDisabled === true) elementClass += ' disabled';
    return elementClass;
  }
  public get isChecked() {
    return this._isChecked;
  }
  public set isChecked(value: boolean) {
    if (this.isDisabled === true) return;
    this._isChecked = value;
    this.onChange(value);
    this.onTouched();
  }
  writeValue(obj: any): void {
    this.isChecked = obj;
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
  setDisabledState?(isDisabled: boolean): void {
    this.isDisabled = isDisabled;
  }
}
