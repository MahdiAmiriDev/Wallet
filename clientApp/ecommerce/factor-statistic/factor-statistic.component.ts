import {Component, Input, OnInit} from '@angular/core';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-factor-statistic',
  templateUrl: './factor-statistic.component.html'
})
export class FactorStatisticComponent extends YashilComponent implements OnInit {


  @Input()
  loadForm = true;
  @Input()
  fixHeight = true;
  @Input()
  title: string;
  formHeight: number;
  // tslint:disable-next-line:variable-name
  private _factorId: number;
  @Input()
  set factorId(value: number) {
    if (value !== this.factorId){
      this._factorId = value;
      this.loadData();
    }
  }
  get factorId(): number {
    return this._factorId;
  }

  @Input()
  factorPaymentId: number;
  // tslint:disable-next-line:variable-name
  private _complexMode: boolean;

  get complexMode(): boolean {
    return this._complexMode;
  }
  @Input()
  set complexMode(value: boolean) {
    this._complexMode = value;
  }
  entityName: string;
  entity: any = {};
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super();
    this.entityName = 'factor';
  }

  ngOnInit(): void {
    if (this.fixHeight) {
      this.formHeight = window.innerHeight - 200;
    }
    this.loadData();
  }

  /*
  دریافت اطلاعات مربوط به پرداختی های فاکتور مثل
  مبلغ تاکنون پرداخت شده و درصدهای کسورات و درصدهای تخفیف و سود و مبالغ آنها
   */
  loadData(): void {
    let url = 'GetFactorStatistic?';
    if (this._factorId) {
      url = url + `factorId=${this._factorId}&`;
      if (this.factorPaymentId) {
        url = url + `factorPaymentId=${this.factorPaymentId}`;
      }
      this.genericDataService.getEntity(this.entityName, this._factorId, url)
        .subscribe(
          res => {
            this.entity = res;
            this.title = 'اطلاعات مربوط به فاکتور  ';
            if (this.entity.companyTitle){
              this.title += ' ' + this.entity.companyTitle;
            }
            if (this.entity.representationTitle){
              this.title +=  ' ' + this.entity.representationTitle;
            }
            if (this.entity.factorCode){
              this.title += ' ' + this.entity.factorCode ;
            }
          }
        );
    }
  }
}
