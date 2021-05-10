using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{ace54e2e-4a80-457a-8fde-a57221bb741a}</MetaDataID>
    public interface IItemMultiPriceHeading : IMenuCanvasItem
    {
        [Association("FoodItemsMultiPriceHeading", Roles.RoleB, "cd60ac6b-0cb2-442c-95fe-37801f2138f9")]
        [RoleBMultiplicityRange(1)]
        System.Collections.Generic.IList<IMenuCanvasFoodItem> FoodItems { get; }


        [Association("ItemPriceSelectionGroup", Roles.RoleA, "56006625-c449-4b2f-8899-ceb8560ce26b")]
        [RoleAMultiplicityRange(1, 1)]
        MenuModel.ItemSelectorOptionsGroup Source { get; set; }



        [Association("MultiPriceHeadings", Roles.RoleA, "c4ef3e80-84ab-4e20-8c00-983a35b19ac0")]
        [RoleAMultiplicityRange(2)]
        System.Collections.Generic.IList<IPriceHeading> PriceHeadings { get; }
    }
}