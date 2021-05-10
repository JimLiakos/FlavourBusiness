using System;
using System.Collections.Generic;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{5b385e5a-ffb8-4ec4-b7d7-314d58f23e32}</MetaDataID>
    public class MenuCanvasFoodItemsGroup : IMenuCanvasFoodItemsGroup
    {
        /// <MetaDataID>{6795327b-8830-47de-9d85-6093a650f1fc}</MetaDataID>
        public IMenuCanvasColumn Column
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{f49e46a5-f11e-41d2-ba04-b77b257fe866}</MetaDataID>
        public IList<IDescriptionHeading> DescriptionHeadings
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{dbd25e1d-133a-442c-9c47-8ba13f77cbd5}</MetaDataID>
        public IList<IMenuCanvasFoodItem> GroupedFoodItems
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <MetaDataID>{68cde684-beab-4b44-a9f7-5f2a79864863}</MetaDataID>
        public IFoodItemsHeading ItemsGroupHeading
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}