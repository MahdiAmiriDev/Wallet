import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {
  UpdateOrderRecipientMobileNumberDetailComponent
} from './update-order-recipient-mobile-number-detail.component';

@Component({
  selector: 'app-update-order-recipient-mobile-number-list',
  templateUrl: './update-order-recipient-mobile-number-list.component.html'
})
export class UpdateOrderRecipientMobileNumberListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'order';
  detailComponent = UpdateOrderRecipientMobileNumberDetailComponent;
  companyId: number;
  representationId: number;
  oldMobileNumber: string;
  customRowButtons: any;
  private baseListUrl = 'order/GetByCustomFilterForList?orderStatusType=1091&';
  showPopUp = false;

  constructor() {
    super();
    this.openMobileNumberPopup = this.openMobileNumberPopup.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomRowButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'کد پیگیری',
      dataField: 'title',
      width: '20%'
    });
    this.columns.push({
      caption: 'نام کاربری',
      dataField: 'userTitle',
      width: '20%'
    });
    this.columns.push({
      caption: 'شماره موبایل دانشجو',
      dataField: 'recipientMobileNumber', //userTitle
      width: '20%'
    });
    this.columns.push({
      caption: 'کدملی تحویل گیرنده',
      dataField: 'nationalCode',
      width: '20%'
    });
    this.columns.push({
      caption: 'تاریخ سفارش',
      dataField: 'creationDateInt',
      cellTemplate: 'intDate',
      width: '20%'
    });
    this.columns.push({
      caption: 'وضعیت سفارش',
      dataField: 'orderStatusTypeTitle',
      width: '20%'
    });
  }

  bindCustomRowButtons() {
    this.customRowButtons = {
      width: '150px',
      customButtons: [
        {
          hint: "ویرایش",
          text: "ویرایش",
          icon: '../../assets/images/select-hand.svg',
          width: '120px',
          type: 'default',
          onClick: this.openMobileNumberPopup,
        }
      ]
    };
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.companyId) {
        customListUrl += `companyId=${this.companyId}&`;
      }
      if (this.representationId) {
        customListUrl += `representationId=${this.representationId}&`;
      }

    }

    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;

  }

  setSelectedCompany(item: any): void {
    if (item.company) {
      this.companyId = item.company.id;
      this.representationId = null;
    }
    this.bindCustomDataSources();
  }

  onSelectedRepresentationChanged(item: any): void {
    if (item.representation) {
      this.representationId = item.representation.id;
    } else {
      this.representationId = null;
    }
    this.bindCustomDataSources();
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = componentInstance.instance as UpdateOrderRecipientMobileNumberDetailComponent;
    comp.oldMobileNumber = this.oldMobileNumber;
    comp.entity.id = this.selectedItemId;
  }

  onSelectedRowChanged(item: any): void {
    if (item != null) {
      this.oldMobileNumber = item?.recipientMobileNumber;
      this.selectedItemId = item.id;
    }
  }

  openMobileNumberPopup() {
    this.showPopUp = true;
  }

  closeMobileNumberPopup() {
    this.showPopUp = false;
    this.listForm.refreshList();
  }

  onMobileNumberChanges(item: any): void {
    this.oldMobileNumber = item;
  }
}
