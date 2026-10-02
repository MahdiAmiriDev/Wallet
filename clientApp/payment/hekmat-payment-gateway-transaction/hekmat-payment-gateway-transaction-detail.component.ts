import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-hekmat-payment-gateway-transaction-detail',
  templateUrl: './hekmat-payment-gateway-transaction-detail.component.html'
})
export class HekmatPaymentGatewayTransactionDetailComponent extends BaseEdit implements OnInit {
  additionalDateDataValue = new FormControl();
  approveDateDataValue = new FormControl();
  approveDateIntDataValue = new FormControl();
  @Input()
  hekmatTerminalId: number;
  hekmatTerminalDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'hekmatPaymentGatewayTransaction';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.hekmatTerminalId) {
      this.entity.hekmatTerminalId = this.hekmatTerminalId;
    }

    this.hekmatTerminalDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'hekmatTerminal');
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
