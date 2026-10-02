import {Directive, EventEmitter, Input, Output, QueryList, ViewChild} from '@angular/core';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {BaseEdit} from './base-edit';

@Directive()
export class Editable extends YashilComponent {

  @ViewChild("mainBaseEdit") mainBaseEdit: BaseEdit;
  dataEntityName: string;
  @Output()
  afterInsertRow: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  afterUpdateRow: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  afterInsertFailed: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  afterUpdateFailed: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  afterDataLoaded: EventEmitter<any> = new EventEmitter<any>();
  public dataSelectedEntityId: number;
  showCloseButton: boolean;
  @Input()
  isNew: boolean;
  @Output()
  closeFormClick = new EventEmitter<boolean>();
  // اگر رکوردی درج شد کل رکورد را برگرداند یا تنها شناسه آنرا برگرداند،
  // اگر مقدار true داشته باشد کل رکورد را برمیگراند و اگر مقدار false داشته باشد تنها شناسه رکورد درج شده را برمیگرداند
  @Input()
  emitEntityAfterInsertedRow = false;
  // اگر رکوردی ویرایش شد کل رکورد را برگرداند یا تنها شناسه آنرا برگرداند،
  // اگر مقدار true داشته باشد کل رکورد را برمیگراند و اگر مقدار false داشته باشد تنها شناسه رکورد درج شده را برمیگرداند
  @Input()
  emitEntityAfterUpdatedRow = false;

  public get entityName(): string {
    return this.dataEntityName;
  }

  @Input()
  public set entityName(value: string) {
    this.dataEntityName = value;
  }

  public get selectedEntityId(): number {
    return this.dataSelectedEntityId;
  }

  @Input()
  public set selectedEntityId(value: number) {
    this.dataSelectedEntityId = value;
  }

  // tslint:disable-next-line:variable-name
  _additionalData: any;

  get additionalData(): any {
    return this._additionalData;
  }

  @Input()
  set additionalData(value: any) {
    this._additionalData = value;
  }
}
