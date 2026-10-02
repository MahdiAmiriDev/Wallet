import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-update-order-recipient-mobile-number-detail',
  templateUrl: './update-order-recipient-mobile-number-detail.component.html'
})
export class UpdateOrderRecipientMobileNumberDetailComponent extends BaseEdit implements OnInit {
  @Input()
  salesPlanningId: number;
  userDataSource: any;

  @Input()
  oldMobileNumber: string;

  customUpdateMethodName = 'updateOrderRecipientMobileNumber';

  @Output()
  onMobileNumberChanges : EventEmitter<any> = new EventEmitter<any>();

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'order';
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.entity.recipientMobileNumber = this.oldMobileNumber;
    this.entity.salesPlanningId = this.salesPlanningId;
  }

  mobileNumberValidate(param): boolean {
    const regex = /^(\+98|0098|98|0)?9\d{9}$/;
    return regex.test(param.value);
  }

  submitForm(withFile: boolean = false) {
    const dto = {
      id: this.salesPlanningId,
      recipientMobileNumber: this.entity.recipientMobileNumber,
      oldMobileNumber: this.oldMobileNumber
    };

    this.setBusy(true);
    this.genericDataService.postEntityByUrl(`/api/order/${this.customUpdateMethodName}/`, dto, false)
      .subscribe(response => {
        this.setBusy(false);
        if (response && response.isSuccess) {
          this.showMessage(response.message);
          this.selectedEntityId = null;
          this.updateOrInsertAffected = true;
          this.closeFormClick.emit(this.updateOrInsertAffected);
          this.onMobileNumberChanges.emit(this.entity.recipientMobileNumber);
        } else {
          this.showError(response.message);
        }
      });
  }
}
