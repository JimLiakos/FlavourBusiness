using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{82f4ed85-b88a-48cd-bfea-4956f55dd342}</MetaDataID>
    public interface IFoodItemsGroupColumn:IMenuCanvasColumn
    {
    

        [Association("FoodItemGroupColumn", Roles.RoleB, "0c83c106-0c70-4c4c-a466-0646276e7652")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItemsGroup FoodItemGroup { get; set; }

      

        /// <MetaDataID>{935b596b-129f-4e65-af91-78404ec9be11}</MetaDataID>
        void RenderMenuCanvasItems(System.Collections.Generic.IList<IMenuCanvasItem> menuCanvasItems, System.Collections.Generic.IList<IMenuCanvasItem> allItemMultiPriceHeadings);

       
    }
}