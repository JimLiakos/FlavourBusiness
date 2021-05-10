using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{6dfbe8fc-cfef-4024-bf96-0dc861c9336f}</MetaDataID>
    public interface IMenuCanvasFoodItemPrice : IMenuCanvasItem
    {
#if MenuPresentationModel
        [Association("PriceForItemSelection", Roles.RoleA, "567fadaf-2bbd-4ed4-9245-c464073361b4")]
        MenuModel.ItemSelectorOption ItemSelection { get; }
#endif


        /// <MetaDataID>{d7b2caba-3963-4a64-b9d3-72af3f85e412}</MetaDataID>
        void ResetSize(MenuStyles.IPriceStyle style);
        /// <MetaDataID>{b525b387-1869-4aaa-a5d4-04f268c26f96}</MetaDataID>
        decimal Price { get; set; }



        [Association("FoodItemPrices", Roles.RoleB, "9af888d3-e504-4524-9994-389228a2c2e0")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItem FoodItem { get; }

        /// <MetaDataID>{26001a81-1349-4cb7-bab2-818aab1b7545}</MetaDataID>
        bool Visisble { get; set; }


    }
}