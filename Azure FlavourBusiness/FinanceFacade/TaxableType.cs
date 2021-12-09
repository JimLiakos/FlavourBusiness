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


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ITaxable> _TaxableSubjects = new OOAdvantech.Collections.Generic.Set<ITaxable>();

        /// <MetaDataID>{55948d8e-1c63-4a2d-9c76-cece3542be1c}</MetaDataID>
        [PersistentMember(nameof(_TaxableSubjects))]
        [BackwardCompatibilityID("+3")]
        public List<ITaxable> TaxableSubjects => _TaxableSubjects.ToThreadSafeList();

        /// <MetaDataID>{495d3804-ab15-45fb-84e7-11e986cdbc54}</MetaDataID>
        [ObjectActivationCall]
        public void OnObjectActivation()
        {
            var sds = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            int ere = sds.GetHashCode();

        }

        public ITax NewTax()
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                var tax = new Tax() { Description = Finance.Properties.Resources.NewTaxDefaultName };
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(tax);
                _Taxes.Add(tax);
                stateTransition.Consistent = true;

                return tax;
            }

        }

        public void RemoveTax(ITax tax)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Taxes.Remove(tax);
                stateTransition.Consistent = true;
            }

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
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
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