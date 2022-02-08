using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{7d881488-7d0a-44f3-930b-ce6d86a0350d}</MetaDataID>
    [BackwardCompatibilityID("{7d881488-7d0a-44f3-930b-ce6d86a0350d}")]
    [Persistent()]
    public class Settings :MarshalByRefObject,OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ServicesContextResources.ISettings
    {
        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _AutoAssignMaxMealProgress;

        /// <MetaDataID>{f061399b-6fef-4095-a47b-fd8bc13bc76c}</MetaDataID>
        [PersistentMember(nameof(_AutoAssignMaxMealProgress))]
        [BackwardCompatibilityID("+1")]
        public double AutoAssignMaxMealProgress
        {
            get => _AutoAssignMaxMealProgress;
            set
            {
                if (_AutoAssignMaxMealProgress != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AutoAssignMaxMealProgress = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _ForgottenSessionLifeTimeSpanInMin;

        /// <MetaDataID>{95e35d96-20eb-4e9b-83a7-a5925b5affea}</MetaDataID>
        [PersistentMember(nameof(_ForgottenSessionLifeTimeSpanInMin))]
        [BackwardCompatibilityID("+2")]
        public double ForgottenSessionLifeTimeSpanInMin
        {
            get => _ForgottenSessionLifeTimeSpanInMin;
            set
            {
                if (_ForgottenSessionLifeTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ForgottenSessionLifeTimeSpanInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _ForgottenSessionDeviceSleepTimeSpanInMin;

        /// <MetaDataID>{4c683def-4c4e-42ae-aa5e-84dfe5a39d2d}</MetaDataID>
        [PersistentMember(nameof(_ForgottenSessionDeviceSleepTimeSpanInMin))]
        [BackwardCompatibilityID("+3")]
        public double ForgottenSessionDeviceSleepTimeSpanInMin
        {
            get => _ForgottenSessionDeviceSleepTimeSpanInMin;
            set
            {
                if (_ForgottenSessionDeviceSleepTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ForgottenSessionDeviceSleepTimeSpanInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _ForgottenSessionLastChangeTimeSpanInMin;

        /// <MetaDataID>{13358e9a-9e2f-4e06-b033-3c011f337170}</MetaDataID>
        [PersistentMember(nameof(_ForgottenSessionLastChangeTimeSpanInMin))]
        [BackwardCompatibilityID("+4")]
        public double ForgottenSessionLastChangeTimeSpanInMin
        {
            get => _ForgottenSessionLastChangeTimeSpanInMin;

            set
            {
                if (_ForgottenSessionLastChangeTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ForgottenSessionLastChangeTimeSpanInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}