import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {FactorVerificationConfigurationDetailComponent} from './factor-verification-configuration-detail.component';


@Component({
  selector: 'app-factor-verification-configuration-list',
  templateUrl: './factor-verification-configuration-list.component.html'
})
export class FactorVerificationConfigurationListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'factorVerificationConfiguration';
  detailComponent = FactorVerificationConfigurationDetailComponent;

  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'عنوان',
      dataField: 'title'
    },{
      caption: 'فعال',
      dataField: 'isActive'
    });
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as FactorVerificationConfigurationDetailComponent);

  }
}
