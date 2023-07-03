using OOAdvantech.Remoting;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{4d8e5461-753a-456c-a900-52f44fa6b943}</MetaDataID>
    [BackwardCompatibilityID("{4d8e5461-753a-456c-a900-52f44fa6b943}")]
    [HttpVisible]
    [GenerateFacadeProxy]
    public interface ITakeawayCashier : HumanResources.IServicesContextWorker, EndUsers.IMessageConsumer, IUser
    {

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        string DeviceFirebaseToken { get; set; }
    }
}