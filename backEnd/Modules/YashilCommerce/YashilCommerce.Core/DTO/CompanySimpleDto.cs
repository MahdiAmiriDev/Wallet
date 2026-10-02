namespace YashilCommerce.Core.DTO
{
    public class CompanySimpleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortName { get; set; }

        public string ModificationDate { get; set; }
        public int DisplayOrder { get; set; }
    }
}
