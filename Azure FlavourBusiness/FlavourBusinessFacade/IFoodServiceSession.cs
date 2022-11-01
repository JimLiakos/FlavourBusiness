using System;
using System.Collections.Generic;
using FinanceFacade;
using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{f1dab94d-0e50-4490-bf2f-7e233e619728}</MetaDataID>
    [BackwardCompatibilityID("{f1dab94d-0e50-4490-bf2f-7e233e619728}")]
    [GenerateFacadeProxy]
    public interface IFoodServiceSession
    {
        [RoleBMultiplicityRange(0, 1)]
        [Association("BillingPayment", Roles.RoleA, "27108d66-3180-46e0-881f-6b52acda72ce")]
        [RoleAMultiplicityRange(1)]
        System.Collections.Generic.List<FinanceFacade.IPayment> BillingPayments { get; }

        /// <MetaDataID>{ebb02282-df96-4989-8e11-5f78e47ac3df}</MetaDataID>
        void AddPayment(IPayment payment);

        /// <MetaDataID>{eb4da815-da0f-48a4-bd0a-edab7d76c371}</MetaDataID>
        void RemovePayment(IPayment payment);

        /// <MetaDataID>{57dcf2ab-8af0-470f-a50a-b6c812373164}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        DateTime WillTakeCareTimestamp { get; set; }

        /// <MetaDataID>{47ec6550-33af-4b17-9d14-fc5b9f3dca9f}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        string SessionID { get; }

        /// <MetaDataID>{34f182d7-6290-458b-896f-9b83e3e53444}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        IFlavoursServicesContextRuntime ServicesContextRuntime { get; }


        /// <MetaDataID>{799eb1a5-caba-4b2d-9ea1-96aab99665e0}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string Description { get; }
        [Association("SessionMeal", Roles.RoleA, "b11e0deb-6ec0-4653-a06b-02610d68abcb")]
        [RoleAMultiplicityRange(1, 1)]
        RoomService.IMeal Meal { get; }


        [RoleAMultiplicityRange(0)]
        [Association("FoodServiceSession", Roles.RoleA, "93808acd-1c78-45da-8c44-dd7666ae0128")]
        System.Collections.Generic.IList<IFoodServiceClientSession> PartialClientSessions { get; }


        [Association("ServicePointSesions", Roles.RoleB, "08fdaee2-f871-4200-9856-8d2cc9754909")]
        [RoleBMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        IServicePoint ServicePoint { get; }


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

        /// <MetaDataID>{fad1d85a-a4bc-45ae-b332-452430da318a}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        SessionType SessionType { get; }

    }

    /// <MetaDataID>{e95ad745-80af-4906-8191-1962f605d70b}</MetaDataID>
    public enum SessionState
    {
        Conversation = 0,
        UrgesToDecide = 1,
        MealValidationDelay = 2,
        MealMonitoring = 3,
        Closed=4
    }




}
