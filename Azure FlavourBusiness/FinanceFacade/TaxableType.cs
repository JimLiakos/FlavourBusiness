using System;
using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;

namespace FinanceFacade
{
    /// <MetaDataID>{45ab4ae9-9fca-406c-a49a-6ace066ba7fb}</MetaDataID>
    [BackwardCompatibilityID("{45ab4ae9-9fca-406c-a49a-6ace066ba7fb}")]
    [Persistent()]
    public class TaxableType : ITaxableType
    {

        [ObjectActivationCall]
        public void OnObjectActivation()
        {
            var sds = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            int ere = sds.GetHashCode();

        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{7202cbbf-f261-4b1d-84ad-1009234441e8}</MetaDataID>
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
        OOAdvantech.Collections.Generic.Set<ITax> _Taxes=new OOAdvantech.Collections.Generic.Set<ITax>();

        /// <MetaDataID>{015abca3-f488-47c7-b0a5-7dd9d5bdf2fa}</MetaDataID>
        [PersistentMember(nameof(_Taxes))]
        [BackwardCompatibilityID("+2")]
        public IList<ITax> Taxes
        {
      
            get
            {
                lock( _Taxes)
                {
                    return _Taxes.ToList();
                }
            }
        }
    }
}