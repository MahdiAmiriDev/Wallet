import {Selectable} from '../../../../shared/base/classes/selectable';
import {BaseList} from '../../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';

@Component({
  selector: 'app-hekmat-transaction-item',
  templateUrl: './hekmat-transaction-item.component.html'
})
export class HekmatTransactionItemComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) tempProductFile: DxFileUploaderComponent;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  showSaveButton = false;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  entityName = 'uploadedProductItem';
  @Output()
  afterFinalRegistration: EventEmitter<any> = new EventEmitter<any>();
  private baseListUrl = 'hekmatTransaction/GetHekmatTransactionItem?';
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

  constructor(private genericDataService: GenericDataService) {
    super();
    this.finalRegistration = this.finalRegistration.bind(this);
  }

  ngOnInit(): void {
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 200;
    }
    this.bindColumns();
    this.bindCustomButtons();
    this.loadAfterSetFilter = true;
  }

  bindColumns(): void {
    this.columns.push({
        caption: 'کد رهگیری - ردیف گردش ترمینال',
        dataField: 'installmentTrackingCode',
        width: '200px',
      }, {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        width: '150px',
      }, {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        width: '100px',
      }, {
        caption: 'تعداد قسط',
        dataField: 'installmentCount',
        width: '120px',
      }, {
        caption: 'مبلغ',
        dataField: 'price',
        cellTemplate: 'number',
        width: '100px',

      }, {
        caption: 'وضعیت',
        dataField: 'status',
        width: '100px',

      }, {
        caption: 'کد خطا',
        dataField: 'errorCode',
        width: '100px',

      }, {
        caption: 'توکن',
        dataField: 'hekmatToken',
        width: '100px',

      }, {
        caption: 'نتیجه تراکنش',
        dataField: 'transactionResult',
        width: '80px',

      }, {
        caption: 'تاریخ انجام تراکنش',
        dataField: 'transactionDate',
        width: '80px',
        cellTemplate: 'jalaliDate',
      }
    );

  }

  bindCustomButtons(): void {
    if (!this._invalidItem) {
      this.customButtons = [{
        icon: 'file',
        text: 'ثبت نهایی',
        type: 'success',
        key: 'finalRegistration',
        float: 'right'
      }];
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;

  }

  /*
    تاییدیه ثبت نهایی از کاربر
       */
  customButtonClicked(key: any): void {
    if (key === 'finalRegistration') {
      this.confirmBeforeAction('ثبت نهایی', 'آیا از به روز رسانی تراکنش های حکمت در سایت اطمینان دارید؟', 'تایید')
        .show().then((dialogResult) => {
        if (dialogResult.res === 'ok') {
          this.finalRegistration();
        }
      });

    }
  }

  /*
    درصورتی که کاربر دکمه ثبت نهایی را بزند این متد صدا زده میشود
      */
  public finalRegistration(): void {
    this.setBusy(true);
    this.genericDataService.postEntityByUrl(`api/hekmatTransaction/updateHekmatTransaction`, {})
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
