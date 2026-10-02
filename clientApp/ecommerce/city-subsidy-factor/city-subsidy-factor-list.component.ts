import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CitySubsidyFactorDetailComponent} from './city-subsidy-factor-detail.component';


@Component({
  selector: 'app-city-subsidy-factor-list',
  templateUrl: './city-subsidy-factor-list.component.html'
})
export class CitySubsidyFactorListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'citySubsidyFactor';
  detailComponent = CitySubsidyFactorDetailComponent;
  _cityId: number;
  @Input()
  set cityId(val) {
    if (val !== this._cityId) {
      this._cityId = val;
    }
  }

  get cityId(): number {
    return this._cityId;
  }

  private baseListUrl = 'citySubsidyFactor/GetByCustomFilterForList?';

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
      caption: 'شهر',
      dataField: 'cityTitle'

    });


    this.columns.push({
      caption: 'ضریب یارانه وزارت',
      dataField: 'vezaratSubsidyFactor'

    });


    this.columns.push({
      caption: 'ضریب یارانه ساتا',
      dataField: 'sataSubsidyFactor'

    });
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
      if (this.cityId) {
        customListUrl += `cityId=${this.cityId}&`;
      }
    }
    let res = false;
    if (this.cityId) {

      this.listForm.customListUrl = customListUrl;
      this.listForm.refreshList();
      res = true;
    }
    return res;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as CitySubsidyFactorDetailComponent);

    if (this.cityId) {
      comp.cityId = this.cityId;
    }

  }
}
