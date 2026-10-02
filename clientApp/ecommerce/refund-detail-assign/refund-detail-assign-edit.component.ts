import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
  selector: 'app-refund-detail-assign-edit',
  templateUrl: './refund-detail-assign-edit.component.html'
})
export class RefundDetailAssignEditComponent extends BaseEdit implements OnInit {
  currentUser: any;
  authorizationHeader: any;
  assigneeListDataSource: any;
  @Output()
  onRefundDetailAssigned: EventEmitter<any> = new EventEmitter<any>();

  constructor(private genericDataService: GenericDataService,private  userDataService:UserDataService) {
    super(genericDataService);
    this.entityName = 'refundDetail';
    this.currentUser = this.userDataService.getCurrentUser();
    this.authorizationHeader = {
      Authorization: `Bearer ${this.currentUser.token}`
    };
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.assigneeListDataSource = this._genericDataService.createCustomDataSourceWithAction('id', 'user', 'GetCrmUser');
  }

  assignRefundDetail(): any {
    this.genericDataService.postEntityByUrl('/api/refundDetail/AssignRefundDetail', this.entity)
      .subscribe(response => {
        if (!response.isSuccess) {
          this.showError(response.message);
        } else {
          this.showMessage(response.message);
          this.onRefundDetailAssigned.emit();
          this.closeFormClick.emit();
        }
      });
  }

  /*
  کنسل کردن و بستن فرم توضیحات آن
   */
  public cancelForm(): void {
    this.closeFormClick.next(true);
  }

  dropDownDisplayExpr(item: any): string {
    let rowTitle = item.userName;
    if (item.firstName) {
      rowTitle += ` ${item.firstName}`;
    }
    if (item.lastName) {
      rowTitle += ` ${item.lastName}`;
    }
    return rowTitle;
  }
}
