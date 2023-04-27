using FlavourBusinessFacade.EndUsers;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{bbc7f3b3-01c7-452c-b755-d80f2c6278eb}</MetaDataID>
    [GenerateFacadeProxy]
    [BackwardCompatibilityID("{bbc7f3b3-01c7-452c-b755-d80f2c6278eb}")]
    public interface ITakeAwayStation : IServicePoint
    {
        /// <MetaDataID>{74e2edf9-6a85-478e-8fba-4baaf85141dd}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string GraphicMenuStorageIdentity { get; set; }

        /// <MetaDataID>{a007d657-c566-4605-bdcc-e204a71b166f}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string TakeAwayStationIdentity { get; set; }

        IFoodServiceClientSession GetUncommittedFoodServiceClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken);

        ///// <MetaDataID>{60768d22-7047-4dd0-b0db-0d34a6734813}</MetaDataID>
        //[BackwardCompatibilityID("+2")]
        //string ServicesContextIdentity { get; set; }
        ///// <MetaDataID>{006f4cf1-7d55-4c31-a313-dfd33f4d20b5}</MetaDataID>
        //[BackwardCompatibilityID("+3")]
        //string Description { get; set; }

    }
}