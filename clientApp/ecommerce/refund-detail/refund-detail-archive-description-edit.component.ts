import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
  selector: 'app-refund-detail-archive-description-edit',
  templateUrl: './refund-detail-archive-description-edit.component.html'
})
export class RefundDetailArchiveDescriptionEditComponent extends BaseEdit implements OnInit {
  @Output()
  onRefundDetailArchived: EventEmitter<any> = new EventEmitter<any>();
  currentUser: any;
  authorizationHeader: any;

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
  }

  archiveRefundDetail(): any {
    this.genericDataService.postEntityByUrl('/api/refundDetail/archiveRefundDetail', this.entity)
      .subscribe(response => {
        if (!response.isSuccess) {
          this.showError(response.message);
        } else {
          this.showMessage(response.message);
          this.onRefundDetailArchived.next(true);
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
}
