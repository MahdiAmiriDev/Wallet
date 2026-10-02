export enum FactorReviewStatusTypeEnums {
  Confirm = 1132,
  Initial = 1134,
  Reject = 1135,
  Processing = 1136
}

// tslint:disable-next-line:typedef
export function IsEqual(factorReviewStatusTypeNumber: number, factorReviewStatusType: FactorReviewStatusTypeEnums) {
  return FactorReviewStatusTypeEnums[factorReviewStatusTypeNumber] === FactorReviewStatusTypeEnums[factorReviewStatusType];
}
