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

        /// <MetaDataID>{5cb3c164-e95e-431e-95ee-4c188e91160c}</MetaDataID>
        IList<IFoodShipping> GetFoodShippings();

        /// <MetaDataID>{2803ebe2-00a5-418d-a1f6-8a9da623f4d3}</MetaDataID>
        ServingBatchUpdates GetFoodShippingUpdates(List<ItemPreparationAbbreviation> servingItemsOnDevice);

        /// <MetaDataID>{078b7291-a5c3-4843-8eb3-2d3d579a9409}</MetaDataID>
        void CommitFoodShipings();
        /// <MetaDataID>{9358540f-c6db-4eda-a7c3-c8026c39fc84}</MetaDataID>
        void DeAssignFoodShipping(IFoodShipping foodShipping);

        /// <MetaDataID>{ef363053-6811-42a3-b955-fb922d797e91}</MetaDataID>
        void AssignFoodShipping(IFoodShipping foodShipping);


        void AssignAndCommitFoodShipping(IFoodShipping foodShipping);

        /// <MetaDataID>{a066838f-a33d-4ed4-a31f-1ca371f51873}</MetaDataID>
        void PrintFoodShippingReceipt(IFoodShipping foodShipping);
        void RemoveFoodShippingAssignment(IFoodShipping foodShipping);
    }

    public delegate void FoodShippingsChangedHandler();
}