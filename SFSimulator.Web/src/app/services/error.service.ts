import { Injectable } from '@angular/core';
import { ValidationErrors } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor() { }

  public getErrorMessage(error: ValidationErrors): string {
    if (error['required'])
      return `Required`;
    if (error['min'])
      return `Minimum ${error['min'].min}`;
    if (error['max'])
      return `Maximum ${error['max'].max}`;
    if (error['belowOrEqualCharacterLevel'])
      return `Level must be above character level`;
    if (error['outOfRange'])
      return `Between ${error['outOfRange'].min} and ${error['outOfRange'].max}`;
    if (error['maxExperience'])
      return `Can't exceed ${error['maxExperience'].max-1}`;
    if (error['runeDamage'])
      return `Rune damage required`;
    if (error['greaterThan']) {
      return `Must be greater than ${error['greaterThan'].name}`;
    }
    return '';
  }
}
