using OOAdvantech.Remoting;
using OOAdvantech;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{4d8e5461-753a-456c-a900-52f44fa6b943}</MetaDataID>
    public interface ITakeawayCashier : FlavourBusinessFacade.HumanResources.IServicesContextWorker
    {

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;
    }
}