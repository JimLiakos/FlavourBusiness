using OOAdvantech.Remoting;
using OOAdvantech;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{9a46c7ba-f831-4e78-8737-9b7015f48847}</MetaDataID>
    public interface ICourier : IServicesContextWorker, EndUsers.IMessageConsumer, IUser
    {

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{b6a6576e-c50f-40c3-8abf-dd3889bf3414}</MetaDataID>
        string DeviceFirebaseToken { get; set; }
        

    }
}