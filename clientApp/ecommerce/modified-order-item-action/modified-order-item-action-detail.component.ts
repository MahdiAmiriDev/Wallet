import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {BusyIndicatorComponent} from '../../../core/components/busy-indicator/busy-indicator.component';


@Component({
  selector: 'app-modified-order-action-item-detail',
  templateUrl: './modified-order-item-action-detail.component.html'
})
export class ModifiedOrderItemActionDetailComponent extends BaseEdit implements OnInit {
  @ViewChild(BusyIndicatorComponent)
  busyIndicatorSetNewItem: BusyIndicatorComponent;
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
  @Input()
  orderId:number;
  @Input()
  expertDescription: string;

  oldOrderItemDataSource: any;
  newOrderDataSource: any;
  modifiedOrderItemTypes: any;
  modifiedOrderItemStatusTypes: any;
  assigneeDataSource: any;
  accessLevels: any[] = [];
  /**
   * Fill In List Form before Open This Form
   */
  oldItemForChange: any;
  /**
   * Fill When Select New Item From The List
   */
  selectedNewItem: any;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'modifiedOrderItem';
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.orderItemGridHeight = window.innerHeight - 220;
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

  afterSave(): void {
    super.afterSave();
    this.showDescriptionPopUp = false;
  }

  /**
   * This Method Raised After Select ProductSalePlanning From list
   * @param selectedProductSalePlanning Selected Item
   */
  openConfirmDialogBeforeSave(selectedProductSalePlanning: any): void {
    if (selectedProductSalePlanning.selectedRepresentation) {
      this.selectedNewItem = selectedProductSalePlanning;
      this.showDescriptionPopUp = true;
    } else {
      this.showError('کاربر گرامی لطفا ابتدا نمایندگی را انتخاب کنید وسپس اقدام به انتخاب محصول کنید.');
    }
  }

  /**
   * This Method Raised When Save Button Pushed On Confirm Dialog
   */
  setNewOrderItem(): void {
    this.busyIndicatorSetNewItem.isBusy = true;
    if (this.selectedNewItem) {
      super.afterSave();
      this._genericDataService.updateEntity('ModifiedOrderItem', {
        id: this.selectedEntityId,
        productSalePlanningId: this.selectedNewItem.item.id,
        representationId: this.selectedNewItem.selectedRepresentation.id,
        expertDescription: this.expertDescription,
      }, 'UpdateNewOrderItem').subscribe(response => {
        if (!response.isSuccess) {
          this.showError(response.message);
        } else {
          this.showMessage(response.message);
        }
        this.busyIndicatorSetNewItem.isBusy = false;
        this.showDescriptionPopUp = false;
        this.updateOrInsertAffected = true;
        this.onCloseFormClick();
      });
    }
  }
}
