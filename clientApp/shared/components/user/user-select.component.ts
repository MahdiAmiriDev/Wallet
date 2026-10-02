import {Component, Input} from '@angular/core';
import {forkJoin} from 'rxjs';
import {TranslateService} from '@ngx-translate/core';
import {Selectable} from '../../../shared/base/classes/selectable';
import {GenericDataService} from "../../base/services/generic-data.service";

@Component({
  selector: 'app-user-select',
  templateUrl: './user-select.component.html'
})
export class UserSelectComponent extends Selectable {
  // tslint:disable-next-line:variable-name
  private _selectedItemId: number;
  @Input()
  set selectedItemId(value: number) {
    if (value !== this._selectedItemId) {
      this._selectedItemId = value;
      this.selectedRowChanged.emit(value);
    }
  }

  get selectedItemId(): number {
    return this._selectedItemId;
  }

  columns: any[] = [];

  userDataSource: any;
  @Input()
  isRequired: boolean;
  @Input()
  requiredMessage: string;
  @Input()
  isReadOnly: boolean;
  @Input()
  customListUrl: string;
  @Input()
  lazyLoading = true;
  @Input()
  showOnlyWithNationalCode = false;
  @Input()
  label: string;
  @Input()
  considerCurrentUserAdditionalData  = true;

  private _companyId: number;
  @Input()
  set companyId(value: number) {
    if (this._companyId != value) {
      this._companyId = value;
      this.setUserDataSource();
    }

  }

  get companyId(): number {
    return this._companyId;
  }

  private _representationId: number;
  @Input()
  set representationId(value: number) {
    if (this._representationId != value) {
      this._representationId = value;
      this.setUserDataSource();
    }

  }

  get representationId(): number {
    return this._representationId;
  }

  private _personId: number;
  @Input()
  set personId(value: number) {
    if (this._personId != value) {
      this._personId = value;
      this.setUserDataSource();
    }
  }

  get personId(): number {
    return this._personId;
  }

  constructor(private genericDataService: GenericDataService, private translate: TranslateService) {
    super();
  }

  ngOnInit(): void {
    this.setUserDataSource();
    let result = forkJoin({
      UserNameOrMobileTitle: this.translate.get('um.User.UserName'),
      isActiveTitle: this.translate.get('um.User.IsActive'),
      needToSendSmsForAuthenticationTitle: this.translate.get('um.User.NeedToSendSmsForAuthentication'),


    });

    result.subscribe(({UserNameOrMobileTitle, isActiveTitle, needToSendSmsForAuthenticationTitle}) => {
      this.columns.push(
        {
          caption: UserNameOrMobileTitle,
          dataField: 'userName',
        }, {
          caption: isActiveTitle,
          dataField: 'isActive',
        }, {
          caption: needToSendSmsForAuthenticationTitle,
          dataField: 'needToSendSmsForAuthentication',
        });
    });
  }

  selectedUserChanged(e: any): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.selectedItemId = e.selectedRowsData[0].id;
      //this.selectedRowChanged.emit(e.selectedRowsData[0].id);
    }
  }

  setUserDataSource() {
    let url = 'GetForSelectByFilter?';
    url += `considerCurrentUserAdditionalData=${this.considerCurrentUserAdditionalData}&`;
    if (this.personId) {
      url += `personId=${this._personId}&`;
    }
    if (this.companyId) {
      url += `companyId=${this._companyId}&`;
    }
    if (this.representationId) {
      url += `representationId=${this.representationId}&`;
    }
    this.userDataSource = this.genericDataService.createCustomDataSourceWithAction('id', 'userExtender', url);
  }


}
