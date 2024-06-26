using System.Collections.Generic;
using ComputationalResources;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.Shipping;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{d2658cac-9f51-4b7c-bee5-1fe2df5c0e7f}</MetaDataID>
    [BackwardCompatibilityID("{d2658cac-9f51-4b7c-bee5-1fe2df5c0e7f}")]
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IFlavoursServicesContext
    {
        /// <MetaDataID>{fd927f39-9d8c-40c7-951b-ded617dac1d7}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string OrganizationStorageIdentity { get; set; }

        /// <MetaDataID>{c6cf8b81-57e2-423f-ac30-eaa5e3167e0c}</MetaDataID>
        void RemoveCallCenterStation(IHomeDeliveryCallCenterStation homeDeliveryCallcenterStation);

        /// <MetaDataID>{37ab9263-abc5-46b7-a53d-dd7976875286}</MetaDataID>
        IHomeDeliveryCallCenterStation NewCallCenterStation();

        [RoleAMultiplicityRange(0)]
        [Association("FlavoursServiceContextCallCenter", Roles.RoleA, "18e50a98-3b7c-4cc5-96f2-6ee6002d426f")]
        [RoleBMultiplicityRange(0)]
        List<IHomeDeliveryCallCenterStation> CallCenterStations { get; }



        [Association("ServiceContexPaymentTerminal", Roles.RoleA, "bbb9a30d-0c4c-46fa-b55d-a46068da59e0")]
        [RoleBMultiplicityRange(1, 1)]
        List<IPaymentTerminal> PaymentTerminals { get; }

        /// <MetaDataID>{c0a8500a-b139-4f50-9ded-ff939a4c3e12}</MetaDataID>
        void RemovePaymentTerminal(IPaymentTerminal paymentTerminal);

        /// <MetaDataID>{91cd236d-f13a-4c5a-9b83-1d846d295280}</MetaDataID>
        IPaymentTerminal NewPaymentTerminal();

        [RoleAMultiplicityRange(0)]
        [Association("ServiceContexTakeAwayStation", Roles.RoleA, "a8709498-f1bc-42f9-a61b-d2f47e5656f3")]
        IList<ITakeAwayStation> TakeAwayStations { get; }

        /// <MetaDataID>{39007561-7984-4eed-8a83-c9b2fa902321}</MetaDataID>
        void RemoveTakeAwayStation(ITakeAwayStation takeAwayStationStation);


        

        /// <MetaDataID>{770ae1ae-dc20-4fdb-83e1-8e4b6806f409}</MetaDataID>
        void RemoveFoodTypes(List<IFoodTypeTag> foodTypeTags);

        /// <MetaDataID>{a5ae81a7-b59b-4cd5-96c8-73e3573492a1}</MetaDataID>
        void AddFoodTypes(List<IFoodTypeTag> foodTypeTags);

        
        [Association("FlavoursServicesContextFoodTypes", Roles.RoleA, "05ca45da-920e-451b-a1ef-5d0c5b894e5a")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(0)]
        List<IFoodTypeTag> FoodTypes { get; }


        [Association("ServiceContextDelivery", Roles.RoleA, "13742cae-d52d-4ee7-a9cc-02be8e8192c7")]
        [RoleAMultiplicityRange(1, 1)]
        IHomeDeliveryServicePoint DeliveryServicePoint { get; }

        /// <MetaDataID>{7868fa11-efee-4608-a447-f102ce810331}</MetaDataID>
        void LaunchHomeDeliveryService();

        /// <MetaDataID>{b825e803-0c37-4ff6-80d5-6761d343e15d}</MetaDataID>
        void RemoveHomeDeliveryService();

        [RoleAMultiplicityRange(1, 1)]
        [Association("FlavoursServicesContextSettings", Roles.RoleA, "b09368df-ec34-4529-ab3d-bce7037059db")]
        ISettings Settings { get; }




        [Association("ServiceContextPreparationStation", Roles.RoleA, "a73cc7de-ca5d-44dd-bc94-4bd2e0b3c5b3")]
        System.Collections.Generic.IList<FlavourBusinessFacade.ServicesContextResources.IPreparationStation> PreparationStations { get; }

        /// <MetaDataID>{4fcb3b84-9bdd-470f-a6d3-4ba48399c329}</MetaDataID>
        void RemovePreparationStation(IPreparationStation prepartionStation);

        /// <MetaDataID>{3a9a0eba-b39d-41be-9c4a-24fa5c2951c2}</MetaDataID>
        IPreparationStation NewPreparationStation();

        /// <MetaDataID>{e5d5e080-cf9f-4a3e-9c1e-ec4d415b9866}</MetaDataID>
        ITakeAwayStation NewTakeAwayStation();




        [RoleAMultiplicityRange(0)]
        [Association("ServiceContextCashierStation", Roles.RoleA, "8d78094d-bc63-4495-9756-5e965b5eece1")]
        IList<ICashierStation> CashierStations { get; }


        /// <MetaDataID>{9a803c21-e6d5-492f-b674-121198f0e2e3}</MetaDataID>
        IList<FinanceFacade.IFisicalParty> FisicalParties { get; }


        /// <MetaDataID>{a1ad8b56-94d9-453e-be84-9485915eb0f1}</MetaDataID>
        ICashierStation NewCashierStation();

        /// <MetaDataID>{7cc3c0eb-0aa6-490f-839b-e4f96a5ab6fa}</MetaDataID>
        void RemoveCashierStation(ICashierStation cashierStation);




        /// <MetaDataID>{1f3cafb1-d5b3-4ef7-ac70-4e864193bb1b}</MetaDataID>
        FinanceFacade.IFisicalParty NewFisicalParty();

        /// <MetaDataID>{067cb138-5b45-485d-869b-9cbb4f6a96d8}</MetaDataID>
        void RemoveFisicalParty(FinanceFacade.IFisicalParty fisicalParty);

        /// <MetaDataID>{40be539c-d860-4329-b9aa-62e1bc9a7ece}</MetaDataID>
        void UpdateFisicalParty(FinanceFacade.IFisicalParty fisicalParty);




        /// <MetaDataID>{19320e10-ee11-4ad2-9050-29003b34cf6f}</MetaDataID>
        ServiceContextResources ServiceContextResources { get; }


        /// <MetaDataID>{8be4e6e4-9524-441d-b53f-36adae823efb}</MetaDataID>
        ServiceContextHumanResources ServiceContextHumanResources { get; }


        /// <MetaDataID>{f12e83a8-39d9-4cf3-ada5-3396e51c178d}</MetaDataID>
        void LaunchCallerIDServer();
        /// <MetaDataID>{5668f772-915c-41f0-95bd-b35b790e098b}</MetaDataID>
        string NewWaiter();
        /// <MetaDataID>{498641ae-629d-4e7a-afc2-3e1856226e22}</MetaDataID>
        void RemoveWaiter(IWaiter waiter);

        /// <MetaDataID>{05aefb56-30b4-4357-b72f-e73255ed0d0e}</MetaDataID>
        void RemoveCallerIDServer();


        [Association("ServiceContextCallerID", Roles.RoleA, "81f090ce-e2b8-4790-a51c-cc956689ef6a")]
        [RoleAMultiplicityRange(1, 1)]
        ICallerIDServer CallerIDServer { get;}

        /// <MetaDataID>{7d554a4c-56ec-44d7-b676-985a47aff214}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string ContextStorageName { get; set; }

        [Association("ContextServiceArea", Roles.RoleA, "21ecf69f-7853-4a4e-8b58-53d4c97f1525")]
        [RoleBMultiplicityRange(1, 1)]
        IList<IServiceArea> ServiceAreas { get; }

        /// <MetaDataID>{45cadef9-78d2-4b56-bb74-12c20e41a1c8}</MetaDataID>
        OrganizationStorageRef GetHallLayoutStorageForServiceArea(IServiceArea serviceArea);

        /// <MetaDataID>{e44462dc-4dcc-47f5-8753-bf4b6ac5410b}</MetaDataID>
        IUploadService UploadService { get; }


        /// <MetaDataID>{95f7a34a-53b6-4cde-be63-698647cecd6c}</MetaDataID>
        IServiceArea NewServiceArea();

        /// <MetaDataID>{09d5a2de-4926-45eb-a16c-9dd4639e2e48}</MetaDataID>
        void RemoveServiceArea(IServiceArea serviceArea);

        /// <MetaDataID>{fce54e6d-5db5-4172-bbad-c9c794ef73a5}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{5ff9f6f0-dc4b-4f77-8241-dd8aef2ace0a}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [CachingDataOnClientSide]
        string Description { get; set; }



        [AssociationEndBehavior(PersistencyFlag.OnConstruction | PersistencyFlag.ReferentialIntegrity | PersistencyFlag.CascadeDelete)]
        [Association("ServicesContexComputing", Roles.RoleA, "b0aa7b24-cff2-46d0-9779-5b998754e85d")]
        [RoleAMultiplicityRange(1, 1)]
        IIsolatedComputingContext RunAtContext { get; /*set;*/ }



        [Association("OrganizationServicesContext", Roles.RoleB, "ee8ad5cc-cee0-4fa8-9c66-8cb708a061cb")]
        [RoleBMultiplicityRange(1, 1)]
        IOrganization Owner { get; }

        /// <MetaDataID>{ffdb8fd4-a147-4c0c-b5a8-dcfe1f6d4891}</MetaDataID>
        IFlavoursServicesContextRuntime GetRunTime();


        //[RemoteEventPublish(InvokeType.Async)]
        //event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        
       

    }


}