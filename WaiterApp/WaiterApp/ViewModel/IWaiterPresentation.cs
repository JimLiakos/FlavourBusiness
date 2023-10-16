using FlavourBusinessFacade;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WaiterApp.ViewModel
{

    public delegate void LaytheTableRequestHandle(IWaiterPresentation waiterPresentation, string messageID,string servicePointIdentity);
    public delegate void ItemsReadyToServeRequesttHandle(IWaiterPresentation waiterPresentation, string messageID, string servicePointIdentity);
    public delegate void MealConversationTimeoutHandle(IWaiterPresentation waiterPresentation, string messageID, string servicePointIdentity, string sessionIdentity);
    public delegate void ServicePointChangeStateHandle(IWaiterPresentation waiterPresentation, string servicePointIdentity, ServicePointState newState);
    /// <MetaDataID>{981df64d-06d2-47af-9900-792dd6492ef0}</MetaDataID>
    [HttpVisible]
    public interface IWaiterPresentation
    {
        [GenerateEventConsumerProxy]
        event LaytheTableRequestHandle LayTheTableRequest;

        [GenerateEventConsumerProxy]
        event ServicePointChangeStateHandle ServicePointChangeState;

        [GenerateEventConsumerProxy]
        event ItemsReadyToServeRequesttHandle ItemsReadyToServeRequest;


        [GenerateEventConsumerProxy]
        event MealConversationTimeoutHandle MealConversationTimeout;

        /// <MetaDataID>{4f9504ee-2809-4910-b6fd-7f6356eb649b}</MetaDataID>
        void WillTakeCareMealConversationTimeout(string servicePointIdentity, string sessionIdentity);

        /// <MetaDataID>{d89e355f-0e0e-4267-b36b-209817f2ad4c}</MetaDataID>
        DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; }

        /// <MetaDataID>{315e4a19-6d46-4ae0-8e38-6bef23632d5a}</MetaDataID>
        Task<bool> AssignWaiter();

        Task<UserData> AssignDeviceToNativeUserWaiter();

        /// <MetaDataID>{b385fbf4-0e92-4edb-9862-20a31ed527e5}</MetaDataID>
        bool IsActiveWaiter { get; }

        /// <MetaDataID>{4ae6f5cd-36bf-4ed3-b400-217627868665}</MetaDataID>
        IList<IHallLayout> Halls
        {
            get;
        }

        /// <MetaDataID>{c884e466-102c-4758-b223-70b88e29d47e}</MetaDataID>
        Dictionary<string, ServicePointState> HallsServicePointsState { get; }
        /// <MetaDataID>{030650aa-927d-4f55-bfc3-5c6b03a2a01f}</MetaDataID>
        List<ServingBatchPresentation> ServingBatches
        {
            get;
        }

        /// <MetaDataID>{a5640169-af7e-447a-88bc-27f193223926}</MetaDataID>
        List<ServingBatchPresentation> AssignedServingBatches
        {
            get;
        }

        /// <MetaDataID>{dfaa4dda-f0ac-4161-8a79-0f674b3e22fc}</MetaDataID>
        bool AssignServingBatch(string serviceBatchIdentity);

        /// <MetaDataID>{18cc804f-3d18-44bd-9af6-cf88b0795166}</MetaDataID>
        void PrintServingBatchReceipt(string serviceBatchIdentity);


        /// <MetaDataID>{d37c92b8-b562-4c44-87b2-61dd58581be7}</MetaDataID>
        bool CommitServingBatches();

        /// <MetaDataID>{9f40b25d-5687-4e9e-ab9a-58e07ce7aae9}</MetaDataID>
        bool DeAssignServingBatch(string serviceBatchIdentity);

        /// <MetaDataID>{6431db8a-0b35-4649-b9e6-60772ac21c31}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{872cf9a1-885b-48ee-b71c-19abceda805f}</MetaDataID>
        System.DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{3c03cff2-dd6b-43e3-8436-12093d1a91f0}</MetaDataID>
        System.DateTime ActiveShiftWorkEndsAt { get; }

        //period
        /// <MetaDataID>{f10b2dc3-edf7-449e-b7da-b11abce01e4d}</MetaDataID>
        void ShiftWorkStart(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{dd0b3e52-feec-4357-a6f5-07145aad2e21}</MetaDataID>
        void ExtendShiftWorkStart(double timespanInHours);

        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{b20c4296-54cc-4ba7-8571-22662563460a}</MetaDataID>
        void ItemsReadyToServeMessageReceived(string messageID);

        /// <MetaDataID>{e1f075a2-29ed-4ebc-ba36-221dbb047ce4}</MetaDataID>
        void MealConversationTimeoutReceived(string messageID);


        /// <MetaDataID>{d2cfeb6c-1497-4e02-a9c1-3af3610ee518}</MetaDataID>
        void LayTheTableMessageReceived(string messageID);


        /// <MetaDataID>{40506b95-ca01-4822-8424-0b9fe9143f53}</MetaDataID>
        void TableIsLay(string servicesPointIdentity);





    }
}
