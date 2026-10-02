import {Pipe, PipeTransform} from '@angular/core';
import * as moment from 'jalali-moment';

@Pipe({
  name: 'jalaliDateWithTime'
})
export class JalaliDateWithTimePipe implements PipeTransform {
  transform(value: any, args?: any): any {
    // 2010-10-09 00:00:00.000
    // 2019-08-01T00:00:00
    try {
      moment.locale('en');
      const momentDate = moment(value);
      return momentDate.locale('fa').format('HH:mm YYYY/MM/DD');
    } catch (e) {
      return value;
    }
  }
}
