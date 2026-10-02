import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {CertificateDetailComponent} from './certificate-detail.component';


@Component({
  selector: 'app-certificate-list',
  templateUrl: './certificate-list.component.html'
})
export class CertificateListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  entityName = 'certificate';
  detailComponent = CertificateDetailComponent;
  searchedNationalCode: string;
  nationalCode: string;


  constructor() {
    super();
  }

  ngOnInit(): void {
    this.bindColumns();
    if (!this.loadAfterSetFilter) {
      if (this.bindCustomDataSources()) {
        this.loadAfterSetFilter = true;
      }
    }
  }

  private bindColumns(): void {
    this.columns.push(
      {
        caption: 'تاریخ صدور',
        dataField: 'issueDateInt',
        cellTemplate: 'intDate',
        allowHeaderFiltering: false
      },
      {
        caption: 'شماره گواهینامه',
        dataField: 'number'
      },
      {
        caption: 'شهر',
        dataField: 'cityTitle',
        allowHeaderFiltering: false
      },
      {
        caption: 'نام دوره',
        dataField: 'courseTitle',
        allowHeaderFiltering: false
      },
      {
        caption: 'نوع دوره',
        dataField: 'courseType',
        allowHeaderFiltering: false
      },
      {
        caption: 'مدت دوره',
        dataField: 'courseDuration',
        allowHeaderFiltering: false
      },
      {
        caption: 'نام',
        dataField: 'name',
        allowHeaderFiltering: false
      },
      {
        caption: 'نام خانوادگی',
        dataField: 'lastName',
        allowHeaderFiltering: false
      },
      {
        caption: 'نام پدر',
        dataField: 'fatherName',
        allowHeaderFiltering: false
      },
      {
        caption: 'کد ملی',
        dataField: 'nationalCode',
        allowHeaderFiltering: false
      },
      {
        caption: 'نمره',
        dataField: 'score',
        allowHeaderFiltering: false
      },
      {
        caption: 'هزینه دوره',
        dataField: 'coursePrice',
        cellTemplate: 'number',
        allowHeaderFiltering: false

      },
    );
  }

  search(): void {
    this.nationalCode = '';
    if (this.searchedNationalCode && this.searchedNationalCode.length > 0) {
      this.nationalCode = this.searchedNationalCode;
    }
    this.bindCustomDataSources();
  }

  afterInitialDetailComponent(componentInstance: any): void {
    const comp = (componentInstance.instance as CertificateDetailComponent);

  }


  public bindCustomDataSources(): boolean {
    let customListUrl = `certificate/GetByCustomFilterForList?`;
    if (this.listForm) {
      if (this.nationalCode) {
        customListUrl += `nationalCode=${this.nationalCode}`;
      }
    }
    this.listForm.customListUrl = customListUrl;
    this.listForm.refreshList();
    return true;
  }
}
