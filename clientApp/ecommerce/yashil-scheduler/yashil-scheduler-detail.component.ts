import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import * as moment from 'jalali-moment';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-yashil-scheduler-detail',
  templateUrl: './yashil-scheduler-detail.component.html'
})
export class YashilSchedulerDetailComponent extends BaseEdit implements OnInit {
  startDateDataValue = new FormControl();
  endDateDataValue = new FormControl();
  @Input()
  occuranceType: number;
  occuranceTypes: any;
  accessLevels: any[] = [];
  toEraMaximumMessageError: string;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'yashilScheduler';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.occuranceType) {
      this.entity.occuranceType = this.occuranceType;
    }
    this.entity.startDate = +moment().locale('fa').format('YYYYMMDD');
    this.entity.repeatNumber = 1;
    this._genericDataService.getCommonBaseDataForSelect('OccuranceType').subscribe(res => this.occuranceTypes = res);
  }
}
