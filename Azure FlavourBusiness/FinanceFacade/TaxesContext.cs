using System;
using System.Collections.Generic;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FinanceFacade
{
    /// <MetaDataID>{7ca6d63b-e8a6-4d70-9de4-b5db24664639}</MetaDataID>
    [BackwardCompatibilityID("{7ca6d63b-e8a6-4d70-9de4-b5db24664639}")]
    [Persistent()]
    public class TaxesContext : ITaxesContext
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{6699a607-0ea0-4bf6-97a5-b2789b42accc}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
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
        OOAdvantech.Collections.Generic.Set<ITax> _TaxOverrides = new OOAdvantech.Collections.Generic.Set<ITax>();

        /// <MetaDataID>{5366a24d-8b93-4c1b-9cf3-9986c226f806}</MetaDataID>
        public IList<ITax> TaxOverrides
        {
            get
            {
                lock (_TaxOverrides)
                {
                    return _TaxOverrides.ToList();
                }
            }
        }
    }
}