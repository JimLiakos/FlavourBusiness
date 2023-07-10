using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{bbc7f3b3-01c7-452c-b755-d80f2c6278eb}</MetaDataID>
    [GenerateFacadeProxy]
    [BackwardCompatibilityID("{bbc7f3b3-01c7-452c-b755-d80f2c6278eb}")]
    public interface ITakeAwayStation : IServicePoint
    {
        /// <MetaDataID>{d20c6721-ce11-4bc1-9f19-acdf58046c75}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string PaymentTerminalIdentity { get; set; }

        /// <MetaDataID>{74e2edf9-6a85-478e-8fba-4baaf85141dd}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string GraphicMenuStorageIdentity { get; set; }

        /// <MetaDataID>{a007d657-c566-4605-bdcc-e204a71b166f}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string TakeAwayStationIdentity { get; set; }

        /// <MetaDataID>{26535cf8-849f-4825-af31-1fd088f7a484}</MetaDataID>
        IList<UserData> GetNativeUsers();
        /// <MetaDataID>{a92cc1f7-b116-4fac-9b3c-8812169fd2a0}</MetaDataID>
        IFoodServiceClientSession GetUncommittedFoodServiceClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken);

        /// <MetaDataID>{75eada58-23a6-48db-8f89-483755d67b94}</MetaDataID>
        UserData SignInNativeUser(string userName, string password);

        ///// <MetaDataID>{60768d22-7047-4dd0-b0db-0d34a6734813}</MetaDataID>
        //[BackwardCompatibilityID("+2")]
        //string ServicesContextIdentity { get; set; }
        ///// <MetaDataID>{006f4cf1-7d55-4c31-a313-dfd33f4d20b5}</MetaDataID>
        //[BackwardCompatibilityID("+3")]
        //string Description { get; set; }

    }
}