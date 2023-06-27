using OOAdvantech.Remoting;
using OOAdvantech;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{4d8e5461-753a-456c-a900-52f44fa6b943}</MetaDataID>
    public interface ITakeawayCashier :  EndUsers.IMessageConsumer, IServicesContextWorker, IUser
    {

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;
    }
}