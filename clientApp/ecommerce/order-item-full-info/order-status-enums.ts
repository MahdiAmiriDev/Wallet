export enum OrderStatusTypeEnum {
  Confirm = 1091,
  IncompletePayment = 1131
}

// tslint:disable-next-line:typedef
export function IsEqual(orderStatusTypeNumber: number, orderStatusTypeEnum: OrderStatusTypeEnum) {
  return OrderStatusTypeEnum[orderStatusTypeNumber] === OrderStatusTypeEnum[orderStatusTypeEnum];
}
