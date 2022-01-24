using System;
using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{f1dab94d-0e50-4490-bf2f-7e233e619728}</MetaDataID>
    [BackwardCompatibilityID("{f1dab94d-0e50-4490-bf2f-7e233e619728}")]
    [GenerateFacadeProxy]
    public interface IFoodServiceSession
    {
        /// <MetaDataID>{34f182d7-6290-458b-896f-9b83e3e53444}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        IFlavoursServicesContextRuntime ServicesContextRuntime { get; }


        /// <MetaDataID>{799eb1a5-caba-4b2d-9ea1-96aab99665e0}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Description { get; }
        [Association("SessionMeal", Roles.RoleA, "b11e0deb-6ec0-4653-a06b-02610d68abcb")]
        [RoleAMultiplicityRange(1, 1)]
        RoomService.IMeal Meal { get;  }

         
        [RoleAMultiplicityRange(0)]
        [Association("FoodServiceSession", Roles.RoleA, "93808acd-1c78-45da-8c44-dd7666ae0128")]
        System.Collections.Generic.IList<IFoodServiceClientSession> PartialClientSessions { get; }


        [Association("ServicePointSesions", Roles.RoleB, "08fdaee2-f871-4200-9856-8d2cc9754909")]
        [RoleBMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        IServicePoint ServicePoint { get;  }


        /// <MetaDataID>{45ae4db3-87ca-4a6e-b867-bb5f33886b05}</MetaDataID>
        void AddPartialSession(IFoodServiceClientSession partialSession);

        /// <MetaDataID>{b88cdc8c-90c3-4a52-8f71-cc551a101066}</MetaDataID>
        void RemovePartialSession(IFoodServiceClientSession partialSession);


        /// <MetaDataID>{5ef17468-8f86-47d4-8ad3-4c8c01bdb790}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        DateTime SessionEnds { get; set; }

        /// <MetaDataID>{49801850-132d-4c7f-98e7-5fb7cce430ef}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        DateTime SessionStarts { get; set; }

        /// <MetaDataID>{1859dba8-9740-40df-92a0-ceacd7586667}</MetaDataID>
        SessionState SessionState { get; set; }



     



    }

    /// <MetaDataID>{e95ad745-80af-4906-8191-1962f605d70b}</MetaDataID>
    public enum SessionState
    {
        Conversation = 0,
        UrgesToDecide = 1,
        MealValidationDelay = 2,
        MealMonitoring = 3
    }
}