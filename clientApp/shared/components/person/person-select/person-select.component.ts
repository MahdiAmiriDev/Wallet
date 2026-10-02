import { Component, Input, OnInit } from '@angular/core';
import { Selectable } from '../../../base/classes/selectable';
import { GenericDataService } from '../../../base/services/generic-data.service';
import { TranslateService } from '@ngx-translate/core';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-person-select',
  templateUrl: './person-select.component.html'
})
export class PersonSelectComponent extends Selectable implements OnInit {
  @Input()
  isRequired: boolean;
  @Input()
  requiredMessage: string;
  @Input()
  isReadOnly: boolean;
  // tslint:disable-next-line:variable-name
  private _selectedItemId: number;
  @Input()
  set selectedItemId(value: number) {
    if (value !== this._selectedItemId) {
      this._selectedItemId = value;
      this.selectedRowChanged.emit(value);
    }
  }

  get selectedItemId(): number {
    return this._selectedItemId;
  }

  columns: any[] = [];
  entityName = 'Person';
  personDataSource: any;
  @Input()
  customListUrl: string;
  @Input()
  lazyLoading = true;
  @Input()
  showOnlyWithNationalCode = false;
  constructor(private genericDataService: GenericDataService, private translate: TranslateService) {
    super();

  }

  ngOnInit(): void {
    if (!this.customListUrl) {
        this.personDataSource = this.genericDataService.createCustomDataSourceWithCustomUrl('id', `person/GetByCustomFilterForSelect?showOnlyWithNationalCode=${this.showOnlyWithNationalCode}&`);
    } else {
      this.personDataSource = this.genericDataService.createCustomDataSourceWithCustomUrl('id', this.customListUrl);
    }
    let result = forkJoin({
      nationalCodeTitle: this.translate.get('um.Person.NationalCode'),
      nameAndLastName: this.translate.get('NameAndLastName')


    });

    result.subscribe(({ nationalCodeTitle, nameAndLastName }) => {
      this.columns.push({
        caption: nationalCodeTitle,
        dataField: 'nationalCode',
        allowHeaderFiltering: false,
      }, {
        caption: nameAndLastName,
        dataField: 'title',
        allowHeaderFiltering: false,
      });
    });
  }

  selectedPersonChanged(e: any): void {
    if (e && e.selectedRowsData && e.selectedRowsData.length > 0) {
      this.selectedItemId = e.selectedRowsData[0].id;
    }
  }
}
