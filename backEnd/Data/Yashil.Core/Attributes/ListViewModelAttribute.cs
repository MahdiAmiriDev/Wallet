using System;

namespace Yashil.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class ListViewModelAttribute : YashilViewModelAttribute
    {
        public ListViewModelAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public class EditViewModelAttribute : YashilViewModelAttribute
    {
        public EditViewModelAttribute(string entityName)
        {
            EntityName = entityName;
        }
    }

    public class YashilViewModelAttribute : Attribute
    {
        public string EntityName;
    }

}
