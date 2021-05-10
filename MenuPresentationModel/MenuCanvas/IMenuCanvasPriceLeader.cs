using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{cc06e695-9afc-4544-8604-168f49ab6847}</MetaDataID>
    public interface IMenuCanvasPriceLeader : IMenuCanvasItem
    {
      
        [Association("FoodItemPriceLeader", Roles.RoleB, "9e0b48c5-d77e-4e97-b510-76e56d8c9266")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItem FoodItem { get; }

        /// <MetaDataID>{155687cc-e726-4176-bb6f-5c0e7ff8cd05}</MetaDataID>
        bool Visisble { get; set; }
    }
}