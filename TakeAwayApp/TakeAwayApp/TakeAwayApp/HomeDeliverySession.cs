using DontWaitApp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
#if DeviceDotNet

using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;

#else
using System.Windows.Media.TextFormatting;
#endif

namespace TakeAwayApp
{
    /// <MetaDataID>{4cc9fb7d-6b96-44f5-9d05-a5f74fd7034d}</MetaDataID>
    public class HomeDeliverySession : MarshalByRefObject, IHomeDeliverySession, OOAdvantech.Remoting.IExtMarshalByRefObject
    {



        public HomeDeliverySession(FlavoursServiceOrderTakingStation flavoursServiceOrderTakingStation, IFoodServicesClientSessionViewModel foodServicesClientSessionViewModel, string callerPhone = "")
        {
            FlavoursServiceOrderTakingStation = flavoursServiceOrderTakingStation;
            FoodServiceClientSession = foodServicesClientSessionViewModel;
            _CallerPhone = callerPhone;
            State = CallerCenterSessionState.OrderTaking;
        }

        public IFoodServicesClientSessionViewModel FoodServiceClientSession { get; }

        private FlavoursServiceOrderTakingStation FlavoursServiceOrderTakingStation;


        string SessionClientOrgDeliveryPlacesJson;

        /// <exclude>Excluded</exclude>
        string _CallerPhone;

        public string CallerPhone
        {
            get => _CallerPhone;
            set
            {
                if (_CallerPhone != value)
                {
                    _CallerPhone = value;
                    if (!string.IsNullOrWhiteSpace(_CallerPhone))
                    {

                        SearchResultClients = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.FoodServiceClientsSearch(_CallerPhone);

                        var homeDeliveryClient = SearchResultClients.FirstOrDefault();


                        if (homeDeliveryClient.FoodServiceClient == null)
                        {
                            homeDeliveryClient.FoodServiceClient = new FoodServiceClient(homeDeliveryClient.UniqueId);
                            homeDeliveryClient.FoodServiceClient.PhoneNumber = _CallerPhone;
                        }
                        else
                        {
                            var foodServiceClient = new FoodServiceClient(homeDeliveryClient.UniqueId);
                            foodServiceClient.Synchronize(homeDeliveryClient.FoodServiceClient);
                            homeDeliveryClient.FoodServiceClient = foodServiceClient;
                        }

                        SessionClient = new FoodServiceClientVM((FoodServiceClientSession as FoodServicesClientSessionViewModel).FlavoursOrderServer, homeDeliveryClient);
                        SessionClient.PhoneNumber = _CallerPhone;

                        SessionClientOrgDeliveryPlacesJson = OOAdvantech.Json.JsonConvert.SerializeObject(SessionClient.Places);
                        

                        if (FoodServiceClientSession.FoodServicesClientSession == null && SessionClient != null && _HomeDeliveryServicePoint != null)
                        {
                            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                            (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetFoodServicesClientSession(SessionClient.FullName, SessionClient.Identity, DeviceType.Desktop, device.FirebaseToken, _HomeDeliveryServicePoint);
                        }




                        if (SessionClient.FoodServiceClient != null)
                            SessionClient = new FoodServiceClientVM((FoodServiceClientSession as FoodServicesClientSessionViewModel).FlavoursOrderServer, homeDeliveryClient);

                        (FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser = SessionClient;

                        var openFoodServiceClientSession = homeDeliveryClient.OpenFoodServiceClientSessions.FirstOrDefault();
                        if (openFoodServiceClientSession != null)
                        {
                            (FoodServiceClientSession as FoodServicesClientSessionViewModel).DeliveryPlace = openFoodServiceClientSession.GetSessionDeliveryPlace();
                            (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = openFoodServiceClientSession;


                            string sessionServicePointIdentity = (FoodServiceClientSession as FoodServicesClientSessionViewModel).ServicePointIdentity;
                            string homeDeliveryServicePointIdentity = sessionServicePointIdentity.Split(';')[1];
                            var homeDeliveryServicePointAbbr = this.HomeDeliveryServicePoints.Where(x => x.ServicesPointIdentity == homeDeliveryServicePointIdentity).FirstOrDefault();
                            if (homeDeliveryServicePointAbbr != null)
                            {
                                _HomeDeliveryServicePoint = homeDeliveryServicePointAbbr;
                            }
                            else
                            {
                                var homeDeliveryServicePoint = openFoodServiceClientSession.ServicePoint;
                                _HomeDeliveryServicePoint = new HomeDeliveryServicePointAbbreviation()
                                {
                                    Description = homeDeliveryServicePoint.Description,
                                    Location = (homeDeliveryServicePoint as IHomeDeliveryServicePoint).PlaceOfDistribution.Location,
                                    OutOfDeliveryRange = false,
                                    ServicesContextIdentity = (homeDeliveryServicePoint as IHomeDeliveryServicePoint).ServicesContextIdentity,
                                    ServicesPointIdentity = (homeDeliveryServicePoint as IHomeDeliveryServicePoint).ServicesPointIdentity
                                };
                            }


                        }

                    }
                    else
                        SessionClient = null;
                }
            }
        }
        List<FoodServiceClienttUri> SearchResultClients = new List<FoodServiceClienttUri>();

        public FoodServiceClientVM SessionClient { get; set; }





        public System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        {
            return FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetNeighborhoodFoodServers(location);
        }

        public CallerCenterSessionState State
        {
            get;
            set;
        }
        public bool OrderCommit()
        {
            if (this.HomeDeliveryServicePoint == null)
                return false;

            var foodServicesClientSessionPresentation = FoodServiceClientSession as FoodServicesClientSessionViewModel;

            var foodServiceClient = (foodServicesClientSessionPresentation.EndUser as FoodServiceClientVM).FoodServiceClient;

            //SessionClientOrgDeliveryPlacesJson
            var deliveryPlacesJson = OOAdvantech.Json.JsonConvert.SerializeObject(foodServiceClient.DeliveryPlaces);

            if (SessionClientOrgDeliveryPlacesJson!= deliveryPlacesJson)
            {

            }
            IPlace extraDeliveryPlace = FoodServiceClientSession.DeliveryPlace;

            FoodServicesClientUpdateData foodServicesClientData = new FoodServicesClientUpdateData()
            {
                Identity = foodServiceClient.Identity,
                Email = foodServiceClient.Email,
                FriendlyName = foodServiceClient.FriendlyName,
                FullName = foodServiceClient.FullName,
                Name = foodServiceClient.Name,
                PhoneNumber = foodServiceClient.PhoneNumber,
                PhotoUrl = foodServiceClient.PhotoUrl,
                SignInProvider = foodServiceClient.SignInProvider,
                UserName = foodServiceClient.UserName,
                NotesForClient = foodServiceClient.NotesForClient
            };

            if (SessionClientOrgDeliveryPlacesJson != deliveryPlacesJson)
                foodServicesClientData.DeliveryPlaces = foodServiceClient.DeliveryPlaces;

            FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.CommitSession(this.FoodServiceClientSession.FoodServicesClientSession, foodServicesClientData, FoodServiceClientSession.DeliveryPlace);

            return true;
        }
        /// <exclude>Excluded</exclude>
        HomeDeliveryServicePointAbbreviation _HomeDeliveryServicePoint;
        public HomeDeliveryServicePointAbbreviation HomeDeliveryServicePoint
        {
            get => _HomeDeliveryServicePoint;


            set
            {
                _HomeDeliveryServicePoint = value;
                if (FoodServiceClientSession.FoodServicesClientSession == null && SessionClient != null)
                {
                    OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
                    (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetFoodServicesClientSession(SessionClient.FullName, SessionClient.Identity, DeviceType.Desktop, device.FirebaseToken, _HomeDeliveryServicePoint);

                }
            }
        }

        /// <exclude>Excluded</exclude>
        List<HomeDeliveryServicePointAbbreviation> _HomeDeliveryServicePoints;

        public List<HomeDeliveryServicePointAbbreviation> HomeDeliveryServicePoints
        {
            get => _HomeDeliveryServicePoints;
            internal set
            {
                _HomeDeliveryServicePoints = value;
                if (_HomeDeliveryServicePoints?.Count == 1)
                    this.HomeDeliveryServicePoint = _HomeDeliveryServicePoints[0];
            }
        }
    }
}