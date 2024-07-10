using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{93d218ca-c96a-43bb-abfa-76d5c7b02d76}</MetaDataID>
    [BackwardCompatibilityID("{93d218ca-c96a-43bb-abfa-76d5c7b02d76}")]
    [GenerateFacadeProxy]
    public interface IServiceContextSupervisor : IServicesContextWorker, EndUsers.IMessageConsumer,IUser
    {
        /// <MetaDataID>{7b3512e8-8584-4d3f-b328-137d039e72ac}</MetaDataID>
        [BackwardCompatibilityID("+10")]
        string DeviceFirebaseToken { get; set; }

        /// <MetaDataID>{49119625-8e20-4fbb-92a2-8bbbe5c2762f}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        string FlavoursServiceContextDescription { get; }

      


        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;


    
   
        /// <MetaDataID>{0a94826c-6b90-4b08-8776-b3c3ea0d6635}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        IFlavoursServicesContextRuntime ServicesContextRunTime { get; }

        /// <MetaDataID>{ded978ff-be44-4d88-955f-e71821410aa7}</MetaDataID>
        FlavourBusinessFacade.IFlavoursServicesContext ServicesContext { get; }

        /// <MetaDataID>{bbda1053-6cc6-4a60-bace-58a955af82ab}</MetaDataID>
        void IWillTakeCare(string messageID);
    }
}