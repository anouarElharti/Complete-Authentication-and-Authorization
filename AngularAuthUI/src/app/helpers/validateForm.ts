import { FormGroup, FormControl } from '@angular/forms';

export default class ValidateForm {
  static validateAllFormFields(formGroup: FormGroup) {
    Object.keys(formGroup.controls).forEach((field) => {
      const control = formGroup.get(field);
      if (control instanceof FormControl) {
        control.markAsDirty({ onlySelf: true });
      }
      if (control instanceof FormGroup) {
        this.validateAllFormFields(control);
      }
    });
  }
}
