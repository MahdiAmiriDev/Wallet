import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'hideButton'
})
export class HideButtonPipe implements PipeTransform {

    transform(value: any): boolean {
     console.log('HideButtonPipe');
      return value === true;
    }
}
