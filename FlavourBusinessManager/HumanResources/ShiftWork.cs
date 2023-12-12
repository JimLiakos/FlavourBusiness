using FlavourBusinessFacade.HumanResources;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{18db2bbc-fa7a-4c3d-a96f-2cf0ebd3e4eb}</MetaDataID>
    [BackwardCompatibilityID("{18db2bbc-fa7a-4c3d-a96f-2cf0ebd3e4eb}")]
    [Persistent()]
    public class ShiftWork : MarshalByRefObject, IShiftWork, OOAdvantech.Remoting.IExtMarshalByRefObject
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAuditWorkerEvents> _AuditEvents = new OOAdvantech.Collections.Generic.Set<IAuditWorkerEvents>();

        /// <MetaDataID>{711c5a50-a446-4be0-80f1-6b7c6e045c56}</MetaDataID>
        [PersistentMember(nameof(_AuditEvents))]
        [BackwardCompatibilityID("+6")]
        public System.Collections.Generic.List<IAuditWorkerEvents> AuditEvents 
        {
            get => _AuditEvents.ToThreadSafeList();
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        double _PeriodInHours;

        /// <MetaDataID>{b33da6ed-51a9-414b-b491-b18519f56b7c}</MetaDataID>
        [PersistentMember(nameof(_PeriodInHours))]
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        public double PeriodInHours
        {
            get => _PeriodInHours;
            internal set
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

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{c8de3776-6ddf-48a8-8469-4f3d97c859cc}</MetaDataID>
        protected void OnObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(_object, member);
        }
        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{23668ad4-c8a0-410e-87de-b63ecd7ae72b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
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
        /// <MetaDataID>{90918721-ebd7-41f4-9142-939bd7c0be42}</MetaDataID
        [CachingDataOnClientSide]
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
        [CachingDataOnClientSide]
        public DateTime StartsAt
        {
            get
            {
                var ssds = _StartsAt.Kind;

                return _StartsAt;
            }
            internal set
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

        
        /// <MetaDataID>{3bd28e5d-2f79-4cc6-9a8c-3bacc7c53524}</MetaDataID>
        public void AddAuditWorkerEvents(IAuditWorkerEvents auditEvent)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(auditEvent);
                _AuditEvents.Add(auditEvent); 
                stateTransition.Consistent = true;
            }

        }
    }
}