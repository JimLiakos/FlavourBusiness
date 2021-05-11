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
        [RoleAMultiplicityRange(0)]
        [Association("FoodServiceSession", Roles.RoleA, "93808acd-1c78-45da-8c44-dd7666ae0128")]
        System.Collections.Generic.IList<IFoodServiceClientSession> PartialClientSessions { get; }
        [Association("ServicePointSesions", Roles.RoleB, "08fdaee2-f871-4200-9856-8d2cc9754909")]
        [RoleBMultiplicityRange(1, 1)]
        IServicePoint ServicePoint { get;set; }


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

       
    }
}