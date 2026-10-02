import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {TaxCategoryDetailComponent} from './tax-category-detail.component';


@Component({
  selector: 'app-tax-category-list',
  templateUrl: './tax-category-list.component.html'
})
export class TaxCategoryListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'taxCategory';
  detailComponent = TaxCategoryDetailComponent;
  private baseListUrl = 'taxCategory/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<TaxCategoryDetailComponent> componentInstance.instance);

  }

  private bindColumns() {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title'
    });
    this.columns.push({
      caption: 'درصد مالیات',
      dataField: 'percentage'
    });
    this.columns.push({
      caption: 'اجازه حذف',
      dataField: 'allowDelete'
    });
    this.columns.push({
      caption: 'شامل بیمه است',
      dataField: 'isIncludeInsurance'
    });
  }


}
