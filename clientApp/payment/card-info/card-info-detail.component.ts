import {Component, Input, OnInit} from '@angular/core';
import {BaseEdit} from '../../../shared/base/classes/base-edit';
import {GenericDataService} from '../../../shared/base/services/generic-data.service';
import {Entity} from '../../../shared/base/base-data/entity.enum';


@Component({
  selector: 'app-card-info-detail',
  templateUrl: './card-info-detail.component.html'
})
export class CardInfoDetailComponent extends BaseEdit implements OnInit {
  @Input()
  cardType: number;
  @Input()
  personId: number;
  accessLevels: any[] = [];
  cardTypes: any;
  personDataSource: any;

  constructor(private genericDataService: GenericDataService) {
    super(genericDataService);
    this.entityName = 'cardInfo';
  }

  ngOnInit(): void {
    super.ngOnInit();
    if (this.cardType) {
      this.entity.cardType = this.cardType;
    }
    if (this.personId) {
      this.entity.personId = this.personId;
    }

    this._genericDataService.getEntitiesByEntityNameForSelect(Entity.AccessLevel).subscribe(res => this.accessLevels = res);
    this._genericDataService.getCommonBaseDataForSelect('CardType').subscribe(res => this.cardTypes = res);
    this.personDataSource = this._genericDataService.createCustomDataSourceForSelect('id', 'person');
  }

  cardValidation(params) {
    const cardNumber = params.value;
    if (typeof cardNumber === 'undefined'
      || cardNumber === null
      || cardNumber.length !== 16) {
      return false;
    }
    let cardTotal = 0;
    for (let i = 0; i < 16; i += 1) {
      const c = Number(cardNumber[i]);
      if (i % 2 === 0) {
        cardTotal += ((c * 2 > 9) ? (c * 2) - 9 : (c * 2));
      } else {
        cardTotal += c;
      }
    }
    return (cardTotal % 10 === 0);
  }

  expireDateValidation(params) {
    const num = params.value;
    if (typeof num === 'undefined'
      || num === null) {
      return true;
    }
    const reg = new RegExp('^\\d+$');
    return reg.test(num);
  }
}
