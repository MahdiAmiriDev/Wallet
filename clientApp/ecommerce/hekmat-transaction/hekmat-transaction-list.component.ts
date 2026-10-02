import {Selectable} from '../../../shared/base/classes/selectable';
import {BaseList} from '../../../shared/base/classes/base-list';
import {Component, Input, OnInit, ViewChild} from '@angular/core';
import {HekmatTransactionDetailComponent} from './hekmat-transaction-detail.component';
import {GenericDataService} from "../../../shared/base/services/generic-data.service";
import {Workbook} from "exceljs";
import {exportDataGrid} from "devextreme/excel_exporter";
import saveAs from 'file-saver';

@Component({
  selector: 'app-hekmat-transaction-list',
  templateUrl: './hekmat-transaction-list.component.html'
})
export class HekmatTransactionListComponent extends Selectable implements OnInit {
  @ViewChild('listForm', {static: true}) listForm: BaseList;
  loadAfterSetFilter: boolean;
  selectedItemId: number;
  columns: any[] = [];
  delimiter = ';'
  detailComponent = HekmatTransactionDetailComponent;
  hekmatTransaction: any;
  entityName = 'HekmatTransaction';

  constructor(private genericDataService: GenericDataService) {
    super();
  }

  ngOnInit(): void {
    this.getHekmatTransaction();

  }

  private bindColumns(): void {

    this.columns.push({
      caption: 'ردیف گردش ترمینال',
      dataField: 'hekmatReservationNumber'
    });
    this.columns.push({
      caption: 'شماره کارت',
      dataField: 'hekmatCardNumber'
    });
    this.columns.push({
      caption: 'کد ملی',
      dataField: 'nationalCode'
    });
    this.columns.push({
      caption: 'قیمت',
      dataField: 'price',
      cellTemplate: 'number'
    });
    this.columns.push({
      caption: 'تعداد قسط',
      dataField: 'installmentCount'
    });
    this.columns.push({
      caption: 'توکن',
      dataField: 'token'
    });
    this.columns.push({
      caption: 'خرید آموزشی است',
      dataField: 'isLearn'
    });

  }


  getHekmatTransaction(): void {
    // tslint:disable-next-line:max-line-length
    this.genericDataService.getEntitiesWithAction('HekmatPaymentGatewayTransaction', `GetTransactionsForSendToHekmatAsync`, null)
      .subscribe(response => {
        if (response && response.length) {
          this.hekmatTransaction = response;
          this.bindColumns();
        } else {
          this.showError('کاربر گرامی تراکنش حکمت برای به روز رسانی وجود ندارد');
          return;
        }
      });
  }

  public onExporting(e) {

    const gridInstance = e.component;
    e.component.columnOption("ردیف گردش ترمینال", "caption", "HekmatID");
    e.component.columnOption("شماره کارت", "caption", "HekmatNo");
    e.component.columnOption("کد ملی", "caption", "NID");
    e.component.columnOption("قیمت", "caption", "Price");
    e.component.columnOption("تعداد قسط", "caption", "InstallmentCount");
    e.component.columnOption("توکن", "caption", "Token");
    e.component.columnOption("خرید آموزشی است", "caption", "IsLearn");
    const workbook = new Workbook();
    const worksheet = workbook.addWorksheet('main sheet');
    try {
      gridInstance.beginUpdate();
      exportDataGrid({
        component: gridInstance,
        worksheet,
      }).then(() => {
        workbook.csv.writeBuffer({formatterOptions: {delimiter: this.delimiter}})
          .then((buffer: BlobPart) => {
            saveAs(new Blob([buffer], {type: 'application/octet-stream'}), 'file.csv');
          }).then(() => {
          gridInstance.endUpdate();
          this.changeCaptionAfterExport(e);
        });
      });
    } catch (e) {
      console.log(e);
      gridInstance.endUpdate();
    }
    e.cancel = true;
  }

  public changeCaptionAfterExport(e): void {

    e.component.columnOption("HekmatID", "caption", "ردیف گردش ترمینال");
    e.component.columnOption("HekmatNo", "caption", "شماره کارت");
    e.component.columnOption("NID", "caption", "کد ملی");
    e.component.columnOption("Price", "caption", "قیمت");
    e.component.columnOption("InstallmentCount", "caption", "تعداد قسط");
    e.component.columnOption("Token", "caption", "توکن");
    e.component.columnOption("IsLearn", "caption", "خرید آموزشی است");
  }
}
