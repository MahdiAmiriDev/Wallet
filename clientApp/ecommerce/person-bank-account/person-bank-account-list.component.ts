import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, EventEmitter, Input, OnInit, Output, ViewChild} from '@angular/core';
import {PersonBankAccountDetailComponent} from './person-bank-account-detail.component';
import {YashilCustomButtonColumn} from '../../../shared/base/classes/yashil-cutom-button-column';


@Component({
  selector: 'app-person-bank-account-list',
  templateUrl: './person-bank-account-list.component.html'
})
export class PersonBankAccountListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'personBankAccount';
  detailComponent = PersonBankAccountDetailComponent;
  customRowButtons: YashilCustomButtonColumn;
  @Output()
  personBankAccountSelect: EventEmitter<any> = new EventEmitter<any>();

  _showSelectButton: boolean;
  @Input()
  set showSelectButton(val) {
    if (val !== this._showSelectButton) {
      this._showSelectButton = val;
      this.bindCustomDataSources();
    }
  }

  get showSelectButton(): boolean {
    return this._showSelectButton;
  }

  // tslint:disable-next-line:variable-name
  _personId: number;
  @Input()
  set personId(val) {
    if (val !== this._personId) {
      this._personId = val;
      this.bindCustomDataSources();
    }
  }

  get personId(): number {
    return this._personId;
  }

  // tslint:disable-next-line:variable-name
  _bankType: number;
  @Input()
  set bankType(val) {
    if (val !== this._bankType) {
      this._bankType = val;
    }
  }

  get bankType(): number {
    return this._bankType;
  }

  private baseListUrl = 'personBankAccount/GetByCustomFilterForList?';

  constructor() {
    super();
    this.selectPersonBankAccount = this.selectPersonBankAccount.bind(this);

  }

  ngOnInit(): void {
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
    this.bindColumns();
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        allowHeaderFiltering: false
      },
      {
        caption: 'شماره موبایل',
        dataField: 'mobileNumber',
        allowHeaderFiltering: false
      },
      {
        caption: 'شخص',
        dataField: 'personTitle',
        allowHeaderFiltering: false
      }, {
        caption: 'بانک',
        dataField: 'bankTypeTitle'
      }, {
        caption: 'شماره کارت',
        dataField: 'cardNumber',
        allowHeaderFiltering: false
      }, {
        caption: 'شماره حساب',
        dataField: 'accountNumber',
        allowHeaderFiltering: false
      }, {
        caption: 'شماره شبا',
        dataField: 'shebaNumber',
        allowHeaderFiltering: false
      }, {
        caption: 'نام شعبه',
        dataField: 'branchName',
        allowHeaderFiltering: false
      });
    if (this._showSelectButton) {
      this.customRowButtons = {
        width: '150px',
        customButtons: [
          {
            hint: 'انتخاب حساب',
            text: 'انتخاب',
            icon: 'fieldchooser',
            type: 'success',
            stylingMode: 'contained',
            onClick: this.selectPersonBankAccount
          }]
      };
    }
  }

  public bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.personId) {
        customListUrl += `personId=${this.personId}&`;
      }
      if (this.bankType) {
        customListUrl += `bankType=${this.bankType}&`;
      }
    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as PersonBankAccountDetailComponent);

    if (this.personId) {
      comp.founderId = this.personId;
    }

    if (this.bankType) {
      comp.bankType = this.bankType;
    }

  }

  selectPersonBankAccount(item): void {
    this.personBankAccountSelect.emit(item);
  }
}
