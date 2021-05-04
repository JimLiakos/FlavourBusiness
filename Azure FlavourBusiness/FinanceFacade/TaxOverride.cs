using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FinanceFacade
{
    /// <MetaDataID>{6730c08f-b107-4846-b47f-11625755c856}</MetaDataID>
    public class TaxOverride : ITaxOverride
    {
        /// <exclude>Excluded</exclude>
        double _TaxRate;
        /// <MetaDataID>{31f5e81a-fe63-49a9-bd87-9301b5939fb5}</MetaDataID>
        [PersistentMember(nameof(_TaxRate))]
        [BackwardCompatibilityID("+4")]
        public double TaxRate
        {
            get
            {
                return _TaxRate;
            }
            set
            {
                if (_TaxRate != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TaxRate = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _AccountID;

        /// <MetaDataID>{12929305-4536-46a4-9f9f-fbb857f360b2}</MetaDataID>
        [PersistentMember(nameof(_AccountID))]
        [BackwardCompatibilityID("+1")]
        public string AccountID
        {
            get
            {
                return _AccountID;
            }
            set
            {
                if (_AccountID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AccountID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        FinanceFacade.ITax _Tax;

        /// <MetaDataID>{aee1d5f9-d02b-4e80-867f-cb73e7bcefca}</MetaDataID>
        [PersistentMember(nameof(_Tax))]
        [BackwardCompatibilityID("+2")]
        public FinanceFacade.ITax Tax
        {
            get
            {
                return _Tax;
            }
            set
            {
                if (_Tax != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Tax = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        ITaxesContext _TaxesContext;

        /// <MetaDataID>{4aaeca1a-4541-473b-8536-873e6664116e}</MetaDataID>
        [PersistentMember(nameof(_TaxesContext))]
        [BackwardCompatibilityID("+3")]
        public ITaxesContext TaxesContext
        {
            get
            {
                return _TaxesContext;
            }
            set
            {
                if (_TaxesContext != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TaxesContext = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}