import { AfterViewInit, Directive, Injector, Type } from '@angular/core';
import {
  AbstractControl,
  ControlValueAccessor,
  NgControl,
  UntypedFormControl,
} from '@angular/forms';

@Directive()
export class BaseControlComponent<T>
  implements ControlValueAccessor, AfterViewInit
{
  protected control!: AbstractControl;
  private _value!: T;
  protected onChange = (_: any) => {};
  protected onTouched = () => {};

  get value() {
    return this._value;
  }

  set value(value: T) {
    this.writeValue(value);
    this.onChange(value);
  }

  constructor(private _injector: Injector) {}

  ngAfterViewInit(): void {
    try {
      const ngControl = this._injector.get<NgControl>(
        NgControl as Type<NgControl>,
      );
      if (ngControl) {
        this.control = ngControl.control as UntypedFormControl;
      }
    } catch (e) {}
  }

  writeValue(val: T): void {
    this._value = val;
  }

  registerOnChange(fn: any): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }
}
