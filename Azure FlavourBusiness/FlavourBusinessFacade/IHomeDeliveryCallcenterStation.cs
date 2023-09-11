using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HomeDelivery;

using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{39b7faeb-7f7c-4152-b423-858e6d611999}</MetaDataID>
    [GenerateFacadeProxy]
    [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("{39b7faeb-7f7c-4152-b423-858e6d611999}")]
    public interface IHomeDeliveryCallCenterStation
    {
        /// <MetaDataID>{0a59d844-6b8e-4e2f-99c9-54f71ccea47d}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        string GraphicMenuStorageIdentity { get; set; }
         
        /// <MetaDataID>{0525f79f-be19-4d04-9ac8-518d03fc14f6}</MetaDataID>
        [RoleAMultiplicityRange(0)]
        [RoleBMultiplicityRange(1, 1)]
        [Association("CallCenterHomeDeliveryServicePoint", Roles.RoleA, "ae8507f7-5b23-43b0-b0d7-1bb755411b42")]
        System.Collections.Generic.List<IHomeDeliveryServicePoint> HomeDeliveryServicePoints { get; }

        /// <MetaDataID>{b68922ea-7080-429a-a9b4-11c9c40fffc9}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        string Description { get; set; }

        /// <MetaDataID>{8bd9c155-7691-4454-82d2-99fb1194fc4d}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        string ServicesContextIdentity { get; set; }

        /// <MetaDataID>{cb48479c-e933-440d-a4d3-ed59e0cab777}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        string CallcenterStationIdentity { get; set; }


        /// <MetaDataID>{e229d169-2500-41cc-98c9-d1daa6e306ca}</MetaDataID>
        void AddHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint);
        void CancelHomeDeliverFoodServicesClientSession(IFoodServiceClientSession foodServicesClientSession);
        IFoodServiceClientSession GetFoodServicesClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken,HomeDeliveryServicePointAbbreviation homeDeliveryServicePoint);

        /// <MetaDataID>{608a2ffa-549d-4e62-aaba-fe09751f93a6}</MetaDataID>
        void RemoveHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint);


        OrganizationStorageRef Menu { get; }

        
        System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location);



        System.Collections.Generic.List<FoodServiceClientUri> FoodServiceClientsSearch(string phone);
        void CommitSession(IFoodServiceClientSession foodServicesClientSession, FoodServicesClientUpdateData foodServicesClientData, IPlace deliveryPlace);

        CallCenterStationWatchingOrders GetWatchingOrders(System.Collections.Generic.List<WatchingOrderAbbreviation> candidateToRemoveWatchingOrders = null);
        FoodServiceClientUri GetFoodServicesOpenSession(HomeDeliveryServicePointAbbreviation homeDeliveryServicePoint, string sessionID);

        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
    }

    /// <MetaDataID>{05a19c03-47b8-443e-886f-1976132586c2}</MetaDataID>
    public struct FoodServicesClientUpdateData
    {
        public string Identity { get; set; }
        public string Email { get; set; }

        public System.Collections.Generic.List<IPlace> DeliveryPlaces { get; set; }
        //public IPlace ExtraDeliveryPlace { get; internal set; }
        public string FriendlyName { get; set; }
        public string FullName { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string PhotoUrl { get; set; }
        public string SignInProvider { get; set; }
        public string UserName { get; set; }
        public string NotesForClient { get; set; }
    }


}