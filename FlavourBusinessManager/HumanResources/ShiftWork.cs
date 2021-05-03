using FlavourBusinessFacade.HumanResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{18db2bbc-fa7a-4c3d-a96f-2cf0ebd3e4eb}</MetaDataID>
    [BackwardCompatibilityID("{18db2bbc-fa7a-4c3d-a96f-2cf0ebd3e4eb}")]
    [Persistent()]
    public class ShiftWork : MarshalByRefObject, IShiftWork, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _PeriodInHours;

        /// <MetaDataID>{b33da6ed-51a9-414b-b491-b18519f56b7c}</MetaDataID>
        [PersistentMember(nameof(_PeriodInHours))]
        [BackwardCompatibilityID("+4")]
        public double PeriodInHours
        {
            get => _PeriodInHours;
            set
            {

                if (_PeriodInHours != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PeriodInHours = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{23668ad4-c8a0-410e-87de-b63ecd7ae72b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Name))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        public string Name
        {
            get => _Name; set
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
        /// <exclude>Excluded</exclude>
        IServicesContextWorker _Worker;

        /// <MetaDataID>{bac08266-66bd-4d22-b5fc-4a5ab72be5d7}</MetaDataID>
        [PersistentMember(nameof(_Worker))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+2")]
        public IServicesContextWorker Worker => _Worker;


        /// <exclude>Excluded</exclude>
        DateTime _StartsAt;

        /// <MetaDataID>{89da019b-c154-4d9f-8454-67e49a55dd6b}</MetaDataID>
        public ShiftWork(string name)
        {
            Name = name;
        }
        /// <MetaDataID>{b916801b-3ba8-418a-ac85-eb1b71754ddb}</MetaDataID>
        public ShiftWork()
        {

        }
        /// <MetaDataID>{90918721-ebd7-41f4-9142-939bd7c0be42}</MetaDataID>
        public System.DateTime EndsAt
        {
            get
            {
                return StartsAt + TimeSpan.FromHours(PeriodInHours);
            }
        }


        /// <MetaDataID>{ff65d889-1672-4a24-91e0-85898d4cb98e}</MetaDataID>
        [PersistentMember(nameof(_StartsAt))]
        [BackwardCompatibilityID("+3")]
        public DateTime StartsAt
        {
            get
            {
                var ssds = _StartsAt.Kind;

                return _StartsAt;
            }
            set
            {

                if (_StartsAt != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StartsAt = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IAccountability> _Accountability = new OOAdvantech.Member<IAccountability>();
        /// <MetaDataID>{333523e6-8494-48ad-8915-d6882d8cfc25}</MetaDataID>
        [PersistentMember(nameof(_Accountability))]
        [BackwardCompatibilityID("+5")]
        public IAccountability Accountability => _Accountability.Value;
    }
}