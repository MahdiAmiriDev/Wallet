import {Component, EventEmitter, Input, Output, ViewChild} from '@angular/core';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';
import {YashilComponent} from '../../../../core/baseClasses/yashilComponent';
import {IBusyIndicator} from '../../../../core/interfaces';


@Component({
  selector: 'app-product-info',
  templateUrl: './product-info.component.html'
})
export class ProductInfoComponent extends YashilComponent {
  @ViewChild('busyIndicatorComponent', {static: true}) busyIndicator: IBusyIndicator;
  entity: any = {};
  // tslint:disable-next-line:variable-name
  _productId: number;
  @Output()
  afterLoadData: EventEmitter<any> = new EventEmitter<any>();


  get productId(): number {
    return this._productId;
  }

  @Input()
  set productId(value: number) {
    if (this.productId !== value) {
      this._productId = value;
      this.loadData();
    }
  }

  @Input()
  simpleMode = true;
  @Input()
  customButtonText = 'انتخاب دوره';
  @Input()
  customButtonIcon = 'checklist';
  @Input()
  customButtonStyle = 'float: left;height: 34px;';
  @Output()
  customButtonClick: EventEmitter<any> = new EventEmitter<any>();
  @Input()
  customButtonStylingMode = 'contained';
  @Input()
  customButtonType = 'default';
  @Input()
  showCustomButton = false;
  @Input()
  isCourse: boolean = true;
  showPopUp: boolean = false;

  constructor(private genericDataService: GenericDataService) {
    super();
    this.loadData = this.loadData.bind(this);
    this.showProductInfo = this.showProductInfo.bind(this);
  }


  /*
  دریافت اطلاعات
   */
  loadData(): void {
    this.busyIndicator.setBusy(true);
    this.genericDataService.getEntity('product', null, `GetProductInfo?productId=${this.productId}`)
      .subscribe(
        res => {
          this.busyIndicator.setBusy(false);
          if (res) {
            this.entity = res;
            this.afterLoadData.emit(res);
          } else {
            this.showError(`کاربر گرامی آیتم مورد نظر یافت نشد`);
          }
        }
      );
  }


  onCustomButtonClick(): void {
    this.customButtonClick.emit(this.entity);
  }

  showProductInfo() {
    if (this.productId) {
      this.showPopUp = true;
    } else {
      this.showError('کاربر گرامی لطفا ابتدا یک دوره را انتخاب نماببد.');
    }

  }
}
