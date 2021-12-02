using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{144b49bc-8f01-4fac-8f18-7e9142b740a5}</MetaDataID>
    [BackwardCompatibilityID("{144b49bc-8f01-4fac-8f18-7e9142b740a5}")]
    [OOAdvantech.MetaDataRepository.Persistent()]
    public class FisicalParty : FinanceFacade.IFisicalParty
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink=new OOAdvantech.ObjectStateManagerLink();

        /// <exclude>Excluded</exclude>
        FinanceFacade.IAddress _Address;

        /// <MetaDataID>{e6b4abad-96cb-41b9-a889-7c072fd83500}</MetaDataID>
        [PersistentMember(nameof(_Address))]
        [BackwardCompatibilityID("+4")]
        public FinanceFacade.IAddress Address
        {
            get => _Address;
            set
            {
                if (_Address != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Address = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _VATNumber;

        /// <MetaDataID>{7bc11ed0-5696-4734-87ec-61cbd725b09d}</MetaDataID>
        [PersistentMember(nameof(_VATNumber))]
        [BackwardCompatibilityID("+3")]
        public string VATNumber
        {
            get => _VATNumber;
            set
            {
                if (_VATNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _VATNumber = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _CountryCode;

        /// <MetaDataID>{23db2d9b-7b6a-4ac4-875b-783f75d6bb23}</MetaDataID>
        [PersistentMember(nameof(_CountryCode))]
        [BackwardCompatibilityID("+5")]
        public string CountryCode
        {
            get => _CountryCode;
            set
            {
                if (_CountryCode != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CountryCode = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Branch;

        /// <MetaDataID>{3e9824cc-dd5f-4f17-bbd0-a3d514bafb58}</MetaDataID>
        [PersistentMember(nameof(_Branch))]
        [BackwardCompatibilityID("+2")]
        public string Branch
        {
            get => _Branch;
            set
            {
                if (_Branch != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Branch = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{cd65ffda-c1b1-442e-9b89-986c24936dbc}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name;
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
    }
}