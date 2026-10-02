import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {YashilCustomButton} from '../../../shared/base/models/yasil-custom-button';
import {HekmatTransactionModel} from '../hekmat-canceled-or-modified-transaction/hekmat-transaction.model';


@Component({
  selector: 'app-confirm-refund-detail',
  templateUrl: './confirm-refund-detail.component.html'
})
export class ConfirmRefundDetailComponent extends BaseEdit implements OnInit {
  @Input()
  fixHeight = true;
  @Input()
  showTitle = true;
  formHeight: number;
  @Output()
  confirmOrRejectClicked: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  hekmatTransactionData: EventEmitter<any> = new EventEmitter<any>();
  // entityName: string;
  entity: any = {};
  accessLevels: any[] = [];
  customButtons: YashilCustomButton[];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'refundDetail';
  }

  // tslint:disable-next-line:variable-name
  _refundDetailId: number;

  get refundDetailId(): number {
    return this._refundDetailId;
  }

  @Input()
  set refundDetailId(val) {
    if (val !== this._refundDetailId) {
      // موقع ست کردن refundDetailId باید اطلاعات قبلی را پاک کنیم و مجددا اطلاعات جدید را دریافت کنیم
      this.entity = {};
      this._refundDetailId = val;
      this.loadData();
    }
  }

  ngOnInit(): void {
    this.bindCustomButtons();
    if (this.fixHeight) {
      this.formHeight = window.innerHeight - 200;
    }
    if (this.refundDetailId) {
      this.entity.refundDetailId = this.refundDetailId;
    }
  }

  /*
  دریافت اطلاعات
   */
  loadData(): void {
    if (this.refundDetailId) {
      this.busyIndicator.setBusy(true);
      let url = 'GetRefundDetailStatistic?';
      url = url + `refundDetailId=${this.refundDetailId}&`;
      this.genericDataService.getEntity(this.entityName, this.refundDetailId, url)
        .subscribe(
          res => {
            this.busyIndicator.setBusy(false);
            if (res) {
              this.entity = res;
              const hekmatTransactionData = new HekmatTransactionModel();
              hekmatTransactionData.CardNumber = this.entity.cardNumber;
              hekmatTransactionData.Price = this.entity.oldPaymentAmount;
              this.hekmatTransactionData.emit(hekmatTransactionData);
            } else {
              this.showError(`کاربر گرامی آیتم مورد نظر یافت نشد`);
            }
          }
        );
    }
  }

  /*
  تایید عودت وجه
   */
  confirmRefundDetail(): void {

    if (this.refundDetailId) {
      const result = this.detailForm.instance.validate();
      if (result.isValid) {
        this.busyIndicator.setBusy(true);
        this.genericDataService.postEntityByUrl('/api/refundDetail/ConfirmRefundDetail', this.entity)
          .subscribe(
            res => {
              this.busyIndicator.setBusy(false);
              if (res) {
                if (!res.isSuccess) {
                  this.showError(res.message);
                } else {
                  this.showMessage(`تایید با موفقیت انجام شد`);
                  this.confirmOrRejectClicked.emit();
                }
              } else {
                this.showError(`بروز خطا`);
              }
            }
          );
      }

    } else {
      this.showError(`بروز خطا`);
    }
  }

  /*
  رد عودت وجه
   */
  rejectRefundDetail(): void {
    if (this.refundDetailId) {
      this.busyIndicator.setBusy(true);
      this.genericDataService.postEntityByUrl('/api/refundDetail/RejectRefundDetail', {id: this.refundDetailId})
        .subscribe(
          res => {
            this.busyIndicator.setBusy(false);
            if (res) {
              if (!res.isSuccess) {
                this.showError(res.message);
              } else {
                this.showMessage(`رد با موفقیت انجام شد`);
                this.confirmOrRejectClicked.emit();

              }
            } else {
              this.showError(`بروز خطا`);
            }
          }
        );
      this.confirmOrRejectClicked.emit();
    } else {
      this.showError(`بروز خطا`);
    }
  }

  /*
   ارسال عودت وجه حکمت تعیین تکلیف نشده به ماه جدید
 */
  sendRefundDetailToNewGroup(): void {
    if (this.refundDetailId) {
      this.busyIndicator.setBusy(true);
      this.genericDataService.postEntityByUrl('/api/refundDetail/SendRefundDetailToNewGroup', {id: this.refundDetailId})
        .subscribe(
          res => {
            this.busyIndicator.setBusy(false);
            if (res) {
              if (res.isSuccess) {
                this.showMessage(res.message);
                this.confirmOrRejectClicked.emit();
              } else {
                this.showError(res.message);
              }
            } else {
              this.showError(`بروز خطا`);
            }
          }
        );
      this.confirmOrRejectClicked.emit();
    } else {
      this.showError(`بروز خطا`);
    }
  }

  confirmOrRejectRefundDetail(key: any): void {
    if (key === 'confirm') {
      this.confirmRefundDetail();
    } else if (key === 'reject') {
      this.rejectRefundDetail();
    } else if (key === 'sendToNewGroup') {
      this.sendRefundDetailToNewGroup();
    }

  }

  /*
  اضافه کردن دکمه های تایید و رد
   */
  private bindCustomButtons(): void {
    this.customButtons = [{
      icon: 'todo',
      text: 'تایید',
      type: 'success',
      key: 'confirm'
    }, {
      icon: 'clear',
      text: 'رد',
      type: 'danger',
      key: 'reject'
    }
      , {
        icon: 'todo',
        text: 'ارسال مجدد به حکمت در ماه جدید',
        type: 'success',
        key: 'sendToNewGroup'
      }
    ];
  }
}
