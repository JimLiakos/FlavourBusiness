using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{93d218ca-c96a-43bb-abfa-76d5c7b02d76}</MetaDataID>
    [BackwardCompatibilityID("{93d218ca-c96a-43bb-abfa-76d5c7b02d76}")]
    [GenerateFacadeProxy]
    public interface IServiceContextSupervisor : IServicesContextWorker, IUser
    {
        /// <MetaDataID>{d56b1c62-f661-4152-bdc5-e1e56ed7de7b}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        bool Suspended { get; }

        /// <MetaDataID>{49119625-8e20-4fbb-92a2-8bbbe5c2762f}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        string FlavoursServiceContextDescription { get; }

        /// <MetaDataID>{ae24a8ef-1a64-4601-94e0-003c4b2f4b65}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        string SupervisorAssignKey { get; set; }


        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{6c52c98d-2c97-4201-a90b-2cc86c8bda2d}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        string SignUpUserIdentity { get; set; }

        /// <MetaDataID>{149349fa-11af-4b3a-9a0a-87f48eca6862}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        string ServicesContextIdentity { get; set; }

        


    }
}