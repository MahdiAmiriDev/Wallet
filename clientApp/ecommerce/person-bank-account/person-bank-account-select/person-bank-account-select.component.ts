import {Component, Input, OnInit} from '@angular/core';
import {Selectable} from '../../../../shared/base/classes/selectable';
import {GenericDataService} from '../../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-person-bank-account-select',
  templateUrl: './person-bank-account-select.component.html'
})
export class PersonBankAccountSelectComponent extends Selectable implements OnInit {
  @Input()
  isRequired: boolean;
  @Input()
  requiredMessage: string;
  @Input()
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'PersonBankAccount';
  personBankAccountDataSource: any;
  @Input()
  customListUrl: string;

  constructor(private genericDataService: GenericDataService) {
    super();
    this.columns.push({
      caption: 'نام و نام خانوادگی صاحب حساب',
      dataField: 'fullName',
      allowSorting: false,
      allowHeaderFiltering: false,
    });

    this.columns.push({
      caption: 'نام بانک',
      dataField: 'bankTypeTitle'
    });
    this.columns.push({
      caption: 'شماره حساب',
      dataField: 'accountNumber',
      allowSorting: false,
      allowHeaderFiltering: false,
    });

  }

  ngOnInit(): void {
    this.customListUrl = `${this.entityName}/GetByCustomForSelect`;
    if (!this.customListUrl) {
      this.personBankAccountDataSource = this.genericDataService.createCustomDataSourceForSelect('id', 'personBankAccount');
    } else {
      this.personBankAccountDataSource = this.genericDataService.createCustomDataSourceWithCustomUrl('id', this.customListUrl);
    }
  }

  selectedPersonBankAccountChanged(e: any): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.selectedItemId = e.selectedRowsData[0].id;
      this.selectedRowChanged.emit(this.selectedItemId);
    }
  }
}
