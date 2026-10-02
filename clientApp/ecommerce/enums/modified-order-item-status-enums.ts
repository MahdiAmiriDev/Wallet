export enum ModifiedOrderItemStatusEnumType {
  // ثبت توسط تامین کننده
  RegisteredBySupplier = 1197,
  // تایید توسط کارشناس crm
  ApprovedByCrmExpert = 1198,
  // رد توسط کارشناس crm
  RejectedByCrmExpert = 1199,
  // اصلاح سفارش انجام شد
  Completed = 1200
}

// tslint:disable-next-line:typedef
export function IsEqual(modifiedOrderItemStatusTypeNumber: number, modifiedOrderItemStatusType: ModifiedOrderItemStatusEnumType) {
  return ModifiedOrderItemStatusEnumType[modifiedOrderItemStatusTypeNumber]
    === ModifiedOrderItemStatusEnumType[modifiedOrderItemStatusType];
}
