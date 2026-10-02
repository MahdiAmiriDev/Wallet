import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CommonValidatorService} from '../../../shared/services/common-validator.service';

@Component({
  selector: 'app-person-bank-account-detail',
  templateUrl: './person-bank-account-detail.component.html'
})
export class PersonBankAccountDetailComponent extends BaseEdit implements OnInit {
  @Input()
  founderId: number;
  @Input()
  bankType: number;
  personDataSource: any;
  founderDataSource: any;
  bankTypes: any[] = [];
  columns: any[] = [];
  allowEditBankAccount = false;

  constructor(private genericDataService: GenericDataService, public commonValidatorService: CommonValidatorService) {
    super(genericDataService);
    this.entityName = 'personBankAccount';
    this.nationalCodeChanged = this.nationalCodeChanged.bind(this);
    this.personCardNumberChanged = this.personCardNumberChanged.bind(this);
    this.getBankList = this.getBankList.bind(this);

  }

  ngOnInit(): void {
    super.ngOnInit();
    this.entity.accountNumber = null;
    if (this.founderId) {
      this.fillPersonById(this.founderId);
    }
    if (this.founderId && this.founderId !== 0) {
      this.entity.personId = this.founderId;
    }

    if (this.bankType) {
      this.entity.bankType = this.bankType;
    }

    this.personDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'person');
    this.founderDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'person');
    this.getBankList();
    this.columns.push({
      caption: 'نام و نام خانوادگی',
      dataField: 'title'
    });
    this.columns.push({
      caption: 'کد ملی',
      dataField: 'nationalCode'
    });
  }

  selectedPersonChanged(e: any): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.entity.personId = e.selectedRowsData[0].id;
    }
  }

  nationalCodeChanged(): void {
    if (this.entity.nationalCode && this.entity.nationalCode.length === 10) {
      this._genericDataService.getEntitiesWithAction('Person', 'GetByNationalCode?nationalCode=' + this.entity.nationalCode, null)
        .subscribe(res => {
          this.setPersonEntity(res);
        });
    }
  }


  personCardNumberChanged(): void {
    if (this.entity.cardNumber.length === 16) {
      this._genericDataService.getEntitiesWithAction('PersonBankAccount',
        'GetPersonBankAccountInformation?cardNumber=' + this.entity.cardNumber, null)
        .subscribe((res: any) => {
          this.getBankList();
          this.entity.cardNumber = res.cardNumber;
          this.entity.bankType = res.bankType;
          this.entity.accountNumber = res.accountNumber;
          this.entity.shebaNumber = res.shebaNumber;
          this.entity.fullName = res.fullName;
          this.setAllowEditBankAccount(res);
        });
    }
  }

  fillPersonById(personId): void {
    this._genericDataService.getEntity('Person', personId).subscribe(res => {
      this.setPersonEntity(res);
    });
  }

  private setPersonEntity(res: any): void {
    if (res) {
      this.founderId = res.id;
      this.entity.personId = res.id;
      this.entity.name = res.name;
      this.entity.lastName = res.lastName;
      this.entity.mobileNumber = res.mobileNumber;
      this.entity.nationalCode = res.nationalCode;

    } else {
      this.entity.personId = null;
    }
  }

  /**
   * در صورتی که اطلاعات حساب برگردانده شود نباید توسط کاربر ویرایش شود
   * @param res
   * @private
   */
  private setAllowEditBankAccount(res: any): void {
    if (res && res.accountNumber) {
      this.allowEditBankAccount = false;
    } else {
      this.allowEditBankAccount = true;
    }
  }

  getBankList(): void {
    this._genericDataService.getEntitiesWithAction('commonBaseData',
      'GetByKeyName?keyName=BankType', null)
      .subscribe(res => {
        this.bankTypes = res;
      });
  }
}
