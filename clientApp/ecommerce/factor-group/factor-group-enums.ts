export enum FactorGroupStatusEnumType {
  Initial = 1182,
  Confirm = 1183
}

// tslint:disable-next-line:typedef
export function IsEqual(factorVerificationConfirmTypeNumber: number, factorVerificationConfirmationType: FactorGroupStatusEnumType) {
  return FactorGroupStatusEnumType[factorVerificationConfirmTypeNumber] === FactorGroupStatusEnumType[factorVerificationConfirmationType];
}
