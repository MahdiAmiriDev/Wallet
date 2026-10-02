import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CardInfoDetailComponent} from './card-info-detail.component';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CardInfoModel} from '../../../_models/payment/card-info.model';
import {PaymentService} from '../../../_services';
import {saveAs} from 'file-saver';
import {DxFileUploaderComponent} from 'devextreme-angular/ui/file-uploader';


@Component({
  selector: 'app-card-info-list',
  templateUrl: './card-info-list.component.html'
})
export class CardInfoListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @ViewChild(DxFileUploaderComponent) cardInfoFile: DxFileUploaderComponent;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'cardInfo';
  detailComponent = CardInfoDetailComponent;
  customButtons: any;
  showDialog: boolean;
  _genericDataService: GenericDataService;
  _paymentService: PaymentService;
  organizationDataSource: any;
  cardTypes: any;
  cardInfoModel: CardInfoModel;
  error = '';
  private baseListUrl = 'cardInfo/GetByCustomFilterForList?';

  constructor(private genericDataService: GenericDataService, private paymentService: PaymentService) {
    super();
    this._genericDataService = genericDataService;
    this._paymentService = paymentService;
    this.cardInfoModel = new CardInfoModel();
    this.customButtonClicked = this.customButtonClicked.bind(this);
  }

  _cardType: number;

  get cardType(): number {
    return this._cardType;
  }

  @Input()
  set cardType(val) {
    if (val !== this._cardType) {
      this._cardType = val;
    }
  }

  _personId: number;

  get personId(): number {
    return this._personId;
  }

  @Input()
  set personId(val) {
    if (val !== this._personId) {
      this._personId = val;
    }
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.cardType) {
        customListUrl += `cardType=${this.cardType}&`;
      }
      if (this.personId) {
        customListUrl += `personId=${this.personId}&`;
      }
    }
    let res = false;
    if (this.cardType || this.personId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  bindCustomButtons() {
    this.customButtons = [{
      icon: 'file',
      text: 'بارگذاری فایل اطلاعات کارت اعتباری',
      type: 'success',
      key: 1,
    }];
  }

  customButtonClicked(e: any) {
    this.organizationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'organization');
    this._genericDataService.getCommonBaseDataForSelect('CardType').subscribe(res => this.cardTypes = res);
    this.showDialog = true;
  }

  addFile(files): void {
    const that = this;
    if (files && files[0]) {
      that.cardInfoModel.FileName = files[0].name;
    }
  }

  onSubmit(): void {
    this._paymentService.addCardInfo(this.cardInfoModel)
      // .pipe(first())
      .subscribe(
        data => {
          if (data.size > 0) {
            saveAs(data, 'Result.xlsx');
            this.showError('در ثبت تعدادی از کارت ها خطا رخ داده است');
          } else {
            this.showMessage('ثبت تمام کارت ها با موفقیت انجام شد');
          }
        },
        error => {
          this.error = error;
        }
      );
  }

  resetCardInfoModel() {
    this.cardInfoModel = new CardInfoModel();
    this.cardInfoFile.instance.reset();
  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (componentInstance.instance as CardInfoDetailComponent);

    if (this.cardType) {
      comp.cardType = this.cardType;
    }

    if (this.personId) {
      comp.personId = this.personId;
    }

  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'شماره کارت',
      dataField: 'cardNumber',
      allowHeaderFiltering: false
    }, {
      caption: 'تاریخ انقضاء',
      dataField: 'expireDate',
      allowHeaderFiltering: false,
      cellTemplate: 'intDate'
    }, {
      caption: 'موجودی اعتبار (ریال)',
      dataField: 'creditAmount',
      allowHeaderFiltering: false
    }, {
      caption: 'موجودی بن ریالی (ریال)',
      dataField: 'cashAmount',
      allowHeaderFiltering: false
    }, {
      caption: 'نوع کارت',
      dataField: 'cardTypeTitle'
    }, {
      caption: 'دارنده',
      dataField: 'personTitle',
      allowHeaderFiltering: false
    }, {
      caption: 'کد ملی دارنده',
      dataField: 'nationalCode',
      allowHeaderFiltering: false
    }, {
      caption: 'سازمان',
      dataField: 'organizationTitle'
    });
  }
}
