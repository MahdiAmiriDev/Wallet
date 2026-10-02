import {Component, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';


@Component({
  selector: 'app-hekmat-terminal-detail',
  templateUrl: './hekmat-terminal-detail.component.html'
})
export class HekmatTerminalDetailComponent extends BaseEdit implements OnInit {
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'hekmatTerminal';
  }

  ngOnInit(): void {
    super.ngOnInit();
  }
}
