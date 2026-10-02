import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {UserDataService} from '../../../_services/user-data.service';


@Component({
  selector: 'app-factor-verification-description-edit',
  templateUrl: './factor-verification-description-edit.component.html'
})
export class FactorVerificationDescriptionEditComponent extends BaseEdit implements OnInit {
  @Input()
  factorConfirmationType: number;
  @Input()
  factorVerificationConfigurationRoleId: number;
  @Output()
  onFactorVerificationRejected: EventEmitter<any> = new EventEmitter<any>();
  currentUser: any;
  authorizationHeader: any;

  constructor(private genericDataService: GenericDataService,private  userDataService:UserDataService) {
    super(genericDataService);
    this.entityName = 'factorVerification';
    this.currentUser = this.userDataService.getCurrentUser();
    this.authorizationHeader = {
      Authorization: `Bearer ${this.currentUser.token}`
    };
  }

  ngOnInit(): void {
    super.ngOnInit();
  }

  rejectFactorVerification(): any {
    if (!this.entity.description) {
      this.showError('کاربر گرامی هنگام رد کردن فاکتور توضیحات لازم است');
      return;
    }
    this.genericDataService.postEntityByUrl('/api/factorVerification/RejectFactorVerification', this.entity)
      .subscribe(response => {
        if (!response.isSuccess) {
          this.showError(response.message);
        } else {
          // this.listForm.refreshList();
          this.showMessage(response.message);
          this.onFactorVerificationRejected.next(true);
          this.closeFormClick.emit();
        }
      });
  }

  /*
  کنسل کردن رد فاکتور و بستن فرم توضیحات آن
   */
  public cancelForm(): void {
    this.closeFormClick.next(true);
  }
}
