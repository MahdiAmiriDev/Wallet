import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-factor-deduction-detail',
  templateUrl: './factor-deduction-detail.component.html'
})
export class FactorDeductionDetailComponent extends BaseEdit implements OnInit {
  @Input()
  factorId: number;
  factorDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'factorDeduction';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.factorId) {
      this.entity.factorId = this.factorId;
    }
  }
}
