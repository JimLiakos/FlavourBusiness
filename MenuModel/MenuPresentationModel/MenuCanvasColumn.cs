using System;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{ba88bcfc-a3e4-47c5-bfee-8831fbd3a03e}</MetaDataID>
            public class MenuCanvasColumn : IMenuCanvasColumn
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

     

        /// <MetaDataID>{8b0944d5-d7c4-463c-b816-c2918295fe81}</MetaDataID>
        public System.Collections.Generic.IList<IMenuCanvasFoodItemsGroup> FoodItemsGroup
        {
            get
            {
                return default(System.Collections.Generic.List<IMenuCanvasFoodItemsGroup>);
            }
        }

        [BackwardCompatibilityID("+2")]
        public MenuPresentationModel.MenuCanvas.IMenuPageCanvas Page
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }
    }
}