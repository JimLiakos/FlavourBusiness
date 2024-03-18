using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FinanceFacade
{
    /// <MetaDataID>{6730c08f-b107-4846-b47f-11625755c856}</MetaDataID>
    [BackwardCompatibilityID("{6730c08f-b107-4846-b47f-11625755c856}")]
    [Persistent()]
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
        ITax _Tax;

        /// <MetaDataID>{aee1d5f9-d02b-4e80-867f-cb73e7bcefca}</MetaDataID>
        [AssociationClassRole(Roles.RoleB, nameof(_Tax))]
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
        [AssociationClassRole(Roles.RoleA, nameof(_TaxesContext))]
        [BackwardCompatibilityID("+3")]
        public FinanceFacade.ITaxesContext TaxesContext
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

        /// <MetaDataID>{2c17bb16-22f1-418c-9268-b96cc366caa3}</MetaDataID>
        public decimal Fee { get => 0; set { } }


        /// <exclude>Excluded</exclude>
        bool _IsActive =true;

        /// <MetaDataID>{7eb22f4b-4f23-485f-9a24-8381db705579}</MetaDataID>
        [PersistentMember(nameof(_IsActive))]
        [BackwardCompatibilityID("+5")]
        public bool IsActive
        {
            get => _IsActive;
            set
            {
                if (_IsActive != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsActive = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}