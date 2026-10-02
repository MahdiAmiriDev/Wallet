import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {SpecificationOptionDetailComponent} from './specification-option-detail.component';


@Component({
  selector: 'app-specification-option-list',
  templateUrl: './specification-option-list.component.html'
})
export class SpecificationOptionListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'specificationOption';
  detailComponent = SpecificationOptionDetailComponent;
  private baseListUrl = 'specificationOption/GetByCustomFilterForList?';

  constructor() {
    super();
  }

  _specificationId: number;

  get specificationId(): number {
    return this._specificationId;
  }

  @Input()
  set specificationId(val) {
    if (val !== this._specificationId) {
      this._specificationId = val;
    }
  }

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  afterInitialDetailComponent(componentInstance: any) {
    const comp = (<SpecificationOptionDetailComponent>componentInstance.instance);

    if (this.specificationId) {
      comp.specificationId = this.specificationId;
    }

  }

  private bindColumns() {
    this.columns.push({
      caption: 'مشخصه',
      dataField: 'specificationTitle',
      groupIndex: 1
    },{
      caption: 'ویژگی',
      dataField: 'title',
      allowGrouping: 0
    },{
      caption: 'ترتیب نمایش',
      dataField: 'displayOrder',
      allowGrouping: 0
    });
  }

  public bindCustomDataSources() {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.specificationId) {
        customListUrl += `specificationId=${this.specificationId}&`;
      }
    }
    let res = false;
    if (this.specificationId) {
      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;
  }
}
