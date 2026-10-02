import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {IssueDetailComponent} from './issue-detail.component';


@Component({
  selector: 'app-issue-list',
  templateUrl: './issue-list.component.html'
})
export class IssueListComponent extends Selectable implements OnInit {

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'issue';
  detailComponent = IssueDetailComponent;
  customButtons: any;

  // tslint:disable-next-line:variable-name
  private _showCustomButtons = true;

  get showCustomButtons(): boolean {
    return this._showCustomButtons;
  }

  @Input()
  set showCustomButtons(value: boolean) {
    if (this.showCustomButtons !== value) {
      this._showCustomButtons = value;
      this.bindCustomButtons();
    }
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

  // tslint:disable-next-line:variable-name
  _resolutionType: number;
  @Input()
  set resolutionType(val) {
    if (val !== this._resolutionType) {
      this._resolutionType = val;
      this.bindCustomDataSources();

    }
  }

  get resolutionType(): number {
    return this._resolutionType;
  }

  // tslint:disable-next-line:variable-name
  _priorityType: number;
  @Input()
  set priorityType(val) {
    if (val !== this._priorityType) {
      this._priorityType = val;
      this.bindCustomDataSources();

    }
  }

  get priorityType(): number {
    return this._priorityType;
  }

  // tslint:disable-next-line:variable-name
  _issueStateType: number;
  @Input()
  set issueStateType(val) {
    if (val !== this._issueStateType) {
      this._issueStateType = val;
      this.bindCustomDataSources();

    }
  }

  get issueStateType(): number {
    return this._issueStateType;
  }

  // tslint:disable-next-line:variable-name
  _assignee: number;
  @Input()
  set assignee(val) {
    if (val !== this._assignee) {
      this._assignee = val;
      this.bindCustomDataSources();

    }
  }

  get assignee(): number {
    return this._assignee;
  }

  // tslint:disable-next-line:variable-name
  _issueOwner: number;
  @Input()
  set issueOwner(val) {
    if (val !== this._issueOwner) {
      this._issueOwner = val;
      this.bindCustomButtons();
      this.bindCustomDataSources();
    }
  }

  get issueOwner(): number {
    return this._issueOwner;
  }

  // tslint:disable-next-line:variable-name
  _nonRegisteredUser: string;
  @Input()
  set nonRegisteredUser(val) {
    if (val !== this._nonRegisteredUser) {
      this._nonRegisteredUser = val;
      this.bindCustomButtons();
      this.bindCustomDataSources();
    }
  }

  get nonRegisteredUser(): string {
    return this._nonRegisteredUser;
  }

  private baseListUrl = 'issue/GetByCustomFilterForList?';

  constructor() {
    super();
    this.customButtonClicked = this.customButtonClicked.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomButtons();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  bindCustomButtons(): void {
    if (this._showCustomButtons) {
      if (this.orderItemId || this.issueOwner) {
        this.customButtons = [{
          icon: 'comment',
          text: 'ثبت مسئله برای کد آیتم سفارش: ' + this.orderItemId,
          type: 'default',
        },
          {
            icon: 'image',
            text: 'ثبت عمومی برای کاربر عضو',
            type: 'success',
          }
        ];
      }
      // tslint:disable-next-line:triple-equals
      if (!this.issueOwner && !this.orderItemId) {
        this.customButtons = [{
          icon: 'video',
          text: 'ثبت عمومی برای کاربر غیر عضو',
          type: 'danger',
        }];
      }
    } else {
      this.customButtons = [{}];
    }

  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title',
      width: '85px'
    }, {
      caption: 'نوع مسئله(پدر)',
      dataField: 'issueTypeTitle'
    }, {
      caption: 'نتیجه مسئله',
      dataField: 'resolutionTypeTitle'
    }, {
      caption: 'وضعیت مسئله',
      dataField: 'issueStateTypeTitle'
    }, {
      caption: 'انجام دهنده',
      dataField: 'assigneeLastName'
    });
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.orderItemId) {
        customListUrl += `orderItemId=${this.orderItemId}&`;
      }
      if (this.orderId) {
        customListUrl += `orderId=${this.orderId}&`;
      }
      if (this.paymentId) {
        customListUrl += `paymentId=${this.paymentId}&`;
      }
      if (this.issueType) {
        customListUrl += `issueType=${this.issueType}&`;
      }
      if (this.resolutionType) {
        customListUrl += `resolutionType=${this.resolutionType}&`;
      }
      if (this.priorityType) {
        customListUrl += `priorityType=${this.priorityType}&`;
      }
      if (this.issueStateType) {
        customListUrl += `issueStateType=${this.issueStateType}&`;
      }
      if (this.assignee) {
        customListUrl += `assignee=${this.assignee}&`;
      }
      if (this.issueOwner) {
        customListUrl += `issueOwner=${this.issueOwner}&`;
      }
      if (this.nonRegisteredUser) {
        customListUrl += `nonRegisteredUser=${this.nonRegisteredUser}&`;
      }
    }
    let res = false;
    // tslint:disable-next-line:max-line-length
    if (this.orderItemId || this.orderId || this.paymentId || this.issueType || this.resolutionType || this.priorityType || this.issueStateType || this.assignee || this.issueOwner || this.nonRegisteredUser) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as IssueDetailComponent);

    if (this.orderItemId) {
      comp.orderItemId = this.orderItemId;
    }

    if (this.issueType) {
      comp.issueType = this.issueType;
    }

    if (this.resolutionType) {
      comp.resolutionType = this.resolutionType;
    }

    if (this.priorityType) {
      comp.priorityType = this.priorityType;
    }

    if (this.issueStateType) {
      comp.issueStateType = this.issueStateType;
    }

    if (this.assignee) {
      comp.assignee = this.assignee;
    }
    if (this.issueOwner) {
      comp.issueOwner = this.issueOwner;
    }
    if (this.nonRegisteredUser) {
      comp.nonRegisteredUser = this.nonRegisteredUser;
    }

  }

  customButtonClicked(e: any): void {
    this.listForm.addEntity();
  }

}
