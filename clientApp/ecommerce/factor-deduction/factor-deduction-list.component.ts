import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {FactorDeductionDetailComponent} from './factor-deduction-detail.component';


@Component({
  selector: 'app-factor-deduction-list',
  templateUrl: './factor-deduction-list.component.html'
})
export class FactorDeductionListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'factorDeduction';
  detailComponent = FactorDeductionDetailComponent;
  // tslint:disable-next-line:variable-name
  _factorId: number;
  @Input()
  set factorId(val) {
    if (val !== this._factorId) {
      this._factorId = val;
    }
  }
  get factorId(): number {
    return this._factorId;
  }

  @Input()
  showAddButton: boolean;
  @Input()
  allowEditInForm: boolean;
  @Input()
  allowDeleteInForm: boolean;
  private baseListUrl = 'factorDeduction/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title',
      width: '250px',
    },{
      caption: 'قیمت(ریال)',
      dataField: 'price',
      width: '180px',
      cellTemplate: 'number',
    },{
      caption: 'توضیحات',
      dataField: 'description'
    });
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.factorId) {
        customListUrl += `factorId=${this.factorId}&`;
      }
    }
    let res = false;
    if (this.factorId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorDeductionDetailComponent);
    if (this.factorId) {
      comp.factorId = this.factorId;
    }

  }
}
