using FlavourBusinessFacade.PriceList;
using MenuModel;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.PriceList
{
    /// <MetaDataID>{d6fab165-c183-4ae8-af4e-bd27ffc79df3}</MetaDataID>
    [BackwardCompatibilityID("{d6fab165-c183-4ae8-af4e-bd27ffc79df3}")]
    [Persistent()]
    public class ItemsPriceInfo : IItemsPriceInfo
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        double? _OverridenPrice;

        /// <MetaDataID>{994ade9d-7095-4153-8813-187cb8734917}</MetaDataID>
        [PersistentMember(nameof(_OverridenPrice))]
        [BackwardCompatibilityID("+1")]
        public double? OverridenPrice
        {
            get => _OverridenPrice;
            set
            {
                if (_OverridenPrice != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OverridenPrice = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double? _AmountDiscount;

        /// <MetaDataID>{eb72c10b-aeaf-4e5d-a4bb-979cb4bf68da}</MetaDataID>
        [PersistentMember(nameof(_AmountDiscount))]
        [BackwardCompatibilityID("+2")]
        public double? AmountDiscount
        {
            get => _AmountDiscount;
            set
            {
                if (_AmountDiscount != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AmountDiscount = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double? _Pricerounding;

        /// <MetaDataID>{984aec00-8594-49e5-9677-6c10f2bef4ed}</MetaDataID>
        [PersistentMember(nameof(_Pricerounding))]
        [BackwardCompatibilityID("+3")]
        public double? Pricerounding
        {
            get => _Pricerounding;
            set
            {
                if (_Pricerounding != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Pricerounding = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double? _PercentageDiscount;
        /// <MetaDataID>{1e7a1978-81c3-4a0d-b2cf-b969f38a22b3}</MetaDataID>
        [PersistentMember(nameof(_PercentageDiscount))]
        [BackwardCompatibilityID("+4")]
        public double? PercentageDiscount
        {
            get => _PercentageDiscount;
            set
            {
                if (_PercentageDiscount != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PercentageDiscount = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{d832ebe9-6059-4c33-b271-f4858d23879c}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+5")]
        public string Description
        {
            get => _Description; 
            set
            {

                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{5741d9d1-3655-47f3-815c-6962fce85bf4}</MetaDataID>
        public MenuModel.IClassified MenuModelObject
        {
            get
            {
                return null;
            }
        }


        /// <exclude>Excluded</exclude>
        ItemsPriceInfoType _ItemsPriceInfoType;

        /// <MetaDataID>{d024e1af-b783-46d2-a0fb-f40f886f10e9}</MetaDataID>
        [PersistentMember(nameof(_ItemsPriceInfoType))]
        [BackwardCompatibilityID("+6")]
        public ItemsPriceInfoType ItemsPriceInfoType
        {
            get => _ItemsPriceInfoType; set
            {
                if (_ItemsPriceInfoType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsPriceInfoType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}