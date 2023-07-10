using System;
using System.Collections.Generic;
using System.Net;
using FinanceFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech;
using OOAdvantech.Remoting;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{ab91ca77-88ab-4ee4-835b-c651d21efde9}</MetaDataID>
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{ab91ca77-88ab-4ee4-835b-c651d21efde9}")]
    [OOAdvantech.MetaDataRepository.GenerateFacadeProxy]
    public interface IFlavoursServicesContextRuntime
    {
        /// <MetaDataID>{6a091d06-0b2c-4ff2-9cad-61d3803d2873}</MetaDataID>
        void RemovePaymentTerminal(IPaymentTerminal paymentTerminal);

        /// <MetaDataID>{6da06f95-9fe2-4cf0-8360-f3b7c0fc080e}</MetaDataID>
        IPaymentTerminal NewPaymentTerminal();

        /// <MetaDataID>{8538c2c0-46b3-4d5c-b8ef-e4184193cfd6}</MetaDataID>
        IHomeDeliveryServicePoint DeliveryServicePoint { get; }

        /// <MetaDataID>{b220d064-7c8e-4ba5-85db-8f94babb3c96}</MetaDataID>
        void RemoveHomeDeliveryService();

        /// <MetaDataID>{3ed4c592-ef05-4260-a64f-86b4e1dc62e4}</MetaDataID>
        void LaunchHomeDeliveryService();

        /// <summary>
        /// Defines the timespan in seconds to wait in AllMessmetesCommited state before move to meal monitoring state and starts meal preparation. 
        /// </summary>
        /// <MetaDataID>{968458ec-cde6-4550-aaf1-7a07b8c2df52}</MetaDataID>
        int AllMessmetesCommitedTimeSpan { get; set; }

        /// <MetaDataID>{477e1b7a-ca3c-4692-9916-8da26be81967}</MetaDataID>
        bool RemoveSupervisor(IServiceContextSupervisor supervisor);

        /// <MetaDataID>{84e2294d-fe41-42e9-b407-e442dc2d15b3}</MetaDataID>
        void MakeSupervisorActive(IServiceContextSupervisor supervisor);

        /// <MetaDataID>{83ec78d7-9509-4af4-a05e-bcda8713ae44}</MetaDataID>
        void RemoveWaiter(HumanResources.IWaiter waiter);

        /// <MetaDataID>{5e9707d0-6984-41c6-a3eb-785aa3a69450}</MetaDataID>
        string NewWaiter();

        /// <MetaDataID>{7648e907-7186-4bfa-b99e-fb057c0af603}</MetaDataID>
        string NewTakeAwayCashier();


        /// <MetaDataID>{e22a146a-d86a-477e-a7f8-6fa9cc5a8a9b}</MetaDataID>
        IShiftWork NewShifWork(IServicesContextWorker worker, System.DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{fde5e9ed-977c-483b-b6d1-bf6b1539d2cc}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        string OrganizationIdentity { get; set; }


        /// <MetaDataID>{8f88ba0b-cdb8-4b42-beca-499bca462511}</MetaDataID>
        void LaunchCallerIDServer();
        /// <MetaDataID>{54ccbe05-9a3e-452d-98d4-cf6dfdbdb8da}</MetaDataID>
        void RemoveCallerIDServer();
        /// <MetaDataID>{81acfcf3-857b-4464-ad9c-dbcb5967376b}</MetaDataID>
        ICallerIDServer CallerIDServer { get; }
        /// <MetaDataID>{65f7cd0c-ed14-437f-851c-dbf9d0a176f6}</MetaDataID>
        IList<IServiceArea> ServiceAreas { get; }

        /// <MetaDataID>{d551f736-e19b-4e48-a2db-37f4449ce5cc}</MetaDataID>
        void RemovePreparationStation(IPreparationStation prepartionStation);

        /// <MetaDataID>{baff1675-bf0a-4f40-86c7-1b3325bf6598}</MetaDataID>
        IServiceArea NewServiceArea();
        /// <MetaDataID>{7d97d36f-4b25-413a-bd6b-faf9400ced7b}</MetaDataID>
        void RemoveServiceArea(IServiceArea serviceArea);
        /// <MetaDataID>{7c1a1795-b412-44a6-92c9-bab75d3eb2dd}</MetaDataID>
        IPreparationStation NewPreparationStation();

        /// <MetaDataID>{936c5091-f18b-46eb-b1b8-320302d8d4dc}</MetaDataID>
        ITakeAwayStation NewTakeAwayStation();
        /// <MetaDataID>{b1100dc0-5804-4cbb-b5c7-d7b3422f4028}</MetaDataID>
        void RemoveTakeAwayStation(ITakeAwayStation takeAwayStationStation);

        /// <MetaDataID>{d0c2a384-0a71-4dd1-b788-afd8055ee9bb}</MetaDataID>
        IList<ITakeAwayStation> TakeAwayStations { get; }

        /// <MetaDataID>{d11922ee-a931-4230-a131-023440ea52f7}</MetaDataID>
        IPreparationStationRuntime GetPreparationStationRuntime(string preparationStationIdentity);


        /// <MetaDataID>{4d1c49d1-745a-4b4f-a934-0476332e894e}</MetaDataID>
        ServiceContextResources ServiceContextResources { get; }

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{e63eeb30-6665-4aaa-9c93-9eb6797c7b44}</MetaDataID>
        ClientSessionData GetClientSession(string servicePointIdentity, string mealInvitationSessionID, string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken, string organizationIdentity, List<OrganizationStorageRef> unSafeGraphicMenus, bool endUser, bool create);



        /// <MetaDataID>{553a6bb9-bbb1-483b-9e4e-d00945788d1f}</MetaDataID>
        IHallLayout GetHallLayout(string servicePointIdentity);

        /// <MetaDataID>{5c2d341d-66d6-4120-9092-f413edd32d87}</MetaDataID>
        IList<FlavourBusinessFacade.ServicesContextResources.IHallLayout> Halls { get; }

        /// <MetaDataID>{fe9ebf62-b446-4fd0-9475-416c4ec2f2bd}</MetaDataID>
        System.Collections.Generic.List<OrganizationStorageRef> GraphicMenus { get; }


        /// <MetaDataID>{d721f72e-a993-4207-804b-6eeff7a18ae8}</MetaDataID>
        bool IsGraphicMenuAssigned(string storageIdentity);


        /// <MetaDataID>{c9d5f6fe-d1f6-4a5b-a15e-ec3698898bbc}</MetaDataID>
        void AssignGraphicMenu(OrganizationStorageRef graphicMenuStorageRef);
        /// <MetaDataID>{002ea0f2-cdfa-4f89-ad65-23e4f0e4d5c9}</MetaDataID>
        void ChangeSiftWork(IShiftWork activeShiftWork, DateTime startedAt, double timespanInHours);



        /// <MetaDataID>{60523b70-56ae-4d3a-962b-2c3911f4c9da}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        string ServicesContextIdentity { get; }


        /// <MetaDataID>{2e3ac379-09ee-4a82-be2f-721452356c5b}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+1")]
        string Description { get; set; }


        /// <MetaDataID>{4e027323-bb1f-4b08-bfbb-cf810db20c32}</MetaDataID>
        void RemoveGraphicMenu(OrganizationStorageRef graphicMenuStorageRef);


        /// <MetaDataID>{875524bb-8d42-4f6a-8286-98a1aaf524df}</MetaDataID>
        void GraphicMenuStorageMetaDataUpdated(OrganizationStorageRef graphicMenuStorageRef);

        /// <MetaDataID>{8e7c1368-77d0-408a-9854-b31b76758af9}</MetaDataID>
        void OperativeRestaurantMenuDataUpdated(OrganizationStorageRef restaurantMenusDataStorageRef);

        /// <MetaDataID>{9ee24cdb-c006-4969-8a2d-a3ba893eac62}</MetaDataID>
        ICashierStation NewCashierStation();
        /// <MetaDataID>{d3539860-1aa7-4c05-b84c-0ad0b2ff832d}</MetaDataID>
        void RemoveCashierStation(ICashierStation cashierStation);

        /// <MetaDataID>{df4c0199-434c-4963-8b6c-d2fffbf4c85d}</MetaDataID>
        IList<ICashierStation> CashierStations { get; }
        /// <MetaDataID>{b3d76316-188d-4b3f-bdbf-6f034c76bfac}</MetaDataID>
        IList<IPreparationStation> PreparationStations { get; }


        /// <MetaDataID>{477c4e32-2f53-4366-8cac-8d666f34f983}</MetaDataID>
        ServiceContextHumanResources ServiceContextHumanResources { get; }

        /// <MetaDataID>{8978b81a-71be-4d49-8965-e87acad21238}</MetaDataID>
        OrganizationStorageRef GetHallLayoutStorageForServiceArea(IServiceArea serviceArea);
        /// <MetaDataID>{58f2181b-5cbd-4a5a-9610-37da7f152f72}</MetaDataID>
        string NewSupervisor();
        /// <MetaDataID>{5ba7f84a-3a31-444b-a307-961b448e9bcc}</MetaDataID>
        IServiceContextSupervisor AssignSupervisorUser(string supervisorAssignKey, string signUpUserIdentity, string userName);
        /// <MetaDataID>{0ce43403-4320-4054-9d8d-80422791c490}</MetaDataID>
        IWaiter AssignWaiterUser(string waiterAssignKey, string signUpUserIdentity, string userName);

        /// <MetaDataID>{489998f0-6ec5-474c-a1fd-5ca4a5956ac8}</MetaDataID>
        IWaiter AssignWaiterNativeUser(string waiterAssignKey, string userName, string password, string userFullName);


        /// <MetaDataID>{5baba715-4433-44f0-9317-8031e719f00c}</MetaDataID>
        ITakeawayCashier AssignTakeAwayCashierNativeUser(string takeAwayCashierAssignKey, string userName, string password, string userFullName);

        


        /// <MetaDataID>{bb9b4c11-c932-495d-ab49-31f5ad2e4241}</MetaDataID>
        string RestaurantMenuDataSharedUri { get; }



        /// <MetaDataID>{18e6acdd-c608-4181-9a3b-64464d21dc8f}</MetaDataID>
        RoomService.IMealsController MealsController { get; }
        /// <MetaDataID>{d1367132-f0be-47db-8063-dd840f4756de}</MetaDataID>
        IList<IFisicalParty> FisicalParties { get; }
        /// <MetaDataID>{310f17fa-17ec-4821-8abe-abe6c2bd39b9}</MetaDataID>
        ISettings Settings { get; }

        /// <MetaDataID>{d723c156-151b-4d32-a096-49c208fe55ed}</MetaDataID>
        List<IPaymentTerminal> PaymentTerminals { get; }

        /// <MetaDataID>{25381c19-c35a-4d5a-b5cd-469f99929a73}</MetaDataID>
        IFisicalParty NewFisicalParty();
        /// <MetaDataID>{3357f691-ce3e-4822-911e-08a60a5433b7}</MetaDataID>
        void RemoveFisicalParty(IFisicalParty fisicalParty);
        /// <MetaDataID>{ee584ef4-bf14-429c-8b13-a27222d54532}</MetaDataID>
        void UpdateFisicalParty(IFisicalParty fisicalParty);
        /// <MetaDataID>{2eb4a008-d3a3-4fc9-ad3f-4c0e9031b71d}</MetaDataID>
        ICashiersStationRuntime GetCashiersStationRuntime(string communicationCredentialKey);


        /// <MetaDataID>{cdb8ac60-733a-4837-8102-b5afcea77679}</MetaDataID>
        IList<UserData> GetNativeUsers(RoleType roleType);



        /// <MetaDataID>{e6d7e9c7-e513-4791-adb7-e9fd2d34ea27}</MetaDataID>
        IFoodServiceClientSession GetMealInvitationInviter(string mealInvitationSessionID);
#if !FlavourBusinessDevice
        /// <MetaDataID>{6465189b-ee0a-4f51-b8ef-857de79ae040}</MetaDataID>
        OOAdvantech.Remoting.RestApi.HookRespnose WebHook(string method, string webHookName, Dictionary<string, string> headers, string content);
        /// <MetaDataID>{3775750f-2f13-458b-ac13-c682be2e15d2}</MetaDataID>
        ITakeAwayStation GetTakeAwayStation(string takeAwayStationCredentialKey);


#endif
    } 


}