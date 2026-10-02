import {Selectable} from '../../../../shared/base/classes/selectable';
import {BaseList} from '../../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';


@Component({
  selector: 'app-renamed-products-list',
  templateUrl: './renamed-products-list.component.html'
})
export class RenamedProductListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
   loadAfterSetFilter: boolean;
  customButtons: any;
  columns: any[] = [];
  value: any[] = [];
  entityName = 'uploadedProductItem';
  @Output()
  afterContinueFinalRegistration: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  afterCancelFinalRegistration: EventEmitter<any> = new EventEmitter<any>();

  private baseListUrl = `uploadedProductItem/GetProductsSoldAndRenamedInUploadedProductsAsync?`;

  constructor() {
    super();
  }

// tslint:disable-next-line:variable-name
  _companyId: number;


  @Input()
  set companyId(val) {
    if (val !== this._companyId) {
      this._companyId = val;
      this.bindCustomDataSources();
    }
  }

  ngOnInit(): void {
    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 200;
    }
    this.bindColumns();
    this.bindCustomButtons();
    this.bindCustomDataSources();
    this.loadAfterSetFilter = true;
  }

  bindColumns(): void {
    this.columns.push({
      caption: 'کد بسته',
      dataField: 'code',
      width: '100px',
    }, {
      caption: 'عنوان',
      dataField: 'title',
      width: '100px',
    }, {
      caption: 'نوع',
      dataField: 'type',
      width: '80px',
    }, {
      caption: 'توضیحات',
      dataField: 'description',
      width: '200px',
    });

  }

  bindCustomButtons(): void {
    /*
       اگر پارمتر نامعتبر برابر با false بود دکمه ثبت نهایی را به کاربر نمایش دهد
         */

    this.customButtons = [{
      icon: 'file',
      text: 'ادامه',
      type: 'success',
      key: 'continue',
      float: 'right'
    },
      {
        icon: 'clear',
        text: 'انصراف',
        type: 'danger',
        key: 'cancel',
        float: 'right'
      }
    ];

  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      customListUrl += `companyId=${this._companyId}&`;
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      return true;
    }
  }

  /*
  اگر کاربر دکمه ادامه را کلیک کند به روز رسانی محصولات انجام می شود و نام و توضیحات محصولات فروش رفته شده تغییر داده نمی شود
  و اکر دکمه انصراف را بزند به روز رسانی محصولات انجام نمی شود
       */
  customButtonClicked(key: any): void {
    if (key === 'continue') {
      this.confirmBeforeAction('ادامه ثبت نهایی', 'آیا از ادامه به روز رسانی اطمینان دارید؟', 'تایید')
        .show().then((dialogResult) => {
        if (dialogResult.res === 'ok') {
          this.afterContinueFinalRegistration.emit();
        }
      });
    }
    if (key === 'cancel') {
      this.afterCancelFinalRegistration.emit();
    }
  }
}
