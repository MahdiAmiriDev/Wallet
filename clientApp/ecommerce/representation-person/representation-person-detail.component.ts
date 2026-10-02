import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';
import {FormControl} from '@angular/forms';


@Component({
  selector: 'app-representation-person-detail',
  templateUrl: './representation-person-detail.component.html'
})
export class RepresentationPersonDetailComponent extends BaseEdit implements OnInit {
  fromDateDataValue = new FormControl();
  toDateDataValue = new FormControl();
  @Input()
  representationId: number;
  @Input()
  personId: number;
  @Input()
  postId: number;
  representationDataSource: any;
  personDataSource: any;
  postDataSource: any;
  accessLevels: any[] = [];

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'representationPerson';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.representationId) {
      this.entity.representationId = this.representationId;
    }
    if (this.personId) {
      this.entity.personId = this.personId;
    }
    if (this.postId) {
      this.entity.postId = this.postId;
    }

    this.representationDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'representation');
    this.personDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'person');
    this.postDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'post');
    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
  }
}
