using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using System.Collections.Generic;

namespace FlavourBusinessFacade.HumanResources
{
    /// <MetaDataID>{6bf0a5f4-a27f-4f50-a3d5-debd96e3023d}</MetaDataID>
    [BackwardCompatibilityID("{6bf0a5f4-a27f-4f50-a3d5-debd96e3023d}")]
    [GenerateFacadeProxy]
    public interface IWaiter : EndUsers.IMessageConsumer, IServicesContextWorker, IUser
    {
        /// <MetaDataID>{aa6ffdd8-59a3-4253-940f-ce7c4e0d1135}</MetaDataID>
        /// <summary>This token is the identity of device for push notification mechanism</summary>
        [BackwardCompatibilityID("+10")]
        string DeviceFirebaseToken { get; set; }
        /// <MetaDataID>{79a921fc-7507-410d-bb75-57d83658cb91}</MetaDataID>
        IList<IHallLayout> GetServiceHalls();

        /// <MetaDataID>{29fff726-6815-43a3-a0e3-cd1fbd138357}</MetaDataID>
        void AddClientSession(EndUsers.IFoodServiceClientSession clientSession);

        /// <MetaDataID>{278fd92b-7ae7-4817-9f43-d22ab6aefbad}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [Association("WaterServiceSession", OOAdvantech.MetaDataRepository.Roles.RoleA, "0c49af08-a143-4f46-8e69-6f3b0f44870b")]
        List<EndUsers.IFoodServiceClientSession> ClientSessions { get; }


        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{a2b1a0a3-2229-426c-a66d-5ba110f0605a}</MetaDataID>
        void RemoveClientSession(EndUsers.IFoodServiceClientSession clientSession);


        /// <MetaDataID>{21cb6307-65d2-4394-b1d9-bb1cc076eea4}</MetaDataID>
        [BackwardCompatibilityID("+8")]
        bool Suspended { get; }


        /// <MetaDataID>{5286bb2d-b59e-43bc-9a73-cdf66128eb44}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+6")]
        string SignUpUserIdentity { get; set; }

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
        ServingBatchUpdates GetServingUpdate(List<ItemPreparationAbbreviation> servingItemsOnDevice);

        /// <MetaDataID>{018c2f0e-7ace-4dd6-b878-d45801ffe0ed}</MetaDataID>
        void AssignServingBatch(IServingBatch servingBatch);

        /// <MetaDataID>{66b51e30-3deb-4365-bcd9-ea6bd3ae251c}</MetaDataID>
        void DeassignServingBatch(IServingBatch servingBatch);


        event ServingBatchesChangedHandler ServingBatchesChanged;


    }


    public delegate void ServingBatchesChangedHandler();

}