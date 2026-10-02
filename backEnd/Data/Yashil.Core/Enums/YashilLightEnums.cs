namespace Yashil.Core.Enums
{
    /// <summary>
    /// وضعیت آزمون کاربر
    /// اگر کاربر در آزمون شرکت نکرده باشد شرکت نکرده در آزمون
    /// اگر در آزمون شرکت کرده باشد و هنوز هم بتواند به آزمون ورود پیدا کند یعنی دکمه پایان آزمون را نزده باشد و زمان باقیمانده ای برای شرکت در ازمون داشته باشد در حال اجرای آزمون
    /// اگر هم زمان آزمون پایان یافته باشد یا کاربر دکمه پایان را زده باشد یا کاربر زمانش به پایان رسیده باشد هم اتمام آزمون
    /// </summary>
    public enum UserExamStateEnum
    {
        /// <summary>
        /// شرکت نکرده
        /// </summary>
        NotParticipate = 0,

        /// <summary>
        /// در حال برگزاری
        /// </summary>
        Participating = 1,

        /// <summary>
        /// اتمام آزمون
        /// </summary>
        Participated = 2
    }

    /// <summary>
    /// وضعیت آزمون بر اساس زمان آزمون
    /// اگر زمان آزمون فرا نرسیده باشد برگزار نشده است
    /// اگر زمان آزمون فرا رسیده باشد و در بازه زمانی آزمون باشیم در حال برگزاری است
    /// در هم زمان آزمون گذشته باشد اتمام آزمون است
    /// </summary>
    public enum ExamStateEnum
    {
        /// <summary>
        /// برگزار نشده
        /// </summary>
        NotPerformed = 0,

        /// <summary>
        /// در حال برگزاری
        /// </summary>
        Performing = 1,

        /// <summary>
        /// اتمام آزمون
        /// </summary>
        Performed = 2
    }

    /// <summary>
    /// وضعیت آزمون
    /// ExamStatusType
    /// </summary>
    public enum ExamStatusTypeEnum
    {
        /// <summary>
        /// برگذار نشده
        /// </summary>
        NotCommit = 1284,

        /// <summary>
        /// در حال برگذاری
        /// </summary>
        Committing = 1285,

        /// <summary>
        /// اتمام آزمون
        /// </summary>
        Commit = 1286
    }

    /// <summary>
    ///وضعیت تکلیف
    /// HomeWorkStatusType
    /// </summary>
    public enum HomeWorkStatusTypeEnum
    {
        /// <summary>
        ///انجام نشده
        /// </summary>
        Undone = 2310,

        /// <summary>
        ///انجام شده
        /// </summary>
        Done = 2311
    }

    /// <summary>
    /// وضعیت برگزاری کلاس مجازی
    /// </summary>
    public enum VirtualClassStateEnum
    {
        /// <summary>
        /// برگزار نشده
        /// </summary>
        NotPerformed = 0,

        /// <summary>
        /// در حال برگزاری
        /// </summary>
        Performing = 1,

        /// <summary>
        /// اتمام کلاس مجازی
        /// </summary>
        Performed = 2
    }

}
