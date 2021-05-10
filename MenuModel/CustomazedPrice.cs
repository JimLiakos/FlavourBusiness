using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{e29ea147-16dd-47c4-ab5c-21c4af37f9d6}</MetaDataID>
    [BackwardCompatibilityID("{e29ea147-16dd-47c4-ab5c-21c4af37f9d6}")]
    [Persistent()]
    public class CustomizedPrice : ICustomizedPrice
    {
 


        /// <MetaDataID>{a401c823-1c41-4456-8f9f-e37bc60235b6}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{8149e8dc-e7ba-4972-9545-0407591fc429}</MetaDataID>
        protected CustomizedPrice()
        {

        }


        /// <MetaDataID>{db1a3fdb-ad6e-401a-bc58-49560dfb6285}</MetaDataID>
        public CustomizedPrice(IPricingContext pricingContext, IPricedSubject pricedSubject)
        {
            _PricingContext = pricingContext;
            _PricedSubject = pricedSubject;
        }
        /// <exclude>Excluded</exclude>
        IPricedSubject _PricedSubject;

        /// <MetaDataID>{4dc5d8fd-5d87-431b-808b-42c95d8f0a01}</MetaDataID>
        [AssociationClassRole(Roles.RoleA, "_PricedSubject")]
        [BackwardCompatibilityID("+2")]
        public IPricedSubject PricedSubject
        {
            get
            {
                return _PricedSubject;
            }
        }

        /// <exclude>Excluded</exclude>
        IPricingContext _PricingContext;

        /// <MetaDataID>{de7f88f2-37ba-419a-abbb-bf44f53eb22a}</MetaDataID>
        [AssociationClassRole(Roles.RoleB, "_PricingContext")]
        [BackwardCompatibilityID("+1")]
        public IPricingContext PricingContext
        {
            get
            {
                return _PricingContext;
            }
        }

        /// <exclude>Excluded</exclude>
        decimal _Price;

        /// <MetaDataID>{bb368eac-8eb3-4e65-826e-b38dc133433e}</MetaDataID>
        [PersistentMember("_Price")]
        [BackwardCompatibilityID("+3")]
        public decimal Price
        {
            get
            {
                return _Price;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Price = value; 
                    stateTransition.Consistent = true;
                }

            }
        }
    }
}