using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Defines the timespan in seconds to wait in AllMessmetesCommited state before move to meal monitoring state and starts meal preparation. 
        /// </summary>
        /// <MetaDataID>{968458ec-cde6-4550-aaf1-7a07b8c2df52}</MetaDataID>
        //// <MetaDataID>{365997d7-44db-4eb6-ac10-1a82b540c918}</MetaDataID>
        int AllMessmetesCommitedTimeSpan { get; set; }

        /// <MetaDataID>{477e1b7a-ca3c-4692-9916-8da26be81967}</MetaDataID>
        bool RemoveSupervisor(IServiceContextSupervisor supervisor);

        /// <MetaDataID>{84e2294d-fe41-42e9-b407-e442dc2d15b3}</MetaDataID>
        void MakeSupervisorActive(IServiceContextSupervisor supervisor);

        /// <MetaDataID>{83ec78d7-9509-4af4-a05e-bcda8713ae44}</MetaDataID>
        void RemoveWaiter(HumanResources.IWaiter waiter);

        /// <MetaDataID>{5e9707d0-6984-41c6-a3eb-785aa3a69450}</MetaDataID>
        string NewWaiter();

        /// <MetaDataID>{e22a146a-d86a-477e-a7f8-6fa9cc5a8a9b}</MetaDataID>
        IShiftWork NewShifWork(IServicesContextWorker worker, System.DateTime startedAt, double timespanInHours);

        /// <MetaDataID>{fde5e9ed-977c-483b-b6d1-bf6b1539d2cc}</MetaDataID>
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+3")]
        string OrganizationIdentity { get; set; }

        /// <MetaDataID>{55317a81-4a84-4735-b0a3-9aef9f0b4dab}</MetaDataID>
        List<ServicePointPreparationItems> GetPreparationStationItemsToPrepare(IPreparationStation preparationStation);

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


        /// <MetaDataID>{d11922ee-a931-4230-a131-023440ea52f7}</MetaDataID>
        IPreparationStationRuntime GetPreparationStation(string preparationStationIdentity);


        /// <MetaDataID>{4d1c49d1-745a-4b4f-a934-0476332e894e}</MetaDataID>
        ServiceContextResources ServiceContextResources { get; }

        [RemoteEventPublish(InvokeType.Async)]
        event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{e63eeb30-6665-4aaa-9c93-9eb6797c7b44}</MetaDataID>
        ClientSessionData GetClientSession(string servicePointIdentity, string mealInvitationSessionID, string clientName, string clientDeviceID, string deviceFirebaseToken, string clientIdentity, IUser user, string organizationIdentity, List<OrganizationStorageRef> unSafeGraphicMenus, bool create);



        /// <MetaDataID>{553a6bb9-bbb1-483b-9e4e-d00945788d1f}</MetaDataID>
        IHallLayout GetHallLayout(string servicePointIdentity);

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



        string RestaurantMenuDataSharedUri { get; }


    }
}