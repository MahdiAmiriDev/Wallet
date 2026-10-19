import {Component, OnInit, ViewChild} from '@angular/core';
import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {WalletProgramDetailComponent} from './wallet-program-detail.component';

@Component({
  selector: 'app-wallet-program-list',
  templateUrl: './wallet-program-list.component.html'
})
export class WalletProgramListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  entityName = 'walletProgram';
  detailComponent = WalletProgramDetailComponent;
  columns: any[] = [];
  statusItems = [
    {id: 3168, title: 'پیش‌نویس'}, {id: 3169, title: 'فعال'},
    {id: 3170, title: 'معلق'}, {id: 3171, title: 'منقضی'},
    {id: 3172, title: 'بسته'}
  ];

  /** ستون‌های وضعیت، بودجه و بازه اعتبار Program را برای مدیر نمایش می‌دهد. */
  ngOnInit(): void {
    this.columns = [
      {caption: 'کد', dataField: 'code'},
      {caption: 'عنوان', dataField: 'title'},
      {caption: 'وضعیت', dataField: 'walletProgramStatusType',
        lookup: {dataSource: this.statusItems, valueExpr: 'id', displayExpr: 'title'}},
      {caption: 'بودجه کل', dataField: 'totalAmount', dataType: 'number'},
      {caption: 'تخصیص‌یافته', dataField: 'allocatedAmount', dataType: 'number'},
      {caption: 'رزروشده', dataField: 'reservedAmount', dataType: 'number'},
      {caption: 'شروع', dataField: 'startDate', dataType: 'date'},
      {caption: 'پایان', dataField: 'endDate', dataType: 'date'}
    ];
  }
}
