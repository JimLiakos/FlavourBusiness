using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HomeDelivery;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.ServicePointRunTime;
using MenuPresentationModel;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceProcess;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{eacd7ffb-4107-4ce3-ae6b-154b56792f54}</MetaDataID>
    [BackwardCompatibilityID("{eacd7ffb-4107-4ce3-ae6b-154b56792f54}")]
    [Persistent()]
    public class HomeDeliveryCallCenterStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IHomeDeliveryCallCenterStation
    {

        public HomeDeliveryCallCenterStation(ServicesContextRunTime servicesContextRunTime)

        {
            _CallcenterStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }
        protected HomeDeliveryCallCenterStation()
        {

        }

        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        List<IHomeDeliveryServicePoint> HomeDeliveryServicePointsForTest;
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IHomeDeliveryServicePoint> _HomeDeliveryServicePoints = new OOAdvantech.Collections.Generic.Set<IHomeDeliveryServicePoint>();

        /// <MetaDataID>{e3266bfc-45a7-4348-8a96-b44092eb9ae7}</MetaDataID>
        [PersistentMember(nameof(_HomeDeliveryServicePoints))]
        [BackwardCompatibilityID("+1")]
        public List<IHomeDeliveryServicePoint> HomeDeliveryServicePoints
        {
            get
            {
                if (HomeDeliveryServicePointsForTest == null)
                {


                    var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);// OpenServicesContextStorageStorage();
                    OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);
                    var servicesContextIdentity = ServicesContextIdentity;
                    var deliveryServicePoint = (from theHomeDeliveryServicePoint in servicesContextStorage.GetObjectCollection<HomeDeliveryServicePoint>()
                                                where theHomeDeliveryServicePoint.ServicesContextIdentity == servicesContextIdentity
                                                select theHomeDeliveryServicePoint).ToList().Where(x => x.ServicesPointIdentity.EndsWith("_test")).FirstOrDefault();



                    HomeDeliveryServicePointsForTest = _HomeDeliveryServicePoints.ToThreadSafeList();
                    if (deliveryServicePoint == null)
                    {
                        deliveryServicePoint = new HomeDeliveryServicePoint();
                        deliveryServicePoint.ServicesContextIdentity = ServicesContextIdentity;
                        deliveryServicePoint.ServicesPointIdentity = _HomeDeliveryServicePoints.First().ServicesPointIdentity + "_test";
                        deliveryServicePoint.Description = "Starbucks Αμπελοκηποι";
                        //homeDeliveryServicePoint.

                        string placeOfDistributionJson = "{\"ExtensionProperties\":{},\"CityTown\":\"Αθήνα\",\"StateProvinceRegion\":null,\"Description\":\"Λάκωνος 26, Αθήνα, Ελλάδα \",\"StreetNumber\":\"26\",\"Street\":\"Λάκωνος\",\"Area\":null,\"PostalCode\":\"115 24\",\"Country\":\"Ελλάδα\",\"Location\":{\"Longitude\":23.765026,\"Latitude\":37.9960904},\"PlaceID\":\"ChIJq1RxdAiYoRQRvVR_EYQH2gw\",\"Default\":true}";
                        var placeOfDistribution = OOAdvantech.Json.JsonConvert.DeserializeObject<EndUsers.Place>(placeOfDistributionJson);
                        deliveryServicePoint.PlaceOfDistribution = placeOfDistribution;

                        string serviceAreaMapJson = "[{\"Longitude\":23.756942191409244,\"Latitude\":37.997785513901349},{\"Longitude\":23.754968085574284,\"Latitude\":37.997075316210584},{\"Longitude\":23.753895201968326,\"Latitude\":37.996314382481863},{\"Longitude\":23.753444590853825,\"Latitude\":37.9951306921011},{\"Longitude\":23.753873744296207,\"Latitude\":37.9936087763995},{\"Longitude\":23.752500453280582,\"Latitude\":37.990564850264242},{\"Longitude\":23.751749434756412,\"Latitude\":37.988247999610365},{\"Longitude\":23.75780049829401,\"Latitude\":37.986962707840867},{\"Longitude\":23.76495109988965,\"Latitude\":37.985793796814477},{\"Longitude\":23.766410221593752,\"Latitude\":37.987079109060993},{\"Longitude\":23.767182697790041,\"Latitude\":37.987890873615555},{\"Longitude\":23.767526020543947,\"Latitude\":37.9884658680742},{\"Longitude\":23.771388401525392,\"Latitude\":37.990867266805964},{\"Longitude\":23.775637020604982,\"Latitude\":37.993031840552639},{\"Longitude\":23.769113888280764,\"Latitude\":37.998206265799595},{\"Longitude\":23.765594830053224,\"Latitude\":38.001114606002155},{\"Longitude\":23.76396404697217,\"Latitude\":38.001114606002155},{\"Longitude\":23.759887089269533,\"Latitude\":38.000810250221747},{\"Longitude\":23.758814205663576,\"Latitude\":37.999897175301392},{\"Longitude\":23.758556713598146,\"Latitude\":37.998950270783524},{\"Longitude\":23.757784237401857,\"Latitude\":37.998104809989734}]";
                        var serviceAreaMap = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Coordinate>>(serviceAreaMapJson);
                        deliveryServicePoint.ServiceAreaMap = serviceAreaMap;
                        objectStorage.CommitTransientObjectState(deliveryServicePoint);
                    }

                    deliveryServicePoint.ObjectChangeState+=DeliveryServicePoint_ObjectChangeState;
                    HomeDeliveryServicePointsForTest.Add(deliveryServicePoint);

                    var sc_deliveryServicePoint = ServicesContextRunTime.Current.DeliveryServicePoint;
                }

                //foreach (var hdsp in HomeDeliveryServicePointsForTest)
                //{
                //    if (hdsp.ServesMealTypes.Count == 0)
                //    {
                //        hdsp.AddMealType(ServicePointRunTime.ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri);

                //    }
                //}
                return HomeDeliveryServicePointsForTest.ToList();
                return _HomeDeliveryServicePoints.ToThreadSafeList();
            }
        }

        private void DeliveryServicePoint_ObjectChangeState(object _object, string member)
        {
            if(member==nameof(HomeDeliveryServicePoint.WatchingOrders))
                ObjectChangeState.Invoke(this, member);
        }




        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{5c6d7b59-3a2b-40b6-90f0-392978df84ee}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get => _Description; set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;


        /// <MetaDataID>{ce07ba48-ffb9-4383-b9cb-e61fc01386b1}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+3")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity; set
            {
                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        object ObjLock = new object();
        ///// <exclude>Excluded</exclude>
        //List<EndUsers.FoodServiceClientSession> _OpenClientSessions;

        public List<EndUsers.FoodServiceClientSession> OpenClientSessions
        {
            get
            {
                return ServicesContextRunTime.Current.OpenClientSessions;
            }
        }


        public List<FoodServiceClientUri> FoodServiceClientsSearch(string phone)
        {
            var organizationObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(ServicesContextRunTime.Current.OrganizationStorageIdentity);

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(organizationObjectStorage);
            var foodServiceClients = (from foodServiceClient in storage.GetObjectCollection<IFoodServiceClient>()
                                      where foodServiceClient.PhoneNumber == phone
                                      select foodServiceClient).ToList().Select(x => new FoodServiceClientUri()
                                      {
                                          UniqueId = x.Identity,
                                          FoodServiceClient = x,
                                          OpenFoodServiceClientSessions = OpenClientSessions.Where(clientSession => clientSession.UserIdentity == x.Identity).OfType<IFoodServiceClientSession>().ToList()

                                      }).ToList();




            if (foodServiceClients.Count == 0)
            {
                var ticks = new DateTime(2022, 1, 1).Ticks;
                var uniqueId = "org_client_" + (DateTime.Now.Ticks - ticks).ToString("x");
                foodServiceClients.Add(new FoodServiceClientUri() { UniqueId = uniqueId });
            }

            return foodServiceClients;
        }
        public FoodServiceClientUri GetFoodServicesOpenSession(HomeDeliveryServicePointAbbreviation homeDeliveryServicePoint, string sessionID)
        {
            string servicePointIdentity = homeDeliveryServicePoint.ServicesContextIdentity + ";" + homeDeliveryServicePoint.ServicesPointIdentity;
            if (homeDeliveryServicePoint.ServicesContextIdentity == ServicesContextIdentity)
            {
                var foodServicesSesion = ServicesContextRunTime.Current.OpenSessions.Where(x => x.SessionID == sessionID).FirstOrDefault();
                if (foodServicesSesion == null)
                    return null;

                IFoodServiceClient foodServiceClient= foodServicesSesion.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery).FirstOrDefault()?.Client;
                if (foodServiceClient == null)
                    return null;


                var foodServiceClientUri = new FoodServiceClientUri()
                {
                    UniqueId = foodServiceClient.Identity,
                    FoodServiceClient = foodServiceClient,
                    OpenFoodServiceClientSessions = foodServicesSesion.PartialClientSessions.ToList()

                };
                return foodServiceClientUri;
            }
            return null;

        }

        public List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        {
            List<HomeDeliveryServicePointAbbreviation> deliveryServicePoints = new List<HomeDeliveryServicePointAbbreviation>();

            foreach (var deliveryServicePoint in HomeDeliveryServicePoints)
            {
                var placeOfDistribution = deliveryServicePoint.PlaceOfDistribution;
                if (placeOfDistribution != null)
                {
                    double distance = Coordinate.CalDistance(location.Latitude, location.Longitude, placeOfDistribution.Location.Latitude, placeOfDistribution.Location.Longitude);

                    var polyGon = new MapPolyGon(deliveryServicePoint.ServiceAreaMap);

                    if (polyGon.FindPoint(location.Latitude, location.Longitude))
                        deliveryServicePoints.Add(new HomeDeliveryServicePointAbbreviation() { Description = deliveryServicePoint.Description, DistanceInKm = distance, Location = placeOfDistribution.Location, ServicesContextIdentity = deliveryServicePoint.ServicesContextIdentity, ServicesPointIdentity = deliveryServicePoint.ServicesPointIdentity, OutOfDeliveryRange = false });
                    else
                        deliveryServicePoints.Add(new HomeDeliveryServicePointAbbreviation() { Description = deliveryServicePoint.Description, DistanceInKm = distance, Location = placeOfDistribution.Location, ServicesContextIdentity = deliveryServicePoint.ServicesContextIdentity, ServicesPointIdentity = deliveryServicePoint.ServicesPointIdentity, OutOfDeliveryRange = true });
                }
            }
            return deliveryServicePoints.OrderBy(x => x.OutOfDeliveryRange).ThenBy(x => x.DistanceInKm).ToList();

        }

        /// <exclude>Excluded</exclude>
        string _CallcenterStationIdentity;
        /// <MetaDataID>{519ff5fa-ec0c-40b7-9131-6c9d156bf632}</MetaDataID>
        [PersistentMember(nameof(_CallcenterStationIdentity))]
        [BackwardCompatibilityID("+4")]
        public string CallcenterStationIdentity
        {
            get => _CallcenterStationIdentity; set
            {
                if (_CallcenterStationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CallcenterStationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _GraphicMenuStorageIdentity;

        /// <MetaDataID>{4e8369d3-e6a4-45ac-8a03-e79a3c051eff}</MetaDataID>
        [PersistentMember(nameof(_GraphicMenuStorageIdentity))]
        [BackwardCompatibilityID("+6")]
        public string GraphicMenuStorageIdentity
        {
            get => _GraphicMenuStorageIdentity; set
            {
                if (_GraphicMenuStorageIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _GraphicMenuStorageIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{ff543df5-ff7a-4610-99df-47046b84bce3}</MetaDataID>
        public void AddHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HomeDeliveryServicePoints.Add(homeDeliveryServicePoint);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{06f8a5e7-45a5-4ab8-97fd-3e75409531fa}</MetaDataID>
        public void RemoveHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HomeDeliveryServicePoints.Remove(homeDeliveryServicePoint);
                stateTransition.Consistent = true;
            }

        }

        public void CancelHomeDeliverFoodServicesClientSession(IFoodServiceClientSession foodServicesClientSession)
        {
            throw new NotImplementedException();
        }

        public IFoodServiceClientSession GetFoodServicesClientSession(string clientName, string clientDeviceID, DeviceType deviceType, string deviceFirebaseToken, HomeDeliveryServicePointAbbreviation homeDeliveryServicePoint)
        {

            string servicePointIdentity = homeDeliveryServicePoint.ServicesContextIdentity + ";" + homeDeliveryServicePoint.ServicesPointIdentity;
            if (homeDeliveryServicePoint.ServicesContextIdentity == ServicesContextIdentity)
            {
                var servicePoint = HomeDeliveryServicePoints.Where(x => x.ServicesContextIdentity == homeDeliveryServicePoint.ServicesContextIdentity && x.ServicesPointIdentity == homeDeliveryServicePoint.ServicesPointIdentity).FirstOrDefault();

                return servicePoint.GetFoodServiceClientSession(clientName, null, clientDeviceID, deviceType, deviceFirebaseToken, true, true);
                // servicePoint.GetFoodServiceClientSession()
            }





            var sds = homeDeliveryServicePoint.ServicesContextIdentity + ";";

            return null;


        }

        public void CommitSession(IFoodServiceClientSession foodServicesClientSession, FoodServicesClientUpdateData foodServicesClientData, IPlace deliveryPlace)
        {

            var organizationObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(ServicesContextRunTime.Current.OrganizationStorageIdentity);
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(organizationObjectStorage);

            var foodServicesClient = (from foodServiceClient in storage.GetObjectCollection<FoodServiceClient>()
                                      where foodServiceClient.Identity == foodServicesClientData.Identity
                                      select foodServiceClient).ToList().FirstOrDefault();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foodServicesClientSession.SetSessionDeliveryPlace(deliveryPlace);
                if (foodServicesClient == null)
                {
                    foodServicesClient = new FoodServiceClient(foodServicesClientData.Identity)
                    {
                        Name = foodServicesClientData.Name,
                        UserName = foodServicesClientData.UserName,
                        Email = foodServicesClientData.Email,
                        FriendlyName = foodServicesClientData.FriendlyName,
                        FullName = foodServicesClientData.FullName,
                        NotesForClient = foodServicesClientData.NotesForClient,
                        PhoneNumber = foodServicesClientData.PhoneNumber,
                        PhotoUrl = foodServicesClientData.PhotoUrl
                    };
                    foodServicesClient.AddDeliveryPlace(deliveryPlace);
                    organizationObjectStorage.CommitTransientObjectState(foodServicesClient);
                }
                else
                {


                    using (ObjectStateTransition objStateTransition = new ObjectStateTransition(foodServicesClient))
                    {
                        foodServicesClient.Name = foodServicesClientData.Name;
                        foodServicesClient.UserName = foodServicesClientData.UserName;
                        foodServicesClient.Email = foodServicesClientData.Email;
                        foodServicesClient.FriendlyName = foodServicesClientData.FriendlyName;
                        foodServicesClient.FullName = foodServicesClientData.FullName;
                        foodServicesClient.NotesForClient = foodServicesClientData.NotesForClient;
                        foodServicesClient.PhoneNumber = foodServicesClientData.PhoneNumber;
                        foodServicesClient.PhotoUrl = foodServicesClientData.PhotoUrl;
                        foodServicesClient.AddDeliveryPlace(deliveryPlace);
                        objStateTransition.Consistent = true;
                    }

                }
                if (foodServicesClientData.DeliveryPlaces != null)
                {
                    foreach (var place in foodServicesClient.DeliveryPlaces)
                    {
                        if (foodServicesClientData.DeliveryPlaces.Where(x => x.PlaceID == place.PlaceID).FirstOrDefault() == null)
                            foodServicesClient.RemoveDeliveryPlace(place);
                    }
                    foreach (var place in foodServicesClientData.DeliveryPlaces)
                    {
                        foodServicesClient.AddDeliveryPlace(place);
                        if (place.Default)
                            foodServicesClient.SetDefaultDelivaryPlace(place);

                    }

                }
                foodServicesClientSession.SetSessionDeliveryPlace(deliveryPlace);
                var unCommitedItems = foodServicesClientSession.FlavourItems.Where(x => x.State.IsInPreviousState(ItemPreparationState.Committed)).ToList();
                foodServicesClientSession.Commit(unCommitedItems);
                (foodServicesClientSession as FoodServiceClientSession).UserIdentity = foodServicesClient.Identity;
                stateTransition.Consistent = true;
            }

 
               
        }

        public CallCenterStationWatchingOrders GetWatchingOrders(List<WatchingOrderAbbreviation> stationWatchingOrders = null)
        {
            if (stationWatchingOrders == null)
                stationWatchingOrders = new List<WatchingOrderAbbreviation>();

            CallCenterStationWatchingOrders callCenterStationWatchingOrders = new CallCenterStationWatchingOrders();
            List<WatchingOrder> watchingOrders = new List<WatchingOrder>();
            foreach (var HomeDeliveryServicePoint in HomeDeliveryServicePoints)
            {
                var servicePoint_CallCenterStationWatchingOrders = HomeDeliveryServicePoint.GetWatchingOrders(stationWatchingOrders);
                callCenterStationWatchingOrders.WatchingOrders.AddRange(servicePoint_CallCenterStationWatchingOrders.WatchingOrders);
                callCenterStationWatchingOrders.RemovedWatchingOrders.AddRange(servicePoint_CallCenterStationWatchingOrders.RemovedWatchingOrders);
            }
            return callCenterStationWatchingOrders;
        }

        DeviceType ClientDeviceType = DeviceType.Desktop;
        /// <exclude>Excluded</exclude>
        OrganizationStorageRef _Menu;

        public OrganizationStorageRef Menu
        {
            get
            {
                if (_Menu == null)
                {
                    OrganizationStorageRef graphicMenu = null;
                    if (ServicesContextRunTime.Current.GraphicMenus.Count == 1)
                        graphicMenu = ServicesContextRunTime.Current.GraphicMenus.FirstOrDefault();
                    else
                    {
                        //var Portrait = null;
                        //var Landscape = null;

                        if (ClientDeviceType == DeviceType.Phone)
                            graphicMenu = ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsPortrait(x)).FirstOrDefault();

                        if (ClientDeviceType == DeviceType.Desktop)
                            graphicMenu = ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (ClientDeviceType == DeviceType.Tablet)
                            graphicMenu = ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (ClientDeviceType == DeviceType.TV)
                            graphicMenu = ServicesContextRunTime.Current.GraphicMenus.Where(x => RestaurantMenu.IsLandscape(x)).FirstOrDefault();

                        if (graphicMenu == null)
                            graphicMenu = ServicesContextRunTime.Current.GraphicMenus.FirstOrDefault();


                    }


                    string versionSuffix = "";
                    if (!string.IsNullOrWhiteSpace(graphicMenu.Version))
                        versionSuffix = "/" + graphicMenu.Version;
                    else
                        versionSuffix = "";
                    graphicMenu.StorageUrl = RawStorageCloudBlob.RootUri + string.Format("/usersfolder/{0}/Menus/{1}{3}/{2}.json", ServicesContextRunTime.Current.OrganizationIdentity, graphicMenu.StorageIdentity, graphicMenu.Name, versionSuffix);
                    _Menu = graphicMenu;
                }
                return _Menu;
            }
        }




        //public System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> GetNeighborhoodFoodServers(Coordinate location)
        //{
        //    System.Collections.Generic.List<HomeDeliveryServicePointAbbreviation> deliveryServicePoints = new OOAdvantech.Collections.Generic.List<HomeDeliveryServicePointAbbreviation>();
        //    foreach (var deliveryServicePoint in this.HomeDeliveryServicePoints)
        //    {
        //        if (deliveryServicePoint != null)
        //        {
        //            var placeOfDistribution = deliveryServicePoint.PlaceOfDistribution;
        //            if (placeOfDistribution!=null)
        //            {
        //                double distance = Coordinate.CalDistance(location.Latitude, location.Longitude, placeOfDistribution.Location.Latitude, placeOfDistribution.Location.Longitude);

        //                var polyGon = new MapPolyGon(deliveryServicePoint.ServiceAreaMap);

        //                if (polyGon.FindPoint(location.Latitude, location.Longitude))
        //                    deliveryServicePoints.Add(new HomeDeliveryServicePointAbbreviation() { Description=deliveryServicePoint.Description,Distance=distance,ServicesContextIdentity=deliveryServicePoint.ServicesContextIdentity,ServicesPointIdentity=deliveryServicePoint.ServicesPointIdentity });
        //            }
        //        }
        //    }
        //    return deliveryServicePoints;
        //}

        /*
         lazaradon 

            ServiceAreaMapJson: [{"Longitude":23.749154882460203,"Latitude":37.995717013223384},{"Longitude":23.746118621855345,"Latitude":37.996469498304741},{"Longitude":23.743919210463133,"Latitude":37.996055209720588},{"Longitude":23.742213325529661,"Latitude":37.996097484173156},{"Longitude":23.736054973631468,"Latitude":37.994913790291953},{"Longitude":23.732074575453368,"Latitude":37.995556369340605},{"Longitude":23.7332332897478,"Latitude":38.000138809324874},{"Longitude":23.734713869124022,"Latitude":38.00538881756335},{"Longitude":23.734955267935362,"Latitude":38.006652652297639},{"Longitude":23.740719335108366,"Latitude":38.006483578679067},{"Longitude":23.741502540140715,"Latitude":38.005760784563485},{"Longitude":23.742610292463866,"Latitude":38.005756557676357},{"Longitude":23.743995032319887,"Latitude":38.006542372746324},{"Longitude":23.744778237352236,"Latitude":38.007391963055511},{"Longitude":23.744831881532534,"Latitude":38.008140101593817},{"Longitude":23.746637986944283,"Latitude":38.00805164316818},{"Longitude":23.747614311025703,"Latitude":38.007451441510376},{"Longitude":23.747743057058418,"Latitude":38.006656800770529},{"Longitude":23.7491807210904,"Latitude":38.007299276929025},{"Longitude":23.74970643405732,"Latitude":38.006538449285159},{"Longitude":23.749975516981781,"Latitude":38.00627449090539},{"Longitude":23.75049050111264,"Latitude":38.006502740849264},{"Longitude":23.751327350325287,"Latitude":38.005302307259825},{"Longitude":23.751627757734955,"Latitude":38.005361484024043},{"Longitude":23.751928165144623,"Latitude":38.00283374825802},{"Longitude":23.750258226046459,"Latitude":38.001926190436642},{"Longitude":23.750526446947948,"Latitude":38.001655655021608},{"Longitude":23.74972178424348,"Latitude":38.000776408029971},{"Longitude":23.750377969770994,"Latitude":37.999766813805238},{"Longitude":23.750809926117686,"Latitude":37.998640586271613}]

            PlaceOfDistributionJson: {"ExtensionProperties":{},"CityTown":"Αθήνα","StateProvinceRegion":null,"Description":"Λαζαράδων 29, Αθήνα, Ελλάδα","StreetNumber":"29","Street":"Λαζαράδων","Area":null,"PostalCode":"113 63","Country":"Ελλάδα","Location":{"Longitude":23.7472703,"Latitude":38.00026},"PlaceID":"ChIJxZ_kbbqioRQRYiLJ5_50sQQ","Default":true}

         */
    }
}