using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using System.Linq;
using OOAdvantech.Transactions;

namespace FinanceFacade
{
    /// <MetaDataID>{d9975ab9-c471-46b3-a81d-33fda33e4230}</MetaDataID>
    [BackwardCompatibilityID("{d9975ab9-c471-46b3-a81d-33fda33e4230}")]
    [Persistent()]
    public class Tax : ITax
    {

        /// <exclude>Excluded</exclude>
        bool _IsActive=true;
        /// <MetaDataID>{98e73c47-836f-48a1-8234-6c97ff2d6be8}</MetaDataID>
        [PersistentMember(nameof(_IsActive))]
        [BackwardCompatibilityID("+7")]
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

      

        /// <exclude>Excluded</exclude>
        decimal _Fee;

        /// <MetaDataID>{ec3ca16f-5707-43b1-8f80-4daec085113d}</MetaDataID>
        [PersistentMember(nameof(_Fee))]
        [BackwardCompatibilityID("+6")]
        public decimal Fee
        {
            get => _Fee;
            set
            {
                if (_Fee != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Fee = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        bool _FeePerUnit;

        /// <MetaDataID>{3bb75918-482e-4f51-b1b1-0bc8bfad79f3}</MetaDataID>
        [PersistentMember(nameof(_FeePerUnit))]
        [BackwardCompatibilityID("+5")]
        public bool FeePerUnit
        {
            get => _FeePerUnit;
            set
            {
                if (_FeePerUnit != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FeePerUnit = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _AccountID;

        /// <MetaDataID>{bc617940-10ac-4940-a1a0-08e7ae433aef}</MetaDataID>
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
        string _Description;

        /// <MetaDataID>{a60fac2b-2071-49e5-bc84-3f990289571e}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get
            {
                return _Description;
            }
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
        OOAdvantech.Collections.Generic.Set<ITaxOverride> _TaxOverrides = new OOAdvantech.Collections.Generic.Set<ITaxOverride>();

        /// <MetaDataID>{5c1b4e8f-8584-4cce-8714-6d54f992f078}</MetaDataID>
        [PersistentMember(nameof(_TaxOverrides))]
        [BackwardCompatibilityID("+3")]
        public IList<ITaxOverride> TaxOverrides
        {
            get
            {
                return _TaxOverrides.ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        double _TaxRate;

        /// <MetaDataID>{ecf3ca70-ccf7-40c8-b36e-d4c67ead14b0}</MetaDataID>
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
    }
}