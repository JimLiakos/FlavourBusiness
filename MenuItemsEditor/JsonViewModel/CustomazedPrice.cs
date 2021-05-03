using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuModel;

namespace MenuItemsEditor.JsonViewModel
{
    /// <MetaDataID>{024c12c8-f337-4c0c-b1da-154d455be06a}</MetaDataID>
    public class CustomizedPrice : MenuModel.ICustomizedPrice
    {
        public decimal Price { get; set; }

        public IPricedSubject PricedSubject { get; set; }
       

        public IPricingContext PricingContext { get; set; }
       
    }
}
