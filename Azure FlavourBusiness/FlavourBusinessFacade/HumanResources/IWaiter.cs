using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using System;
using System.Collections.Generic;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{6bf0a5f4-a27f-4f50-a3d5-debd96e3023d}</MetaDataID>
    [BackwardCompatibilityID("{6bf0a5f4-a27f-4f50-a3d5-debd96e3023d}")]
    [GenerateFacadeProxy]
    public interface IWaiter : EndUsers.IMessageConsumer, IServicesContextWorker, IUser
    {
        ///// <MetaDataID>{9d5dfa9d-7c42-414f-b9c8-be802f878bb6}</MetaDataID>

        ///// <MetaDataID>{9c2ff85f-2c0a-41c7-b73b-c27dbf792966}</MetaDataID>
        //void TransferSession(IFoodServiceSession foodServiceSession,  string targetServicePointIdentity);
        /// <MetaDataID>{91e3898f-1fb5-4292-8fd6-a8205b016900}</MetaDataID>
        IShiftWork NewShiftWork(System.DateTime startedAt, double timespanInHours, decimal openingBalanceFloatCash);

        /// <MetaDataID>{aa6ffdd8-59a3-4253-940f-ce7c4e0d1135}</MetaDataID>
        /// <summary>This token is the identity of device for push notification mechanism</summary>
        [BackwardCompatibilityID("+10")]
        string DeviceFirebaseToken { get; set; }
        /// <MetaDataID>{79a921fc-7507-410d-bb75-57d83658cb91}</MetaDataID>
        IList<IHallLayout> GetServiceHalls();

        /// <MetaDataID>{68e377ff-5d13-448b-ac74-5ea71044ae75}</MetaDataID>
        Dictionary<string, ServicePointState> HallsServicePointsState { get; }


        ///// <MetaDataID>{29fff726-6815-43a3-a0e3-cd1fbd138357}</MetaDataID>
        //void AddClientSession(EndUsers.IFoodServiceClientSession clientSession);

     


        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        ///// <MetaDataID>{a2b1a0a3-2229-426c-a66d-5ba110f0605a}</MetaDataID>
        //void RemoveClientSession(EndUsers.IFoodServiceClientSession clientSession);


        /// <MetaDataID>{21cb6307-65d2-4394-b1d9-bb1cc076eea4}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        bool Suspended { get; }


   

        /// <MetaDataID>{915f5c19-eff7-4bc5-81a2-ee312d6f8902}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+5")]
        string WaiterAssignKey { get; set; }

        /// <MetaDataID>{b7089f78-0d6d-456d-b88d-7b1ea2744972}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+4")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{0db5ebc9-5c07-4512-ba47-dc6c0dd7e177}</MetaDataID>
        [BackwardCompatibilityID("+9")]
        string Password { get; set; }

        /// <MetaDataID>{91d91e46-e2d0-4bc5-b079-b31c86a3c210}</MetaDataID>
        IList<IServingBatch> GetServingBatches();


        /// <MetaDataID>{738255e8-f3cf-4f76-be25-11890cab1610}</MetaDataID>
        ServingBatchUpdates GetServingUpdates(List<ItemPreparationAbbreviation> servingItemsOnDevice);

        /// <MetaDataID>{018c2f0e-7ace-4dd6-b878-d45801ffe0ed}</MetaDataID>
        void AssignServingBatch(IServingBatch servingBatch);

        /// <MetaDataID>{66b51e30-3deb-4365-bcd9-ea6bd3ae251c}</MetaDataID>
        void DeassignServingBatch(IServingBatch servingBatch);


        event ServingBatchesChangedHandler ServingBatchesChanged;

        /// <MetaDataID>{14db7294-fc48-453c-8f4c-bc9029b1c84f}</MetaDataID>
        void TableIsLay(string servicesPointIdentity);

        /// <summary></summary>
        /// <MetaDataID>{669c2345-01db-40c9-b76b-48c313968acc}</MetaDataID>
        // <MetaDataID>{bc8e1dff-883d-4487-8b67-e4c7fdcbb2b7}</MetaDataID>
        void CommitServingBatches();
        /// <MetaDataID>{83e301d6-ada6-4731-b494-5729b217b500}</MetaDataID>
        void WillTakeCareMealConversationTimeout(string servicePointIdentity,string sessionID);
        /// <MetaDataID>{28dc371b-df3d-44b0-b7b8-7a6e4e724cef}</MetaDataID>
        void TransferPartialSession(string partialSessionID, string targetSessionID);
        /// <MetaDataID>{781bd75e-a110-48ad-950c-90838b2e70fd}</MetaDataID>
        void TransferItems(List<SessionItemPreparationAbbreviation> itemPreparations, string targetServicePointIdentity);
        /// <MetaDataID>{b36c8832-4ae0-49a8-b3b5-4c86eb259a1d}</MetaDataID>
        IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations, EndUsers.IFoodServiceClientSession foodServicesClientSession);
        List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate);
    }


    public delegate void ServingBatchesChangedHandler();

    
   

}