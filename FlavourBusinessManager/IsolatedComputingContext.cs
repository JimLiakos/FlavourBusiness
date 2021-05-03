using System;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ComputingResources
{
    /// <MetaDataID>{479fb553-541b-46c7-a485-d66023613d8c}</MetaDataID>
    [BackwardCompatibilityID("{479fb553-541b-46c7-a485-d66023613d8c}")]
    [Persistent()]
    public class IsolatedComputingContext : FlavourBusinessFacade.ComputingResources.IIsolatedComputingContext
    {


        /// <exclude>Excluded</exclude>
        int _ComputingResourceID;

        /// <MetaDataID>{fe664248-7f9c-4b8c-8f40-3bb7d5b1aaae}</MetaDataID>
        [PersistentMember(nameof(_ComputingResourceID))]
        [BackwardCompatibilityID("+3")]
        public int ComputingResourceID
        {
            get
            {
                return _ComputingResourceID;
            }

            set
            {
                if (_ComputingResourceID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ComputingResourceID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink=new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        string _ContextID;
        /// <MetaDataID>{5cfe27ca-5a62-4012-b95e-b28ffb9b3c8e}</MetaDataID>
        [PersistentMember(nameof(_ContextID))]
        [BackwardCompatibilityID("+1")]
        public string ContextID
        {
            get
            {
                return _ContextID;
            }
            set
            {
                if (_ContextID != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ContextID = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{4e16d515-dc2a-443e-9693-d7d3eea016f2}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
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
    }
}