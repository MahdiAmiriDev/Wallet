namespace Yashil.Core.CustomDomainModels.UserManagement
{
    public class ResourceAttributeRoleDto
    {
        public int ResourceAttributeId { get; set; }
        public string ResourceAttributeTitle { get; set; }
        public bool AllowOperation { get; set; }
        public int? ResourceAttributeRoleId { get; set; }
        public string Description { get; set; }
    }
}
