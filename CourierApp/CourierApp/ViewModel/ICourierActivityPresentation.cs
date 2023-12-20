using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;


using OOAdvantech.MetaDataRepository;
using ServiceContextManagerApp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

#if DeviceDotNet
#else
using FlavourBusinessManager.HumanResources;
using FlavourBusinessFacade.Shipping;
#endif

namespace CourierApp.ViewModel
{
    public delegate void ItemsReadyToServeRequesttHandle(ICourierActivityPresentation courierActivityPresentation, string messageID, string servicePointIdentity);
    /// <MetaDataID>{99fe8217-2732-4d1a-b074-c88a95272563}</MetaDataID>
    [HttpVisible]
    public interface ICourierActivityPresentation
    {

        IServingShiftWork ActiveShiftWork { get; }
        /// <MetaDataID>{fc4f212d-57ff-417d-8b8b-ea68cb3d8c2a}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{04f664fc-a085-4e55-9879-a312a68cedec}</MetaDataID>
        System.DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{d6adeae7-662f-4f1a-a607-368bc24b15b6}</MetaDataID>
        System.DateTime ActiveShiftWorkEndsAt { get; }

        /// <MetaDataID>{a5b9e378-ae89-401e-855f-d6ccbeb90358}</MetaDataID>
        void ShiftWorkStart(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{33205e02-25f6-40e9-bba7-908e20107a91}</MetaDataID>
        void ExtendShiftWorkStart(double timespanInHours);

        ICourierPresentation CourierPresentation { get; }

        bool CourierOnTheRoadToReturn { get; }

        bool CourierOnTheRoad { get; }

        [GenerateEventConsumerProxy]
        event ItemsReadyToServeRequesttHandle ItemsReadyToServeRequest;

        void ItemsReadyToServeMessageReceived(string messageID);
        void PrintFoodShippingReceipt(string serviceBatchIdentity);

        /// <MetaDataID>{46645e91-8e91-4f6a-b0ce-186746d60b37}</MetaDataID>
        Task<bool> AssignCourier();

        /// <MetaDataID>{727612e2-821b-4e1c-8626-25c56b73a190}</MetaDataID>
        Task<UserData> AssignDeviceToNativeUserCourier();

        bool ScanShippingEnabled { get;set; }

        Task<bool> AssignScannerDevice();

        bool IsScannerDevice { get; }
        string PairedWithCourierFullName { get; }


        bool ThereIsPairedFoodShippings { get; }

        Task CourierShippingPair();

        List<FoodShippingPresentation> FoodShippings { get; }

        List<FoodShippingPresentation> AssignedFoodShippings { get; }

        Task ImBack();
        Task<bool> AssignFoodShipping(string foodShippingIdentity);

        Task<bool> DeAssignFoodShipping(string foodShippingIdentity);

        Task<bool> RemoveScannedFoodShipping(string foodShippingIdentity);


        void FoodShippingReturn(string foodShippingIdentity, string returnReasonIdentity, string customReturnReasonDescription = null);
        void FoodShippingDelivered(string foodShippingIdentity);
        void PhoneCall(string foodShippingIdentity);

        void Navigate(string foodShippingIdentity);

        bool CommitFoodShippings();

        /// <MetaDataID>{34608ba4-b311-465a-aecb-ec3cd816d36f}</MetaDataID>
        void PrintFoodShippingsReceipt(string foodShippingIdentity);


        IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations, string foodShippingIdentity);

        Task Pay(FinanceFacade.IPayment payment, FinanceFacade.PaymentMethod paymentMethod, decimal tipAmount);







    }


    /// <MetaDataID>{d776ea9a-7189-4215-99f8-763e6228dae0}</MetaDataID>
    public class CourierShippingPairPresentation
    {
        public FoodShippingPresentation FoodShipping { get; set; }

        public ICourierPresentation CourierPresentation { get; set; }

    }
}
