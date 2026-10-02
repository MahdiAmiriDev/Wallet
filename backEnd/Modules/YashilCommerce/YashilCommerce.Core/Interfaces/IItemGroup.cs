namespace YashilCommerce.Core.Interfaces
{
    public interface IItemGroup<ItemGroupType>
    {
        bool AllowAddChild(int itemGroupId);
    }
}
