using System;
using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using System.Linq;
using OOAdvantech.Transactions;
using System.Collections.Generic;

namespace MenuModel
{
    /// <MetaDataID>{0df8d337-6074-4ea1-bb60-d111c0e01f90}</MetaDataID>
    [BackwardCompatibilityID("{0df8d337-6074-4ea1-bb60-d111c0e01f90}")]
    [Persistent()]
    public class MenuItemPrice : IMenuItemPrice
    {



        /// <MetaDataID>{d32ef62f-7e9f-443d-840a-3123e3251c16}</MetaDataID>
        public MenuItemPrice()
        {

        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<ItemSelectorOption> _ItemSelector = new OOAdvantech.Member<ItemSelectorOption>();

        [Association("MenuItemOptionPrice", Roles.RoleB, "de4dea3d-cb9c-4e14-99c5-bec3cf9f766b")]
        [PersistentMember(nameof(_ItemSelector))]
        public ItemSelectorOption ItemSelector
        {
            get
            {
                return _ItemSelector;
            }
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        Set<ICustomizedPrice> _PricedSubjects = new Set<ICustomizedPrice>();

        /// <MetaDataID>{ebfdce27-f1c2-4761-b83b-e38739974db8}</MetaDataID>
        [PersistentMember("_PricedSubjects")]
        [BackwardCompatibilityID("+4")]
        public IList<ICustomizedPrice> PricedSubjects
        {
            get
            {
                return _PricedSubjects.AsReadOnly();
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{9c7113a6-a80a-4525-8bad-9049d63cccf7}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                if (ItemSelector != null)
                    return ItemSelector.Name;

                return _Name;
            }
            set
            {
                if (ItemSelector != null)
                    throw new NotSupportedException("Renaming is not supported when the MenuItemPrice was created for item selection.");
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{d340e738-50a3-4df1-be42-44bf5ebecbc1}</MetaDataID>
        public bool IsPriceOverridden
        {
            get
            {
                return _Price.HasValue;
            }
        }


        /// <exclude>Excluded</exclude>
        decimal? _Price;

        /// <MetaDataID>{38f7ebc3-520e-4ce8-aa80-26e9ed9c6041}</MetaDataID>
        [PersistentMember("_Price")]
        [BackwardCompatibilityID("+2")]
        public decimal Price
        {
            get
            {
                if (_Price.HasValue && _Price.Value != -1)
                    return _Price.Value;
                else if (ItemSelector != null)
                    return ItemSelector.Price;
                else
                    return 0;
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

        /// <exclude>Excluded</exclude>
        Set<ICustomizedPrice> _PricingContexts = new Set<ICustomizedPrice>();

        /// <MetaDataID>{87f8579a-33f0-4c44-9c2a-aff7d656580d}</MetaDataID>
        [PersistentMember("_PricingContexts")]
        [BackwardCompatibilityID("+3")]
        public IList<ICustomizedPrice> PricingContexts
        {
            get
            {
                return _PricingContexts.AsReadOnly();
            }
        }

        /// <exclude>Excluded</exclude>
        IMenuItem _MenuItem;

        /// <MetaDataID>{3c1f6925-a232-43fe-8700-41dd6bd0675c}</MetaDataID>
        [PersistentMember("_MenuItem")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+5")]
        public IMenuItem MenuItem
        {
            get
            {
                return _MenuItem;
            }
        }

        /// <MetaDataID>{af7d9bdb-4de9-411d-ba79-ab35c018af58}</MetaDataID>
        bool _IsDefaultPrice;
        /// <MetaDataID>{9c020017-c644-4112-8f98-84b7dbc7d31f}</MetaDataID>
        public bool IsDefaultPrice
        {
            get
            {
                if (ItemSelector != null && MenuItem != null)
                    return !ItemSelector.GetInitialFor(MenuItem).UncheckOption;
                else
                    return _IsDefaultPrice;
            }
            internal set
            {
                _IsDefaultPrice = value;
            }
        }

        /// <MetaDataID>{ff59b7f0-b0b3-44d6-a567-5c1049a53cee}</MetaDataID>
        public decimal GetPrice(IPricingContext pricicingContext)
        {
            if (pricicingContext != null)
            {
                ICustomizedPrice customizedPrice = (from ancustomazedPrice in _PricingContexts
                                                    where ancustomazedPrice.PricingContext == pricicingContext
                                                    select ancustomazedPrice).FirstOrDefault();
                if (customizedPrice != null)
                    return customizedPrice.Price;
            }

            return Price;
        }

        /// <MetaDataID>{e5e9af0a-4deb-4c96-9244-289c6fe94135}</MetaDataID>
        public void RemoveCustomizedPrice(ICustomizedPrice customizedPrice)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                if (customizedPrice.PricedSubject == this)
                    _PricingContexts.Remove(customizedPrice);
                if (customizedPrice.PricingContext == this)
                    _PricedSubjects.Remove(customizedPrice);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{d1f4f42a-e60f-439f-8015-be3113c2e5f2}</MetaDataID>
        public void SetPrice(IPricingContext pricicingContext, decimal price)
        {
            ICustomizedPrice customizedPrice = (from ancustomazedPrice in _PricingContexts
                                                where ancustomazedPrice.PricingContext == pricicingContext
                                                select ancustomazedPrice).FirstOrDefault();

            if (customizedPrice != null)
                customizedPrice.Price = price;
            else
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    customizedPrice = new CustomizedPrice(pricicingContext, this);
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(customizedPrice);
                    customizedPrice.Price = price;
                    _PricingContexts.Add(customizedPrice);
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <MetaDataID>{3b079de3-e0d0-4f23-a208-ba38544cba56}</MetaDataID>
        public ICustomizedPrice GetCustomizedPrice(IPricedSubject pricedSubject)
        {
            return (from customizedPrice in _PricedSubjects
                    where customizedPrice.PricedSubject == pricedSubject
                    select customizedPrice).FirstOrDefault();
        }

        public decimal GetDefaultPrice(IPricedSubject pricedSubject)
        {
            return pricedSubject.Price;
        }


    }
}