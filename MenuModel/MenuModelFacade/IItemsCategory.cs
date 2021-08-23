using System.Collections.Generic;

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


        /// <MetaDataID>{b55303f3-ccbd-4725-be5b-9bf1b357ba88}</MetaDataID>
        IItemsCategory Parent { get; }

        /// <MetaDataID>{f83e2ab4-8195-4b6a-9f39-8f45ca66ce25}</MetaDataID>
        IList<IItemsCategory> SubCategories { get; }

        /// <MetaDataID>{4b6f0f11-a30f-4b42-8f5d-e60763869e92}</MetaDataID>
        IList<IMenuItem> MenuItems { get; }
       
    }
}