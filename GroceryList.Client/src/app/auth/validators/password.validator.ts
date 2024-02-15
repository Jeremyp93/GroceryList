import { AbstractControl, ValidatorFn } from "@angular/forms";

export function passwordMatchValidator(otherControlName: string): ValidatorFn  {
    return (control: AbstractControl): { [key: string]: any } | null => {
      const otherControl = control.root.get(otherControlName);

      if (otherControl && control.value !== otherControl.value) {
        return { passwordMismatch: true };
      }

      return null;
    };
  }