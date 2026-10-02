import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-terminal-filter',
  templateUrl: './terminal-filter.component.html'
})
export class TerminalFilterComponent implements OnInit {


  // tslint:disable-next-line:variable-name
  private _selectedMerchantId: number;

  get selectedMerchantId(): number {
    return this._selectedMerchantId;
  }


  set selectedMerchantId(val: number) {
    if (val && val !== this._selectedMerchantId) {
      this._selectedMerchantId = val;
    }
  }

  // tslint:disable-next-line:variable-name
  selectedYashilHekmatTerminalId: number;


  @Input()
  showInOneRow = true;
  @Input()
  isRequired: false;
  // terminalId: number;
  selectedData: any = {};
  @Output()
  selectedTerminalChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  selectedMerchantChanged: EventEmitter<any> = new EventEmitter<any>();
  terminalDataSource: any;
  contentHeight: number;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'representationPerson';
  merchantDataSource: any;
  constructor(private genericDataService: GenericDataService) {
    this.contentHeight = window.innerHeight - 110;
  }

  ngOnInit(): void {
    this.getUserMerchantId();
    if (this.selectedYashilHekmatTerminalId) {
      // this.firstLoad = true;
    }

    this.merchantDataSource = this.genericDataService.createCustomDataSourceWithAction('id', 'MerchantInfo', 'getUserMerchants');
    if (this.selectedMerchantId) {
      this.bindDataSources(this.selectedMerchantId);
    }
  }

  onSelectedMerchantChanged(item: any): void {
    if (item) {
      this.selectedData.merchant = item.selectedItem;
      this.selectedYashilHekmatTerminalId = null;
      this.selectedData.terminal = null;
    }
    this.selectedMerchantChanged.emit(this.selectedData);
    if (item.selectedItem) {
      this.bindDataSources(item.selectedItem.id);
    } else {
      this.terminalDataSource = null;
    }
  }

  private bindDataSources(merchantId): void {
    if (merchantId) {
      this.terminalDataSource =
        this.genericDataService.createCustomDataSourceWithAction('id', 'YashilHekmatTerminal', `GetByCustomForSelect?merchantId=${merchantId}`);
    } else {
      this.terminalDataSource = null;
    }
  }

  onSelectedYashilHekmatTerminalChanged(item: any): void {
    if (item) {
      this.selectedData.terminal = item.selectedItem;
      this.selectedTerminalChanged.emit(this.selectedData);
    } else {
      this.selectedData.terminal = null;
      this.selectedTerminalChanged.emit(this.selectedData);
    }
  }

  getUserMerchantId(): void {
    this.genericDataService.getEntity('MerchantInfo', null, 'GetCurrentUserMerchantId').subscribe(res => {
        if (res && res.id) {
          this.selectedMerchantId = res.id;
        }
      }
    );
  }
}
