
/// <summary>
/// وضعیت درخواست رزرو کلاس فیزیکی
/// </summary>
export enum ClassRoomReservationStatusTypeEnum{
  /// <summary>
  /// ثبت اولیه
  /// </summary>
  InitialRegistration = 3118,
  /// <summary>
  /// تایید شده
  /// </summary>
  Confirmed = 3119,
  /// <summary>
  /// رد شده
  /// </summary>
  Reject  =3120  ,
  /// <summary>
  /// درخواست ویرایش
  /// </summary>
  EditRequest = 3121
}



export enum ClassRoomReservationScheduleEnum {
  //تاریخ مشخص
  SpecificDate = 3123,
  //نکرار در بازه
  RepeatInRange= 3122
}

export enum ClassRoomReservationRequestTypeEnum{
  //درخواست رزرو برای برنامه ریزی دوره
  AssignToProductSalesPlanning = 3124,
  //درخواست رزرو کلاس
  ClassRoomReservation= 3125
}
