using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HomeDelivery;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.Shipping;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessManager.Shipping;
using FlavourBusinessToolKit;
using MenuModel;
using OOAdvantech;

using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{6412585a-256a-42d3-8bc5-e28467ff30b2}</MetaDataID>
    [BackwardCompatibilityID("{6412585a-256a-42d3-8bc5-e28467ff30b2}")]
    [Persistent()]
    public class HomeDeliveryServicePoint : ServicePoint, IHomeDeliveryServicePoint
    {

        /// <MetaDataID>{ba185288-883f-492a-8106-98826b150320}</MetaDataID>
        public HomeDeliveryServicePoint()
        {

        }
        /// <MetaDataID>{f5213c16-1b33-465c-9c83-9ffb82f8259f}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+10")]
        private string MapCenterJson;

        /// <exclude>Excluded</exclude> 
        decimal _MinimumOrderValue;
        /// <MetaDataID>{ad37bdfb-0a9f-4753-9723-51fdd393e086}</MetaDataID>
        [PersistentMember(nameof(_MinimumOrderValue))]
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        public decimal MinimumOrderValue
        {
            get => _MinimumOrderValue;
            set
            {
                if (_MinimumOrderValue != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MinimumOrderValue = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        public string GeocodingApiKey { get => "AIzaSyAuon626ZLzKmYgmCCpAF3dvILvSizjaTI"; }

        /// <MetaDataID>{4bcce2d4-e720-47bd-bafe-b5dea36afbc3}</MetaDataID>
        internal bool CanBeAssignedTo(ICourier waiter, IShiftWork shiftWork)
        {
            return true;
        }


        /// <exclude>Excluded</exclude>
        bool _IsActive;
        /// <MetaDataID>{a59cb9b3-7a94-4d32-8548-bbf64f058696}</MetaDataID>
        [PersistentMember(nameof(_IsActive))]
        [BackwardCompatibilityID("+5")]
        internal bool IsActive
        {
            get => _IsActive;
            set
            {

                if (_IsActive != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsActive = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude> 
        Dictionary<DayOfWeek, List<OpeningHours>> _WeeklyDeliverySchedule = new Dictionary<DayOfWeek, List<OpeningHours>>();

        /// <MetaDataID>{b7115410-3a04-441e-a776-36d3bc068202}</MetaDataID>
        public Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule
        {
            get => _WeeklyDeliverySchedule;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _WeeklyDeliverySchedule = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        List<Coordinate> _ServiceAreaMap = new List<Coordinate>();
        /// <MetaDataID>{28d81c75-938c-4dba-8c5a-c2f456e725b5}</MetaDataID>
        public List<Coordinate> ServiceAreaMap
        {
            get => _ServiceAreaMap;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ServiceAreaMap = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        decimal _FreeShippingMinimumOrderValue;

        /// <MetaDataID>{962cc95f-7c44-456b-9817-831fd6928525}</MetaDataID>
        [PersistentMember(nameof(_FreeShippingMinimumOrderValue))]
        [BackwardCompatibilityID("+3")]
        [CachingDataOnClientSide]
        public decimal FreeShippingMinimumOrderValue
        {
            get => _FreeShippingMinimumOrderValue;
            set
            {
                if (_FreeShippingMinimumOrderValue != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FreeShippingMinimumOrderValue = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        decimal _ShippingCost;

        /// <MetaDataID>{48980b23-8e25-47c9-906c-0ced1401a34e}</MetaDataID>
        [PersistentMember(nameof(_ShippingCost))]
        [BackwardCompatibilityID("+11")]
        [CachingDataOnClientSide]
        public decimal ShippingCost
        {
            get => _ShippingCost;
            set
            {
                if (_ShippingCost != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ShippingCost = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{1c2670e3-8d2d-4b7c-aaed-1ec452eab8ee}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        string PlaceOfDistributionJson = null;

        /// <exclude>Excluded</exclude>
        EndUsers.Place _PlaceOfDistribution;
        /// <MetaDataID>{67337dcd-a5e2-4ca9-8c70-077d6e594510}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        public IPlace PlaceOfDistribution
        {
            get => _PlaceOfDistribution;
            set
            {
                if (_PlaceOfDistribution != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PlaceOfDistribution = value as EndUsers.Place;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Coordinate? _MapCenter;
        /// <MetaDataID>{14a3ba03-5e0d-43c1-bbd1-eb01a8c246ea}</MetaDataID>
        public Coordinate? MapCenter { get => _MapCenter; set => _MapCenter = value; }


        /// <exclude>Excluded</exclude>
        double _Zoom;

        /// <MetaDataID>{42346420-ec90-4b15-9070-2eac25381892}</MetaDataID>
        [PersistentMember(nameof(_Zoom))]
        [BackwardCompatibilityID("+8")]
        public double Zoom
        {
            get => _Zoom;
            set
            {
                if (_Zoom != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Zoom = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _IsPolyline;
        /// <MetaDataID>{003f8343-7a42-40a2-a86e-b9940dcdddeb}</MetaDataID>
        [PersistentMember(nameof(_IsPolyline))]
        [BackwardCompatibilityID("+9")]
        public bool IsPolyline
        {
            get => _IsPolyline;
            set
            {
                if (_IsPolyline != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsPolyline = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _BrandName;

        /// <MetaDataID>{d2e4dee5-03b9-4948-9f69-28c0e36b114b}</MetaDataID>
        [PersistentMember(nameof(_BrandName))]
        [BackwardCompatibilityID("+12")]
        public string BrandName
        {
            get => _BrandName;
            set
            {
                if (_BrandName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BrandName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _LogoImageUrl;

        /// <MetaDataID>{d55c84f7-803d-4684-a946-135dfdf49d7d}</MetaDataID>
        [PersistentMember(nameof(_LogoImageUrl))]
        [BackwardCompatibilityID("+13")]
        public string LogoImageUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LogoImageUrl))
                    return null;
                return RawStorageCloudBlob.RootUri + "/" + _LogoImageUrl;
            }
        }


        /// <exclude>Excluded</exclude>
        string _LogoBackgroundImageUrl;

        /// <MetaDataID>{8ddadddb-aa2d-4383-927d-f5fab9a46357}</MetaDataID>
        [PersistentMember(nameof(_LogoBackgroundImageUrl))]
        [BackwardCompatibilityID("+14")]
        public string LogoBackgroundImageUrl
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_LogoBackgroundImageUrl))
                    return null;
                return RawStorageCloudBlob.RootUri + "/" + _LogoBackgroundImageUrl;
            }
        }


        /// <MetaDataID>{1348a699-3c57-4358-a22a-845dcb992d0a}</MetaDataID>
        public void Update(string brandName, IPlace placeOfDistribution, Coordinate? mapCenter, List<Coordinate> serviceAreaMap, bool isPolyline, double zoom, Dictionary<DayOfWeek, List<OpeningHours>> weeklyDeliverySchedule, decimal minimumOrderValue, decimal shippingCost, decimal freeShippingMinimumOrderValue)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                PlaceOfDistribution = placeOfDistribution;
                MapCenter = mapCenter;
                ServiceAreaMap = serviceAreaMap;
                IsPolyline = isPolyline;
                Zoom = zoom;
                WeeklyDeliverySchedule = weeklyDeliverySchedule;
                MinimumOrderValue = minimumOrderValue;
                ShippingCost = shippingCost;
                BrandName = brandName;
                FreeShippingMinimumOrderValue = freeShippingMinimumOrderValue;


                stateTransition.Consistent = true;
            }
            base.RunObjectChangeState(this, null);

        }

        /// <MetaDataID>{ae158781-fd71-4b27-8311-76438eaabc23}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        string ServiceAreaMapJson;


        /// <MetaDataID>{23169d00-d175-494e-9d0f-ab960b760834}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        string WeeklyDeliveryScheduleJson;

        /// <MetaDataID>{41e6bdf1-c64e-4d58-ae16-5280acb3324a}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            ServiceAreaMapJson = OOAdvantech.Json.JsonConvert.SerializeObject(_ServiceAreaMap);
            if (_MapCenter != null)
                MapCenterJson = OOAdvantech.Json.JsonConvert.SerializeObject(_MapCenter);

            WeeklyDeliveryScheduleJson = OOAdvantech.Json.JsonConvert.SerializeObject(_WeeklyDeliverySchedule);

            PlaceOfDistributionJson = OOAdvantech.Json.JsonConvert.SerializeObject(_PlaceOfDistribution);

        }
        /// <MetaDataID>{d602d811-8ffd-4679-a1ac-6511c47a057a}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {

            string tmp = @"[{""Latitude"":38.000240262320936, ""Longitude"":23.750335054426756},{""Latitude"":37.99832108602798,""Longitude"":23.751161174803343},{ ""Latitude"":37.99534499527491,""Longitude"":23.749326543837157},{ ""Latitude"":37.996503317677686,""Longitude"":23.74603279116687},{ ""Latitude"":37.99605520972059,""Longitude"":23.743919210463133},{ ""Latitude"":37.99616512324652,""Longitude"":23.742170410185423},{ ""Latitude"":37.99491379029195,""Longitude"":23.736054973631468},{ ""Latitude"":37.995556369340605,""Longitude"":23.732074575453368},{ ""Latitude"":38.00013880932487,""Longitude"":23.7332332897478},{ ""Latitude"":38.00572696945966,""Longitude"":23.734713869124022},{ ""Latitude"":38.00665265229764,""Longitude"":23.734955267935362},{ ""Latitude"":38.00648357867907,""Longitude"":23.740719335108366},{ ""Latitude"":38.005760784563485,""Longitude"":23.741502540140715},{ ""Latitude"":38.00575655767636,""Longitude"":23.742610292463866},{ ""Latitude"":38.006542372746324,""Longitude"":23.743995032319887},{ ""Latitude"":38.00739196305551,""Longitude"":23.744778237352236},{ ""Latitude"":38.00814010159382,""Longitude"":23.744831881532534},{ ""Latitude"":38.00805164316818,""Longitude"":23.746637986944283},{ ""Latitude"":38.007451441510376,""Longitude"":23.747614311025703},{ ""Latitude"":38.00665680077053,""Longitude"":23.74774305705842},{ ""Latitude"":38.007299276929025,""Longitude"":23.7491807210904},{ ""Latitude"":38.00653844928516,""Longitude"":23.74970643405732},{ ""Latitude"":38.00627449090539,""Longitude"":23.74997551698178},{ ""Latitude"":38.006502740849264,""Longitude"":23.75049050111264},{ ""Latitude"":38.005302307259825,""Longitude"":23.751327350325287},{ ""Latitude"":38.00536148402404,""Longitude"":23.751627757734955},{ ""Latitude"":38.00283374825802,""Longitude"":23.751928165144623},{ ""Latitude"":38.00192619043664,""Longitude"":23.75025822604646},{ ""Latitude"":38.00165565502161,""Longitude"":23.750526446947948},{ ""Latitude"":38.00077640802997,""Longitude"":23.74972178424348},{ ""Latitude"":38.000240262320936,""Longitude"":23.750335054426756},{ ""Latitude"":38.000240262320936,""Longitude"":23.750335054426756}]";
            if (!string.IsNullOrWhiteSpace(ServiceAreaMapJson))
                _ServiceAreaMap = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Coordinate>>(ServiceAreaMapJson);

            if (!string.IsNullOrWhiteSpace(MapCenterJson))
                _MapCenter = OOAdvantech.Json.JsonConvert.DeserializeObject<Coordinate>(MapCenterJson);

            if (!string.IsNullOrWhiteSpace(WeeklyDeliveryScheduleJson))
                _WeeklyDeliverySchedule = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<DayOfWeek, List<OpeningHours>>>(WeeklyDeliveryScheduleJson);

            if (!string.IsNullOrWhiteSpace(PlaceOfDistributionJson))
                _PlaceOfDistribution = OOAdvantech.Json.JsonConvert.DeserializeObject<EndUsers.Place>(PlaceOfDistributionJson);


            ServicePointRunTime.ServicesContextRunTime.Current.MealsController.NewMealCoursesInProgress += MealsController_NewMealCoursesInrogress;


        }



        /// <MetaDataID>{5ef13716-024b-4a7d-9d11-806aeefd906e}</MetaDataID>
        public IUploadSlot GetUploadSlotForLogoImage()
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);
                var organizationIdentity = authUserRef?.GetContextRoleObject<Organization>(true)?.Identity;
                if (string.IsNullOrEmpty(organizationIdentity))
                    throw new AuthenticationException();
                if (organizationIdentity != ServicePointRunTime.ServicesContextRunTime.Current.OrganizationIdentity)
                    throw new AuthenticationException();

                int version = 1;
                string removePreviousVersionBlobUrl = null;
                if (_LogoImageUrl != null && _LogoImageUrl.LastIndexOf("_v") != -1)
                {
                    version = int.Parse(_LogoImageUrl.Substring(_LogoImageUrl.LastIndexOf("_v")).Split('.')[0].Replace("_v", "")) + 1;
                    removePreviousVersionBlobUrl = _LogoImageUrl;
                }


                string versionExtension = "_v" + version;
                string blobUrl = "usersfolder/" + ServicePointRunTime.ServicesContextRunTime.Current.OrganizationIdentity + "/LogoImages/HomeDeliveryLogo" + ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity + versionExtension + ".png";

                var uploadSlot = new UploadSlot(blobUrl, removePreviousVersionBlobUrl, FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer, "image/png");

                uploadSlot.FileUploaded += LogoImageUploadSlot_FileUploaded;
                return uploadSlot;
            }
            else
                throw new AuthenticationException();

        }

        /// <MetaDataID>{daa89329-225f-4e59-9a4c-c2a31054e455}</MetaDataID>
        private void LogoImageUploadSlot_FileUploaded(object sender, EventArgs e)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _LogoImageUrl = (sender as UploadSlot).BlobUrl;
                (sender as UploadSlot).FileUploaded -= LogoImageUploadSlot_FileUploaded;
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{2f1ca4a1-24ba-4a88-94e2-8f5bd89402d1}</MetaDataID>
        public IUploadSlot GetUploadSlotForLogoBackgroundImage()
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            if (authUser != null)
            {
                AuthUserRef authUserRef = AuthUserRef.GetAuthUserRef(authUser, false);

                var organizationIdentity = authUserRef?.GetContextRoleObject<Organization>(true)?.Identity;
                if (string.IsNullOrEmpty(organizationIdentity))
                    throw new AuthenticationException();
                if (organizationIdentity != ServicePointRunTime.ServicesContextRunTime.Current.OrganizationIdentity)
                    throw new AuthenticationException();


                int version = 1;
                string removePreviousVersionBlobUrl = null;
                if (_LogoBackgroundImageUrl != null && _LogoBackgroundImageUrl.LastIndexOf("_v") != -1)
                {
                    version = int.Parse(_LogoBackgroundImageUrl.Substring(_LogoBackgroundImageUrl.LastIndexOf("_v")).Split('.')[0].Replace("_v", "")) + 1;
                    removePreviousVersionBlobUrl = _LogoBackgroundImageUrl;
                }


                string versionExtension = "_v" + version;
                string blobUrl = "usersfolder/" + ServicePointRunTime.ServicesContextRunTime.Current.OrganizationIdentity + "/LogoImages/HomeDeliveryLogoBK" + ServicePointRunTime.ServicesContextRunTime.Current.ServicesContextIdentity + versionExtension + ".avif";

                var uploadSlot = new UploadSlot(blobUrl, removePreviousVersionBlobUrl, FlavourBusinessManagerApp.CloudBlobStorageAccount, FlavourBusinessManagerApp.RootContainer, "image/avif");

                uploadSlot.FileUploaded += LogoBackgroundImageUploadSlot_FileUploaded;
                return uploadSlot;
            }
            else
                throw new AuthenticationException();

        }

        /// <MetaDataID>{e3e12b32-3725-48b5-bca5-7b1aecd599d3}</MetaDataID>
        private void MealsController_NewMealCoursesInrogress(IList<FlavourBusinessFacade.RoomService.IMealCourse> mealCoursers)
        {
            RunObjectChangeState(this, nameof(HomeDeliveryServicePoint.WatchingOrders));
        }


        /// <MetaDataID>{c5ef0e0f-e18d-4731-813f-608a051e2d8a}</MetaDataID>
        public List<WatchingOrder> WatchingOrders
        {
            get
            {

                var foodServicesSessions = this.ActiveFoodServiceClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery && x.MainSession != null && x.MainSession.Meal != null).Select(x => x.MainSession).Distinct().ToList();
                var serviceAreaMapPolyGon = new MapPolyGon(ServiceAreaMap);
                return (from foodServicesSession in foodServicesSessions
                        where foodServicesSession.DeliveryPlace != null
                        select new WatchingOrder()
                        {
                            SessionID = foodServicesSession.SessionID,
                            ClientPhone = foodServicesSession.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery)?.FirstOrDefault()?.Client?.PhoneNumber,
                            ClientIdentity = foodServicesSession.PartialClientSessions.Where(x => x.SessionType == SessionType.HomeDelivery)?.FirstOrDefault()?.Client?.Identity,
                            SessionType = foodServicesSession.SessionType,
                            DeliveryPlace = foodServicesSession.DeliveryPlace,
                            EntryDateTime = foodServicesSession.SessionStarts,
                            HomeDeliveryServicePoint = new HomeDeliveryServicePointAbbreviation() { Description = Description, DistanceInKm = GetRouteDistanceInKm(foodServicesSession.DeliveryPlace), Location = PlaceOfDistribution?.Location ?? default(Coordinate), ServicesContextIdentity = ServicesContextIdentity, ServicesPointIdentity = ServicesPointIdentity, OutOfDeliveryRange = IsOutOfDeliveryRange(foodServicesSession.DeliveryPlace, serviceAreaMapPolyGon) },
                            MealCourses = foodServicesSession.Meal.Courses,
                            TimeStamp = (foodServicesSession.PartialClientSessions.OrderByDescending(x => x.ModificationTime).FirstOrDefault()?.ModificationTime.Ticks - new DateTime(2022, 1, 1).Ticks)?.ToString("x"),
                            State = WatchingOrderState.InProggres,
                            OrderTotal = Bill.GetTotal(foodServicesSession)

                        }).ToList();
            }
        }

        /// <MetaDataID>{1cb5aafb-70aa-4c3e-a71c-070a5a330be9}</MetaDataID>
        private bool IsOutOfDeliveryRange(IPlace deliveryPlace, MapPolyGon serviceAreaMapPolyGon)
        {
            return !serviceAreaMapPolyGon.FindPoint(deliveryPlace.Location.Latitude, deliveryPlace.Location.Longitude);
        }

        /// <MetaDataID>{0d15bff7-fe2b-45b6-853d-53aab0caa5f9}</MetaDataID>
        public CallCenterStationWatchingOrders GetWatchingOrders(List<WatchingOrderAbbreviation> candidateToRemoveWatchingOrders = null)
        {

            CallCenterStationWatchingOrders callCenterStationWatchingOrders = new CallCenterStationWatchingOrders();
            //List<WatchingOrderAbbreviation> removedWatchingOrders = new OOAdvantech.Collections.Generic.List<WatchingOrderAbbreviation>();
            if (candidateToRemoveWatchingOrders != null)
            {
                List<WatchingOrder> watchingOrders = WatchingOrders.ToList();
                callCenterStationWatchingOrders.WatchingOrders = watchingOrders.Where(x => !candidateToRemoveWatchingOrders.Any(y => y.SessionID == x.SessionID && y.TimeStamp == x.TimeStamp)).ToList();
                callCenterStationWatchingOrders.MissingWatchingOrders = candidateToRemoveWatchingOrders.Where(x => !watchingOrders.Any(y => y.SessionID == x.SessionID)).ToList();
                return callCenterStationWatchingOrders;
            }
            else
            {

                callCenterStationWatchingOrders.WatchingOrders = WatchingOrders.ToList();
                return callCenterStationWatchingOrders;
            }

        }


        public CourierShippingPair GetCourierShipping(string scannedCode)
        {

            var mealCourse = ServicesContextRunTime.Current.MealsController.MealCoursesInProgress.Where(x => x.Identity == scannedCode).FirstOrDefault();
            var foodShiping = (ServicesContextRunTime.Current.MealsController as MealsController).GetServingBatchesAtTheCounter().OfType<FoodShipping>().Where(x => x.MealCourse == mealCourse).FirstOrDefault();

            if (foodShiping != null)
                return new CourierShippingPair() { FoodShipping = foodShiping };

            var activeCouriers = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                  where shiftWork.Worker is ICourier && CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork)
                                  select shiftWork.Worker).OfType<HumanResources.Courier>().ToList();

            ICourier courier = activeCouriers.Where(x => x.Identity == scannedCode).FirstOrDefault();
            if (courier == null)
            {
                courier = ServicesContextRunTime.Current.ServiceContextHumanResources.Couriers.Where(x => x.Identity == scannedCode).FirstOrDefault();

                if (courier != null)
                    throw new CourierOutOfShiftException("CourierOutOfShiftException", courier.Name);
            }
            return new CourierShippingPair() { Courier = courier };


        }


        /// <MetaDataID>{0458fef5-cdd1-44f1-b0ef-01d23c88ceef}</MetaDataID>
        private double GetRouteDistanceInKm(IPlace deleiveryPlace)
        {
            double.TryParse(deleiveryPlace.GetExtensionProperty("RouteDistanceInMeters"), out var distance);
            distance = Math.Round(distance / 100) / 10;//round  to 100 meters
            return distance;

        }


        /// <MetaDataID>{0a16384f-6eea-40de-9219-93dd5d23b135}</MetaDataID>
        private void LogoBackgroundImageUploadSlot_FileUploaded(object sender, EventArgs e)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _LogoBackgroundImageUrl = (sender as UploadSlot).BlobUrl;
                (sender as UploadSlot).FileUploaded -= LogoBackgroundImageUploadSlot_FileUploaded;
                stateTransition.Consistent = true;
            }


        }
        /// <MetaDataID>{625536ac-7eae-40cf-a660-dc392e37e604}</MetaDataID>
        public override IList<IFoodServiceClientSession> GetServicePointOtherPeople(IFoodServiceClientSession serviceClientSession)
        {
            return new List<IFoodServiceClientSession>();
        }

        /// <MetaDataID>{b5104872-4456-4bc8-8a36-ee77cdeefbb5}</MetaDataID>
        internal WatchingOrder GetWatchingOrder(IFoodServiceClientSession foodServicesClientSession)
        {
            var watchingOrders = WatchingOrders;
            var watchingOrder = watchingOrders.Where(x => x.SessionID == foodServicesClientSession.MainSession.SessionID).FirstOrDefault();
            return watchingOrder;
        }

        /// <MetaDataID>{f2a204fe-6d7e-48ec-9896-4643f0907848}</MetaDataID>
        [OnDemandCachingDataOnClientSide]
        public List<ReturnReason> ReturnReasons
        {
            get
            {
                return new List<ReturnReason>() {
                    new ReturnReason("LTDLV", new Dictionary<string, string>() { { "el", "Αργοπορημένη Παραγγελία" }, { "en", "Late Delivery" } }),
                    new ReturnReason("WRNGPROD",new Dictionary<string, string>() { { "el", "Λάθος Προϊόν" }, { "en", "Wrong Product" } }),
                    new ReturnReason("WRNGORD",new Dictionary<string, string>() { { "el", "Λαθος Παραγγελία" }, { "en", "Wrong Order" } }),
                    new ReturnReason("BDQLPROD", new Dictionary<string, string>() { { "el", "Κακής Ποιότητας Προϊον" }, { "en", "Bad Quality Product" } })

                };
            }
        }

        /// <MetaDataID>{e1a28e62-dc78-49c0-a6d9-515f8c4034b8}</MetaDataID>
        public TimeSpan DelayedFoodShippingAtTheCounterTimespan { get => TimeSpan.FromMinutes(2); }
        //public TimeSpan DelayedFoodShippingDeliveryTimespan { get => TimeSpan.FromMinutes(5); }

        /// <MetaDataID>{0f06a47c-7b63-4eb4-943c-3d7bf170b2a6}</MetaDataID>
        public int DelayedFoodShippingDeliveryPerc { get => 10; }
        /// <MetaDataID>{af2c33c5-e03a-43a6-aa53-8960a124331f}</MetaDataID>
        public int DelayedCourierReturnPerc { get => 10; }

        /// <MetaDataID>{5736d8b6-6ce2-4a29-8337-ee2454f4c52f}</MetaDataID>
        public TimeSpan DeliveryAndCollectMoneyTimespan { get => TimeSpan.FromMinutes(4); }
        public bool CourierAuditingEnabled
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
    }
}