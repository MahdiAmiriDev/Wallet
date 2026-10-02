import {Selectable} from '../../../../shared/base/classes/selectable';
import {BaseList} from '../../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';
import {UserDataService} from "../../../../_services/user-data.service";

@Component({
  selector: 'app-uploaded-product-item-list',
  templateUrl: './uploaded-product-item-list.component.html'
})
export class UploadedProductItemListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) tempProductFile: DxFileUploaderComponent;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  showSaveButton = false;
  customButtons: any;
  showRenamedProductDialog = false;
  showRenamedProductList = false;
  columns: any[] = [];
  value: any[] = [];
  entityName = 'uploadedProductItem';
  @Output()
  afterFinalRegistration: EventEmitter<any> = new EventEmitter<any>();
  private baseListUrl = 'uploadedProductItem/GetUploadedProductItemsAsync?';
  lmsMode: boolean = false;

  constructor(private genericDataService: GenericDataService, private userDataService: UserDataService) {
    super();
    this.finalRegistration = this.finalRegistration.bind(this);
    this.existsProductsSoldAndRenamed = this.existsProductsSoldAndRenamed.bind(this);

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

  ngOnInit(): void {

    this.lmsMode = this.userDataService.getSiteConfigurationLmsMode();

    if (!this.gridHeight) {
      this.gridHeight = window.innerHeight - 200;
    }

    this.bindColumns();

    this.bindCustomButtons();

    this.loadAfterSetFilter = true;

    if (this._invalidItem) {

    }
  }

  bindColumns(): void {

    if (this.lmsMode) {
      this.columns.push({
          caption: 'کد',
          dataField: 'code',
        },
        {
          caption: 'عنوان دوره',
          dataField: 'title',
        },
        {
          caption: 'وضعیت انتشار',
          dataField: 'published',
        },
        {
          caption: 'پیام خطا',
          dataField: 'errorMessage',
        },
        {
          caption: 'دسته بندی',
          dataField: 'categoryTitle',
        },
        {
          caption: 'نام کاربری استاد',
          dataField: 'teacherUserName',
        },
        {
          caption: 'نام کاربری دانشجو',
          dataField: 'studentUserName',
        },
        {
          caption: 'گروه عملی',
          dataField: 'representation',
        },
        {
          caption: 'تاریخ شروع',
          dataField: 'startDate',
        },
        {
          caption: 'تاریخ پایان',
          dataField: 'endDate',
        })
    } else {
      this.columns.push({
          caption: 'کد بسته',
          dataField: 'code',
          width: '80px',
        },
        {
          caption: 'عنوان',
          dataField: 'title',
          width: '100px',
        }, {
          caption: 'نوع',
          dataField: 'type',
          width: '50px',
        }, {
          caption: 'دسته اصلی(پایه)/زیر دسته (رشته)',
          dataField: 'categoryTitle',
          width: '200px',
        }, {
          caption: 'دسته بندی اصلی یا زیر دسته وجود ندارد',
          dataField: 'categoryTitleIsInValid',
          width: '80px',
        }, {
          caption: 'مبلغ پایه آزمون',
          dataField: 'testBasePrice',
          width: '100px',

        }, {
          caption: 'مبلغ پایه آزمون مقدار عددی نیست',
          dataField: 'testBasePriceIsInValid',
          width: '100px',

        }, {
          caption: 'مبلغ پایه کتاب',
          dataField: 'bookBasePrice',
          width: '100px',

        }, {
          caption: 'مبلغ پایه کتاب مقدار عددی نیست',
          dataField: 'bookBasePriceIsInValid',
          width: '100px',

        }, {
          caption: 'درصد تخفیف طرح باقرالعلوم',
          dataField: 'tbaoCustomerClubDiscountPercentage',
          width: '80px',

        }, {
          caption: 'درصد تخفیف طرح باقرالعلوم مقدار عددی یا مقدار خالی نیست',
          dataField: 'tbaoCustomerClubDiscountPercentageIsInValid',
          width: '100px',
        }
        , {
          caption: 'درصد تخفیف سایر',
          dataField: 'otherCustomerClubDiscountPercentage',
          width: '80px',

        }, {
          caption: 'درصد تخفیف سایر مقدار عددی یا مقدار خالی نیست',
          dataField: 'otherCustomerClubDiscountPercentageIsInValid',
          width: '100px',
        }
        , {
          caption: 'هزینه ارسال',
          dataField: 'deliveryCost',
          width: '100px',

        }, {
          caption: 'هزینه ارسال مقدار عددی نیست',
          dataField: 'deliveryCostIsInValid',
          width: '80px',
        }, {
          caption: 'وضعیت انتشار',
          dataField: 'published',
          width: '100px',

        }, {
          caption: 'وضعیت نمایش زیر نویس',
          dataField: 'productSalePlanningDisplaySubTitle',
          width: '100px',

        }, {
          caption: 'وضعیت انتشار از لیست انتخاب نشده است',
          dataField: 'publishedIsInValid',
          width: '100px',

        },
        {
          caption: 'نوع ووچر',
          dataField: 'vocherTypeTitle',
          width: '100px',

        },
        {
          caption: 'پیغام خطا',
          dataField: 'errorMessage',
          width: '300px',

        });

    }


  }

  bindCustomButtons(): void {
    /*
       اگر پارمتر نامعتبر برابر با false بود دکمه ثبت نهایی را به کاربر نمایش دهد
         */
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
    if (this.listForm) {
      customListUrl += `showInvalidItem=${this._invalidItem}&`;
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      return true;
    }
  }

  /*
    تایید ثبت نهایی و اعمال محصولات بارگذاری شده از کاربر
       */
  customButtonClicked(key: any): void {
    if (key === 'finalRegistration') {
      this.existsProductsSoldAndRenamed();
    }
  }

  /*
 بررسی  می کند که محصولات فروخته شده و تغییر نام یافته وجود دارد
   */
  public existsProductsSoldAndRenamed() {

    //زمانی که سامانه آموزش مجازی آویسا باشد نیازی به انجام ادامه عملیات وجود ندارد
    if(this.lmsMode){
      this.confirmFinalRegistration();
      return;
    }


    this.genericDataService.postEntityByUrl(`api/uploadedProductItem/CheckExistsProductsSoldAndRenamedInUploadedProductsAsync?companyId=${this.companyId}`, {})
      .subscribe((response: boolean) => {
        if (!response) {
          this.confirmFinalRegistration();
        } else {
          this.showRenamedProductDialog = true;
          this.showRenamedProductList = true;
        }
      });
  }

  public confirmFinalRegistration() {
    this.confirmBeforeAction('ثبت نهایی', 'آیا از ثبت نهایی محصول بارگذاری شده در سایت اطمینان دارید؟', 'تایید')
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
      this.genericDataService.postEntityByUrl(`api/uploadedProductItem/AddDataFromUploadedProductItem?companyId=${this.companyId}`, {})
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

  /*
  کاربر لیست محصولاتی که به فروش رفته است و امکان تغییر نام و توضیحات وجود ندارد را مشاهده کرده
    و دکمه انصراف از به روز رسانی محصولات را زده است
    این متد صدا زده می شود تا فرم لیست محصولات به فروش رفته بسته شود
   */
  public afterCancelFinalRegistration() {
    this.showRenamedProductDialog = false;
    this.showRenamedProductList = false;
  }

  /*
 کاربر لیست محصولاتی که به فروش رفته است و امکان تغییر نام و توضیحات وجود ندارد را مشاهده کرده
   و دکمه ادامه از به روز رسانی محصولات را زده است
   این متد صدا زده می شود تا فرم لیست محصولات به فروش رفته بسته شود
  */
  public afterContinueFinalRegistration() {
    this.showRenamedProductDialog = false;
    this.showRenamedProductList = false;
    this.finalRegistration();
  }
}
