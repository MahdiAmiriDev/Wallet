import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';


@Component({
  selector: 'app-time-picker',
  templateUrl: './time-picker.component.html'
})
export class TimePickerComponent implements OnInit {
  private _value: any;
  get value(): any {
    return this._value;
  }

  @Input()
  set value(value: any) {
    this._value = value;
    if(value){
    this.selectedHour = this.value.split(':')[0] ? this.value.split(':')[0] : "00";
    this.selectedMinutes = this.value.split(':')[1] ? this.value.split(':')[1] : "00";
  }
}

@Output()
valueChange = new EventEmitter<any>();
hours = ['00', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23'];
minutes = ['00', '01', '02', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12', '13', '14', '15', '16', '17', '18', '19', '20', '21', '22', '23', '24', '25', '26', '27', '28', '29', '30', '31', '32', '33', '34', '35', '36', '37', '38', '39', '40', '41', '42', '43', '44', '45', '46', '47', '48', '49', '50', '51', '52', '53', '54', '55', '56', '57', '58', '59'];

selectedHour: string;
selectedMinutes: string;
@Input()
required
:
boolean = false;

constructor()
{
}

ngOnInit()
:
void {}

selectedHoursChanged(item
:
any
)
{
  if (item && item.selectedItem) {
    this.selectedHour = item.selectedItem;
    this.valueChange.emit(this.selectedHour + ":" + this.selectedMinutes + ":00");
  }
}

selectedMinutesChanged(item
:
any
)
{
  if (item && item.selectedItem) {
    this.selectedMinutes = item.selectedItem;
    this.valueChange.emit(this.selectedHour + ":" + this.selectedMinutes + ":00");
  }
}
}

