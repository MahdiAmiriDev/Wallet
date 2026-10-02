import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-yashil-hekmat-terminal-detail',
  templateUrl: './yashil-hekmat-terminal-detail.component.html'
})
export class YashilHekmatTerminalDetailComponent extends BaseEdit implements OnInit {
  @Input()
  hekmatTerminalId: number;
  @Input()
  merchantId: number;
  hekmatTerminalDataSource: any;
  merchantDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'yashilHekmatTerminal';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.hekmatTerminalId)
      this.entity.hekmatTerminalId = this.hekmatTerminalId;
    if (this.merchantId)
      this.entity.merchantId = this.merchantId;

    this.hekmatTerminalDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'hekmatTerminal');
  }

}
