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
        OOAdvantech.Collections.Generic.Set<ITaxesContext> _TaxOverrides = new OOAdvantech.Collections.Generic.Set<ITaxesContext>();

        /// <MetaDataID>{5c1b4e8f-8584-4cce-8714-6d54f992f078}</MetaDataID>
        [PersistentMember(nameof(_TaxOverrides))]
        [BackwardCompatibilityID("+3")]
        public IList<ITaxesContext> TaxOverrides
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