import {Component, Input, OnInit} from '@angular/core';


@Component({
  selector: 'app-factor-group-report-tab-based',
  templateUrl: './factor-group-report-tab-based.component.html'
})
export class FactorGroupReportTabBasedComponent implements OnInit {
  parentId: number;
  factorGroupStatus: number;
  tabs: any[] = [];
  @Input()
  factorGroupId: number;

  constructor() {
  }

  ngOnInit(): void {
    this.bindTabs();
  }
  bindTabs(): void {
    this.tabs = [
      {id: 1, title: 'گزارش کانورت', template: 'factorGroupConvertReport'},
      {id: 2, title: 'گزارش فاکتورهای گروه', template: 'factorGroupItemReport'},
      {id: 3, title: 'ریز آیتم های گروه', template: 'factorGroupFactorItems'},
      {id: 4, title: 'ریز پرداخت های گروه', template: 'factorGroupFactorItemPayments'},
    ];
  }




}


