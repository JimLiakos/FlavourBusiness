using OOAdvantech.Remoting;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using System.Collections.Generic;
using FlavourBusinessFacade.Shipping;
using FlavourBusinessFacade.RoomService;
using OOAdvantech.Remoting.RestApi;
using FlavourBusinessFacade.ServicesContextResources;

namespace FlavourBusinessFacade.HumanResources
{




    /// <MetaDataID>{9a46c7ba-f831-4e78-8737-9b7015f48847}</MetaDataID>
    [GenerateFacadeProxy]
    public interface ICourier : IServicesContextWorker, EndUsers.IMessageConsumer, IUser
    {

        IHomeDeliveryServicePoint HomeDeliveryServicePoint { get; }

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{b6a6576e-c50f-40c3-8abf-dd3889bf3414}</MetaDataID>
        [BackwardCompatibilityID("+1")]
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


        /// <MetaDataID>{71a003c6-49aa-435b-bef2-8aec8d26b833}</MetaDataID>
        void AssignAndCommitFoodShipping(IFoodShipping foodShipping);

        void AssignAndCommitFoodShippings(List<IFoodShipping> foodShippings,string apiKey);

        /// <MetaDataID>{afb21fc6-2563-4fc0-b82b-04f40bffff7b}</MetaDataID>
        void FoodShippingReturn(IFoodShipping foodShipping, string returnReasonIdentity, string customReturnReasonDescription = null);


        /// <MetaDataID>{d4f798a5-fd05-4d74-85cb-fb4ee037e0e7}</MetaDataID>
        void Delivered(IFoodShipping foodShipping);


        /// <MetaDataID>{f79b75a4-bc33-483a-b07c-d6b4555143f5}</MetaDataID>
        IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations, IFoodShipping foodShipping);

        /// <MetaDataID>{a066838f-a33d-4ed4-a31f-1ca371f51873}</MetaDataID>
        void PrintFoodShippingReceipt(IFoodShipping foodShipping);
        /// <MetaDataID>{b816807d-4fb9-44c0-877e-b59cd332fdf5}</MetaDataID>
        void RemoveFoodShippingAssignment(IFoodShipping foodShipping);

        /// <summary>
        /// The courier informs that he has returned
        /// </summary>
        // <MetaDataID>{215e7bea-ba6a-4879-9368-d2b61f0422cb}</MetaDataID>
        void ImBack();


        /// <MetaDataID>{eb973ca2-1979-488f-a143-f9a9c0238324}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        CourierState State { get; }
    }

    public delegate void FoodShippingsChangedHandler();

    /// <summary>
    /// Courier can be in following state like, wait for shipments,
    /// collect shipments, on the road, he has deliver all shipment and he returning
    /// </summary>
    /// <MetaDataID>{6b30fbb3-87aa-4dae-8cfa-24c96c1b065b}</MetaDataID>
    public enum CourierState
    {
        /// <summary>
        ///Courier is idle when he isn't in shift 
        /// </summary>
        Idle = 0,
        /// <summary>
        /// wait for shipments,
        /// </summary>
        PendingForFoodShiping = 1,
        /// <summary>
        /// Collect foodShipings 
        /// </summary>
        CollectFoodShiping = 2,
        NearDeliveryServicePoint = 3,
        /// <summary>
        /// On the road to deliver shipment,
        /// </summary>
        OnTheRoad = 4,
        /// <summary>
        ///  He has deliver all shipment and he returning
        /// </summary>
        EndOfDeliveryAndReturn = 5

    }

    /// <MetaDataID>{ca6e1cc6-8c42-464f-abb8-e32a9675b204}</MetaDataID>
    public class PaidFoodShippingException : SerializableException
    {
        /// <MetaDataID>{27f80ffa-a129-4f43-8cfd-851370e0ab79}</MetaDataID>
        public PaidFoodShippingException(string message) : base(message) { }

        /// <MetaDataID>{fcbb4bef-c1d3-463d-bada-dea2de26f2f1}</MetaDataID>
        public PaidFoodShippingException(string message, System.Exception innerException) : base(message, innerException)
        {

        }
    }
}