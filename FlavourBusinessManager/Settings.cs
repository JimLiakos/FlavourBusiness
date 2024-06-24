using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager
{
    /// <MetaDataID>{7d881488-7d0a-44f3-930b-ce6d86a0350d}</MetaDataID>
    [BackwardCompatibilityID("{7d881488-7d0a-44f3-930b-ce6d86a0350d}")]
    [Persistent()]
    public class Settings : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ServicesContextResources.ISettings
    {

        /// <exclude>Excluded</exclude> 
        double _MealConversationTimeoutInMin;


        /// <summary>
        /// Defines the max time of meal conversation
        /// Is the time between the time where the first item selected and present
        /// </summary>
        /// <MetaDataID>{5d73813e-4080-45d8-aa97-f0fc84971158}</MetaDataID>
        [PersistentMember(nameof(_MealConversationTimeoutInMin))]
        [BackwardCompatibilityID("+6")]
        public double MealConversationTimeoutInMin
        {
            get => _MealConversationTimeoutInMin;
            set
            {
                if (_MealConversationTimeoutInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealConversationTimeoutInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

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


        /// <exclude>Excluded</exclude>
        double _MealConversationTimeoutWaitersUpdateTimeSpanInMin;


        /// <summary>
        /// Defines the minimum time span between  meal conversation timeout waiter reminders
        /// </summary>
        /// <MetaDataID>{5839ae5a-abaa-4bf7-94d7-f9ff16f8fb6c}</MetaDataID>
        [PersistentMember(nameof(_MealConversationTimeoutWaitersUpdateTimeSpanInMin))]
        [BackwardCompatibilityID("+5")]
        public double MealConversationTimeoutWaitersUpdateTimeSpanInMin
        {
            get => _MealConversationTimeoutWaitersUpdateTimeSpanInMin;
            set
            {
                if (_MealConversationTimeoutWaitersUpdateTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealConversationTimeoutWaitersUpdateTimeSpanInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        double _MealConversationTimeoutCareGivingWaiterUpdateTimeSpanInMin;

        /// <MetaDataID>{eb5cb90b-33e9-4e04-bd6f-f9c0e8577766}</MetaDataID>
        [PersistentMember(nameof(_MealConversationTimeoutCareGivingWaiterUpdateTimeSpanInMin))]
        [BackwardCompatibilityID("+7")]
        public double MealConversationTimeoutCareGivingUpdateTimeSpanInMin
        {
            get => _MealConversationTimeoutCareGivingWaiterUpdateTimeSpanInMin;
            set
            {
                if (_MealConversationTimeoutCareGivingWaiterUpdateTimeSpanInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealConversationTimeoutCareGivingWaiterUpdateTimeSpanInMin = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        double _MealConversationTimeoutInMinForSupervisor;

        /// <MetaDataID>{f1b72039-3e13-4748-9c46-ea61b08adec2}</MetaDataID>
        [PersistentMember(nameof(_MealConversationTimeoutInMinForSupervisor))]
        [BackwardCompatibilityID("+8")]
        public double MealConversationTimeoutInMinForSupervisor
        {
            get => _MealConversationTimeoutInMinForSupervisor;
            set
            {
                if (_MealConversationTimeoutInMinForSupervisor != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealConversationTimeoutInMinForSupervisor = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        double _UrgesToDecideTimeoutInMin;
        /// <summary>
        /// Defines the maximum time where session can be in UrgeToDecide state, before the system initiates the reminders to waiters.
        /// </summary>
        /// <MetaDataID>{2a6926e3-a090-4631-ac05-abcd730288c2}</MetaDataID>
        [PersistentMember(nameof(_UrgesToDecideTimeoutInMin))]
        [BackwardCompatibilityID("+9")]
        public double UrgesToDecideTimeoutInMin
        {
            get => _UrgesToDecideTimeoutInMin;
            set
            {
                if (_UrgesToDecideTimeoutInMin != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UrgesToDecideTimeoutInMin = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}