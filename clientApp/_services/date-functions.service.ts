import * as moment from 'jalali-moment';
import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DateFunctionsService {

  constructor() {
  }

  static convertIntDateToString(value: number): any {
    if (value) {
      return value.toString().substring(0, 4) + '/' + value.toString().substring(4, 6) + '/' + value.toString().substring(6, 8);
    }
    return '';
  }

  todayString(): string {
    return moment().locale('fa').format('YYYY/M/D');
  }

  todayIn(): number {
    return +moment().locale('fa').format('YYYYMMDD');
  }
}
