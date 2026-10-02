using System;
using Yashil.Core.Enums;

namespace Yashil.Core.CustomDomainModels.YashilLight
{
    public class UserExamCustomDomainModel
    {
        public int Id { get; set; }
        public int ExamId { get; set; }
        public string ExamCode { get; set; }

        /// <summary>
        /// عنوان آزمون
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// مدت زمان آزمون
        /// </summary>
        public int Duration { get; set; }
        /// <summary>
        /// تاریخ شروع آزمون
        /// </summary>
        public int StartDateInt { get; set; }
        /// <summary>
        /// تاریخ پایان آزمون
        /// </summary>
        public int EndDateInt { get; set; }
        /// <summary>
        /// ساعت شروع آزمون
        /// </summary>
        public string StartTime { get; set; }

        /// <summary>
        /// ساعت پایان آزمون
        /// </summary>
        public string EndTime { get; set; }


        /// <summary>
        /// وضعیت آزمون
        /// </summary>
        public string ExamStatusTypeTitle { get; set; }
        /// <summary>
        /// وضعیت آزمون
        /// </summary>
        public int ExamStatusType { get; set; }

        /// <summary>
        /// امتیاز آزمون
        /// </summary>
        public double? UserScore { get; set; }

        /// <summary>
        /// آیا کاربر مجاز به شرکت در آزمون است یا نه؟
        /// </summary>
        public bool AllowEnterExam { get; set; }
        /// <summary>
        /// وضعیت آزمون
        /// اگر زمان آزمون نرسیده باشد برگزار نشده است
        /// اگر در مدت زمان آزمون باشد در حال برگزاری است
        /// اگر زمان آزمون پایان یافته باشد اتمام آزمون است
        /// </summary>
        public int ExamState { get; set; }
        /// <summary>
        /// شناسه آزمون نظر سنجی 
        /// </summary>
        public int? ExamSurveyId { get; set; }
        /// <summary>
        /// آیا کاربر در نظر سنجی آزمون شرکت کرده است یا خیر؟
        /// </summary>
        public bool UserParticipatedInExamSurvey { get; set; }
        /// <summary>
        /// آیا  کاربر محتوای الزامی را مشاهده کرده است یا خیر؟
        /// </summary>
        public bool UserHasViewedRequiredContents { get; set; }
        /// <summary>
        /// کد آزمون نظر سنجی
        /// </summary>
        public string ExamSurveyCode { get; set; }

        /// <summary>
        ///         /// تاریخ شروع میلادی
        ///</summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        ///         /// تاریخ پایان میلادی
        ///</summary>
        public DateTime EndDate { get; set; }

        public int UserExamState
        {
            get
            {
                // اگر اجازه شرکت در آزمون نداشت و وضعیت آزمون کاربر در حال برگزاری بود سعنی تایم آزمون کاربر به پایان رسیده است
                if ((!AllowEnterExam && ExamStatusType == (int)ExamStatusTypeEnum.Committing) || ExamStatusType == (int)ExamStatusTypeEnum.Commit)
                {
                    return (int)UserExamStateEnum.Participated;
                }
                if (AllowEnterExam && ExamStatusType == (int)ExamStatusTypeEnum.Committing)
                {
                    return (int)UserExamStateEnum.Participating;
                }
                if (AllowEnterExam && ExamStatusType == (int)ExamStatusTypeEnum.NotCommit)
                {
                    return (int)UserExamStateEnum.NotParticipate;
                }
                // در غیر این صورت یعنی زمان آزمون به پایان رسیده است و کاربر در آزمون شرکت نکرده است
                else
                {
                    return (int)UserExamStateEnum.NotParticipate;
                }
            }

        }

        /// <summary>
        /// فعال سازی دکمه نمایش پاسخ نامه آزمون
        /// </summary>
        public bool EnableShowExamAnswerSheetBtn { get; set; }
    }
}
