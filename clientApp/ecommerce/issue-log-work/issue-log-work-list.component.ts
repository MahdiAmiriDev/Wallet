import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {IssueLogWorkDetailComponent} from './issue-log-work-detail.component';


@Component({
  selector: 'app-issue-log-work-list',
  templateUrl: './issue-log-work-list.component.html'
})
export class IssueLogWorkListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;

  selectedItemId: number;
  columns: any[] = [];
  entityName = 'issueLogWork';
  detailComponent = IssueLogWorkDetailComponent;
  customButtons: any;
  // tslint:disable-next-line:variable-name
  private _showCustomButtons = true;

  @Input()
  set showCustomButtons(value: boolean) {
    if (value !== this.showCustomButtons) {
      this._showCustomButtons = value;
      this.bindCustomButtons();
    }
  }

  get showCustomButtons(): boolean {
    return this._showCustomButtons;
  }

  // tslint:disable-next-line:variable-name
  _issueId: number;
  @Input()
  set issueId(val) {
    if (val !== this._issueId) {
      this._issueId = val;
      this.bindCustomButtons();
      this.bindCustomDataSources();
    }
  }

  get issueId(): number {
    return this._issueId;
  }

  // tslint:disable-next-line:variable-name
  private _paymentId: number;
  get paymentId(): number {
    return this._paymentId;
  }

  @Input()
  set paymentId(value: number) {
    if (this.paymentId !== value) {
      this._paymentId = value;
      this.bindCustomDataSources();
    }
  }


  // tslint:disable-next-line:variable-name
  private _orderId: number;
  get orderId(): number {
    return this._orderId;
  }

  @Input()
  set orderId(value: number) {
    if (this.orderId !== value) {
      this._orderId = value;
      this.bindCustomButtons();
      this.bindCustomDataSources();
    }
  }

  // tslint:disable-next-line:variable-name
  _orderItemId: number;
  @Input()
  set orderItemId(val) {
    if (val !== this._orderItemId) {
      this._orderItemId = val;
      this.bindCustomButtons();
      this.bindCustomDataSources();

    }
  }

  get orderItemId(): number {
    return this._orderItemId;
  }

  // tslint:disable-next-line:variable-name
  _issueType: number;
  @Input()
  set issueType(val) {
    if (val !== this._issueType) {
      this._issueType = val;
      this.bindCustomDataSources();
    }
  }

  get issueType(): number {
    return this._issueType;
  }


  private baseListUrl = 'issueLogWork/GetByCustomFilterForList?';
  @Input()
  allowEditInForm = true;
  @Input()
  allowDeleteInForm = true;

  constructor() {
    super();
    this.customButtonClicked = this.customButtonClicked.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.issueId && this.issueId !== -1) {
      this.bindCustomButtons();
    }
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomButtons(): void {
    this.customButtons = null;
    if (this.showCustomButtons && this.issueId && this.issueId !== -1) {
      this.customButtons = [{
        icon: 'comment',
        text: 'ثبت پیگیری',
        type: 'default',
      }];
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'ثبت کننده پیگیری',
        dataField: 'createByFullName',
        allowSorting: false,
        allowHeaderFiltering: false,
        width: '15%'
      });
    if (!this.issueType) {
      this.columns.push(
        {
          caption: 'نوع مشکل',
          dataField: 'issueTypeTitle',
          width: '15%'
        });
    }
    this.columns.push(
      {
        caption: 'توضیحات',
        dataField: 'description',
        allowSorting: false,
        allowHeaderFiltering: false,
        width: '50%'
      }, {
        caption: 'زمان صرف شده(دقیقه)',
        dataField: 'timeSpent',
        allowSorting: false,
        allowHeaderFiltering: false,
        width: '10%'
      }, {
        caption: 'تاریخ',
        dataField: 'creationDate',
        cellTemplate: 'jalaliDate',
        allowHeaderFiltering: false,
        width: '10%'
      });

  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.issueId) {
        customListUrl += `issueId=${this.issueId}&`;
      }
      if (this.issueType) {
        customListUrl += `issueType=${this.issueType}&`;
      }
      if (this.orderId) {
        customListUrl += `orderId=${this.orderId}&`;
      }
      if (this.orderItemId) {
        customListUrl += `orderItemId=${this.orderItemId}&`;
      }
      if (this.paymentId) {
        customListUrl += `paymentId=${this.paymentId}&`;
      }
    }
    let res = false;
    if (this.issueId || this.orderItemId || this.paymentId || this.orderId || this.issueType) {
      this.bindCustomButtons();
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }

    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as IssueLogWorkDetailComponent);
    if (this.issueId) {
      comp.issueId = this.issueId;
    }

  }

  customButtonClicked(e: any): void {
    if (this.issueId && this.issueId !== -1) {
      this.listForm.addEntity();
    }
  }
}
