using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YashilCommerce.Core.Model
{
    /// <summary>
    /// مدل مربوط به رزومه کاربر
    /// این مدل برای
    /// serialize
    /// کردن جی سون ارسالی کاربر به هنگام پرکردن رزومه به مدل سی شارپ و بررسی ولیدیشن های مربوط به آن
    /// و همچنین
    /// Deserialize
    /// کردن مدل به جی سون برای ذخیره سازی آن در جدول
    /// additionalUserProp
    /// کاربرد دارد
    /// </summary>
    public class Resume
    {
        [JsonProperty("information")]
        public Information? Information { get; set; }

        [JsonProperty("education")]
        public List<Education>? Education { get; set; }

        [JsonProperty("certificates")]
        public List<Certificate>? Certificates { get; set; }

        [JsonProperty("languages")]
        public List<Language>? Languages { get; set; }

        [JsonProperty("careerProfile")]
        public CareerProfile? CareerProfile { get; set; }
    }

    public class Attachment
    {
        [JsonProperty("entityId")]
        public int? EntityId { get; set; }

        [JsonProperty("isSuccess")]
        public bool? IsSuccess { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("returnUrl")]
        public string? ReturnUrl { get; set; }
    }

    public class CareerProfile
    {
        [JsonProperty("jobs")]
        public List<Job>? Jobs { get; set; }

        [JsonProperty("jobPreferences")]
        public JobPreferences? JobPreferences { get; set; }

        [JsonProperty("careerStatus")]
        public string? CareerStatus { get; set; }
    }

    public class Certificate
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("title")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، عنوان دوره یا مهارت طی شده بیش تر از حد بازه مجاز می باشد.")]
        public string? Title { get; set; }

        [JsonProperty("courseLevel")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، عنوان سطح دوره بیش تر از حد بازه مجاز می باشد.")]
        public string? CourseLevel { get; set; }

        [JsonProperty("courseLocation")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، عنوان محل دوره بیش تر از حد بازه مجاز می باشد.")]
        public string? CourseLocation { get; set; }

        [JsonProperty("attachment")]
        public List<Attachment>? Attachment { get; set; }

        [JsonProperty("docTypeId")]
        public string? DocTypeId { get; set; }
    }

    public class Education
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("major")]
        [StringLength(100, ErrorMessage = "کاربر، گرامی عنوان رشته تحصیلی بیش تر از حد بازه مجاز می باشد.")]
        public string? Major { get; set; }

        [JsonProperty("institution")]
        [StringLength(100, ErrorMessage = "کاربر، گرامی نام دانشگاه یا موسسه آموزشی بیش تر از حد بازه مجاز می باشد.")]
        public string? Institution { get; set; }

        [JsonProperty("institutionType")]
        public string? InstitutionType { get; set; }

        [JsonProperty("average")]
        [StringLength(10, ErrorMessage = "کاربر گرامی، معدل وارد شده بیش تر از حد بازه مجاز می باشد.")]
        public string? Average { get; set; }

        [JsonProperty("attachment")]
        public List<Attachment>? Attachment { get; set; }

        [JsonProperty("docTypeId")]
        public int? DocTypeId { get; set; }

        [JsonProperty("grade")]
        public string? Grade { get; set; }

        [JsonProperty("educationStartYear")]
        public string? EducationStartYear { get; set; }

        [JsonProperty("graduationYear")]
        public string? GraduationYear { get; set; }

        [JsonProperty("educationDescription")]
        [StringLength(300, ErrorMessage = "کاربر گرامی، میزان توضیحات وارد شده بیش تر از حد بازه مجاز می باشد.")]
        public string? EducationDescription { get; set; }

        [JsonProperty("stillEducating")]
        public string? StillEducating { get; set; }
    }

    public class Information
    {
        [JsonProperty("nationalId")]
        [Required(ErrorMessage = "کاربر گرامی، مقدار کد ملی اجباری می باشد.")]
        [StringLength(10, ErrorMessage = "کاربر گرامی، طول کد ملی نمی تواند بیش تر از 10 رقم باشد.")]
        public string? NationalId { get; set; }

        [JsonProperty("email")]
        [EmailAddress(ErrorMessage = "کاربر گرامی، آدرس ایمیل وارد شده معتبر نمی باشد.")]
        public string? Email { get; set; }

        [JsonProperty("province")]
        [Required(ErrorMessage = "کاربر گرامی، مقدار استاد محل سکونت اجباری می باشد.")]
        public string? Province { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("mobile")]
        [Required(ErrorMessage = "کاربر گرامی، مقدار شماره موبایل اجباری می باشد.")]
        [StringLength(11, ErrorMessage = "کاربر گرامی، طول شماره موبایل نمی تواند بیش تر از 11 رقم باشد.")]
        public string? Mobile { get; set; }

        [JsonProperty("birthYear")]
        [Required(ErrorMessage = "کاربر گرامی، مقدار سال تولد اجباری می باشد.")]
        public string? BirthYear { get; set; }

        [JsonProperty("guardianNationalId")]
        [StringLength(10, ErrorMessage = "کاربر گرامی، طول کد ملی نمی تواند بیش تر از 10 رقم باشد.")]
        public string? GuardianNationalId { get; set; }

        [JsonProperty("guardianMobile")]
        [StringLength(11, ErrorMessage = "کاربر گرامی، طول شماره موبایل نمی تواند بیش تر از 11 رقم باشد.")]
        public string? GuardianMobile { get; set; }

        [JsonProperty("physicalStatus")]
        public string? PhysicalStatus { get; set; }

        [JsonProperty("marriageStatus")]
        public string? MarriageStatus { get; set; }

        [JsonProperty("gender")]
        public string? Gender { get; set; }

        [JsonProperty("militaryService")]
        public string? MilitaryService { get; set; }

        [JsonProperty("disabilityType")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، مقدار نوع معلولیت بیش تر از حد بازه مجاز است.")]
        public string? DisabilityType { get; set; }

        [JsonProperty("childrenCount")]
        public string? ChildrenCount { get; set; }
    }

    public class JobField
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("title")]
        [StringLength(600, ErrorMessage = "کاربر گرامی، طول عنوان دسته بندی شغلی و زمینه درخواستی وارده شده بیش تر از حد بازه مجاز می باشد.")]
        public string? Title { get; set; }
    }

    public class JobPreferences
    {
        [JsonProperty("preferredLocations")]
        public List<PreferredLocation>? PreferredLocations { get; set; }

        [JsonProperty("jobFields")]
        public List<JobField>? JobFields { get; set; }

        [JsonProperty("seniorityLevels")]
        public List<string>? SeniorityLevels { get; set; }

        [JsonProperty("contracts")]
        public List<string>? Contracts { get; set; }

        [JsonProperty("exactJobTitle")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، طول عنوان عنوان دقیق شغل درخواستی وارده شده بیش تر از حد بازه مجاز می باشد.")]
        public string? ExactJobTitle { get; set; }

        [JsonProperty("seniorityLevelsDescription")]
        public string? SeniorityLevelsDescription { get; set; }

        [JsonProperty("contractsDescription")]
        public string? ContractsDescription { get; set; }
    }

    public class Language
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("language")]
        public string? language { get; set; }

        [JsonProperty("mastery")]
        public string? Mastery { get; set; }

        [JsonProperty("attachment")]
        public List<Attachment>? Attachment { get; set; }

        [JsonProperty("docTypeId")]
        public string? DocTypeId { get; set; }
    }

    public class PreferredLocation
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("province")]
        public string? Province { get; set; }

        [JsonProperty("city")]
        public string? City { get; set; }

        [JsonProperty("isNew")]
        public bool? IsNew { get; set; }
    }

    public class Job
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("jobTitle")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، طول عنوان شغل وارد شده بیش تر از حد بازه مجاز می باشد.")]
        public string? JobTitle { get; set; }

        [JsonProperty("company")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، نام طول شرکت وارده شده بیش تر از حد بازه مجاز می باشد.")]
        public string? Company { get; set; }

        [JsonProperty("role")]
        [StringLength(100, ErrorMessage = "کاربر گرامی، طول عنوان سمت شغلی وارده شده بیش تر از حد بازه مجاز می باشد.")]
        public string? Role { get; set; }

        [JsonProperty("startMonth")]
        public string? StartMonth { get; set; }

        [JsonProperty("startYear")]
        public string? StartYear { get; set; }

        [JsonProperty("stillWorking")]
        public string? StillWorking { get; set; }

        [JsonProperty("jobDescription")]
        [StringLength(600, ErrorMessage = "کاربر گرامی، طول توضیحات  وارده شده بیش تر از حد بازه مجاز می باشد.")]
        public string? JobDescription { get; set; }

        [JsonProperty("finishMonth")]
        public string? FinishMonth { get; set; }

        [JsonProperty("finishYear")]
        public string? FinishYear { get; set; }
    }
}
