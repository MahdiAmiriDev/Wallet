import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-factor-upload-file',
  templateUrl: './factor-upload-file.component.html'
})
export class FactorUploadFileComponent extends BaseEdit implements OnInit {

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
  }

  ngOnInit(): void {
    super.ngOnInit();
    this.entity = 'factor';
  }
}
