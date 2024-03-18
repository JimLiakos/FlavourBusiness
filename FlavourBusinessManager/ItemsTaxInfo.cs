using FinanceFacade;
using FlavourBusinessFacade.PriceList;
using MenuModel;
using OOAdvantech.Collections;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.PriceList
{
    /// <MetaDataID>{bde00c44-4e59-40c7-8f7e-ea006e3ed7da}</MetaDataID>
    [BackwardCompatibilityID("{bde00c44-4e59-40c7-8f7e-ea006e3ed7da}")]
    [Persistent()]
    public class ItemsTaxInfo : IItemsTaxInfo
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{2b71126c-f4f4-4e31-983d-dc611c8aa9c5}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+3")]
        public string Description
        {
            get => _Description;
            set
            {
                if (_Description!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ItemsInfoObjectUri;

        /// <MetaDataID>{fb583e4d-ce38-4cf4-b15c-9803cfe6b8bd}</MetaDataID>
        [PersistentMember(nameof(_ItemsInfoObjectUri))]
        [BackwardCompatibilityID("+1")]
        public string ItemsInfoObjectUri
        {
            get => _ItemsInfoObjectUri;
            set
            {
                if (_ItemsInfoObjectUri!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ItemsInfoObjectUri=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        ItemsPriceInfoType _ItemsPriceInfoType;

        /// <MetaDataID>{534de307-5b6a-46ec-a79b-f106a905d5a3}</MetaDataID>
        [PersistentMember(nameof(_ItemsPriceInfoType))]
        [BackwardCompatibilityID("+2")]
        public ItemsPriceInfoType ItemsPriceInfoType
        {
            get => _ItemsPriceInfoType;
            set
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

        /// <MetaDataID>{4fa40424-2778-449e-a5c1-6efbe32fae7f}</MetaDataID>
        protected ItemsTaxInfo()
        {

        }

        /// <MetaDataID>{76175d40-8fdf-4220-8851-35f11d55f9ec}</MetaDataID>
        public ItemsTaxInfo(ItemsCategory itemsCategory)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(itemsCategory).GetPersistentObjectUri(itemsCategory);
            _Description = itemsCategory.Name;
        }
        /// <MetaDataID>{ce4ee592-7b93-4de6-9d9c-e983dc85c6e4}</MetaDataID>
        public ItemsTaxInfo(IMenuItem menuItem)
        {
            _ItemsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem);
            _Description = menuItem.Description;
        }


        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
      OOAdvantech.Member< ITaxableType> _TaxableType=new OOAdvantech.Member<ITaxableType>();
        /// <MetaDataID>{5e632c89-43de-4020-a085-dbaee7a13a7c}</MetaDataID>
        [PersistentMember(nameof(_TaxableType))]
        [BackwardCompatibilityID("+4")]
        public FinanceFacade.ITaxableType TaxableType
        {
            get
            {
                //if (_TaxableType == null)
                //    _TaxableType = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(ItemsInfoObjectUri) as ITaxableType;

                return _TaxableType.Value;
            }
            set
            {
                if (_TaxableType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TaxableType.Value = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
        IClassified _MenuModelObject;

        /// <MetaDataID>{4ed84fbe-f293-4242-867c-62d1733f6e6a}</MetaDataID>
        [OOAdvantech.Json.JsonIgnore]
        public IClassified MenuModelObject
        {
            get
            {
                if (_MenuModelObject == null)
                    _MenuModelObject = OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(ItemsInfoObjectUri) as IClassified;

                return _MenuModelObject;
            }
        }

    }
}