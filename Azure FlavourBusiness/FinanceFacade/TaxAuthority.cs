using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using System.Linq;
using OOAdvantech.Transactions;

namespace FinanceFacade
{
    /// <MetaDataID>{e74cbe1f-6959-40ae-b0ba-449cb8f0bf1e}</MetaDataID>
    [BackwardCompatibilityID("{e74cbe1f-6959-40ae-b0ba-449cb8f0bf1e}")]
    [Persistent()]
    public class TaxAuthority : ITaxAuthority
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ITaxesContext> _TaxesContexts = new OOAdvantech.Collections.Generic.Set<ITaxesContext>();

        /// <MetaDataID>{638d2802-adb4-450c-b832-c52a8c516ffe}</MetaDataID>
        [PersistentMember(nameof(_TaxesContexts))]
        [BackwardCompatibilityID("+5")]
        public List<ITaxesContext> TaxesContexts => _TaxesContexts.ToThreadSafeList();


        /// <MetaDataID>{2e3f1232-7301-4f78-bbc1-d5f5f7dfe93d}</MetaDataID>
        protected TaxAuthority()
        {
        }

        /// <MetaDataID>{11e773a1-1a44-4424-aef3-ad90f1c3270e}</MetaDataID>
        public TaxAuthority(string name, string identity)
        {
            _Name = name;
            _Identity = identity;
        }



        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink = new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{9ee3f248-6387-4ff7-80cf-ea6b7be1be27}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return _Name;
            }

            set
            {

                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Notes;

        /// <MetaDataID>{258495d8-d3c9-4b48-8fbc-22d6baaa12f3}</MetaDataID>
        [PersistentMember(nameof(_Notes))]
        [BackwardCompatibilityID("+2")]
        public string Notes
        {
            get
            {
                return _Notes;

            }

            set
            {

                if (_Notes != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Notes = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ITaxableType> _TaxableTypes = new OOAdvantech.Collections.Generic.Set<ITaxableType>();


        /// <MetaDataID>{82f5108f-ed1a-405a-94a7-e3161d892490}</MetaDataID>
        [PersistentMember(nameof(_TaxableTypes))]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        [BackwardCompatibilityID("+3")]
        public IList<ITaxableType> TaxableTypes
        {
            get
            {
                lock (_TaxableTypes)
                {
                    return _TaxableTypes.ToList();
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{5f3c0ef3-0b11-4f53-8621-5cd7e886187e}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+4")]
        public string Identity
        {
            get
            {
                return _Identity;
            }

            set
            {
                if (_Identity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

     
        /// <MetaDataID>{530a45b7-854f-4e6c-865a-f57f879aaaac}</MetaDataID>
        public void AddTaxableType(ITaxableType taxableType)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _TaxableTypes.Add(taxableType);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{8e8145bb-fc80-4e3f-a331-0aa731a8250d}</MetaDataID>
        public bool RemoveTaxableType(ITaxableType taxableType)
        {
            if (taxableType.TaxableSubjects.Count == 0)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {

                    _TaxableTypes.Remove(taxableType);
                    stateTransition.Consistent = true;
                }
                return true;
            }
            else
                return false;
        }

        /// <MetaDataID>{bac8ca9f-8792-44c5-90a1-5007b5bce3e4}</MetaDataID>
        public ITaxableType NewTaxableType()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.RequiresNew))
            {
                TaxableType taxableType = new TaxableType();
                taxableType.Description = Finance.Properties.Resources.NewTaxableType;
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(taxableType);
                _TaxableTypes.Add(taxableType);
                stateTransition.Consistent = true;
                return taxableType;

            }


        }

        /// <MetaDataID>{4dc360a9-3451-4d65-8c0a-db8e6f95d262}</MetaDataID>
        public bool RemoveTaxesContext(ITaxesContext taxesContext)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var taxOverride in taxesContext.TaxOverrides)
                    OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(taxOverride);
                _TaxesContexts.Remove(taxesContext);
                OOAdvantech.PersistenceLayer.ObjectStorage.DeleteObject(taxesContext);
                stateTransition.Consistent = true;
            }
            return true;

        }

        /// <MetaDataID>{fef89375-cfd9-4395-95c6-e555e5c9daf6}</MetaDataID>
        public ITaxesContext NewTaxesContext()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.RequiresNew))
            {
                TaxesContext taxesContext = new TaxesContext();
                taxesContext.Description = Finance.Properties.Resources.NewTaxesContext;
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(taxesContext);
                _TaxesContexts.Add(taxesContext);
                stateTransition.Consistent = true;
                return taxesContext;

            }

        }
    }
}