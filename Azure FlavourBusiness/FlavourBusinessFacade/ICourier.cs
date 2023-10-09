using OOAdvantech.Remoting;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using FlavourBusinessFacade.Shipping;
using FlavourBusinessFacade.RoomService;

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

        IList<IFoodShipping> GetFoodShippings();

        ServingBatchUpdates GetFoodShippingUpdates(List<ItemPreparationAbbreviation> servingItemsOnDevice);

        void CommitFoodShipings();
        void DeAssignFoodShipping(IFoodShipping foodShipping);

        void AssignFoodShipping(IFoodShipping foodShipping);

        void PrintFoodShippingReceipt(IFoodShipping foodShipping);
    }

    public delegate void FoodShippingsChangedHandler();
}