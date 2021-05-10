using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{ace54e2e-4a80-457a-8fde-a57221bb741a}</MetaDataID>
    public interface IItemMultiPriceHeading : IMenuCanvasItem
    {
 

        /// <MetaDataID>{eee0edeb-be30-46a9-94b3-77c893c41f74}</MetaDataID>
        TransformOrigin TransformOrigin { get; set; }

        /// <MetaDataID>{e8c204c6-f856-45ed-8f51-6211a559b0ff}</MetaDataID>
        double PriceHeadingsAngle { get; set; }


        /// <MetaDataID>{996b87b3-1732-411c-9c85-24dfbf8fed02}</MetaDataID>
        double PriceHeadingsHorizontalPos { get; set; }



        /// <MetaDataID>{be9618b2-bc60-47ce-b1f6-db556a8114cc}</MetaDataID>
        double PriceHeadingsTopMargin { get; set; }


        /// <MetaDataID>{5e43c2dc-b95d-4223-92c5-3533b340f10a}</MetaDataID>
        double PriceHeadingsBottomMargin { get; set; }

        [Association("FoodItemsMultiPriceHeading", Roles.RoleB, "cd60ac6b-0cb2-442c-95fe-37801f2138f9")]
        [RoleBMultiplicityRange(1)]
        System.Collections.Generic.IList<IMenuCanvasFoodItem> FoodItems { get; }


#if MenuPresentationModel
        [Association("ItemPriceSelectionGroup", Roles.RoleA, "56006625-c449-4b2f-8899-ceb8560ce26b")]
        [RoleAMultiplicityRange(1, 1)]
        MenuModel.ItemSelectorOptionsGroup Source { get; set; }
#endif


        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [Association("MultiPriceHeadings", Roles.RoleA, "c4ef3e80-84ab-4e20-8c00-983a35b19ac0")]
        [RoleAMultiplicityRange(2)]
        System.Collections.Generic.IList<IPriceHeading> PriceHeadings { get; }

        /// <MetaDataID>{88ae1a27-4148-457e-bd9a-8cfba1dd9bf6}</MetaDataID>
        Rect CanvasFrameArea { get; }

        /// <MetaDataID>{af5d33ef-f1ed-46dd-b22e-ae10e66de7d4}</MetaDataID>
        void ResetValuesToStyleDefaults();
    }
}