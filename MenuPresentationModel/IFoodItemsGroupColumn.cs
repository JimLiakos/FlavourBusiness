using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{82f4ed85-b88a-48cd-bfea-4956f55dd342}</MetaDataID>
    public interface IFoodItemsGroupColumn
    {
      
        [Association("FoodItemGroupColumn", Roles.RoleB, "0c83c106-0c70-4c4c-a466-0646276e7652")]
        [RoleBMultiplicityRange(1, 1)]
        IMenuCanvasFoodItemsGroup FoodItemGroup { get; set; }

        /// <MetaDataID>{c6aa6378-f885-49a7-94d6-8d238ecfd0e0}</MetaDataID>
        double XPos { get; set; }

        /// <MetaDataID>{cdf93fd8-65bc-4f1f-aead-03e824935aeb}</MetaDataID>
        double YPos { get; set; }

        /// <MetaDataID>{4ca61320-e288-49c0-bdf8-b03a98a5fe2a}</MetaDataID>
        double Width { get; set; }

        /// <MetaDataID>{e75e458e-c096-4793-a352-1fa9dfc047c5}</MetaDataID>
        double MaxHeight { get; set; }

        /// <MetaDataID>{935b596b-129f-4e65-af91-78404ec9be11}</MetaDataID>
        void RenderMenuCanvasItems(System.Collections.Generic.IList<IMenuCanvasItem> menuCanvasItems, System.Collections.Generic.IList<IMenuCanvasItem> allItemMultiPriceHeadings);

        /// <MetaDataID>{c9b3e73c-d45c-41c4-91b8-2bd251ab37a0}</MetaDataID>
        double Height { get; set; }
    }
}