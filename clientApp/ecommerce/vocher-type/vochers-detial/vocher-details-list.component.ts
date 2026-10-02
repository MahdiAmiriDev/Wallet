import {Component, Input, OnInit, ViewChild} from "@angular/core";
import {BaseList} from "../../../../shared/base/classes/base-list";
import {Selectable} from "../../../../shared/base/classes/selectable";

@Component({
  selector: 'app-vocher-type-Vochers-list',
  templateUrl: './vocher-details-list.component.html'
})

export class VocherDetailsListComponent extends Selectable implements OnInit {
  constructor() {
    super();
  }

  @ViewChild('listForm', {static: true}) listForm: BaseList;
  @Input() vocherTypeId:number;
  public readonly screen = screen;
  customDataSourceUrl = 'vocherType/GetVochersList?';
  columns: any[] =[];

  ngOnInit(): void {
    this.bindColumns();
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  private bindColumns(): void {
    this.columns.push({
      caption: 'شناسه کوپن',
      dataField: 'id',
      width: '15%',
      alignment: 'center'
    });
    this.columns.push({
      caption: 'شناسه  سفارش',
      dataField: 'orderId',
      width: '15%',
      alignment: 'center'
    });

    this.columns.push({
      caption: 'وضعیت',
      dataField: 'state',
      width: '30%',
      alignment: 'center',
      cellTemplate: (container, options) => {
        let message = options.data.state ? 'تخصیص یافته' : 'تخصیص نیافته';
        container.textContent = message;
      }
    });
  }

  bindCustomDataSources(): boolean {
    let res = false;
    let customListUrl = `${this.customDataSourceUrl}`;
    if (this.listForm) {
      if (this.vocherTypeId) {
        customListUrl += `vocherTypeId=${this.vocherTypeId}`;
        this.listForm.customListUrl = customListUrl;
        this.listForm.refreshList();
        res = true;
      }
    }
    return res;
  }
}
