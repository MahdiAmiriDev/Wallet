import {AfterViewInit, Component, Input, OnInit, ViewChild} from "@angular/core";
import {BaseList} from "../../../shared/base/classes/base-list";
import {GenericDataService} from "../../../shared/base/services/generic-data.service";
import {BaseEdit} from "../../../shared/base/classes/base-edit";
import {FactorPaymentDetailComponent} from "../factor-payment/factor-payment-detail.component";

@Component({
    selector: 'app-factor-payment-refunded-subsidy-details',
    templateUrl: './factor-payment-refunded-subsidy-details.component.html'
})


export class FactorPaymentRefundedSubsidyDetailsComponent extends BaseEdit implements OnInit, AfterViewInit {
    @ViewChild('listForm', {static: true}) listForm: BaseList;

    loadAfterSetFilter: boolean;
    columns: any[] = [];
    _factorId: number;
    private baseListUrl = 'api/orderItem/GetAllRefundedOrderItemsByDate?';
    radioButtonItems = [];
    subsidyType: number;
    contentHeight: number;

    public readonly screen = screen;


    constructor(private genericDataService: GenericDataService) {
        super(genericDataService);
        this.entityName = 'factorItem';
    }

    @Input()
    set factorId(val) {
        if (val !== this._factorId) {
            this._factorId = val;
        }
    }

    get factorId(): number {
        return this._factorId;
    }

    ngOnInit(): void {
        this.radioButtonItems =
            [
              {text: 'همه موارد', value: 0},
              {text: 'ساتا', value: 1106},
              {text: 'وزارت', value: 1107}];
      this.subsidyType = 0;

    }


    ngAfterViewInit(): void {
        this.bindColumns();
        if (this.bindCustomDataSources()) {
            this.loadAfterSetFilter = true;
        }
    }


    private bindColumns(): void {
      this.columns.push(
        {
          caption: 'شماره فاکتور',
          dataField: 'id',
          width: '120px'
        },
        {
          caption:'نوع یارانه',
          dataField: 'subsidyType',
          width: '120px'
        }
        ,
        {
          caption:'عنوان',
          dataField: 'factorGroupTitle',
          width: '120px'
        }
        , {
          caption: 'شماره کارت',
          dataField: 'cardNumber',
          width: '140px'
        }
        , {
          caption: 'کد ملی',
          dataField: 'nationalCode',
          width: '140px',
        },
        {
          caption: 'تاریخ خرید',
          dataField: 'paidDateInt',
          width: '120px',
          cellTemplate: 'intDate'
        }
        ,
        {
          caption: 'تاریخ عودت',
          dataField: 'refundDateInt',
          width: '120px',
          cellTemplate: 'intDate'
        },
        {
          caption: 'کانورت شده',
          dataField: 'isConverted',
          width: '100px',
        }
        , {
          caption: 'یارانه خام',
          dataField: 'subsidyPrice',
          width: '120px',
          cellTemplate: 'number',
        }
        , {
          caption: 'یارانه باکارمزد',
          dataField: 'subsidyPriceIncProfit',
          width: '120px',
          cellTemplate: 'number',
        }
        , {
          caption: 'یارانه با کارمزد و مالیات',
          dataField: 'subsidyPriceIncTaxIncProfit',
          width: '120px',
          cellTemplate: 'number',
        },
      );
    }

    //دریافت گزارش فاکتورهایی که یارانه گرفته و عودت وجه رخ داده است
    bindCustomDataSources(): any {
        this.genericDataService.getCustomEntitiesByUrl(this.baseListUrl + `factorId=${this.factorId}&subsidyType=${this.subsidyType}`)
            .subscribe(
                (res: any) => {
                    if (res) {
                        this.listForm.gridDataSource = res.data;
                    }
                }
            );
    }


    afterInitialDetailComponent(componentInstance: any): void {
        const comp = (componentInstance.instance as FactorPaymentDetailComponent);

        if (this.factorId) {
            comp.factorId = this.factorId;
        }
    }
}
