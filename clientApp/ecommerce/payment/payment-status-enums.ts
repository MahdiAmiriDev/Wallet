export enum PaymentStatusTypeEnum {
  Successfull = 1095,
}

// tslint:disable-next-line:typedef
export function IsEqual(paymentStatusTypeNumber: number, paymentStatusTypeEnum: PaymentStatusTypeEnum) {
  return PaymentStatusTypeEnum[paymentStatusTypeNumber] === PaymentStatusTypeEnum[paymentStatusTypeEnum];
}
