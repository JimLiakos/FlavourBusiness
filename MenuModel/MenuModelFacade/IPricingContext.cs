using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{c1d0c530-aa29-4c25-ab79-6f2aa5cc3c43}</MetaDataID>
    public interface IPricingContext
    {
        /// <MetaDataID>{fc392da1-3ce8-475f-be85-d5492d3d7c09}</MetaDataID>
        string Name { get; set; }
        [Association("CustomizedPrice", Roles.RoleA, "30caadeb-0046-4f1b-9cc7-00d19a96b4a4")]
        [AssociationClass(typeof(ICustomizedPrice))]
        IList<ICustomizedPrice> PricedSubjects { get; }



        /// <MetaDataID>{840c2f1d-1a5c-4e20-a932-8da15022276b}</MetaDataID>
        void RemoveCustomizedPrice(ICustomizedPrice customizedPrice);


        /// <MetaDataID>{94dc1543-7310-4ce6-b528-92dd09cde403}</MetaDataID>
        ICustomizedPrice GetCustomizedPrice(IPricedSubject pricedSubject);

        decimal GetDeafultPrice(IPricedSubject pricedSubject);



        

    }
}