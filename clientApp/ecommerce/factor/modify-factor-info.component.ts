import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {custom} from "devextreme/ui/dialog";


@Component({
  selector: 'app-modify-factor-info',
  templateUrl: './modify-factor-info.component.html'
})
export class ModifyFactorInfoComponent extends BaseEdit implements OnInit {
  @Output()
  afterModifyFactorInfo: EventEmitter<any> = new EventEmitter<any>();

  // tslint:disable-next-line:variable-name
  private _factorId: number;
  @Input()
  set factorId(value: number) {
    if (value !== this.factorId) {
      this._factorId = value;
    }
  }

  get factorId(): number {
    return this._factorId;
  }


  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'factor';
    this.customUpdateMethodName = 'ModifyFactorInformation';
  }

  ngOnInit(): void {
    super.ngOnInit();
  }

  selectedPersonBankAccountChanged(selectedPersonBankAccountId: any): void {
    this.entity.personBankAccountId = selectedPersonBankAccountId;
  }

  /*
  قبل از تایید ویرایش فاکتور از کاربر تاییدیه می گیرد
    */
  doSubmitForm() {
    this.confirmBeforeAction('ثبت نهایی', 'کاربر گرامی  بعد از تغییر و ذخیره اطلاعات فروشنده، این فاکتور مجددا قابل ویرایش نخواهد بود، آیا از تغییر اطلاعات فروشنده در فاکتور اطمینان دارید؟', 'تایید')
      .show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        super.doSubmitForm();
      }
    });

  }
}
