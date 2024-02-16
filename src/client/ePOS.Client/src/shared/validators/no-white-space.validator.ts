import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function NoWhitespaceValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    let value = control.value;
    if (typeof value === 'number') {
      value = `${value}`;
    }
    let isWhitespace = (value || '').trim().length === 0;
    let isValid = !isWhitespace;
    return isValid ? null : { onlyWhiteSpace: true };
  };
}
