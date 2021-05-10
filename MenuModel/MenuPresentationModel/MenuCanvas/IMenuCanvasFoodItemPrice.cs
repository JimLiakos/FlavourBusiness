using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{6dfbe8fc-cfef-4024-bf96-0dc861c9336f}</MetaDataID>
    public interface IMenuCanvasFoodItemPrice : IMenuCanvasItem
    {
        /// <MetaDataID>{b525b387-1869-4aaa-a5d4-04f268c26f96}</MetaDataID>
        decimal Price { get; set; }

  

        [Association("FoodItemPrices", Roles.RoleB, "9af888d3-e504-4524-9994-389228a2c2e0")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItem FoodItem { get; }

        /// <MetaDataID>{26001a81-1349-4cb7-bab2-818aab1b7545}</MetaDataID>
        bool Visisble { get; set; }
    }
}