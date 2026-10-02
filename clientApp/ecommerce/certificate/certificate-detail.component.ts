import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {CommonValidatorService} from '../../../shared/services/common-validator.service';



@Component({
  selector: 'app-certificate-detail',
  templateUrl: './certificate-detail.component.html'
})
export class CertificateDetailComponent extends BaseEdit implements OnInit {


  constructor(private genericDataService: GenericDataService ,public commonValidatorService: CommonValidatorService) {
    super(genericDataService);
    this.entityName = 'certificate';
  }
z
  ngOnInit(): void {
    super.ngOnInit();

  }
}
