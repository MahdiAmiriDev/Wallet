import {Component, OnInit} from '@angular/core';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {BaseEdit} from '../../../shared/base/classes/base-edit';

@Component({
  selector: 'app-refund-detail-assigned-users',
  templateUrl: './refund-detail-assigned-users.component.html'
})
export class RefundDetailAssignedUsersComponent extends BaseEdit implements OnInit {
  entity: any = {};
  data: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
  }

  ngOnInit(): void {
    this.loadData();
  }

  /*
 دریافت اطلاعات
  */
  private loadData(): void {
    this.genericDataService.getEntity('RefundDetail', null,
      `GetAssignedUsers`)
      .subscribe(
        res => {

          if (res) {
            this.data = res;
            const selectedData: any = [];
            this.data.forEach((item, index) => {
              const start = 0;
              const end = item.questionCount;
              const dataSource = Array.from({length: (end + 1 - start)}, (v, k) => {
                return {id: k + start};
              });
              selectedData[index] = {
                assigneeFullName: item.assigneeFullName,
                assignedCount: item.count
              };
            });
            this.entity.selectedData = selectedData;
          } else {
            this.showError(`اطلاعاتی برای نمایش وجود ندارد.`);
          }
        }
      );
  }
}
