import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
    name: 'intToStringDate'
})
export class IntToStringDatePipe implements PipeTransform {

    transform(value: any): any {
        if (value) {
          if (value.toString().length === 6) {
            return  value.toString().substring(0, 4) + '/' + value.toString().substring(4, 6);
          } else {
            return value.toString().substring(0, 4) + '/' + value.toString().substring(4, 6) + '/' + value.toString().substring(6, 8);
          }
        }
        return null;
    }

}
