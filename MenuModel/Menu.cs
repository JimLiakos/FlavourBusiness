using System;
using FinanceFacade;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuModel
{
    /// <MetaDataID>{7eefea4a-0207-49ee-95be-bc326f2afcdb}</MetaDataID>
    [BackwardCompatibilityID("{7eefea4a-0207-49ee-95be-bc326f2afcdb}")]
    [Persistent()]
    public class Menu :MarshalByRefObject, IMenu,OOAdvantech.PersistenceLayer.IObjectStateEventsConsumer
    {
        /// <MetaDataID>{bcbe82be-1ca3-4766-a3a9-a4eaaa4e0533}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;
        /// <MetaDataID>{f9351f51-21cf-4715-bd1e-b965c074c4be}</MetaDataID>

        [PersistentMember(255, "_Name")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<MenuModel.IItemsCategory> _RootCategory=new OOAdvantech.Member<IItemsCategory>();

        /// <MetaDataID>{cba06346-d3e1-482a-8140-c3d0dbf1434f}</MetaDataID>
        [PersistentMember("_RootCategory")]
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public MenuModel.IItemsCategory RootCategory
        {
            get
            {
                return _RootCategory.Value;
            }
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _RootCategory.Value = value; 
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{9016aae0-96aa-4ac6-81a1-f84d106b6a3b}</MetaDataID>
        OOAdvantech.Member<ITaxAuthority> _TaxAuthority = new OOAdvantech.Member<ITaxAuthority>();

        /// <MetaDataID>{509986d9-e260-48c8-8c66-1b47ee2979ad}</MetaDataID>
        [PersistentMember(nameof(_TaxAuthority))]
        [BackwardCompatibilityID("+3")]
        public FinanceFacade.ITaxAuthority TaxAuthority
        {
            get
            {
                return _TaxAuthority.Value;
            }

            set
            {

                if (_TaxAuthority.Value != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TaxAuthority.Value = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{7d515fe2-878d-4c62-b6e2-a82d8b453b91}</MetaDataID>
        public void OnCommitObjectState()
        {
            if (_RootCategory.Value == null)
            {
                _RootCategory.Value = new ItemsCategory(Properties.Resources.ItemsCategoryDefaultName);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(RootCategory);
            }
        }



        /// <MetaDataID>{46f6f717-3983-4e7d-bb19-64d3a009eb30}</MetaDataID>
        public Menu()
        {

        }
        /// <MetaDataID>{f0273832-3385-4eda-97b1-630cc0de5979}</MetaDataID>
        public void OnActivate()
        {

            var tt = _RootCategory.Value;


        }

        /// <MetaDataID>{517cb5d5-ee5c-4ac4-8c36-6bb6f661c2ff}</MetaDataID>
        public void OnDeleting()
        {

        }

        /// <MetaDataID>{753c6d7f-1706-4a14-8c80-3bfcf7fa304f}</MetaDataID>
        public void LinkedObjectAdded(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{f4dfdcc9-1ac5-4a54-8951-0f3cd42b29bf}</MetaDataID>
        public void LinkedObjectRemoved(object linkedObject, AssociationEnd associationEnd)
        {

        }

        /// <MetaDataID>{20b41867-4995-4028-8268-af1ee865c46c}</MetaDataID>
        public void BeforeCommitObjectState()
        {
        }
    }
}