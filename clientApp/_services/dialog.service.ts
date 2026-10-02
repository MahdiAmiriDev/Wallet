import {EventEmitter, Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  dialogOpend: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor() {
  }
}
