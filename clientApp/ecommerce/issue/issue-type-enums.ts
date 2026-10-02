export enum IssueTypeEnums {
  /*
  مشکل
   */
  Problem = 1184,
  /*
  کار
   */
  Task = 1148,
  /*
  خطا
   */
  Error = 1149,
  /*
  نامشخص بودن وضعیت رهگیری
   */
  UnknownOfTrackingStatus = 1166,
  /*
  خطا در ثبت پایه/مقطع تحصیلی
   */
  ErrorRegisteringBase = 1167,
  /*
  خطا در انتخاب رشته/گرایش تحصیلی
   */
  ErrorInChoosingFieldOfStudy = 1167,
  /*
  خطا در انتخاب نوع کالا
   */
  ErrorSelectingProductType = 1169,
  /*
  خطا در ثبت مشخصات پرداخت
   */
  ErrorRegisteringPaymentDetails = 1170,
  /*
  مشکل مشاهده اطلاعات سایت
   */
  DifficultyViewingSiteInformation = 1171,
  /*
  خطا در انتخاب مرکز
   */
  ErrorSelectingCompany = 1172,
  /*
  خطا در انتخاب نمایندگی
   */
  ErrorSelectingRepresentation = 1173,
  /*
  اطلاع  رسانی نامناسب نمایندگی
   */
  InappropriateInformationRepresentation = 1174,
  /*
  برخورد نامناسب نمایندگی
   */
  ImproperRepresentation = 1175,
  /*
  تخلف نمایندگی
   */
  RepresentationViolation = 1176,
  /*
  تأخیر در تحویل  محصول
   */
  DelayInProductDelivery = 1177,
  /*
  نقص در  محصول تحویلی
   */
  DefectsInTheDeliveredProduct = 1178,
  /*
  شکایت
   */
  Complaint = 1179,
  /*
  انصراف
   */
  Cancel = 1180,
  /*
  عودت
   */
  Refund = 1253
}

// tslint:disable-next-line:typedef
export function IsEqual(issueTypeNumber: number, issueType: IssueTypeEnums) {
  return IssueTypeEnums[issueTypeNumber] === IssueTypeEnums[issueType];
}
