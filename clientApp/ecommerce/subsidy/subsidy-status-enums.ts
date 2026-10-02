export enum SubsidyStatusTypeEnum {
  Confirmed = 1118,
  Rejected = 1119
}

// tslint:disable-next-line:typedef
export function IsEqual(subsidyStatusTypeNumber: number, subsidyStatusTypeEnum: SubsidyStatusTypeEnum) {
  return SubsidyStatusTypeEnum[subsidyStatusTypeNumber] === SubsidyStatusTypeEnum[subsidyStatusTypeEnum];
}
