export enum FactorVerificationConfirmationType {
  Confirm = 1138,
  Reject = 1139
}

// tslint:disable-next-line:typedef
export function IsEqual(factorVerificationConfirmTypeNumber: number, factorVerificationConfirmationType: FactorVerificationConfirmationType) {
  return FactorVerificationConfirmationType[factorVerificationConfirmTypeNumber] === FactorVerificationConfirmationType[factorVerificationConfirmationType];
}
