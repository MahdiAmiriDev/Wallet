import {Selectable} from '../../../../shared/base/classes/selectable';
import {Component, EventEmitter, Input, Output} from '@angular/core';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';

@Component({
  selector: 'app-uploaded-voucher-verify',
  templateUrl: './uploaded-voucher-verify.component.html'
})
export class UploadedVoucherVerifyComponent extends Selectable {
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  showSaveButton = false;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  entityName = 'UploadedVoucher';
  @Output()
  afterFinalRegistration: EventEmitter<any> = new EventEmitter<any>();

  constructor(private genericDataService: GenericDataService) {
    super();
    this.finalRegistration = this.finalRegistration.bind(this);
  }

  // tslint:disable-next-line:variable-name
  _message: string;
  get message(): string {
    return this._message;
  }

  @Input()
  set message(value: string) {
    this._message = value;
  }
  // tslint:disable-next-line:variable-name
  _invalidItem: boolean;

  get invalidItem(): boolean {
    return this._invalidItem;
  }
  @Input()
  set invalidItem(val) {
    if (val !== this._invalidItem) {
      this._invalidItem = val;
      this.bindCustomDataSources();
    }
  }

  // tslint:disable-next-line:variable-name
  _companyId: number;

  get companyId(): number {
    return this._companyId;
  }

  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
      this.bindCustomDataSources();
    }
  }

  // tslint:disable-next-line:variable-name
  _voucherTypeId: number;

  get voucherTypeId(): number {
    return this._voucherTypeId;
  }

  @Input()
  set voucherTypeId(val) {
    if (val !== this._voucherTypeId) {
      this._voucherTypeId = val;
      this.bindCustomDataSources();
    }
  }

  /*
    تایید ثبت نهایی و اعمال کد کوپن بارگذاری شده از کاربر
       */
  finalRegistrationButtonClicked(): void {
    this.confirmBeforeAction('ثبت نهایی', 'آیا از ثبت نهایی کد کوپن بارگذاری شده در سایت اطمینان دارید؟', 'تایید')
      .show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.finalRegistration();
      }
    });
  }

  /*
    درصورتی که فایل اکسل بارگذاری شده دارای خطا نباشد و کاربر دکمه ثبت نهایی را بزند این متد صدا زده میشود
      */
  public finalRegistration(): void {
    if (this.companyId == null && this.companyId === undefined) {
      this.showError('کاربر گرامی لطفا مرکز را انتخاب کنید');
    } else {
      this.setBusy(true);
      this.genericDataService.postEntityByUrl(`api/UploadedVoucher/AddVoucherFromUploadedVoucher?companyId=${this.companyId}&voucherTypeId=${this.voucherTypeId}`, {})
        .subscribe(response => {
          if (!response.isSuccess) {
            this.setBusy(false);
            this.afterFinalRegistration.emit();
            this.showError(response.message);
          } else {
            this.setBusy(false);
            this.afterFinalRegistration.emit();
            this.showMessage(response.message);
          }
        });
    }
  }
}
