import {Component, Input, OnInit} from '@angular/core';


@Component({
  selector: 'app-factor-payment-tab-based',
  templateUrl: './factor-payment-tab-based.component.html'
})
export class FactorPaymentTabBasedComponent implements OnInit {
  tabs: any[] = [];
  @Input()
  factorId: number;

  constructor() {
  }

  ngOnInit(): void {
    this.bindTabs();
  }
  bindTabs(): void {
    this.tabs = [
      {id: 1, title: 'پرداختی های فاکتور', template: 'factorPaymentList'},
      {id: 2, title: 'کسورات فاکتور', template: 'factorDeductionList'},
    ];
  }




}


