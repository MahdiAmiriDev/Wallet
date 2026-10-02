import {Selectable} from "../../../../shared/base/classes/selectable";
import {AfterViewInit, Component, Input, OnInit, ViewChild} from "@angular/core";
import {BaseList} from "../../../../shared/base/classes/base-list";
import {GenericDataService} from "../../../../shared/base/services/generic-data.service";
import {YashilCustomButtonColumn} from "../../../../shared/base/classes/yashil-cutom-button-column";

@Component({
  selector: 'app-factor-payment-installment-reference-list',
  templateUrl: './factor-payment-installment-reference-list.component.html'
})
export class FactorPaymentInstallmentReferenceListComponent
  extends Selectable
  implements OnInit, AfterViewInit {

  @ViewChild('listForm', {static: true})
  listForm: BaseList;

  columns: any[] = [];
  entityName = 'uploadedFactorInstallmentPayment';
  private baseListUrl =
    'uploadedFactorInstallmentPayment/getFactorPaymentInstallmentReferenceGroup?';

  @Input()
  yearMonth: number;
  customRowButtons: YashilCustomButtonColumn;
  showReferenceDetails = false;
  paymentReferenceDate: string;
  creationDate:string;
  paymentReferenceId:string;

  constructor(private genericDataService: GenericDataService) {
    super();
    this.showSelectItemReferenceDetails = this.showSelectItemReferenceDetails.bind(this);
  }

  ngOnInit(): void {
    this.bindColumns();
    this.bindCustomRowButtons();
  }

  ngAfterViewInit(): void {
    if (this.bindCustomDataSources()) {
      this.loadAfterSetFilter = true;
    }
  }

  public bindCustomDataSources(): boolean {
    if (!this.yearMonth || !this.listForm) {
      return false;
    }

    this.listForm.customListUrl = `${this.baseListUrl}yearMonth=${this.yearMonth}&`;
    this.listForm.refreshList();

    return true;
  }

  bindColumns(): void {
    this.columns = [
      {
        caption: 'زمان فایل بارگذاری شده',
        dataField: 'creationPersianDateTime',
        visibleIndex: 0,
        alignment: 'right'
      },
      {
        caption: 'شناسه مرجع پرداخت',
        dataField: 'paymentReferenceId',
        visibleIndex: 1,
        alignment: 'right'
      },
      {
        caption: 'تاریخ مرجع پرداخت',
        dataField: 'paymentReferencePersianDateTime',
        visibleIndex: 2,
        alignment: 'right',
        cellTemplate: 'intDate'
      }
    ];
  }

  bindCustomRowButtons(): void {
    this.customRowButtons = {
      width: '180px',
      setAtFirstColumn: true,
      customButtons: [
        {
          text: 'جزئیات و ثبت شناسه',
          hint: 'نمایش جزئیات و ثبت شناسه مرجع پرداخت',
          icon: 'checklist',
          type: 'default',
          onClick: this.showSelectItemReferenceDetails,
          width: '180px'
        }
      ]
    };
  }

  showSelectItemReferenceDetails(item: any): void {
    this.paymentReferenceDate = item.data.paymentReferencePersianDateTime;
    this.yearMonth = item.data.yearMonth;
    this.creationDate = item.data.creationDate;
    this.paymentReferenceId = item.data.paymentReferenceId;
    this.showReferenceDetails = true;
  }

  closePopUp(): void {
    this.listForm.refreshList();
  }
}
