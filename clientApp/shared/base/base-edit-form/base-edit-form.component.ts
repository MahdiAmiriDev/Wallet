import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {YashilCustomButton} from '../models/yasil-custom-button';

@Component({
  selector: 'app-base-edit-form',
  templateUrl: './base-edit-form.component.html'
})
export class BaseEditFormComponent extends YashilComponent implements OnInit {
  @Input()
  customButtons: YashilCustomButton[];
  @Input()
  loadForm = true;
  @Input()
  fixHeight = true;
  @Input()
  allowSave = true;
  @Input()
  showSaveButton = true;
  @Input()
  showCloseButton = true;
  @Output()
  customButtonClicked: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  cancelEditOrInsert = new EventEmitter();
  @Output()
  submitEvent = new EventEmitter<string>();
  formHeight: number;
  disabled: boolean;
  @Input()
  saveButtonText = 'ذخیره';

  onFormSubmit(e): void {
    this.disabled = true;
    this.submitEvent.next(e);
  }

  onCancelClicked(): void {
    this.cancelEditOrInsert.next();
  }

  ngOnInit(): void {
    if (this.fixHeight) {
      this.formHeight = window.innerHeight - 200;
    }
  }

  selectedCustomButtonChanged(e: any): void {
    this.customButtonClicked.emit(e);
  }
}
