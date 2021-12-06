using System.Collections.Generic;
using ComputationalResources;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{d2658cac-9f51-4b7c-bee5-1fe2df5c0e7f}</MetaDataID>
    [BackwardCompatibilityID("{d2658cac-9f51-4b7c-bee5-1fe2df5c0e7f}")]
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IFlavoursServicesContext
    {
        [Association("ServiceContextPreparationStation", Roles.RoleA, "a73cc7de-ca5d-44dd-bc94-4bd2e0b3c5b3")]
        System.Collections.Generic.IList<FlavourBusinessFacade.ServicesContextResources.IPreparationStation> PreparationStations { get; }

        /// <MetaDataID>{4fcb3b84-9bdd-470f-a6d3-4ba48399c329}</MetaDataID>
        void RemovePreparationStation(IPreparationStation prepartionStation);

        /// <MetaDataID>{3a9a0eba-b39d-41be-9c4a-24fa5c2951c2}</MetaDataID>
        IPreparationStation NewPreparationStation();

        [RoleAMultiplicityRange(0)]
        [Association("ServiceContextCashierStation", Roles.RoleA, "8d78094d-bc63-4495-9756-5e965b5eece1")]
        IList<ICashierStation> CashierStations { get; }


        IList<FinanceFacade.IFisicalParty> FisicalParties { get; }


        /// <MetaDataID>{a1ad8b56-94d9-453e-be84-9485915eb0f1}</MetaDataID>
        ICashierStation NewCashierStation();

        /// <MetaDataID>{7cc3c0eb-0aa6-490f-839b-e4f96a5ab6fa}</MetaDataID>
        void RemoveCashierStation(ICashierStation cashierStation);




        FinanceFacade.IFisicalParty NewFisicalParty();

        void RemoveFisicalParty(FinanceFacade.IFisicalParty fisicalParty);

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


        [RemoteEventPublish(InvokeType.Async)]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

    }
}