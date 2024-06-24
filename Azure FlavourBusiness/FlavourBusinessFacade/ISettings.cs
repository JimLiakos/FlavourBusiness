using OOAdvantech.MetaDataRepository;
using System;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{8173beba-c4bd-466c-bd65-971c6aeb72bb}</MetaDataID>
    [BackwardCompatibilityID("{8173beba-c4bd-466c-bd65-971c6aeb72bb}")]
    public interface ISettings
    {
        /// <summary>
        /// Defines the maximum meal progress when the meals controller can auto assign a Food service client session
        /// </summary>
        /// <MetaDataID>{6ce43cca-6699-45e2-b4b0-2016fde1ee31}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        double AutoAssignMaxMealProgress { get; set; }

        /// <summary>
        /// Defines the minimum lifespan of the session that can be marked as forgotten
        /// </summary>
        /// <MetaDataID>{2802ce16-4fc0-4c15-a4e0-59dacb93443c}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double ForgottenSessionLifeTimeSpanInMin { get; set; }
        /// <summary>
        ///  Defines the minimum sleep duration of the device app that can be marked as forgotten
        /// </summary>
        /// <MetaDataID>{27134864-4f08-40b4-9e69-0d19bc6b3791}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        double ForgottenSessionDeviceSleepTimeSpanInMin { get; set; }

        /// <summary>
        ///  Defines the minimum time since the last change in session that can be marked as forgotten
        /// </summary>
        /// <MetaDataID>{261708e9-3e50-4ed3-b733-801e22b46ff5}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        double ForgottenSessionLastChangeTimeSpanInMin { get; set; }

        /// <summary>
        /// Defines the minimum time span between  meal conversation timeout waiter reminders
        /// </summary>
        /// <MetaDataID>{3b1b42ac-62c6-4dc1-b174-eb2155bfd1aa}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        double MealConversationTimeoutWaitersUpdateTimeSpanInMin { get; set; }

        /// <summary>
        /// Defines the minimum time span between the two remind messages for care giving worker of long time conversation
        /// </summary>
        /// <MetaDataID>{4b780f51-7d65-4672-b144-80bcbe1d379c}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        double MealConversationTimeoutCareGivingUpdateTimeSpanInMin { get; set; }

        /// <summary>
        /// Defines the meal conversation available time before system  start the waiters reminder 
        /// </summary>
        /// <MetaDataID>{e1882094-aa09-46cb-b015-3f3a3897e79a}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        double MealConversationTimeoutInMin { get; set; }


        /// <summary>
        /// Defines the maximum time where session can be in UrgeToDecide state, before the system initiates the reminders to waiters.
        /// </summary>
        /// <MetaDataID>{f0b390e2-5feb-4f38-8189-381c30d88a32}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        double UrgesToDecideTimeoutInMin { get; set; }

        /// <summary>
        /// Defines the meal conversation available time before system  start the supervisor reminder 
        /// </summary>
        /// <MetaDataID>{4c2cb394-4f65-4e9e-8e06-f0dc753b6cec}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        double MealConversationTimeoutInMinForSupervisor { get; set; }
    }

}