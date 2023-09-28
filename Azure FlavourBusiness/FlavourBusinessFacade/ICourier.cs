using OOAdvantech.Remoting;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{9bef8e35-06f8-4a19-962b-5d198d719468}</MetaDataID>
    [GenerateFacadeProxy]
    /// <MetaDataID>{9a46c7ba-f831-4e78-8737-9b7015f48847}</MetaDataID>
    public interface ICourier : IServicesContextWorker, EndUsers.IMessageConsumer, IUser
    {

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{b6a6576e-c50f-40c3-8abf-dd3889bf3414}</MetaDataID>
        string DeviceFirebaseToken { get; set; }

        event FoodShippingsChangedHandler FoodShippingsChanged; 


    }

    public delegate void FoodShippingsChangedHandler();
}