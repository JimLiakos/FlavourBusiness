using DontWaitApp;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using System;
using System.Collections.Generic;
using System.Linq;

using Xamarin.Forms;
using OOAdvantech.Transactions;
using OOAdvantech.Json;
using System.Threading.Tasks;


#if DeviceDotNet

using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;

#else
using FlavourBusinessManager.ServicesContextResources;
using System.Windows.Media.TextFormatting;
#endif

namespace TakeAwayApp.ViewModel
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

        public static HomeDeliverySession GetHomeDeliverySession(FlavoursServiceOrderTakingStation flavoursServiceOrderTakingStation, HomeDeliveryServicePointAbbreviation homeDeliveryServicePoint, List<HomeDeliveryServicePointAbbreviation> homeDeliveryServicePoints, IFoodServicesClientSessionViewModel foodServicesClientSessionViewModel, string sessionID)
        {
            DateTime dateTime = DateTime.Now;
            try
            {
                FoodServiceClientUri homeDeliveryClient = flavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetFoodServicesOpenSession(homeDeliveryServicePoint, sessionID);
                if (homeDeliveryClient == null)
                    return null;
                else
                    return new HomeDeliverySession(flavoursServiceOrderTakingStation, homeDeliveryServicePoint, homeDeliveryServicePoints, homeDeliveryClient, foodServicesClientSessionViewModel);
            }
            finally
            {
                var mms = (DateTime.Now - dateTime).TotalMilliseconds;
            }



        }
        HomeDeliverySession(FlavoursServiceOrderTakingStation flavoursServiceOrderTakingStation, HomeDeliveryServicePointAbbreviation homeDeliveryServicePoint, List<HomeDeliveryServicePointAbbreviation> homeDeliveryServicePoints, FoodServiceClientUri homeDeliveryClient, IFoodServicesClientSessionViewModel foodServicesClientSessionViewModel) : this(flavoursServiceOrderTakingStation, foodServicesClientSessionViewModel)
        {
            _CallerPhone = homeDeliveryClient.FoodServiceClient?.PhoneNumber;
            _HomeDeliveryServicePoint = homeDeliveryServicePoint;
            _HomeDeliveryServicePoints = homeDeliveryServicePoints;
            LoadSessionDataForClient(homeDeliveryClient);

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

                        LoadSessionDataForClient(homeDeliveryClient);

                        if (OrgHomeDeliverySessionState == null || !UncommittedChanges)
                            OrgHomeDeliverySessionState = HomeDeliverySessionState;

                    }
                    else
                        SessionClient = null;


                }
            }
        }

        public string SimulationDeviceID;

        private void LoadSessionDataForClient(FoodServiceClientUri homeDeliveryClient)
        {
            if (homeDeliveryClient.FoodServiceClient == null)
            {
                //There is not server side user.

                homeDeliveryClient.FoodServiceClient = new FoodServiceClient(homeDeliveryClient.UniqueId);
                homeDeliveryClient.FoodServiceClient.PhoneNumber = _CallerPhone;
            }
            else
            {
                //There is server side user.
                var foodServiceClient = new FoodServiceClient(homeDeliveryClient.UniqueId);
                foodServiceClient.Synchronize(homeDeliveryClient.FoodServiceClient);
                homeDeliveryClient.FoodServiceClient = foodServiceClient;
            }

            SessionClient = new FoodServiceClientVM((FoodServiceClientSession as FoodServicesClientSessionViewModel).FlavoursOrderServer, homeDeliveryClient);
            SessionClient.PhoneNumber = _CallerPhone;

            SessionClientOrgDeliveryPlacesJson = JsonConvert.SerializeObject(SessionClient.Places);


            if (FoodServiceClientSession.FoodServicesClientSession == null && SessionClient != null && _HomeDeliveryServicePoint != null)
            {
                if (homeDeliveryClient.OpenFoodServiceClientSessions?.Count == 1)
                {
                    (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = homeDeliveryClient.OpenFoodServiceClientSessions.FirstOrDefault();
                    if (FoodServiceClientSession.FoodServicesClientSession != null)
                    {
                        _DeliveryPlace = FoodServiceClientSession.DeliveryPlace as Place;
                        _HomeDeliveryServicePoint = GetHomeDeliveryServicePoint((FoodServiceClientSession as FoodServicesClientSessionViewModel));
                    }

                }
                else
                {
                    OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;

                    (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetFoodServicesClientSession(SessionClient.FullName, SessionClient.Identity, DeviceType.Desktop, device.FirebaseToken, _HomeDeliveryServicePoint);

                    if (FoodServiceClientSession.FoodServicesClientSession != null)
                        _DeliveryPlace = FoodServiceClientSession.FoodServicesClientSession.GetSessionDeliveryPlace() as Place;
                }
                (FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser = SessionClient;
            }
            else
            {
                (FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser = SessionClient;

                var openFoodServiceClientSession = homeDeliveryClient.OpenFoodServiceClientSessions?.FirstOrDefault();
                if (openFoodServiceClientSession != null)
                {
                    (FoodServiceClientSession as FoodServicesClientSessionViewModel).DeliveryPlace = openFoodServiceClientSession.GetSessionDeliveryPlace();
                    if ((FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession != openFoodServiceClientSession)
                    {

                    }
                    (FoodServiceClientSession as FoodServicesClientSessionViewModel).FoodServicesClientSession = openFoodServiceClientSession;
                    _DeliveryPlace = (FoodServiceClientSession as FoodServicesClientSessionViewModel).DeliveryPlace as Place;

                    string sessionServicePointIdentity = (FoodServiceClientSession as FoodServicesClientSessionViewModel).ServicePointIdentity;

                    _HomeDeliveryServicePoint = GetHomeDeliveryServicePoint((FoodServiceClientSession as FoodServicesClientSessionViewModel));

                }
            }
        }

        private HomeDeliveryServicePointAbbreviation GetHomeDeliveryServicePoint(FoodServicesClientSessionViewModel foodServicesClientSession)
        {
            string sessionServicePointIdentity = foodServicesClientSession.ServicePointIdentity;
            string homeDeliveryServicePointIdentity = sessionServicePointIdentity.Split(';')[1];
            var homeDeliveryServicePointAbbr = HomeDeliveryServicePoints.Where(x => x.ServicesPointIdentity == homeDeliveryServicePointIdentity).FirstOrDefault();
            if (homeDeliveryServicePointAbbr != null)
            {
                return homeDeliveryServicePointAbbr;
            }
            else
            {
                var homeDeliveryServicePoint = foodServicesClientSession.FoodServicesClientSession.ServicePoint;
                return new HomeDeliveryServicePointAbbreviation()
                {
                    Description = homeDeliveryServicePoint.Description,
                    Location = (homeDeliveryServicePoint as IHomeDeliveryServicePoint).PlaceOfDistribution.Location,
                    OutOfDeliveryRange = false,
                    ServicesContextIdentity = (homeDeliveryServicePoint as IHomeDeliveryServicePoint).ServicesContextIdentity,
                    ServicesPointIdentity = (homeDeliveryServicePoint as IHomeDeliveryServicePoint).ServicesPointIdentity
                };
            }
        }

        List<FoodServiceClientUri> SearchResultClients = new List<FoodServiceClientUri>();

        public FoodServiceClientVM SessionClient { get; set; }



        string HomeDeliverySessionState
        {
            get
            {
                var state = (from homeDeliverySession in new List<HomeDeliverySession>() { this }
                             select new HomeDeliverySessionState()
                             {
                                 Client = new ClientState()
                                 {
                                     Identity = homeDeliverySession.SessionClient.Identity,
                                     FullName = homeDeliverySession.SessionClient?.FullName,
                                     Places = homeDeliverySession.SessionClient.Places.OfType<Place>().ToList(),
                                     EmailAddress = homeDeliverySession.SessionClient.EmailAddress,
                                     PhoneNumber = homeDeliverySession.CallerPhone
                                 },
                                 DeliveryPlace = homeDeliverySession.DeliveryPlace as Place,
                                 OrderItems = homeDeliverySession.FoodServiceClientSession.OrderItems

                             }).FirstOrDefault();


                return JsonConvert.SerializeObject(state, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });

            }
        }



        public List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        {
            return FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.GetNeighborhoodFoodServers(location);
        }

        public CallerCenterSessionState State
        {
            get;
            set;
        }
        public Task<bool> OrderCommit()
        {
            if (HomeDeliveryServicePoint == null)
                return Task<bool>.FromResult(false);

            FlavoursOrderServer.SerializeTaskScheduler.Wait();

            return Task<bool>.Run(() =>
              {
                  var foodServicesClientSessionPresentation = FoodServiceClientSession as FoodServicesClientSessionViewModel;

                  var foodServiceClient = (foodServicesClientSessionPresentation.EndUser as FoodServiceClientVM).FoodServiceClient;

                  //SessionClientOrgDeliveryPlacesJson
                  var deliveryPlacesJson = JsonConvert.SerializeObject(foodServiceClient.DeliveryPlaces);

                  if (SessionClientOrgDeliveryPlacesJson != deliveryPlacesJson)
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

                  FlavoursOrderServer.SerializeTaskScheduler.Wait();
                  var watchingOrder = FlavoursServiceOrderTakingStation.HomeDeliveryCallCenterStation.CommitSession(FoodServiceClientSession.FoodServicesClientSession, foodServicesClientData, DeliveryPlace);
                  if (watchingOrder != null)
                      FlavoursServiceOrderTakingStation.UpdateWatchingOrder(watchingOrder);


                  OrgHomeDeliverySessionState = HomeDeliverySessionState;
                  return true;
              });
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
                    HomeDeliveryServicePoint = _HomeDeliveryServicePoints[0];
            }
        }

        /// <exclude>Excluded</exclude>
        Place _DeliveryPlace;
        private string OrgHomeDeliverySessionState;

        public IPlace DeliveryPlace
        {
            get => _DeliveryPlace;
            set
            {

                if (_DeliveryPlace != value)
                {


                    if (value != null)// && (FoodServiceClientSession as FoodServicesClientSessionViewModel)?.CanChangeDeliveryPlace(value) == ChangeDeliveryPlaceResponse.OK)
                    {

                        _DeliveryPlace = value as Place;
                        if (_DeliveryPlace != null && _DeliveryPlace.PlaceID == null)
                        {
                            var ticks = new DateTime(2022, 1, 1).Ticks;
                            var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
                            _DeliveryPlace.PlaceID = uniqueId;
                        }


                        Place existingPlace = ((FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser as IGeocodingPlaces).Places.Where(x => x.PlaceID == value.PlaceID).FirstOrDefault() as Place;
                        if (existingPlace != null)
                        {
                            existingPlace.Update(value);
                            ((FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser as IGeocodingPlaces).SavePlace(existingPlace);
                        }
                        else
                            ((FoodServiceClientSession as FoodServicesClientSessionViewModel).EndUser as IGeocodingPlaces).SavePlace(value);


                    }
                }
            }
        }

        public bool UncommittedChanges
        {
            get
            {
                if (string.IsNullOrWhiteSpace(OrgHomeDeliverySessionState))
                    return false;
                var foodServicesClientSessionPresentation = FoodServiceClientSession as FoodServicesClientSessionViewModel;


                if (foodServicesClientSessionPresentation.OrderItems.Any(x => x.IsInPreviousState(FlavourBusinessFacade.RoomService.ItemPreparationState.Committed)))
                    return true;

                if (OrgHomeDeliverySessionState != HomeDeliverySessionState && foodServicesClientSessionPresentation.OrderItems.Count > 0)
                {

                    //var orgHomeDeliverySessionState = JsonConvert.DeserializeObject<HomeDeliverySessionState>(OrgHomeDeliverySessionState, new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.All });
                    //if (orgHomeDeliverySessionState.OrderItems.Count == 0)
                    //    return false;
                    return true;
                }



                return false;

            }

        }

    }


    /// <MetaDataID>{943b6775-9e29-4571-b047-72ba55782504}</MetaDataID>
    class HomeDeliverySessionState
    {
        public ClientState Client { get; set; }
        public Place DeliveryPlace { get; set; }
        public List<FlavourBusinessManager.RoomService.ItemPreparation> OrderItems { get; set; }

    }
    /// <MetaDataID>{629d2916-5528-4409-9ed1-1fab25d91e56}</MetaDataID>
    class ClientState
    {
        public string FullName { get; set; }
        public List<Place> Places { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Identity { get; internal set; }
    }
}