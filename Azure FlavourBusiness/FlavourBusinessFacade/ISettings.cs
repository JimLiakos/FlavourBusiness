using System;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{8173beba-c4bd-466c-bd65-971c6aeb72bb}</MetaDataID>
    public interface ISettings
    {
        /// <summary>
        /// Defines the maximum meal progress when the meals controller can auto assign a Food service client session
        /// </summary>
        /// <MetaDataID>{6ce43cca-6699-45e2-b4b0-2016fde1ee31}</MetaDataID>
        double AutoAssignMaxMealProgress { get; set; }

        /// <summary>
        /// Defines the minimum lifespan of the session that can be marked as forgotten
        /// </summary>
        /// <MetaDataID>{2802ce16-4fc0-4c15-a4e0-59dacb93443c}</MetaDataID>
        double ForgottenSessionLifeTimeSpanInMin { get; set; }
        /// <summary>
        ///  Defines the minimum sleep duration of the device app that can be marked as forgotten
        /// </summary>
        /// <MetaDataID>{27134864-4f08-40b4-9e69-0d19bc6b3791}</MetaDataID>
        double ForgottenSessionDeviceSleepTimeSpanInMin { get; set; }

        /// <summary>
        ///  Defines the minimum time since the last change in session that can be marked as forgotten
        /// </summary>
        /// <MetaDataID>{261708e9-3e50-4ed3-b733-801e22b46ff5}</MetaDataID>
        double ForgottenSessionLastChangeTimeSpanInMin { get; set; }


        /// <MetaDataID>{3b1b42ac-62c6-4dc1-b174-eb2155bfd1aa}</MetaDataID>
        double MealConversationTimeoutWaitersUpdateTimeSpanInMin { get; set; }
    }
    
}