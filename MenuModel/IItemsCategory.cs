namespace MenuModel
{
    /// <MetaDataID>{8c293bb3-0601-4bd3-b71d-a0e79621aa37}</MetaDataID>
    public interface IItemsCategory : IClassified, IClass
    {
        /// <MetaDataID>{98d78feb-3f41-4adb-8998-8db6e66de779}</MetaDataID>
        IItemsCategory NewSubCategory(string newItemsCategoryName);
        /// <MetaDataID>{2a2458b3-eede-4301-993c-f771be4447ee}</MetaDataID>
        bool CanDeleteSubCategory(IItemsCategory itemsCategory);

        /// <MetaDataID>{b8bb42f0-9d30-4f90-a222-056a571ff195}</MetaDataID>
        System.Collections.Generic.List<MenuModel.IMenuItemType> MenuItemTypes { get; }
    }
}