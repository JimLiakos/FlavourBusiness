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

    /// <MetaDataID>{981df64d-06d2-47af-9900-792dd6492ef0}</MetaDataID>
    [HttpVisible]
    public interface IWaiterPresentation
    {
        [GenerateEventConsumerProxy]
        event LaytheTableRequestHandle LayTheTableRequest;

        [GenerateEventConsumerProxy]
        event ItemsReadyToServeRequesttHandle ItemsReadyToServeRequest;
        /// <MetaDataID>{d89e355f-0e0e-4267-b36b-209817f2ad4c}</MetaDataID>
        DontWaitApp.IFlavoursOrderServer FlavoursOrderServer { get; }

        /// <MetaDataID>{315e4a19-6d46-4ae0-8e38-6bef23632d5a}</MetaDataID>
        Task<bool> AssignWaiter();

        /// <MetaDataID>{b385fbf4-0e92-4edb-9862-20a31ed527e5}</MetaDataID>
        bool IsActiveWaiter { get; }

        /// <MetaDataID>{4ae6f5cd-36bf-4ed3-b400-217627868665}</MetaDataID>
        IList<IHallLayout> Halls
        {
            get;
        }
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

        /// <MetaDataID>{d37c92b8-b562-4c44-87b2-61dd58581be7}</MetaDataID>
        bool CommitServingBatches();

        /// <MetaDataID>{9f40b25d-5687-4e9e-ab9a-58e07ce7aae9}</MetaDataID>
        bool DeassignServingBatch(string serviceBatchIdentity);

        /// <MetaDataID>{6431db8a-0b35-4649-b9e6-60772ac21c31}</MetaDataID>
        bool InActiveShiftWork { get; }

        /// <MetaDataID>{872cf9a1-885b-48ee-b71c-19abceda805f}</MetaDataID>
        System.DateTime ActiveShiftWorkStartedAt { get; }

        /// <MetaDataID>{3c03cff2-dd6b-43e3-8436-12093d1a91f0}</MetaDataID>
        System.DateTime ActiveShiftWorkEndsAt { get; }

        //period
        /// <MetaDataID>{f10b2dc3-edf7-449e-b7da-b11abce01e4d}</MetaDataID>
        void SiftWorkStart(DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{dd0b3e52-feec-4357-a6f5-07145aad2e21}</MetaDataID>
        void ExtendSiftWorkStart(double timespanInHours);

        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
    }
}
