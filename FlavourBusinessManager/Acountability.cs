using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Collections.Generic;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{7b3f0d4b-2343-4dbd-94bf-af1b56401419}</MetaDataID>
    [BackwardCompatibilityID("{7b3f0d4b-2343-4dbd-94bf-af1b56401419}")]
    [Persistent()]
    public class Accountability : OOAdvantech.Remoting.ExtMarshalByRefObject, FlavourBusinessFacade.HumanResources.IAccountability
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IActivity> _Activities = new OOAdvantech.Collections.Generic.Set<IActivity>();

        /// <MetaDataID>{bcbb36b2-37f5-4b30-8797-1b70e74ab371}</MetaDataID>
        [PersistentMember(nameof(_Activities))]
        [BackwardCompatibilityID("+6")]
        public List<IActivity> Activities => _Activities.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        string _AccountabilityType;

        /// <MetaDataID>{d9bf4959-9505-495f-81cc-993717379d20}</MetaDataID>
        [PersistentMember(nameof(_AccountabilityType))]
        [BackwardCompatibilityID("+5")]
        public string AccountabilityType
        {
            get => _AccountabilityType;
            set
            {
                if (_AccountabilityType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AccountabilityType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

     
        /// <MetaDataID>{0be89ac3-2b6c-4847-9899-11b5466603a5}</MetaDataID>
        public void AddActivity(IActivity activity)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Activities.Add(activity);
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{875a41df-3c52-45bd-bedc-64e8bfcaf9aa}</MetaDataID>
        public void RemoveActivity(IActivity activity)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Activities.Remove(activity); 
                stateTransition.Consistent = true;
            }

        }


        /// <exclude>Excluded</exclude>
        IParty _Responsible;
        /// <MetaDataID>{7290e442-7f1b-4f45-820d-fcc0ac5ea700}</MetaDataID>
        [PersistentMember(nameof(_Responsible))]
        [BackwardCompatibilityID("+2")]
        public IParty Responsible
        {
            get => _Responsible;
            set
            {
                if (_Responsible != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Responsible = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        FlavourBusinessFacade.IParty _Commissioner;

        /// <MetaDataID>{4be57654-7177-4bac-8801-b53dbad2161c}</MetaDataID>
        [PersistentMember(nameof(_Commissioner))]
        [BackwardCompatibilityID("+3")]
        public FlavourBusinessFacade.IParty Commissioner
        {
            get => _Commissioner;
            set
            {
                if (_Commissioner != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Commissioner = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{e2bb06b1-04ea-4636-9e8a-419fca86f924}</MetaDataID>
        protected Accountability()
        {

        }
        /// <MetaDataID>{6cfd3a3b-08e9-4591-9202-5ff74594e5dd}</MetaDataID>
        public Accountability(IParty commissioner, IParty responsible)
        {
            _Commissioner = commissioner;
            _Responsible = responsible;
        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{bbd78ce1-cbda-434a-b11b-4abb15fbec85}</MetaDataID>
        string _Description;

        /// <MetaDataID>{a188ee8e-2201-40c2-91fd-9ddf1f9dab4c}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+4")]
        public string Description
        {
            get => _Description;
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

    /// <MetaDataID>{aa3f02d6-e4bf-4c36-8bc6-0f519640217c}</MetaDataID>
    public class AccountabilityType
    {
        public const string MenuMaking = "E4C54DF2-65C6-4D5B-A124-DD62E7AFBF30";
        public const string Translation = "85ED670B-DECD-4D3E-AFEB-26D7FAA43B7E";
    }
}