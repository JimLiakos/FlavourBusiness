using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{12381e49-a4ae-414e-b035-19d3128922ee}</MetaDataID>
    public interface IMenuCanvasFoodItemText : IMenuCanvasItem
    {
        /// <MetaDataID>{584b6b33-2952-4308-a474-d699e1a16d4f}</MetaDataID>
        [Association("FoodItemSubTexts", Roles.RoleB, "6848fa01-6ad3-40c0-8812-21e3248a2a91")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItem FoodItem { get; }
    }
}