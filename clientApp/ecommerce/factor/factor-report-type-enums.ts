// نوع گزارش فاکتور
export enum FactorReportTypeEnums {
  WithDetail = 1140, // باجزئیات
  WithoutDetail = 1141 // بدون جزئیات
}

// tslint:disable-next-line:typedef
export function IsEqual(factorReportTypeNumber: number, factorReportType: FactorReportTypeEnums) {
  return FactorReportTypeEnums[factorReportTypeNumber] === FactorReportTypeEnums[factorReportType];
}
