using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{1d94ea2d-a31a-433c-9eec-cc19692ef1a4}</MetaDataID>
    public interface IPricedSubject
    {
        /// <MetaDataID>{0842c7fe-1392-4eb5-b745-09516d89125e}</MetaDataID>
        decimal GetPrice(IPricingContext pricingContext);

        /// <MetaDataID>{ac0b0039-14ac-4a6d-b6a9-a88c804eb153}</MetaDataID>
        /// <summary>Defines the default price </summary>
        decimal Price { get; set; }

      
         
        /// <MetaDataID>{dfff3bff-dcf0-463e-ab6c-6ce4dc7394ec}</MetaDataID>
        void SetPrice(IPricingContext pricingContext, decimal price);

        [Association("CustomazedPrice", Roles.RoleB, "30caadeb-0046-4f1b-9cc7-00d19a96b4a4")]
        [AssociationClass(typeof(ICustomizedPrice))]
        IList<MenuModel.ICustomizedPrice> PricingContexts { get; }


        /// <MetaDataID>{ac867b6d-f8b7-4f8d-a373-84fe3531bb0d}</MetaDataID>
        void RemoveCustomizedPrice(ICustomizedPrice customizedPrice);
    }
}