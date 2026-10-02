import {YashilComponent} from '../../../../core/baseClasses/yashilComponent';
import {Component, EventEmitter, OnInit, Output, ViewChild} from '@angular/core';
import {PersonBankAccountListComponent} from '../person-bank-account-list.component';
import {Selectable} from '../../../../shared/base/classes/selectable';

@Component({
  selector: 'app-person-bank-account-search',
  templateUrl: './person-bank-account-search.component.html'
})
export class PersonBankAccountSearchComponent extends Selectable implements OnInit {
  @ViewChild('personBankAccount', {static: true}) personBankAccount: PersonBankAccountListComponent;
  @Output()
  afterSelectedPersonBankAccountChanged = new EventEmitter();
  contentHeight: number;
  selectedPersonId: number;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.contentHeight = window.innerHeight - 110;
  }

  selectedPersonChanged(selectedPersonId: any): void {
    this.selectedPersonId = selectedPersonId;
    this.personBankAccount.bindCustomDataSources();
    this.personBankAccount._showSelectButton = true;
  }

  selectedPersonBankAccountChanged(item: any): void {
    this.confirmBeforeAction('انتخاب اطلاعات حساب شخص', 'آیا از انتخاب اطلاعات حساب شخص اطمینان دارید؟', 'انتخاب اطلاعات حساب شخص')
      .show().then((dialogResult) => {
      if (dialogResult.res === 'ok') {
        this.afterSelectedPersonBankAccountChanged.emit(item);
      }
    });


  }

  clearForm(): void {
    this.selectedPersonId = null;
  }
}
