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
        decimal? _OverridenPrice;

        /// <MetaDataID>{994ade9d-7095-4153-8813-187cb8734917}</MetaDataID>
        [PersistentMember(nameof(_OverridenPrice))]
        [BackwardCompatibilityID("+1")]
        public decimal? OverriddenPrice
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
        public double? PriceRounding
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
        double? _OptionsPricesRounding;

        /// <MetaDataID>{1ccd46a0-9b7e-48e8-ab01-db2294757b65}</MetaDataID>
        [PersistentMember(nameof(_OptionsPricesRounding))]
        [BackwardCompatibilityID("+8")]
        public double? OptionsPricesRounding
        {
            get => _OptionsPricesRounding;
            set
            {
                if (_OptionsPricesRounding != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OptionsPricesRounding = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




        /// <exclude>Excluded</exclude>
        bool? _IsOptionsPricesDiscountEnabled;


        /// <MetaDataID>{faf629b4-5b06-457e-8ed1-8bd7a4300ab6}</MetaDataID>

        [PersistentMember(nameof(_IsOptionsPricesDiscountEnabled))]
        [BackwardCompatibilityID("+9")]
        public bool? IsOptionsPricesDiscountEnabled
        {
            get => _IsOptionsPricesDiscountEnabled;
            set
            {
                if (_IsOptionsPricesDiscountEnabled != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsOptionsPricesDiscountEnabled = value;
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


        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
        object _MenuModelObject;

        /// <MetaDataID>{5741d9d1-3655-47f3-815c-6962fce85bf4}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
        public object MenuModelObject
        {
            get
            {
                if (_MenuModelObject == null)
                    _MenuModelObject = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(ItemsInfoObjectUri);

                return _MenuModelObject;
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

        /// <exclude>Excluded</exclude>
        string _ItemsInfoObjectUri;
      


        /// <MetaDataID>{f92c1291-e0d7-499f-8c9e-6cd611a33e66}</MetaDataID>
        public ItemsPriceInfo()
        {

        }

        /// <MetaDataID>{9b4ada15-ca29-4cc7-9b3c-e78c9ab8cff2}</MetaDataID>
        public ItemsPriceInfo(IMenuItemPrice menuItemPrice)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItemPrice).GetPersistentObjectUri(menuItemPrice);

            _Description = menuItemPrice.Name;
        }

        /// <MetaDataID>{5c1d7c10-9c0a-4af4-b621-718671c70920}</MetaDataID>
        public ItemsPriceInfo(IItemsCategory itemsCategory)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);

            _Description = itemsCategory.Name;
        }

        /// <MetaDataID>{783a888e-ce32-429f-97b0-d2ea043bc308}</MetaDataID>
        public ItemsPriceInfo(IMenuItem menuItem)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);

            _Description = menuItem.Name;
        }



        /// <MetaDataID>{8ff8374e-4373-44be-89c8-7968fd9dbc97}</MetaDataID>
        [PersistentMember(nameof(_ItemsInfoObjectUri))]
        [BackwardCompatibilityID("+7")]
        public string ItemsInfoObjectUri
        {
            get => _ItemsInfoObjectUri;
            set
            {
                if (_ItemsInfoObjectUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsInfoObjectUri = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


    }
}