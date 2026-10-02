import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, OnInit, ViewChild} from '@angular/core';
import {HekmatTerminalDetailComponent} from './hekmat-terminal-detail.component';


@Component({
  selector: 'app-hekmat-terminal-list',
  templateUrl: './hekmat-terminal-list.component.html'
})
export class HekmatTerminalListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'hekmatTerminal';
  detailComponent = HekmatTerminalDetailComponent;
  private baseListUrl = 'hekmatTerminal/GetByCustomFilterForList?';

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
    this.columns.push(
      {
        caption: 'عنوان',
        dataField: 'title'
      },
      {
        caption: 'شماره ترمینال مربوط به ای پی آی حکمت',
        dataField: 'terminalNumber'
      },
      {
        caption: 'سقف تعداد اقساط',
        dataField: 'maxInstallmentCount'
      },
      {
        caption: 'آیا شماره ترمینال آموزشی می باشد ؟',
        dataField: 'isLearn'
      },
      {
        caption: 'فعال',
        dataField: 'isActive'
      },
    );
  }

  bindCustomDataSources(): boolean {
    let customListUrl = `${this.baseListUrl}`;
    if (this.listForm) {
    }
    this.listForm.refreshList();
    return false;

  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as HekmatTerminalDetailComponent);

  }
}
