using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{024c12c8-f337-4c0c-b1da-154d455be06a}</MetaDataID>
    public class CustomizedPrice : MenuModel.ICustomizedPrice
    {
        /// <MetaDataID>{37cd5839-12da-43d0-83a0-ae6c04692bc4}</MetaDataID>
        public decimal Price { get; set; }

        /// <MetaDataID>{22ff31ce-a43c-42bb-b66d-2dc55eaf809d}</MetaDataID>
        public IPricedSubject PricedSubject { get; set; }


        /// <MetaDataID>{7e142ce9-11d9-4abf-ab8d-04dbe50e7a6c}</MetaDataID>
        public IPricingContext PricingContext { get; set; }

    }
}
