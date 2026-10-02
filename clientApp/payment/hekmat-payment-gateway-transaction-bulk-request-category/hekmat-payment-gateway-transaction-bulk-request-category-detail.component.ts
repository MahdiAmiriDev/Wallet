import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {HekmatTerminalEnum} from "../../../shared/enums/hekmat-Terminal-enum";


@Component({
  selector: 'app-hekmat-payment-gateway-transaction-bulk-request-category-detail',
  templateUrl: './hekmat-payment-gateway-transaction-bulk-request-category-detail.component.html'
})
export class HekmatPaymentGatewayTransactionBulkRequestCategoryDetailComponent extends BaseEdit implements OnInit {

  @Input()
  parentId: number;
  @Input()
  yashilHekmatTerminalId: number;

  hekmatPaymentGatewayTransactionBulkRequestCategoryDataSource: any;
  parentDataSource: any;

  yashilHekmatTerminalDataSource: any;

  accessLevels: any[] = [];


  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'hekmatPaymentGatewayTransactionBulkRequestCategory';
  }

  ngOnInit(): void {
    super.ngOnInit();


    if (this.parentId)
      this.entity.parentId = this.parentId;
    if (this.yashilHekmatTerminalId)
      this.entity.yashilHekmatTerminalId = this.yashilHekmatTerminalId;
    this.hekmatPaymentGatewayTransactionBulkRequestCategoryDataSource =
      this._genericDataService.createCustomDataSourceForSelect('id', 'hekmatPaymentGatewayTransactionBulkRequestCategory');


    this._genericDataService.getEntitiesWithAction('YashilHekmatTerminal',
      'GetByCustomForSelect?hekmatTerminalId=' + HekmatTerminalEnum.EducationalAssistance, null)
      .subscribe((res: any) => {
        if (res && res.data) {
          this.yashilHekmatTerminalDataSource = res.data;
        }
      });

    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);

  }
}
