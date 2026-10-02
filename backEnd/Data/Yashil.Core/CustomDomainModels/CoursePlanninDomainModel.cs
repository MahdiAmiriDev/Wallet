namespace Yashil.Core.CustomDomainModels
{
    public class CoursePlanningDomainModel
    {

        public int Id { get; set; }
        public bool? Published { get; set; }
        public bool Confirmed { get; set; }
        public string CoursePlanningSchedule { get; set; }
        
        public int? EndDate { get; set; }
        public string Code { get; set; }

        public string RepresentationTitle { get; set; }

        public bool Organizational { get; set; }

        public string CourseStatusTitle { get; set; }

        public string TeacherTitle { get; set; }

        public decimal? Price { get; set; }

        public string CourseTitle { get; set; }

        public string AgeCategoryTitle { get; set; }

        public string ImplementationTypeTitle { get; set; }

        public string CourseTypeTitle { get; set; }

        public string RunTypeTitle { get; set; }

        public int? StartDate { get; set; }

        public string CustomGenderTitle { get; set; }

        public int MaxCapacity { get; set; }

        public string CategoryTitle { get; set; }
    }
}
