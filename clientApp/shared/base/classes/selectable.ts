import {EventEmitter, Output, Directive, Input} from '@angular/core';
import {custom} from 'devextreme/ui/dialog';
import {YashilComponent} from '../../../core/baseClasses/yashilComponent';
import notify from 'devextreme/ui/notify';
import * as moment from 'jalali-moment';
import {IActiveDate} from 'ng-persian-datepicker';

@Directive()
// tslint:disable-next-line:directive-class-suffix
export class Selectable extends YashilComponent {

  @Input()
  selectByButton = false;
  @Input()
  selectable = false;
  @Input()
  gridHeight: number;
  @Input()
  fixHeight = true;
  @Input()
  allowFilter = false;
  @Input()
  showListHeader = true;
  @Input()
  loadAfterSetFilter = false;
  @Output()
  public selectedRowChanged: EventEmitter<any> = new EventEmitter<any>();
  @Output()
  public onSelectionChanged: EventEmitter<any> = new EventEmitter<any>();

  public bindCustomDataSources(): boolean {
    return true;
  }

  public confirmBeforeAction(title: string, messageHtml: string, confirmText: string): any {
    return custom({
      title,
      messageHtml,
      buttons: [{
        text: confirmText,
        onClick: (e) => {
          return {res: 'ok'};
        }
      }, {
        text: 'انصراف',
        onClick: (e) => {
          return {res: 'cancel'};
        }
      }]
    });
  }
  public convertMomentDateToInt(p: moment.Moment): number {
    if (p) {
      if (typeof p === 'string') {
        if (String(p).length === 10) {
          const regex = /\//gi;
          const res = +String(p).replace(regex, '');
          if (!isNaN(res)) {
            return res;
          }
          return 14000101;
        }
        return 14000101;
      }
      return +p.format('YYYYMMDD');
    }
    return null;
  }
  public convertDateToInt(activeDate: IActiveDate): number {
    if (activeDate) {
      const persianDate = activeDate.shamsi;
      if (String(persianDate).length === 10) {
        const regex = /\//gi;
        const res = +String(persianDate).replace(regex, '');
        if (!isNaN(res)) {
          return res;
        }
        return 14000101;
      }
      return 14000101;
    }
  }
}
