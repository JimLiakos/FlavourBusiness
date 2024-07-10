using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade;
using FlavourBusinessManager.EndUsers;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech;
using System.Collections.Generic;
using System;
using System.Linq;
using OOAdvantech.PersistenceLayer;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;

using FlavourBusinessFacade.Shipping;
using FlavourBusinessManager.Shipping;
using OOAdvantech.Remoting.RestApi;
using FlavourBusinessManager.ServicesContextResources;
using FlavourBusinessFacade.ServicesContextResources;
using System.Globalization;
using System.Net.Http;
using GoogleMap;
using System.Threading.Tasks;

namespace FlavourBusinessManager.HumanResources
{
    /// <MetaDataID>{4c8009f5-08ad-49f6-a32c-c51648eba5bd}</MetaDataID>
    [BackwardCompatibilityID("{4c8009f5-08ad-49f6-a32c-c51648eba5bd}")]
    [Persistent()]
    public class Courier : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICourier
    {

        /// <exclude>Excluded</exclude>
        string _UserLanguageCode;
        /// <MetaDataID>{166cbf0a-733f-428f-af53-6a7dafa1c865}</MetaDataID>
        [PersistentMember(nameof(_UserLanguageCode))]
        [BackwardCompatibilityID("+21")]
        public string UserLanguageCode
        {
            get => _UserLanguageCode;
            set
            {
                if (_UserLanguageCode != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserLanguageCode = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _StateTimestamp;
        /// <MetaDataID>{3fcdfbf5-8dc7-408d-9d57-0583cddd517c}</MetaDataID>
        [PersistentMember(nameof(_StateTimestamp))]
        [BackwardCompatibilityID("+20")]
        internal DateTime StateTimestamp
        {
            get => _StateTimestamp;
            private set
            {

                if (_StateTimestamp != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _StateTimestamp = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }



        /// <MetaDataID>{9934c842-848c-4aee-9503-cae650097b4e}</MetaDataID>
        public Courier()
        {
        }



        /// <MetaDataID>{b509508a-b16e-46ac-b9d6-7270e5325433}</MetaDataID>
        public void FoodShippingReturn(IFoodShipping foodShipping, string returnReasonIdentity, string customReturnReasonDescription = null)
        {
            var returnReason = (foodShipping.ServicePoint as IHomeDeliveryServicePoint).ReturnReasons.Where(x => x.Identity == returnReasonIdentity).FirstOrDefault();

            var states = foodShipping.PreparedItems.ToDictionary(x => x.uid, x => x.State);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var itemPreparation in foodShipping.PreparedItems)
                    itemPreparation.State = ItemPreparationState.Canceled;

                states = foodShipping.PreparedItems.ToDictionary(x => x.uid, x => x.State);


                if (returnReason != null)
                {
                    foodShipping.ReturnReasonID = returnReasonIdentity;
                    if (string.IsNullOrWhiteSpace(customReturnReasonDescription))
                        foodShipping.ReturnReason = returnReason.Description;
                    else
                        foodShipping.ReturnReason = customReturnReasonDescription;
                }
                else
                {
                    foodShipping.ReturnReasonID = "CSTMRTNR";
                    foodShipping.ReturnReason = customReturnReasonDescription;
                }

                stateTransition.Consistent = true;
            }
            Transaction.RunOnTransactionCompleted(() =>
            {
                var newItemsState = foodShipping.PreparedItems.ToDictionary(x => x.uid, x => x.State);
                (foodShipping as FoodShipping).OnItemsChangeState(newItemsState);
                (foodShipping.MealCourse as MealCourse).RaiseItemsStateChanged(newItemsState);
            });
        }

        /// <MetaDataID>{8a492145-2db2-44ff-8c70-31fa8e1cf7c3}</MetaDataID>
        public void ImBack()
        {
            if (State == CourierState.EndOfDeliveryAndReturn)
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _State = CourierState.Idle;
                    var endOfDeliveryTimeStamp = StateTimestamp;
                    StateMachineMonitoring();

                    AuditForDelayedCourierReturn(endOfDeliveryTimeStamp);
                    stateTransition.Consistent = true;
                }


            }
        }
        /// <MetaDataID>{0a5bf956-7ee4-429b-85b7-f97c68d90d83}</MetaDataID>
        public void Delivered(IFoodShipping foodShipping)
        {
            var states = foodShipping.PreparedItems.ToDictionary(x => x.uid, x => x.State);
            var onTheRoadStateTimestamp = StateTimestamp;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                EnsureCommonDistributionForOnRoadFoodShiping();// in case where delivered before on road delay time
                foreach (var itemPreparation in foodShipping.PreparedItems)
                    itemPreparation.State = ItemPreparationState.Served;

                (foodShipping as FoodShipping).DeliveryTime = DateTime.UtcNow;

                var distributionFoodShippings = foodShipping.ShiftWork.ServingBatches.OfType<FoodShipping>().Where(x => x.DistributionIdentity == foodShipping.DistributionIdentity);
                if (distributionFoodShippings.All(x => (x.State == ItemPreparationState.Served || x.State == ItemPreparationState.Canceled)))
                    State = CourierState.EndOfDeliveryAndReturn;

                stateTransition.Consistent = true;
            }

            Transaction.RunOnTransactionCompleted(() =>
            {
                var distributionFoodShippings = foodShipping.ShiftWork.ServingBatches.OfType<FoodShipping>().Where(x => x.DistributionIdentity == foodShipping.DistributionIdentity);

                if (distributionFoodShippings.All(x => (x.State == ItemPreparationState.Served || x.State == ItemPreparationState.Canceled)))
                {

                    if (distributionFoodShippings.All(x => x.Place?.Location.IsEmpty == false && !string.IsNullOrWhiteSpace(x.Place?.GetExtensionProperty("RouteDurationInSeconds"))))//valid geocode data
                        this.AuditForDelayedDelivery(foodShipping, onTheRoadStateTimestamp);
                }
                var newItemsState = foodShipping.PreparedItems.ToDictionary(x => x.uid, x => x.State);
                (foodShipping as FoodShipping).OnItemsChangeState(newItemsState);
                (foodShipping.MealCourse as MealCourse).RaiseItemsStateChanged(newItemsState);
            });

            //if (foodShipping.ShiftWork.ServingBatches.OfType<FoodShipping>().Where(x => x.State == ItemPreparationState.OnRoad).Count()==0)
            //{
            //    this.AuditForDelayedDelivery(foodShipping);
            //}

        }

        //   
        //{"DeliveryPlaceBell":"Λιάκος Δημήτρης","RouteDistanceInMeters":"2851","RouteDurationInMinutes":"578","RouteOrigin":"{\"Longitude\":23.765026,\"Latitude\":37.9960904}","DeliveryPlaceFloor":"3ος"}

        /// <MetaDataID>{d8860b6a-f71c-4642-80d8-326ab9a9e0ca}</MetaDataID>
        private Task AuditForDelayedDelivery(IFoodShipping foodShipping, DateTime onTheRoadStateTimestamp)
        {

            //37.946925, 22.987680

            //https://maps.google.com/maps/api/directions/json?origin=37.937782,22.923665&destination=37.946925,22.987680&sensor=false&key=AIzaSyAuon626ZLzKmYgmCCpAF3dvILvSizjaTI

            return Task.Run(() =>
            {

                try
                {
                    HomeDeliveryServicePoint homeDeliveryServicePoint = foodShipping.MealCourse.Meal.Session.ServicePoint as HomeDeliveryServicePoint;

                    var distributionFoodShippings = foodShipping.ShiftWork.ServingBatches.OfType<FoodShipping>().Where(x => x.DistributionIdentity == foodShipping.DistributionIdentity);

                    if (distributionFoodShippings.All(x => (x.State == ItemPreparationState.Served || x.State == ItemPreparationState.Canceled) &&
                                                    x.Place?.Location.IsEmpty == false &&
                                                    !string.IsNullOrWhiteSpace(x.Place?.GetExtensionProperty("RouteDurationInSeconds"))))
                    {
                        distributionFoodShippings = distributionFoodShippings.OrderBy(x => x.DeliveryTime).ToList();
                        int forecastDeliveryDurationInSecs = 0;
                        int totalRouteDurationInSecs = 0;
                        IPlace previousFoodShippingDeliveryPlace = null;
                        foreach (var distributionFoodShipping in distributionFoodShippings)
                        {
                            if (previousFoodShippingDeliveryPlace?.Location.IsEmpty == false && distributionFoodShipping?.Place?.Location.IsEmpty == false)
                            {
                                string originLat = previousFoodShippingDeliveryPlace.Location.Latitude.ToString(CultureInfo.GetCultureInfo(1033));
                                string originLng = previousFoodShippingDeliveryPlace.Location.Longitude.ToString(CultureInfo.GetCultureInfo(1033));
                                string destanationLat = distributionFoodShipping.Place.Location.Latitude.ToString(CultureInfo.GetCultureInfo(1033));
                                string destanationLng = distributionFoodShipping.Place.Location.Longitude.ToString(CultureInfo.GetCultureInfo(1033));

                                string url = $"https://maps.google.com/maps/api/directions/json?origin={originLat},{originLng}&destination={destanationLat},{destanationLng}&sensor=false&key={homeDeliveryServicePoint.GeocodingApiKey}";
                                Directions directions = null;
                                try
                                {
                                    using (HttpClient httpClient = new HttpClient())
                                    {
                                        var directionsJsonTask = httpClient.GetStringAsync(url);
                                        directionsJsonTask.Wait();
                                        var directionsJson = directionsJsonTask.Result;
                                        directions = OOAdvantech.Json.JsonConvert.DeserializeObject<Directions>(directionsJson);
                                        if (directions.routes.Count == 0)
                                            return;
                                    }

                                }
                                catch (Exception error)
                                {
                                }
                                if (directions?.routes.FirstOrDefault()?.legs.Count != 0)
                                {
                                    int duration = directions.routes[0].legs[0].duration.value;
                                    totalRouteDurationInSecs += duration;
                                }
                                else
                                    return; //missing route data to calculate the forecast delivery duration;

                            }
                            else
                            {
                                previousFoodShippingDeliveryPlace = distributionFoodShipping.Place;
                                int duration = 0;
                                int.TryParse(distributionFoodShipping.Place?.GetExtensionProperty("RouteDurationInSeconds"), out duration);
                                totalRouteDurationInSecs += duration + (int)homeDeliveryServicePoint.DeliveryAndCollectMoneyTimespan.TotalSeconds;
                            }
                        }
                        forecastDeliveryDurationInSecs = totalRouteDurationInSecs + (int)homeDeliveryServicePoint.DeliveryAndCollectMoneyTimespan.TotalSeconds * distributionFoodShippings.Count();

                        if (forecastDeliveryDurationInSecs < (DateTime.UtcNow - onTheRoadStateTimestamp.ToUniversalTime()).TotalSeconds)
                        {

                            var deliveryDurationOverTime = TimeSpan.FromSeconds((DateTime.UtcNow - onTheRoadStateTimestamp.ToUniversalTime()).TotalSeconds - forecastDeliveryDurationInSecs);
                            var acceptableDelayTimespan = TimeSpan.FromSeconds(forecastDeliveryDurationInSecs * (homeDeliveryServicePoint.DelayedFoodShippingDeliveryPerc / 100));
                            if (deliveryDurationOverTime.TotalMinutes > acceptableDelayTimespan.TotalMinutes)
                            {

                                var auditCourierDalay = new AuditCourierDelay()
                                {
                                    Description = Properties.Resources.DelayedFoodshippingDelivery,
                                    DelayInMinutes = deliveryDurationOverTime.TotalMinutes,
                                    EventTimeStamp = DateTime.UtcNow,
                                    TotalRouteDurationInSecs = totalRouteDurationInSecs,
                                    StartOfDelayTimeStamp = onTheRoadStateTimestamp.ToUniversalTime(),
                                    DistributionIdentity = distributionFoodShippings.First().DistributionIdentity,
                                    TypeOfDelauy = CourierDelayType.DeliveryDelay

                                };
                                this.ShiftWork.AddAuditWorkerEvents(auditCourierDalay);
                            }
                        }
                    }

                }
                catch (Exception error)
                {
                }

            });
        }




        /// <MetaDataID>{47b09226-4e11-4b69-8835-f4d336b59c9c}</MetaDataID>
        private Task AuditForDelayedCourierReturn(DateTime endOfDeliveryTimeStamp)
        {

            var lastFoodShipping = (ShiftWork as ServingShiftWork)?.ServingBatches.OfType<FoodShipping>().Where(x => x.State == ItemPreparationState.Canceled || x.State == ItemPreparationState.Served).OrderBy(x => x.DeliveryTime).Last();
            string routeOriginJson = lastFoodShipping?.Place.GetExtensionProperty("RouteOrigin");
            if (lastFoodShipping != null && !string.IsNullOrWhiteSpace(routeOriginJson))
            {
                HomeDeliveryServicePoint homeDeliveryServicePoint = lastFoodShipping.MealCourse.Meal.Session.ServicePoint as HomeDeliveryServicePoint;

                int returnRouteDuration = 0;
                {
                    int.TryParse(lastFoodShipping.Place?.GetExtensionProperty("RouteDurationInSeconds"), out returnRouteDuration);
                    TimeSpan returnDurationOverTime = TimeSpan.FromSeconds(((DateTime.UtcNow - endOfDeliveryTimeStamp.ToUniversalTime()).TotalSeconds) - returnRouteDuration);
                    var acceptableDelayTimespan = TimeSpan.FromSeconds((returnRouteDuration * homeDeliveryServicePoint.DelayedCourierReturnPerc / 100));
                    if (returnDurationOverTime.TotalMinutes > acceptableDelayTimespan.TotalMinutes)
                    {

                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            var auditCourierDalay = new AuditCourierDelay()
                            {
                                Description = Properties.Resources.CourierDelayedHisReturn,
                                DelayInMinutes = returnDurationOverTime.TotalMinutes,
                                EventTimeStamp = DateTime.UtcNow,
                                TotalRouteDurationInSecs = returnRouteDuration,
                                StartOfDelayTimeStamp = endOfDeliveryTimeStamp,
                                DistributionIdentity = lastFoodShipping.DistributionIdentity,
                                TypeOfDelauy = CourierDelayType.DelayToReturn

                            };
                            this.ShiftWork.AddAuditWorkerEvents(auditCourierDalay);
                            stateTransition.Consistent = true;
                        }

                    }

                }
                return Task.CompletedTask;
                return Task.Run(() =>
                {


                    var routeOrigin = OOAdvantech.Json.JsonConvert.DeserializeObject<Coordinate>(routeOriginJson);

                    string originLat = lastFoodShipping.Place.Location.Latitude.ToString(CultureInfo.GetCultureInfo(1033));
                    string originLng = lastFoodShipping.Place.Location.Longitude.ToString(CultureInfo.GetCultureInfo(1033));
                    string destanationLat = routeOrigin.Latitude.ToString(CultureInfo.GetCultureInfo(1033));
                    string destanationLng = routeOrigin.Longitude.ToString(CultureInfo.GetCultureInfo(1033));

                    string url = $"https://maps.google.com/maps/api/directions/json?origin={originLat},{originLng}&destination={destanationLat},{destanationLng}&sensor=false&key={homeDeliveryServicePoint.GeocodingApiKey}";
                    Directions directions = null;
                    try
                    {
                        using (HttpClient httpClient = new HttpClient())
                        {
                            var directionsJsonTask = httpClient.GetStringAsync(url);
                            directionsJsonTask.Wait();
                            var directionsJson = directionsJsonTask.Result;
                            directions = OOAdvantech.Json.JsonConvert.DeserializeObject<Directions>(directionsJson);
                            if (directions.routes.Count == 0)
                                return;
                        }

                    }
                    catch (Exception error)
                    {
                    }
                    if (directions?.routes.FirstOrDefault()?.legs.Count != 0)
                    {
                        returnRouteDuration = directions.routes[0].legs[0].duration.value;
                        TimeSpan returnDurationOverTime = TimeSpan.FromSeconds((DateTime.UtcNow - endOfDeliveryTimeStamp.ToUniversalTime()).TotalSeconds - returnRouteDuration);
                        var acceptableDelayTimespan = TimeSpan.FromSeconds(homeDeliveryServicePoint.DelayedCourierReturnPerc * returnRouteDuration);
                        if (returnDurationOverTime.TotalMinutes > acceptableDelayTimespan.TotalMinutes)
                        {

                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                            {
                                var auditCourierDalay = new AuditCourierDelay()
                                {
                                    Description = Properties.Resources.CourierDelayedHisReturn,
                                    DelayInMinutes = returnDurationOverTime.TotalMinutes,
                                    EventTimeStamp = DateTime.UtcNow,
                                    TotalRouteDurationInSecs = returnRouteDuration,
                                    StartOfDelayTimeStamp = endOfDeliveryTimeStamp,
                                    DistributionIdentity = lastFoodShipping.DistributionIdentity,
                                    TypeOfDelauy = CourierDelayType.DelayToReturn

                                };
                                this.ShiftWork.AddAuditWorkerEvents(auditCourierDalay);
                                stateTransition.Consistent = true;
                            }

                        }
                    }
                });
                return Task.CompletedTask;
            }
            else
                return Task.CompletedTask;
        }


        /// <exclude>Excluded</exclude>
        string _DeviceFirebaseToken;

        /// <MetaDataID>{9e7f8fe2-0f57-4d64-be1f-c88a5fc2ea49}</MetaDataID>
        /// <summary>This token is the identity of device for push notification mechanism</summary>
        [PersistentMember(nameof(_DeviceFirebaseToken))]
        [BackwardCompatibilityID("+1")]
        public string DeviceFirebaseToken
        {
            get => _DeviceFirebaseToken;
            set
            {

                if (_DeviceFirebaseToken != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceFirebaseToken = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{969e2ac6-6d6d-4722-aa8e-fb59818914ce}</MetaDataID>
        public string DeviceToken => DeviceFirebaseToken;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{54f7a325-c970-4b1c-a4df-329219094002}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+17")]
        private DateTime? LastThreeShiftsPeriodStart;

        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{48dc1d34-f201-4c2b-8664-ea76961a718a}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+4")]
        public string Identity
        {
            get
            {
                if (_Identity == null)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = Guid.NewGuid().ToString("N");
                        stateTransition.Consistent = true;
                    }
                }
                return _Identity;
            }
        }


        /// <exclude>Excluded</exclude>
        string _OAuthUserIdentity;

        /// <MetaDataID>{350ec78b-469c-4035-ba60-694083495251}</MetaDataID>
        [PersistentMember(nameof(_OAuthUserIdentity))]
        [BackwardCompatibilityID("+5")]
        public string OAuthUserIdentity
        {
            get => _OAuthUserIdentity;
            set
            {
                if (_OAuthUserIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _OAuthUserIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _Suspended;

        /// <MetaDataID>{f9555559-cc44-404e-b3e9-8ede2d3d52a9}</MetaDataID>
        [PersistentMember(nameof(_Suspended))]
        [BackwardCompatibilityID("+6")]
        public bool Suspended
        {
            get => _Suspended; set
            {
                if (_Suspended != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Suspended = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{6c8bfcfe-1032-436c-b78b-1e69e9060be2}</MetaDataID>
        public IBill GetBill(List<SessionItemPreparationAbbreviation> itemPreparations, IFoodShipping foodShipping)
        {
            return Bill.GetBillFor(itemPreparations, foodShipping as FoodShipping);
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <exclude>Excluded</exclude>
        event MessageReceivedHandle _MessageReceived;
        public event MessageReceivedHandle MessageReceived
        {
            add
            {

                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* add MessageReceived **************************************");

                _MessageReceived += value;
            }
            remove
            {
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");
                System.Diagnostics.Debug.WriteLine("******************************* remove MessageReceived **************************************");

                _MessageReceived -= value;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IAccountability> _Responsibilities = new OOAdvantech.Collections.Generic.Set<IAccountability>();

        /// <MetaDataID>{ccdf75df-49e2-43af-b45c-12061579634e}</MetaDataID>
        [PersistentMember(nameof(_Responsibilities))]
        [BackwardCompatibilityID("+3")]
        public List<IAccountability> Responsibilities => _Responsibilities.ToThreadSafeList();



        /// <MetaDataID>{7e0bb09a-102d-4e24-9cc3-954b9763a962}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        public List<IAccountability> Commissions => new List<IAccountability>();


        /// <exclude>Excluded</exclude>
        string _WorkerAssignKey;

        /// <MetaDataID>{29cdf983-6d05-405d-b3c6-6cde8c5cf522}</MetaDataID>
        [PersistentMember(nameof(_WorkerAssignKey))]
        [BackwardCompatibilityID("+8")]
        public string WorkerAssignKey
        {
            get => _WorkerAssignKey; set
            {
                if (_WorkerAssignKey != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _WorkerAssignKey = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;
        /// <MetaDataID>{078d4774-50c4-41b9-9633-5e6b98ae0b3d}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+9")]
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
        OOAdvantech.Collections.Generic.Set<IShiftWork> _ShiftWorks = new OOAdvantech.Collections.Generic.Set<IShiftWork>();

        /// <MetaDataID>{1909309d-6473-4d8d-8ec9-eaf329ef9f42}</MetaDataID>
        [PersistentMember(nameof(_ShiftWorks))]
        [BackwardCompatibilityID("+10")]
        public IList<IShiftWork> ShiftWorks => _ShiftWorks;


        /// <MetaDataID>{f233ab03-3b88-44a3-a035-b018afa68f5d}</MetaDataID>
        List<ShiftWork> RecentlyShiftWorks;

        /// <MetaDataID>{acb9a452-7452-47e9-be6f-bd4c05a720c3}</MetaDataID>
        public FlavourBusinessFacade.HumanResources.IShiftWork ShiftWork
        {
            get
            {
                var mileStoneDate = System.DateTime.UtcNow - TimeSpan.FromDays(1);
                var objectStorage = ObjectStorage.GetStorageOfObject(this);
                if (objectStorage != null)
                {
                    if (RecentlyShiftWorks == null)
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                        RecentlyShiftWorks = (from shiftWork in storage.GetObjectCollection<ShiftWork>()
                                              where shiftWork.StartsAt > mileStoneDate && shiftWork.Worker == this
                                              select shiftWork).ToList();
                    }

                    if (RecentlyShiftWorks.Count > 0)
                    {
                    }


                    return RecentlyShiftWorks.OrderBy(x => x.StartsAt).LastOrDefault();
                }
                else
                    return _ShiftWorks.ToThreadSafeList().Where(x => x.StartsAt > mileStoneDate).LastOrDefault();
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{88c23b6a-47b2-46c0-9649-c2be0fe3e298}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+11")]
        public string Name
        {
            get => _Name; set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<Message> _Messages = new OOAdvantech.Collections.Generic.Set<Message>();

        /// <MetaDataID>{b36c929d-744f-4ed1-b3c6-aee5d9d4f712}</MetaDataID>
        [BackwardCompatibilityID("+12")]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        public System.Collections.Generic.IList<FlavourBusinessFacade.EndUsers.Message> Messages => _Messages.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        string _PhotoUrl;

        /// <MetaDataID>{54d0280b-fc9e-45a8-a9d0-2cb49617ffaf}</MetaDataID>
        [PersistentMember(nameof(_PhotoUrl))]
        [BackwardCompatibilityID("+13")]
        public string PhotoUrl
        {
            get => _PhotoUrl; set
            {
                if (_PhotoUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhotoUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _FullName;
        /// <MetaDataID>{aec913c7-6413-49e9-bee1-b2c52ed93245}</MetaDataID>
        [PersistentMember(nameof(_FullName))]
        [BackwardCompatibilityID("+14")]
        [CachingDataOnClientSide]
        public string FullName
        {
            get => _FullName; set
            {
                if (_PhotoUrl != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhotoUrl = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Email;

        /// <MetaDataID>{ced46e0b-9207-45f6-af79-f6c9c753f328}</MetaDataID>
        [PersistentMember(nameof(_Email))]
        [BackwardCompatibilityID("+18")]
        public string Email
        {
            get => _Email; set
            {
                if (_Email != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Email = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _PhoneNumber;

        /// <MetaDataID>{4a1d8d66-f25f-4cd2-81d5-dbcd97541bde}</MetaDataID>
        [PersistentMember(nameof(_PhoneNumber))]
        [BackwardCompatibilityID("+15")]
        public string PhoneNumber
        {
            get => _PhoneNumber; set
            {
                if (_PhoneNumber != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PhoneNumber = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _UserName;

        /// <MetaDataID>{7c71f0c5-24ba-4bac-9765-5356b8c92ae1}</MetaDataID>
        [PersistentMember(nameof(_UserName))]
        [BackwardCompatibilityID("+16")]
        public string UserName
        {
            get => _UserName; set
            {
                if (_UserName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _UserName = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        List<UserData.UserRole> _Roles;

        /// <MetaDataID>{134c4f86-90e1-4f84-b602-7e13936e5a2a}</MetaDataID>
        public System.Collections.Generic.List<FlavourBusinessFacade.UserData.UserRole> Roles
        {
            get
            {
                if (_Roles == null)
                {
                    UserData.UserRole role = new FlavourBusinessFacade.UserData.UserRole() { User = this, RoleType = FlavourBusinessFacade.UserData.UserRole.GetRoleType(GetType().FullName) };
                    _Roles = new List<UserData.UserRole>() { role };
                }
                return _Roles;
            }
        }

        /// <MetaDataID>{8aec60ce-6276-49b4-8c05-73fc9b3c4625}</MetaDataID>
        public bool NativeUser { get; set; }


        /// <MetaDataID>{1478a073-3a9b-4f68-9d77-b1859d666c17}</MetaDataID>
        public void ChangeSiftWork(FlavourBusinessFacade.HumanResources.IShiftWork shiftWork, System.DateTime startedAt, double timespanInHours)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                (shiftWork as ShiftWork).StartsAt = startedAt;
                (shiftWork as ShiftWork).PeriodInHours = timespanInHours;
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ShiftWork));
        }

        /// <MetaDataID>{22cc3ca5-1bc7-43cc-b0da-f3cd5272cf48}</MetaDataID>
        public IShiftWork NewShiftWork(DateTime startedAt, double timespanInHours)
        {
            ShiftWork shiftWork = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                shiftWork = new ServingShiftWork(Name);
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shiftWork);
                shiftWork.StartsAt = startedAt;
                shiftWork.PeriodInHours = timespanInHours;
                AddShiftWork(shiftWork);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ShiftWork));


            ServicePointRunTime.ServicesContextRunTime.Current.CourierSiftWorkUpdated(this);

            return shiftWork;
        }
        /// <MetaDataID>{5e438d51-5635-44c5-8de2-a6870dabc50c}</MetaDataID>
        public IShiftWork NewShiftWork(System.DateTime startedAt, double timespanInHours, decimal openingBalanceFloatCash)
        {
            ServingShiftWork shiftWork = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition())
            {
                shiftWork = new ServingShiftWork(Name);
                ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(shiftWork);
                shiftWork.StartsAt = startedAt;
                shiftWork.PeriodInHours = timespanInHours;
                shiftWork.OpeningBalanceFloatCash = openingBalanceFloatCash;
                AddShiftWork(shiftWork);
                stateTransition.Consistent = true;
            }
            ObjectChangeState?.Invoke(this, nameof(ShiftWork));


            ServicePointRunTime.ServicesContextRunTime.Current.CourierSiftWorkUpdated(this);

            return shiftWork;


        }

        /// <MetaDataID>{f9f29918-cbb2-43e5-ae35-e5db6c60c504}</MetaDataID>
        internal void AddShiftWork(ShiftWork shiftWork)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Add(shiftWork);

                stateTransition.Consistent = true;
            }
            RecentlyShiftWorks = null;

        }

        /// <MetaDataID>{bf3a1c9c-a784-45a6-b94a-a9683a18332a}</MetaDataID>
        public void RemoveShiftWork(IShiftWork shiftWork)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _ShiftWorks.Remove(shiftWork);
                stateTransition.Consistent = true;
            }
            RecentlyShiftWorks = null;
        }

        /// <MetaDataID>{7660bcb0-8af4-43d8-939e-d60bb15550ba}</MetaDataID>
        public void RemoveMessage(string messageId)
        {
            var message = Messages.Where(x => x.MessageID == messageId).FirstOrDefault();
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Messages.Remove(message);
                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{41cd19e1-abb9-4711-b7f0-42dd9c2e3921}</MetaDataID>
        public Message GetMessage(string messageId)
        {
            var message = Messages.Where(x => x.MessageID == messageId).FirstOrDefault();
            return message;
        }

        /// <MetaDataID>{df39629d-ed30-45bd-8207-3bdcc01594ab}</MetaDataID>
        public Message PeekMessage()
        {
            var message = Messages.OrderBy(x => x.MessageTimestamp).FirstOrDefault();
            if (message != null)
                message.MessageReaded = true;
            return message;
        }

        /// <MetaDataID>{8db90e1e-0487-4436-93c2-b682b3bfede8}</MetaDataID>
        public Message PopMessage()
        {
            var message = Messages.OrderBy(x => x.MessageTimestamp).FirstOrDefault();
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Messages.Remove(message);
                    stateTransition.Consistent = true;
                }
            }
            return message;
        }

        /// <MetaDataID>{67802e91-1da5-4ddb-ac25-bae8a89c968b}</MetaDataID>
        public void PushMessage(Message message)
        {
            if (message != null)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.RequiresNew))
                {

                    message.MessageTimestamp = DateTime.UtcNow;
                    if (!_Messages.Contains(message))
                    {
                        _Messages.Add(message);
                        OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(message);
                    }

                    stateTransition.Consistent = true;
                }
            }
            _MessageReceived?.Invoke(this);

        }

        /// <MetaDataID>{03cbec0d-378b-4d10-93d5-834db2202c06}</MetaDataID>
        public List<IServingShiftWork> GetLastThreeSifts()
        {
            if (LastThreeShiftsPeriodStart != null)
            {
                List<IServingShiftWork> lastThreeSifts = GetSifts(LastThreeShiftsPeriodStart.Value, DateTime.UtcNow);
                lastThreeSifts = lastThreeSifts.OrderByDescending(x => x.StartsAt).ToList();
                if (lastThreeSifts.Count > 3)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        LastThreeShiftsPeriodStart = lastThreeSifts[2].StartsAt;
                        stateTransition.Consistent = true;
                    }
                }
                lastThreeSifts = lastThreeSifts.Take(3).ToList();
                foreach (var shiftWork in lastThreeSifts)
                    shiftWork.RecalculateDeptData();

                return lastThreeSifts;
            }
            else
            {
                var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                List<IServingShiftWork> lastThreeSifts = (from shiftWork in storage.GetObjectCollection<IServingShiftWork>()
                                                          where shiftWork.Worker == this
                                                          orderby shiftWork.StartsAt descending
                                                          select shiftWork).ToList();
                if (lastThreeSifts.Count > 3)
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        LastThreeShiftsPeriodStart = lastThreeSifts[2].StartsAt;
                        stateTransition.Consistent = true;
                    }
                }
                lastThreeSifts = lastThreeSifts.Take(3).ToList();
                foreach (var shiftWork in lastThreeSifts)
                    shiftWork.RecalculateDeptData();

                return lastThreeSifts;
            }

        }



        /// <MetaDataID>{add76ae6-335c-4982-8330-8210f352b30f}</MetaDataID>
        public List<IServingShiftWork> GetSifts(DateTime startDate, DateTime endDate)
        {
            var periodStartDate = startDate;
            var periodEndDate = endDate;

            var objectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this);
            if (objectStorage != null)
            {


                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);
                var shiftWorks = (from shiftWork in storage.GetObjectCollection<ShiftWork>()
                                  where shiftWork.StartsAt >= periodStartDate && shiftWork.StartsAt <= periodEndDate && shiftWork.Worker == this
                                  select shiftWork).ToList();

                return shiftWorks.OrderBy(x => x.StartsAt).OfType<IServingShiftWork>().ToList();
            }
            else
                return _ShiftWorks.ToThreadSafeList().Where(x => x.StartsAt > periodStartDate && x.StartsAt > periodEndDate).OfType<IServingShiftWork>().ToList();
        }

        /// <MetaDataID>{2c04fdb9-b716-4fbf-82df-24b7f505ce11}</MetaDataID>
        List<IFoodShipping> AssignedFoodShippings = new List<IFoodShipping>();
        /// <MetaDataID>{27ad5e17-e923-4a75-adfd-541c33f9f2d7}</MetaDataID>
        List<IFoodShipping> FoodShippings = new List<IFoodShipping>();

        /// <MetaDataID>{351160dd-9807-4a43-8176-dc3a41933d62}</MetaDataID>
        public System.Collections.Generic.IList<FlavourBusinessFacade.Shipping.IFoodShipping> GetFoodShippings()
        {
            var foodShippings = (ServicesContextRunTime.Current.MealsController as RoomService.MealsController).GetFoodShippings(this).OfType<IFoodShipping>().ToList();
            AssignedFoodShippings = foodShippings.Where(x => x.IsAssigned && (ShiftWork != null && x.ShiftWork == this.ShiftWork)).ToList();
            FoodShippings = foodShippings.Where(x => !x.IsAssigned).ToList();
            return AssignedFoodShippings.Union(FoodShippings).ToList();
        }

        /// <MetaDataID>{706291dc-5c67-4906-b9d2-ea8072166738}</MetaDataID>
        public ServingBatchUpdates GetFoodShippingUpdates(List<ItemPreparationAbbreviation> servingItemsOnDevice)
        {
            System.Diagnostics.Debug.WriteLine("####   GetFoodShippingUpdates : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff"));
            //System.IO.File.AppendAllText(@"f:\debugOut.txt", "####   GetFoodShippingUpdates : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff")+Environment.NewLine);
            List<FoodShipping> servingBatches = (ServicesContextRunTime.Current.MealsController as RoomService.MealsController).GetFoodShippings(this).ToList();

            var itemsToServe = (from servingBatch in servingBatches
                                from itemsContext in servingBatch.ContextsOfPreparedItems
                                from itemPreparation in itemsContext.PreparationItems
                                select new { servingBatch, itemsContext, itemPreparation }).ToList();
            itemsToServe.AddRange((from servingBatch in servingBatches
                                   from itemsContext in servingBatch.ContextsOfUnderPreparationItems
                                   from itemPreparation in itemsContext.PreparationItems
                                   select new { servingBatch, itemsContext, itemPreparation }).ToList());
            var copy = itemsToServe.ToList();
            foreach (var itemToServe in itemsToServe.ToList())
            {
                var servingItemOnDevice = servingItemsOnDevice.Where(x => x.uid == itemToServe.itemPreparation.uid && x.StateTimestamp == itemToServe.itemPreparation.StateTimestamp).FirstOrDefault();
                if (servingItemOnDevice != null)
                {
                    itemsToServe.Remove(itemToServe);
                    servingItemsOnDevice.Remove(servingItemOnDevice);
                }
                else
                {
                    var updatedServingItemOnDevice = servingItemsOnDevice.Where(x => x.uid == itemToServe.itemPreparation.uid).FirstOrDefault();
                    if (updatedServingItemOnDevice != null) //servingItemOnDevice exist
                        servingItemsOnDevice.Remove(updatedServingItemOnDevice);


                }
            }
            if (servingItemsOnDevice.Count > 0)
            {

            }
            servingBatches = itemsToServe.Select(x => x.servingBatch).Distinct().Where(x => !x.IsAssigned || (ShiftWork != null && x.ShiftWork == this.ShiftWork)).ToList();




            return new ServingBatchUpdates(servingBatches.OfType<IServingBatch>().ToList(), servingItemsOnDevice);
        }


        event FoodShippingsChangedHandler _FoodShippingsChanged;
        public event FoodShippingsChangedHandler FoodShippingsChanged
        {
            add
            {
                _FoodShippingsChanged += value;
            }
            remove
            {
                _FoodShippingsChanged -= value;
            }
        }

        /// <MetaDataID>{ddfec1ac-825d-4e36-8fe0-76dc31bada46}</MetaDataID>
        public void CommitFoodShipings()
        {
            if (ShiftWork is ServingShiftWork)
            {
                lock (this)
                {

                    try
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {

                            foreach (var foodShipping in GetFoodShippings().OfType<FoodShipping>().Where(x => x.State == ItemPreparationState.Serving && x.IsAssigned && x.ShiftWork.Worker == this))
                                foodShipping.OnTheRoad();

                            stateTransition.Consistent = true;
                        }
                        StateMachineMonitoring();
                    }
                    catch (Exception error)
                    {

                        throw;
                    }
                }
            }
        }

        /// <MetaDataID>{63d19f55-e0f7-46ce-aacf-44e729404923}</MetaDataID>
        public void DeAssignFoodShipping(IFoodShipping foodShipping)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            bool foodShippingAssignmentFromMe = this.OAuthUserIdentity == authUser?.User_ID;


            if (foodShipping.PreparedItems.All(x => x.State.IsInPreviousState(ItemPreparationState.OnRoad)))
            {

                if ((foodShipping as FoodShipping).GetPayments().Where(x => x.State != FinanceFacade.PaymentState.New).Count() > 0)
                {
                    throw new PaidFoodShippingException($"The food shipping {foodShipping.Description} has payments to courier");
                }

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    (foodShipping.ShiftWork as ServingShiftWork)?.RemoveServingBatch(foodShipping);
                    (foodShipping as FoodShipping).DistributionIdentity = null;
                    (foodShipping as FoodShipping).AtTheCounter();
                    StateMachineMonitoring();
                    Transaction.RunOnTransactionCompleted(() =>
                    {
                        (foodShipping.MealCourse as MealCourse)?.ServingBatchAssigned();
                    });

                    stateTransition.Consistent = true;
                }
                StateMachineMonitoring();

                //FindFoodShippingsChanges();

                //var mealCourse = foodShipping.MealCourse;
                //var preparedItems = foodShipping.ContextsOfPreparedItems;
                //var underPreparationItems = foodShipping.ContextsOfUnderPreparationItems;
                //ObjectStorage.DeleteObject(foodShipping);

                //(foodShipping as FoodShipping).Update(mealCourse, preparedItems, underPreparationItems);

                (ServicesContextRunTime.Current.MealsController as RoomService.MealsController).FoodShippingDeAssigned(this, foodShipping);
            }
            //if (!foodShippingAssignmentFromMe)
            //    FindFoodShippingsChanges();
        }

        /// <MetaDataID>{3ee99f40-a2f1-44a7-bd23-4ff54f9e3584}</MetaDataID>
        public void RemoveFoodShippingAssignment(IFoodShipping foodShipping)
        {
            AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
            bool isSupervisor = AuthUserRef.GetAuthUserRef(authUser.User_ID)?.HasRoleType(RoleType.ServiceContextSupervisor) == true || AuthUserRef.GetAuthUserRef(authUser.User_ID)?.HasRoleType(RoleType.Organization) == true;


            if (isSupervisor && foodShipping.ShiftWork != null)
            {
                if ((foodShipping as FoodShipping).GetPayments().Where(x => x.State != FinanceFacade.PaymentState.New).Count() > 0)
                {
                    throw new PaidFoodShippingException($"The food shipping {foodShipping.Description} has payments to courier");
                }
                if (foodShipping.PreparedItems.All(x => x.State.IsInTheSameOrPreviousState(ItemPreparationState.OnRoad)))
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        (foodShipping as FoodShipping).DistributionIdentity = null;
                        (foodShipping.ShiftWork as ServingShiftWork)?.RemoveServingBatch(foodShipping);
                        (foodShipping as FoodShipping).AtTheCounter();
                        StateMachineMonitoring();
                        Transaction.RunOnTransactionCompleted(() =>
                        {
                            (foodShipping.MealCourse as MealCourse)?.ServingBatchAssigned();
                        });

                        stateTransition.Consistent = true;
                    }
                    FindFoodShippingsChanges();

                    StateMachineMonitoring();
                }
                else
                {
                    throw new InvalidOperationException("Only on the road food shipping can you deassign.");
                }
            }

        }

        /// <MetaDataID>{02b58641-354e-4b16-a172-95018e80f233}</MetaDataID>
        public void AssignFoodShipping(IFoodShipping foodShipping)
        {
            if (ShiftWork is ServingShiftWork)
            {

                AuthUser authUser = System.Runtime.Remoting.Messaging.CallContext.GetData("AutUser") as AuthUser;
                bool foodShippingAssignmentFromMe = this.OAuthUserIdentity == authUser?.User_ID;

                lock (foodShipping)
                {
                    if (!foodShipping.IsAssigned)
                    {
                        (ShiftWork as ServingShiftWork).AddServingBatch(foodShipping);
                        StateMachineMonitoring();
                        Transaction.RunOnTransactionCompleted(() =>
                        { 
                            (foodShipping.MealCourse as MealCourse)?.ServingBatchAssigned();

                            var homeDeliveryServicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<ServicesContextResources.HomeDeliveryServicePoint>().Where(x => x.ServicesPointIdentity == foodShipping.ServicesPointIdentity).FirstOrDefault();
                            var activeCouriers = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                  where shiftWork.Worker is ICourier && homeDeliveryServicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork)
                                                  select shiftWork.Worker).OfType<HumanResources.Courier>().ToList();
                            foreach (var activeCourier in activeCouriers)
                            {
                                activeCourier.AuditForDelayedMealAtTheCounter(foodShipping);
                            }



                        });

                        bool isAssined = foodShipping.IsAssigned;
                    }
                }
                (ServicesContextRunTime.Current.MealsController as RoomService.MealsController).FoodShippingAssigned(this, foodShipping);
                //if(!foodShippingAssignmentFromMe)
                //    FindFoodShippingsChanges();

                StateMachineMonitoring();
            }

        }

        /// <MetaDataID>{57bd771c-e39c-4377-96f2-e686cf73ce57}</MetaDataID>
        private void AuditForDelayedMealAtTheCounter(IFoodShipping foodShipping)
        {
            HomeDeliveryServicePoint homeDeliveryServicePoint = foodShipping.MealCourse.Meal.Session.ServicePoint as HomeDeliveryServicePoint;

            if (homeDeliveryServicePoint.CourierAuditingEnabled)
            {
                if (State == CourierState.PendingForFoodShiping && this.ShiftWork?.IsActive() == true)
                {
                    DateTime availableForshippingTimestamp = foodShipping.PreparedItems.OrderBy(x => x.StateTimestamp).LastOrDefault().StateTimestamp;
                    TimeSpan delayedFoodshippingAtTheCounterTimespan = homeDeliveryServicePoint.DelayedFoodShippingAtTheCounterTimespan;

                    DateTime startOfDelayTimeStamp;
                    TimeSpan delayForCourier;

                    if (this.StateTimestamp.ToUniversalTime() > availableForshippingTimestamp.ToUniversalTime())
                        startOfDelayTimeStamp = this.StateTimestamp.ToUniversalTime();
                    else
                        startOfDelayTimeStamp = availableForshippingTimestamp.ToUniversalTime();

                    var lastEventTimeStamp = this.ShiftWork.AuditEvents.OrderBy(x => x.EventTimeStamp).LastOrDefault()?.EventTimeStamp;
                    if (lastEventTimeStamp.HasValue && lastEventTimeStamp.Value > startOfDelayTimeStamp)
                        startOfDelayTimeStamp = lastEventTimeStamp.Value;

                    delayForCourier = DateTime.UtcNow - startOfDelayTimeStamp;

                    if (delayForCourier > delayedFoodshippingAtTheCounterTimespan)
                    {
                        AuditCourierDelay auditCourierDalay = new AuditCourierDelay()
                        {
                            Description = Properties.Resources.DelayedFoodshippingAtTheCounter,
                            DelayInMinutes = delayForCourier.TotalMinutes,
                            EventTimeStamp = DateTime.UtcNow,
                            StartOfDelayTimeStamp = startOfDelayTimeStamp,
                            TypeOfDelauy = CourierDelayType.DelayAtTheCounter
                        };

                        this.ShiftWork.AddAuditWorkerEvents(auditCourierDalay);

                    }
                }

            }
        }

        /// <MetaDataID>{c6418c1a-451e-4842-b7f3-f4b3c9cef916}</MetaDataID>
        public void AssignAndCommitFoodShippings(List<IFoodShipping> foodShippings, string apiKey)
        {
            if (ShiftWork is ServingShiftWork)
            {
                lock (this)
                {
                    try
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            foreach (var foodShipping in foodShippings)
                            {
                                AssignFoodShipping(foodShipping);
                                if ((foodShipping as FoodShipping).State == ItemPreparationState.Serving && foodShipping.IsAssigned && foodShipping.ShiftWork.Worker == this)
                                    (foodShipping as FoodShipping).OnTheRoad();

                                var homeDeliveryServicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<ServicesContextResources.HomeDeliveryServicePoint>().Where(x => x.ServicesPointIdentity == foodShipping.ServicesPointIdentity).FirstOrDefault();
                                var activeCouriers = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                      where shiftWork.Worker is ICourier && homeDeliveryServicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork)
                                                      select shiftWork.Worker).OfType<HumanResources.Courier>().ToList();
                                foreach (var activeCourier in activeCouriers)
                                {
                                    activeCourier.AuditForDelayedMealAtTheCounter(foodShipping);
                                }
                            }

                            Transaction.RunOnTransactionCompleted(() =>
                            {
                                foreach (var foodShipping in foodShippings)
                                    (foodShipping.MealCourse as MealCourse)?.ServingBatchAssigned();
                            });

                            StateMachineMonitoring();
                            stateTransition.Consistent = true;
                        }
                        FindFoodShippingsChanges();
                        StateMachineMonitoring();
                    }
                    catch (Exception error)
                    {
                        throw;
                    }
                }
            }

        }

        /// <MetaDataID>{d1b04d2d-8219-4a1f-947f-b16de95a053a}</MetaDataID>
        public void AssignAndCommitFoodShipping(IFoodShipping foodShipping)
        {
            if (ShiftWork is ServingShiftWork)
            {
                lock (this)
                {
                    try
                    {
                        using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                        {
                            AssignFoodShipping(foodShipping);
                            if ((foodShipping as FoodShipping).State == ItemPreparationState.Serving && foodShipping.IsAssigned && foodShipping.ShiftWork.Worker == this)
                                (foodShipping as FoodShipping).OnTheRoad();

                            var homeDeliveryServicePoint = ServicesContextRunTime.Current.OpenSessions.Select(x => x.ServicePoint).OfType<ServicesContextResources.HomeDeliveryServicePoint>().Where(x => x.ServicesPointIdentity == foodShipping.ServicesPointIdentity).FirstOrDefault();
                            var activeCouriers = (from shiftWork in ServicesContextRunTime.Current.GetActiveShiftWorks()
                                                  where shiftWork.Worker is ICourier && homeDeliveryServicePoint.CanBeAssignedTo(shiftWork.Worker as ICourier, shiftWork)
                                                  select shiftWork.Worker).OfType<HumanResources.Courier>().ToList();
                            foreach (var activeCourier in activeCouriers)
                            {
                                activeCourier.AuditForDelayedMealAtTheCounter(foodShipping);
                            }



                            Transaction.RunOnTransactionCompleted(() =>
                            {
                                (foodShipping.MealCourse as MealCourse)?.ServingBatchAssigned();
                            });

                            StateMachineMonitoring();
                            stateTransition.Consistent = true;
                        }
                        FindFoodShippingsChanges();
                        StateMachineMonitoring();
                    }
                    catch (Exception error)
                    {
                        throw;
                    }
                }
            }

        }



        /// <MetaDataID>{62b4500f-aeaa-4663-a3dc-248c16c43d10}</MetaDataID>
        internal void FindFoodShippingsChanges()
        {
            //FindServingBatchesChanged
            var foodShippings = (ServicesContextRunTime.Current.MealsController as RoomService.MealsController).GetFoodShippings(this).OfType<IServingBatch>().ToList();
            var assignedFoodShippings = foodShippings.Where(x => x.IsAssigned).ToList();
            var unAssignedFoodShippings = foodShippings.Where(x => !x.IsAssigned).ToList();
            System.Diagnostics.Debug.WriteLine("####   FindFoodShippingsChanges : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff"));
            //System.IO.File.AppendAllText(@"f:\debugOut.txt", "####   FindFoodShippingsChanges : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff")+Environment.NewLine);
            if (assignedFoodShippings.Count != AssignedFoodShippings.Count ||
                unAssignedFoodShippings.Count != FoodShippings.Count)
            {
                _FoodShippingsChanged?.Invoke();
            } else  if (AssignedFoodShippings.Count > 0 && !AssignedFoodShippings.ContainsAll(assignedFoodShippings))
                _FoodShippingsChanged?.Invoke();
            else if (FoodShippings.Count > 0 && !FoodShippings.ContainsAll(unAssignedFoodShippings))
                _FoodShippingsChanged?.Invoke();
        }

        /// <MetaDataID>{a3e17ca5-33b0-4d87-aa73-5ee31252a66d}</MetaDataID>
        public void PrintFoodShippingReceipt(IFoodShipping foodShipping)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{fd795163-663f-4522-8a14-9aeca43cfe50}</MetaDataID>
        object StateMachineLock = new object();

        /// <MetaDataID>{9c20c023-8ddc-41f6-8534-399df325bba5}</MetaDataID>
        private void StateMachineMonitoring()
        {

            if (this.ShiftWork?.IsActive() == true)
            {
                lock (StateMachineLock)
                {
                    if (State == CourierState.Idle || State == CourierState.CollectFoodShiping)
                        State = CourierState.PendingForFoodShiping;

                    if (this.ShiftWork == null || (this.ShiftWork as IServingShiftWork)?.ServingBatches.Count == 0)
                        State = CourierState.PendingForFoodShiping;


                    bool someAtTheCounter = (this.ShiftWork as IServingShiftWork).ServingBatches.OfType<FoodShipping>().Any(x => x.State == ItemPreparationState.Serving);
                    bool allOntheRoad = (this.ShiftWork as IServingShiftWork).ServingBatches.Count() > 0 && (this.ShiftWork as IServingShiftWork).ServingBatches.OfType<FoodShipping>().All(x => x.State == ItemPreparationState.OnRoad);
                    if (someAtTheCounter)
                        State = CourierState.CollectFoodShiping;
                    else if (allOntheRoad)
                    {

                        var onTheRoadTimespan = DateTime.UtcNow - (this.ShiftWork as IServingShiftWork).ServingBatches.SelectMany(x => x.PreparedItems).OrderBy(x => x.StateTimestamp).Last().StateTimestamp.ToUniversalTime();
                        if (onTheRoadTimespan.TotalMinutes > 2)
                        {
                            State = CourierState.OnTheRoad;
                        }
                        else
                        {
                            State = CourierState.CollectFoodShiping;
                            Delay.Do((TimeSpan.FromMinutes(2) - onTheRoadTimespan).TotalMilliseconds /*in ms*/, () =>
                            {
                                allOntheRoad = (this.ShiftWork as IServingShiftWork).ServingBatches.Count() > 0 && (this.ShiftWork as IServingShiftWork).ServingBatches.OfType<FoodShipping>().All(x => x.State == ItemPreparationState.OnRoad);
                                if (allOntheRoad)
                                {


                                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                                    {
                                        State = CourierState.OnTheRoad;

                                        EnsureCommonDistributionForOnRoadFoodShiping();
                                        stateTransition.Consistent = true;
                                    }


                                }
                                onTheRoadTimespan = DateTime.UtcNow - (this.ShiftWork as IServingShiftWork).ServingBatches.SelectMany(x => x.PreparedItems).OrderBy(x => x.StateTimestamp).Last().StateTimestamp.ToUniversalTime();
                                /* Do somthing */
                            });
                        }
                    }
                }

            }
            else
            {
                lock (StateMachineLock)
                    State = CourierState.Idle;
            }


        }

        /// <MetaDataID>{9d292119-46ee-4b16-b185-a55a9750f602}</MetaDataID>
        private void EnsureCommonDistributionForOnRoadFoodShiping()
        {
#if DEBUG
            var foodShippingsStates = (this.ShiftWork as IServingShiftWork).ServingBatches.OfType<FoodShipping>().Select(s => s.State).ToArray();
#endif

            var ticks = new DateTime(2022, 1, 1).Ticks;
            var uniqueId = (DateTime.Now.Ticks - ticks).ToString("x");
            foreach (var foodShipping in (this.ShiftWork as IServingShiftWork).ServingBatches.OfType<FoodShipping>().Where(x => x.State == ItemPreparationState.OnRoad && string.IsNullOrWhiteSpace(x.DistributionIdentity)))
                foodShipping.DistributionIdentity = uniqueId;
        }

        /// <MetaDataID>{e8881f39-2f46-4b4a-b1ca-237ecea8a5c1}</MetaDataID>
        [ObjectActivationCall]
        internal void OnActivated()
        {
            StateMachineMonitoring();
        }





        /// <exclude>Excluded</exclude>
        CourierState _State;
        /// <MetaDataID>{eedff8e1-ca31-41d6-a954-01ed1e0fbe0c}</MetaDataID>
        [PersistentMember(nameof(_State))]
        [BackwardCompatibilityID("+19")]

        public CourierState State
        {
            get => _State;
            private set
            {

                if (_State != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {


                        _State = value;
                        StateTimestamp = DateTime.UtcNow;
                        stateTransition.Consistent = true;
                    }
                    Transaction.RunOnTransactionCompleted(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(State));

                    });
                }

            }

        }
        /// <MetaDataID>{e29e812d-3ff8-4aee-9bd7-abdfb801024d}</MetaDataID>
        [OnDemandCachingDataOnClientSide]
        public IHomeDeliveryServicePoint HomeDeliveryServicePoint => ServicePointRunTime.ServicesContextRunTime.Current.DeliveryServicePoint;
    }



}