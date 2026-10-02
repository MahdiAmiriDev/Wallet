import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Observable} from 'rxjs';


@Component({
  selector: 'app-modified-order-item-detail',
  templateUrl: './modified-order-item-detail.component.html'
})
export class ModifiedOrderItemDetailComponent extends BaseEdit implements OnInit {
  orderItemGridHeight: number;
  @Input()
  showDescriptionPopUp = false;
  @Input()
  oldOrderItemId: number;
  @Input()
  newOrderId: number;
  @Input()
  modifiedOrderItemType: number;
  @Input()
  modifiedOrderItemStatusType: number;
  @Input()
  assignee: number;
  oldOrderItemDataSource: any;
  newOrderDataSource: any;
  modifiedOrderItemTypes: any;
  modifiedOrderItemStatusTypes: any;
  assigneeDataSource: any;
  accessLevels: any[] = [];
  disabled: boolean;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'modifiedOrderItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.orderItemGridHeight = window.innerHeight - 150;
    if (this.oldOrderItemId) {
      this.entity.oldOrderItemId = this.oldOrderItemId;
    }
    if (this.newOrderId) {
      this.entity.newOrderId = this.newOrderId;
    }
    if (this.modifiedOrderItemType) {
      this.entity.modifiedOrderItemType = this.modifiedOrderItemType;
    }
    if (this.modifiedOrderItemStatusType) {
      this.entity.modifiedOrderItemStatusType = this.modifiedOrderItemStatusType;
    }
    if (this.assignee) {
      this.entity.assignee = this.assignee;
    }

    this.oldOrderItemDataSource = this._genericDataService.createCustomDataSourceWithCustomUrl('id', 'orderItem/GetLimitedInformation');
    this.newOrderDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'orderItem');
    this._genericDataService.getCommonBaseDataForSelect('ModifiedOrderItemType')
      .subscribe(res => this.modifiedOrderItemTypes = res);
    this._genericDataService.getCommonBaseDataForSelect('ModifiedOrderItemStatusType')
      .subscribe(res => this.modifiedOrderItemStatusTypes = res);
    this.assigneeDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'user');
  }

  /*
1195	لغو سفارش
1196	تغییر سفارش
*/
  onOrderItemCanceled(id: number): void {
    this.entity.oldOrderItemId = id;
    this.entity.ModifiedOrderItemType = 1195;
    this.showDescriptionPopUp = true;
  }

  onOrderItemEdited(id: number): void {
    this.entity.oldOrderItemId = id;
    this.entity.ModifiedOrderItemType = 1196;
    this.showDescriptionPopUp = true;
  }

  /*
     دکمه ذخیره را غیر فعال می کنیم.
  */
  doBeforeSubmit(e): Observable<boolean> {
    this.disabled = true;
    return super.doBeforeSubmit(e);
  }
   /*
   بعد از ذخیره فرم popup  بسته می شود
   */
  afterSave(): void {
    super.afterSave();
    this.showDescriptionPopUp = false;
  }
}
