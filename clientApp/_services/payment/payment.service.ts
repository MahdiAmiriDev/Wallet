import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {CardChargeModel} from '../../_models/payment/card-charge.model';
import {CardInfoModel} from '../../_models/payment/card-info.model';

@Injectable({providedIn: 'root'})
export class PaymentService {
  constructor(private http: HttpClient) {
  }

  doCardCharge(cardChargeModel: CardChargeModel) {
    return this.http.post<any>(`api/cardInfo/DoCardCharge`, cardChargeModel);
  }

  addCardInfo(cardInfoModel: CardInfoModel) {
    return this.http.post<any>(`api/cardInfo/addCardInfo`, cardInfoModel, {responseType: 'blob' as 'json'});
  }

}
