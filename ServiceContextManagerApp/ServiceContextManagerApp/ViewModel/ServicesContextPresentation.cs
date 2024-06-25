using FlavourBusinessFacade;

using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FlavourBusinessFacade.HumanResources;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService.ViewModel;
using FlavourBusinessFacade.ServicesContextResources;
using System.Threading.Tasks;
using ServiceContextManagerApp.ViewModel;
using FlavourBusinessFacade.EndUsers;

using FlavourBusinessFacade.Shipping;
using UIBaseEx;
using CourierApp.ViewModel;

using OOAdvantech;
using System.Linq.Expressions;
using OOAdvantech.Remoting.RestApi;
using Xamarin.Forms;
using System.Windows.Controls;
using System.Security.Principal;
using FlavourBusinessFacade.Print;
using FlavourBusinessManager.ServicesContextResources;
using FlavourBusinessFacade.Printing;














#if DeviceDotNet
using MarshalByRefObject = OOAdvantech.Remoting.MarshalByRefObject;
using ZXing;
using WaiterApp.ViewModel;
#else
using FlavourBusinessManager.Shipping;
using MarshalByRefObject = System.MarshalByRefObject;
using System.Drawing.Imaging;
using QRCoder;
using System.IO;
using System;
using FlavourBusinessManager.HumanResources;
using CourierApp.ViewModel;
#endif


namespace ServiceContextManagerApp
{



    /// <MetaDataID>{43d3b2e4-4576-47ec-bf5f-6ad3bb87aa57}</MetaDataID>
    public class ServicesContextPresentation : MarshalByRefObject, INotifyPropertyChanged, IServicesContextPresentation, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{4bac94b5-4113-4921-b7c7-eb6a21ce2889}</MetaDataID>
        internal IFlavoursServicesContext ServicesContext;

        /// <MetaDataID>{c30df083-03ce-4bad-93d2-54776257f64f}</MetaDataID>
        public string ServicesContextName { get => ServicesContext.Description; set { } }

        public static OOAdvantech.SerializeTaskScheduler SerializeTaskScheduler
        {
            get
            {
                return AppLifeTime.SerializeTaskScheduler;
            }
        }

        static IAppLifeTime AppLifeTime
        {
            get
            {
#if DeviceDotNet
                return Xamarin.Forms.Application.Current as IAppLifeTime;
#else
                return System.Windows.Application.Current as IAppLifeTime;
#endif
            }
        }


        /// <exclude>Excluded</exclude>
        List<IWaiterPresentation> _Waiters;
        /// <MetaDataID>{774737b1-2f37-4b4f-ac97-7fa4fcf58b24}</MetaDataID>
        public List<IWaiterPresentation> Waiters
        {
            get
            {
                if (_Waiters == null && ServicesContext != null)
                {
                    _Waiters = ServicesContext.ServiceContextHumanResources.Waiters.Select(x => new WaiterPresentation(x, ServicesContextRuntime)).OfType<IWaiterPresentation>().ToList();

                    //var administrator = _Waiters.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
                    //if (administrator != null)
                    //{
                    //    _Waiters.Remove(administrator);
                    //    _Waiters.Insert(0, administrator);
                    //}
                    return _Waiters;
                }
                else if (_Waiters != null)
                    return _Waiters;
                else
                    return new List<IWaiterPresentation>();
            }
        }

        /// <exclude>Excluded</exclude>
        List<ITakeawayCashierPresentation> _TakeawayCashiers;

        /// <MetaDataID>{17dd7c37-a140-43ec-b66e-365b5bd675ed}</MetaDataID>
        public List<ITakeawayCashierPresentation> TakeawayCashiers
        {
            get
            {
                if (_TakeawayCashiers == null && ServicesContext != null)
                {
                    _TakeawayCashiers = ServicesContext.ServiceContextHumanResources.TakeawayCashiers.Select(x => new TakeawayCashierPresentation(x, ServicesContextRuntime)).OfType<ITakeawayCashierPresentation>().ToList();

                    return _TakeawayCashiers;
                }
                else if (_TakeawayCashiers != null)
                    return _TakeawayCashiers;
                else
                    return new List<ITakeawayCashierPresentation>();
            }
        }



        /// <MetaDataID>{bb909b78-942c-44e6-b13c-48e90ac84504}</MetaDataID>
        List<ICourierPresentation> _Couriers;

        /// <MetaDataID>{8489002c-9e5c-4d88-8b84-f20d062b022c}</MetaDataID>
        public List<ICourierPresentation> Couriers
        {
            get
            {
                if (_Couriers == null && ServicesContext != null)
                {
                    _Couriers = ServicesContext.ServiceContextHumanResources.Couriers.Select(x => new CourierPresentation(x, ServicesContextRuntime)).OfType<ICourierPresentation>().ToList();

                    return _Couriers;
                }
                else if (_Couriers != null)
                    return _Couriers;
                else
                    return new List<ICourierPresentation>();
            }
        }




        /// <MetaDataID>{6ff67af0-81de-49b0-895d-4ec537422ea4}</MetaDataID>
        ViewModelWrappers<IServiceContextSupervisor, SupervisorPresentation> _Supervisors = new ViewModelWrappers<IServiceContextSupervisor, SupervisorPresentation>();

        /// <MetaDataID>{627e4712-a41e-489e-8677-d82d25ad0fda}</MetaDataID>
        public List<ISupervisorPresentation> Supervisors
        {
            get
            {
                if (_Supervisors == null && ServicesContext != null)
                {
                    _Supervisors=new ViewModelWrappers<IServiceContextSupervisor, SupervisorPresentation>();
                    ServicesContext.ServiceContextHumanResources.Supervisors.Select(x => _Supervisors.GetViewModelFor(x, x, ServicesContextRuntime)).OfType<ISupervisorPresentation>().ToList();

                    var signedInSupervisor = SignedInSupervisor;

                    if (_SignedInSupervisor != null)
                    {
                        (SignedInSupervisor as SupervisorPresentation).GetActiveShiftWork();
                        var inActiveShiftWork = SignedInSupervisor.InActiveShiftWork;
                        if (inActiveShiftWork)
                            init();
                        else
                            SignedInSupervisor.ObjectChangeState += ServicesContextPresentation_ObjectChangeState;
                    }
                    else
                        init();

                    return _Supervisors.Values.OfType<ISupervisorPresentation>().ToList();
                }
                else if (_Supervisors != null)
                    return _Supervisors.Values.OfType<ISupervisorPresentation>().ToList();
                else
                    return new List<ISupervisorPresentation>();



            }
        }




        public async Task<bool> AssignFoodShipping(string foodShippingIdentity, IWorkerPresentation worker)
        {
            CourierPresentation courierPresentation = worker as CourierPresentation;

            System.Diagnostics.Debug.WriteLine("####   AssignFoodShipping : " + System.DateTime.UtcNow.ToString());
            //System.IO.File.AppendAllText(@"f:\debugOut.txt", "####   AssignFoodShipping : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff")+Environment.NewLine);

            var foodShippings = FoodShippings.Values.OrderBy(x => x.FoodShipping.SortID).ToList();
            var foodShipping = foodShippings.Where(x => x.ServiceBatchIdentity == foodShippingIdentity).FirstOrDefault();

            if (foodShipping != null)
            {

                return await SerializeTaskScheduler.AddTask(async () =>
                {
                    int tries = 30;
                    while (tries > 0)
                    {
                        try
                        {

                            courierPresentation.Courier.AssignAndCommitFoodShipping(foodShipping.FoodShipping);
                            return true;
                        }
                        catch (System.Net.WebException commError)
                        {
                            await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(1));
                        }
                        catch (System.Exception error)
                        {
                            var er = error;
                            throw;

                        }
                    }
                    return true;

                });

            }

            return false;
        }


        public IPrintManager PrintManager { get => this.ServicesContextRuntime?.PrintManager; }

        public IList<IPreparationStation> PreparationStations
        {
            get
            {
                List<IPreparationStation> preparationStations = new List<IPreparationStation>();

                foreach (var station in this.ServicesContextRuntime.PreparationStations)
                {
                    preparationStations.Add(station);
                    foreach (var subStation in station.SubStations)
                    {
                        if (!preparationStations.Contains(subStation))
                            preparationStations.Add(subStation);
                    }
                }
                return preparationStations;
            }
        }

        public async Task<bool> RemoveFoodShippingAssignment(string foodShippingIdentity)
        {

            System.Diagnostics.Debug.WriteLine("####   RemoveFoodShippingAssignment : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff"));
            //System.IO.File.AppendAllText(@"f:\debugOut.txt", "####   RemoveFoodShippingAssignment : " + System.DateTime.UtcNow.ToString("hh.mm.ss.fffffff")+Environment.NewLine);

            var foodShippings = FoodShippings.Values.OrderBy(x => x.FoodShipping.SortID).ToList();
            var foodShipping = foodShippings.Where(x => x.ServiceBatchIdentity == foodShippingIdentity).FirstOrDefault();

            if (foodShipping != null)
            {

                try
                {
                    return await SerializeTaskScheduler.AddTask(async () =>
                       {
                           int tries = 30;
                           while (tries > 0)
                           {
                               try
                               {
                                   var courier = RemotingServices.CastTransparentProxy<ICourier>(foodShipping.FoodShipping.ShiftWork.Worker);
                                   if (courier != null)
                                       courier.RemoveFoodShippingAssignment(foodShipping.FoodShipping);
                                   return true;
                               }
                               catch (System.Net.WebException commError)
                               {
                                   await System.Threading.Tasks.Task.Delay(System.TimeSpan.FromSeconds(1));
                               }
                               catch (System.InvalidOperationException error)
                               {
                                   throw;
                               }
                               catch (System.Exception error)
                               {
                                   var er = error;
                                   throw;
                               }
                           }
                           return true;

                       });
                }
                catch (System.Exception error)
                {

                    throw;
                }
            }


            return false;
        }


        public void ShiftWorkStart(IWorkerPresentation worker, System.DateTime startedAt, double timespanInHours)
        {
            worker.NewShiftWork(startedAt, timespanInHours);
        }



        public event MealCoursesUpdatedHandle MealCoursesUpdated;

        /// <MetaDataID>{65515374-781c-4324-a6bd-6c652b62fc4f}</MetaDataID>
        internal void OnMealCourseUpdated(MealCourse mealCourse)
        {
            if (mealCourse.FoodItemsInProgress.Count == 0)
                _ObjectChangeState?.Invoke(this, nameof(MealCoursesInProgress));
            else
                MealCoursesUpdated?.Invoke(new List<MealCourse>() { mealCourse });
        }

        public event FlavourBusinessFacade.EndUsers.ItemsStateChangedHandle ItemsStateChanged;
        /// <MetaDataID>{a59bdd94-b1ac-4980-a537-41e91934db63}</MetaDataID>
        internal void OnItemsStateChanged(Dictionary<string, ItemPreparationState> newItemsState)
        {
            ItemsStateChanged?.Invoke(newItemsState);
        }




        //IFlavoursServicesContextRuntime _FlavoursServicesContextRuntime;
        //IFlavoursServicesContextRuntime FlavoursServicesContextRuntime
        //{
        //    get
        //    {
        //        if (_FlavoursServicesContextRuntime == null)
        //            _FlavoursServicesContextRuntime = ServicesContext.GetRunTime();

        //        return _FlavoursServicesContextRuntime;
        //    }
        //}
        /// <MetaDataID>{d14ea68e-a1e9-4729-b2cb-f1add46cff2a}</MetaDataID>
        public bool RemoveSupervisor(ISupervisorPresentation supervisorPresentation)
        {

            var administrator = _Supervisors.Values.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
            if (supervisorPresentation == administrator)
                return false;

            bool removed = ServicesContextRuntime.RemoveSupervisor((supervisorPresentation as SupervisorPresentation).Supervisor);

            if (removed)
                _Supervisors.Remove((supervisorPresentation as SupervisorPresentation).Supervisor);

            return removed;

        }


        /// <MetaDataID>{8bd9063b-ef6f-4fa3-9af9-6787961f3b0d}</MetaDataID>
        public void MoveBefore(string mealCourseUri, string movedMealCourseUri)
        {
            var mealCourse = this.MealCoursesInProgress.Where(x => x.MealCourseUri == mealCourseUri).FirstOrDefault();
            var movedMealCourse = this.MealCoursesInProgress.Where(x => x.MealCourseUri == movedMealCourseUri).FirstOrDefault();
            var description = mealCourse.Description;
            MealsController.MoveCourseBefore(mealCourseUri, movedMealCourseUri);
        }
        /// <MetaDataID>{d5425607-f1e3-4b54-87d9-aa1ba38d98da}</MetaDataID>
        public void MoveAfter(string mealCourseUri, string movedMealCourseUri)
        {
            var mealCourse = this.MealCoursesInProgress.Where(x => x.MealCourseUri == mealCourseUri).FirstOrDefault();
            var movedMealCourse = this.MealCoursesInProgress.Where(x => x.MealCourseUri == movedMealCourseUri).FirstOrDefault();
            var description = mealCourse.Description;
            MealsController.MoveCourseAfter(mealCourseUri, movedMealCourseUri);



        }


        public ISupervisorPresentation SignedInSupervisor
        {
            get
            {

                if (_SignedInSupervisor == null)
                    return null;
                var signedInSupervisor = Supervisors.Where(x => x.SupervisorIdentity == AdministratorIdentity).First();

                return signedInSupervisor;

            }
        }

        /// <MetaDataID>{e16b4d6b-81e8-425f-91e2-087c50ab527f}</MetaDataID>
        public void MakeSupervisorActive(ISupervisorPresentation supervisorPresentation)
        {
            var administrator = _Supervisors.Values.Where(x => x.SupervisorIdentity == AdministratorIdentity).FirstOrDefault();
            if (supervisorPresentation != administrator)
                ServicesContextRuntime.MakeSupervisorActive((supervisorPresentation as SupervisorPresentation).Supervisor);
        }

        /// <MetaDataID>{e1567326-c4f7-4c4f-b5d3-0dc851eddec7}</MetaDataID>
        IServiceContextSupervisor _SignedInSupervisor;


        /// <MetaDataID>{eff5994f-4dc3-461e-8a17-a0d7a1e9a787}</MetaDataID>
        public IFlavoursServicesContextRuntime ServicesContextRuntime { get; private set; }
        /// <MetaDataID>{f87f95f7-c34f-4baf-a972-a2e082ac98d8}</MetaDataID>
        public IMealsController MealsController { get; private set; }
        /// <MetaDataID>{0019d0de-61a8-45ab-9726-7fa37d0cc972}</MetaDataID>
        public List<MealCourse> MealCoursesInProgress
        {
            get
            {
                return _MealCoursesInProgress.Values.ToList();
            }
        }

        /// <MetaDataID>{0a39cf32-9393-4efe-b375-1d09a0dd1ba6}</MetaDataID>
        ViewModelWrappers<IMealCourse, MealCourse> _MealCoursesInProgress = new UIBaseEx.ViewModelWrappers<IMealCourse, MealCourse>();
        /// <MetaDataID>{24fad512-b507-4fa6-809b-9a1ec434d852}</MetaDataID>
        string AdministratorIdentity;
        /// <MetaDataID>{e203631b-ddea-42fb-be21-3e7098466561}</MetaDataID>
        public ServicesContextPresentation(IFlavoursServicesContext servicesContext, IServiceContextSupervisor signedInSupervisor)
        {
            AdministratorIdentity = "";
            _SignedInSupervisor = signedInSupervisor;
            if (_SignedInSupervisor != null)
                AdministratorIdentity = _SignedInSupervisor.Identity;
            ServicesContext = servicesContext;

            ServicesContextRuntime = ServicesContext.GetRunTime();
            _Supervisors.GetViewModelFor(signedInSupervisor, signedInSupervisor, ServicesContextRuntime);
            if (_SignedInSupervisor != null)
            {
                (SignedInSupervisor as SupervisorPresentation).GetActiveShiftWork();
                var inActiveShiftWork = SignedInSupervisor.InActiveShiftWork;
                if (inActiveShiftWork)
                    init();
                else
                    SignedInSupervisor.ObjectChangeState += ServicesContextPresentation_ObjectChangeState;
            }
            else
                init();


        }


        string SupervisorComunicationChannelUri;
        private void MonitorSupervisorComunicationChannel()
        {
            if (_SignedInSupervisor != null)
            {
                var supervisorComunicationChannelUri = OOAdvantech.Remoting.RemotingServices.GetChannelUri(_SignedInSupervisor);
                if (SupervisorComunicationChannelUri != supervisorComunicationChannelUri)
                {
                    if (!string.IsNullOrWhiteSpace(SupervisorComunicationChannelUri))
                        OOAdvantech.Remoting.RestApi.Connectivity.GetConnectivity(SupervisorComunicationChannelUri).ConnectivityChanged -= ServicesContextPresentation_ConnectivityChanged;

                    SupervisorComunicationChannelUri = supervisorComunicationChannelUri;
                    if (!string.IsNullOrWhiteSpace(SupervisorComunicationChannelUri))
                        OOAdvantech.Remoting.RestApi.Connectivity.GetConnectivity(SupervisorComunicationChannelUri).ConnectivityChanged += ServicesContextPresentation_ConnectivityChanged;

                }
            }
        }

        private void ServicesContextPresentation_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var networkAccess = e.ToString();
            var channelState = e.ComunicationChannelConectivity?.ChannelState;
#if DeviceDotNet
            OOAdvantech.IDeviceOOAdvantechCore device = DependencyService.Get<OOAdvantech.IDeviceInstantiator>().GetDeviceSpecific(typeof(OOAdvantech.IDeviceOOAdvantechCore)) as OOAdvantech.IDeviceOOAdvantechCore;
#endif
            if (channelState == ChannelState.Connecting)
            {
#if DeviceDotNet
                if (device.StatusBarColor == Color.DarkViolet)
                    device.StatusBarColor = Color.LightSalmon;
#endif
            }
            else if (channelState == ChannelState.Open)
            {
#if DeviceDotNet
                device.StatusBarColor = Color.LightGreen;
#endif

            }
            else
            {
#if DeviceDotNet
                device.StatusBarColor = Color.DarkViolet;
#endif

            }
        }
        bool ServicesContextPresentationInittialized = false;

        private void init()
        {

            if (_SignedInSupervisor != null)
            {
                _SignedInSupervisor.MessageReceived -= SignedInSupervisor_MessageReceived;
                _SignedInSupervisor.MessageReceived += SignedInSupervisor_MessageReceived;
            }

            if (!ServicesContextPresentationInittialized)
            {
                // using (var remoteCallContext = new RemoteCallContext(System.TimeSpan.FromMinutes(5), System.TimeSpan.FromMinutes(5)))
                {


                    try
                    {
                        if (ServicesContextRuntime == null)
                            ServicesContextRuntime = ServicesContext.GetRunTime();
                        (ServicesContextRuntime as ITransparentProxy).Reconnected += ServicesContextPresentation_Reconnected;
                        this.ServicesContextRuntime.ObjectChangeState += ServicesContext_ObjectChangeState;
                        MealsController = this.ServicesContextRuntime.MealsController;

                        _MealCoursesInProgress.OnNewViewModelWrapper += MealCoursesInProgress_OnNewViewModelWrapper;
                        GetServiceContextPresentationData();
                        ServicesContextPresentationInittialized = true;
                    }
                    catch (System.Exception error)
                    {
                        throw;
                    }

                }

            }
        }


        private void GetServiceContextPresentationData(bool reconnectedToServer = false)
        {
            MealsController.NewMealCoursesInProgress += MealsController_NewMealCoursesInrogress;
            MealsController.ObjectChangeState += MealsController_ObjectChangeState;
            MealsController.MealCourseChangeState += MealsController_MealCourseChangeState;
            Task.Run(() =>
            {
                //System.Threading.Thread.Sleep(9000);

                string param = MealsController.ToString() + "asdas";

                var clientSideMealCourses = _MealCoursesInProgress.Keys.Select(x => new MealCourseAbbreviation() { Identity = x.Identity, TimeStamp = x.StateTimestamp }).ToList();

                var userLanguageCode = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
                var mealCoursesInProgress = MealsController.Fetching(mc => mc.GetMealCoursesInProgress(clientSideMealCourses, userLanguageCode).Caching(mealCourses => mealCourses.Select(mealCourse => new
                {
                    mealCourse.Name,
                    mealCourse.ServingBatches,
                    mealCourse.StateTimestamp,
                    mealCourse.PartiallyUnderServingProcess,
                    mealCourse.UnderServingProcess,
                    //mealCourse.Meal,
                    FoodItemsInProgress = mealCourse.FoodItemsInProgress.Select(itemsContext => new
                    {
                        itemsContext.MealCourse,
                        itemsContext.Description,
                        itemsContext.PreparationState,
                        itemsContext.PreparationItems

                    })
                })));


                //MealCoursesUpdated?.Invoke(new List<MealCourse>() { mealCourse });

                var mealCoursePresentations = mealCoursesInProgress.Select(x => _MealCoursesInProgress.GetViewModelFor(x, () => { return new MealCourse(x, MealsController); })).
                Where(x => x.StateTimestamp != x.ServerSideMealCourse.StateTimestamp).Select(x => x.MealCourseUpdate(x.ServerSideMealCourse)).ToList();

                if (reconnectedToServer)
                    MealCoursesUpdated?.Invoke(mealCoursePresentations);
                else
                    _ObjectChangeState?.Invoke(this, nameof(MealCoursesInProgress));

                DelayedServingBatchesAtTheCounter = MealsController.GetDelayedServingBatchesAtTheCounter(4);
                _ObjectChangeState?.Invoke(this, nameof(DelayedServingBatchesAtTheCounter));
                GetMessages();

            });
        }

        private void ServicesContextPresentation_Reconnected(object sender)
        {

            MealsController.NewMealCoursesInProgress -= MealsController_NewMealCoursesInrogress;
            MealsController.ObjectChangeState -= MealsController_ObjectChangeState;
            MealsController.MealCourseChangeState -= MealsController_MealCourseChangeState;

            MealsController = ServicesContextRuntime.MealsController;
            _Waiters = null;
            _Couriers = null;
            _Supervisors = null;
            _TakeawayCashiers = null;
            _Supervisors = null;
            GetServiceContextPresentationData(true);
        }

        private void MealsController_MealCourseChangeState(IMealCourse mealCourser, string memberName)
        {


            if (DelayedServingBatchesAtTheCounter?.Count > 0 == true && memberName == nameof(MealCourse.PreparationState) && mealCourser?.PreparationState == ItemPreparationState.OnRoad)
            {
                DelayedServingBatchesAtTheCounter = MealsController.GetDelayedServingBatchesAtTheCounter(4);
                _ObjectChangeState?.Invoke(this, nameof(DelayedServingBatchesAtTheCounter));
            }
        }

        public void IWillTakeCare(string messageID)
        {
            _SignedInSupervisor.IWillTakeCare(messageID);
        }


        public void MessageHasBeenRead(string messageID)
        {
            _SignedInSupervisor.RemoveMessage(messageID);
        }

        public event DelayedMealAtTheCounterHandle DelayedMealAtTheCounter;

        public event MealConversationTimeExceededHandle MealConversationTimeExceeded;
        object MessagesLock = new object();
        private void GetMessages()
        {
            lock (MessagesLock)
            {
                if (_SignedInSupervisor != null)
                {
                    var message = _SignedInSupervisor.PeekMessage();
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.DelayedMealAtTheCounter)
                    {
                        if ((System.DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 20)
                            _SignedInSupervisor.RemoveMessage(message.MessageID);
                        else
                        {
                            if (message != null && SignedInSupervisor?.InActiveShiftWork == true)
                            {
                                DelayedServingBatchesAtTheCounter = MealsController.GetDelayedServingBatchesAtTheCounter(4);
                                DelayedMealAtTheCounter?.Invoke(SignedInSupervisor, message.MessageID);
                                
                                return;
                            }
                        }


                    }
                    if (message != null && message.GetDataValue<ClientMessages>("ClientMessageType") == ClientMessages.MealConversationTimeout)
                    {
                        if ((System.DateTime.UtcNow - message.MessageTimestamp.ToUniversalTime()).TotalMinutes > 20)
                            _SignedInSupervisor.RemoveMessage(message.MessageID);
                        else
                        {
                            string servicesPointIdentity = message.GetDataValue<string>("ServicesPointIdentity");
                            
                            if (message != null && SignedInSupervisor?.InActiveShiftWork == true)
                            {

                                MealConversationTimeExceeded?.Invoke(SignedInSupervisor, message.MessageID, servicesPointIdentity);
                                
                                return;
                            }
                        }


                    }
                }
            }
        }



            private void SignedInSupervisor_MessageReceived(FlavourBusinessFacade.EndUsers.IMessageConsumer sender)
            {

                GetMessages();

            }

            /// <MetaDataID>{f71132d5-0dac-4929-82bc-03294e24dc21}</MetaDataID>
            private void MealCoursesInProgress_OnNewViewModelWrapper(UIBaseEx.ViewModelWrappers<IMealCourse, MealCourse> sender, IMealCourse key, MealCourse value)
            {

                value.MealCourseUpdated += OnMealCourseUpdated;
                value.ItemsStateChanged += OnItemsStateChanged;
            }




            /// <MetaDataID>{b8625da6-194e-4943-b4ee-5c4a434741cc}</MetaDataID>
            private void MealsController_ObjectChangeState(object _object, string member)
            {



                if (member == nameof(IMealsController.MealCoursesInProgress))
                {
                    foreach (var mealCourseInProgress in _MealCoursesInProgress.Values)
                    {
                        mealCourseInProgress.MealCourseUpdated -= OnMealCourseUpdated;
                        mealCourseInProgress.ItemsStateChanged -= OnItemsStateChanged;
                    }

                    _MealCoursesInProgress.Clear();

                    MealsController.MealCoursesInProgress.Select(x => _MealCoursesInProgress.GetViewModelFor(x, x, MealsController)).ToList();
                    _ObjectChangeState?.Invoke(this, nameof(IMealsController.MealCoursesInProgress));
                }
            }

            /// <MetaDataID>{58f876a1-ba32-41f9-9161-0aaf1efa07f8}</MetaDataID>
            private void MealsController_NewMealCoursesInrogress(IList<IMealCourse> mealCoursers)
            {
                mealCoursers.Select(x => _MealCoursesInProgress.GetViewModelFor(x, x, MealsController)).ToList();
                _ObjectChangeState?.Invoke(this, nameof(MealCoursesInProgress));
            }

            /// <MetaDataID>{e6a39536-5932-4a67-ac6b-4f303a3da563}</MetaDataID>
            List<IHallLayout> _Halls;
        /// <MetaDataID>{aa3fa3fd-8063-48af-8492-4425a1effc7c}</MetaDataID>
        public IList<IHallLayout> Halls
        {
            get
            {
                if (_Halls == null)
                {

                    _Halls = this.ServicesContextRuntime?.Halls.ToList();
                    foreach (var hall in this._Halls)
                    {
                        hall.FontsLink = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
                        (hall as RestaurantHallLayoutModel.HallLayout).SetShapesImagesRoot("https://angularhost.z16.web.core.windows.net/halllayoutsresources/Shapes/");
                        (hall as RestaurantHallLayoutModel.HallLayout).ServiceArea.ServicePointChangeState += ServiceArea_ServicePointChangeState;
                    }
                }

                return _Halls;
            }
        }

        public List<DelayedServingBatchAbbreviation> DelayedServingBatchesAtTheCounter { get; private set; }

        public event ServicePointChangeStateHandle ServicePointChangeState;

        ViewModelWrappers<IFoodShipping, FoodShippingPresentation> FoodShippings = new ViewModelWrappers<IFoodShipping, FoodShippingPresentation>();
        ViewModelWrappers<IServingBatch, WaiterApp.ViewModel.ServingBatchPresentation> ServingBatches = new ViewModelWrappers<IServingBatch, WaiterApp.ViewModel.ServingBatchPresentation>();

        public FoodShippingPresentation GetFoodShipping(DelayedServingBatchAbbreviation delayedServingBatch)
        {
            if (OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFoodShipping>(delayedServingBatch.ServingBatch) is IFoodShipping && (delayedServingBatch.SessionType == SessionType.HomeDelivery || delayedServingBatch.SessionType == SessionType.HomeDeliveryGuest))
                return FoodShippings.GetViewModelFor(OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFoodShipping>(delayedServingBatch.ServingBatch), OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFoodShipping>(delayedServingBatch.ServingBatch));
            throw new System.ArgumentException("servingBatch is not food shipping");
        }
        public WaiterApp.ViewModel.ServingBatchPresentation GetServingBatch(DelayedServingBatchAbbreviation delayedServingBatch)
        {
            if (delayedServingBatch.SessionType == SessionType.Hall)
                return ServingBatches.GetViewModelFor(delayedServingBatch.ServingBatch, delayedServingBatch.ServingBatch);

            return null;
        }


        public Task<FoodShippingPresentation> GetMealCourseFoodShipping(string mealCourseUri)
        {
            return Task<FoodShippingPresentation>.Run(() =>
            {
                var mealCourse = MealCoursesInProgress.Where(x => x.MealCourseUri == mealCourseUri).FirstOrDefault();
                if (mealCourse.SessionType == SessionType.HomeDelivery)
                {
                    //mealCourse.ServerSideMealCourse.PreparationState
                    IFoodShipping foodShipping = MealsController.Fetching(mc => mc.GetMealCourseFoodShipping(mealCourseUri).Caching(_foodShipping => new
                    {
                        _foodShipping.ClientFullName,
                        _foodShipping.PhoneNumber,
                        _foodShipping.MealCourseUri,
                        _foodShipping.Identity,
                        _foodShipping.MealCourse,
                        _foodShipping.DeliveryRemark,
                        _foodShipping.NotesForClient,
                        _foodShipping.ServicePoint
                    }));
                    if (foodShipping == null)
                        return null;

                    FoodShippingPresentation foodShippingPresentation = FoodShippings.GetViewModelFor(foodShipping, foodShipping);
                    if (foodShipping.ShiftWork != null && foodShippingPresentation.Courier == null)
                    {
                        var siftWorkCourier = RemotingServices.CastTransparentProxy<ICourier>(foodShipping.ShiftWork?.Worker);
                        foodShippingPresentation.Courier = this.Couriers.OfType<CourierPresentation>().Where(x => x.Courier == siftWorkCourier).FirstOrDefault();

                    }
                    if (foodShipping.ShiftWork == null && foodShippingPresentation.Courier != null)
                        foodShippingPresentation.Courier = null;

                    return foodShippingPresentation;

                }
                return null;
            });



        }



        /// <MetaDataID>{9c93fbca-e49d-45f4-9077-72d58f986f89}</MetaDataID>
        private void ServiceArea_ServicePointChangeState(object _object, IServicePoint servicePoint, ServicePointState newState)
        {
            this.ServicePointChangeState?.Invoke(this, servicePoint.ServicesPointIdentity, newState);

            var shape = (from hall in _Halls.OfType<RestaurantHallLayoutModel.HallLayout>()
                         from hallShape in hall.Shapes
                         where hallShape.ServicesPointIdentity == servicePoint.ServicesPointIdentity
                         select hallShape).FirstOrDefault();

            if (shape != null)
                shape.ServicesPointState = newState;

        }

        private void ServicesContextPresentation_ObjectChangeState(object _object, string member)
        {
            if (member == "ShiftWork")
            {
                if ((_object as SupervisorPresentation).InActiveShiftWork)
                    init();
            }
        }


        /// <MetaDataID>{084e9ed1-2d32-40f6-bf87-6d69c0c782aa}</MetaDataID>
        private void ServicesContext_ObjectChangeState(object _object, string member)
        {

            var serviceContextHumanResources = ServicesContext.ServiceContextHumanResources;
            //_Supervisors = serviceContextHumanResources.Supervisors.Select(x => new SupervisorPresentation(x, ServicesContextRuntime)).OfType<ISupervisorPresentation>().ToList();



            serviceContextHumanResources.Supervisors.Select(x => _Supervisors.GetViewModelFor(x, x, ServicesContextRuntime)).OfType<ISupervisorPresentation>().ToList();
            _ObjectChangeState?.Invoke(this, nameof(Supervisors));

            _Waiters = serviceContextHumanResources.Waiters.Select(x => new WaiterPresentation(x, ServicesContextRuntime)).OfType<IWaiterPresentation>().ToList();
            _ObjectChangeState?.Invoke(this, nameof(Waiters));

            _TakeawayCashiers = serviceContextHumanResources.TakeawayCashiers.Select(x => new TakeawayCashierPresentation(x, ServicesContextRuntime)).OfType<ITakeawayCashierPresentation>().ToList();
            _ObjectChangeState?.Invoke(this, nameof(TakeawayCashiers));

            _Couriers = serviceContextHumanResources.Couriers.Select(x => new CourierPresentation(x, ServicesContextRuntime)).OfType<ICourierPresentation>().ToList();
            _ObjectChangeState?.Invoke(this, nameof(Couriers));




        }

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event OOAdvantech.ObjectChangeStateHandle _ObjectChangeState;

        [OOAdvantech.MetaDataRepository.HttpInVisible]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState
        {
            add
            {
                _ObjectChangeState += value;
            }
            remove
            {
                _ObjectChangeState -= value;
            }
        }

        /// <MetaDataID>{99857176-697b-4410-8dd7-5abe58705592}</MetaDataID>
        public IWaiter AssignWaiterNativeUser(string waiterAssignKey, string userName, string password, string userFullName)
        {
            return ServicesContextRuntime.AssignWaiterNativeUser(waiterAssignKey, userName, password, userFullName);
        }


        /// <MetaDataID>{e40706f4-fbc7-4e8e-ad98-99c498635ef7}</MetaDataID>
        public ITakeawayCashier AssignTakeAwayCashierNativeUser(string takeAwayCashierAssignKey, string userName, string password, string userFullName)
        {
            return ServicesContextRuntime.AssignTakeAwayCashierNativeUser(takeAwayCashierAssignKey, userName, password, userFullName);
        }

        /// <MetaDataID>{4ba14714-e524-41b7-83a5-bcdad7490f90}</MetaDataID>
        public ICourier AssignCourierNativeUser(string courierAssignKey, string userName, string password, string userFullName)
        {
            return ServicesContextRuntime.AssignCourierNativeUser(courierAssignKey, userName, password, userFullName);
        }

        /// <MetaDataID>{90f9110e-6717-4a51-abce-ad205981bf6c}</MetaDataID>
        public NewUserCode GetNewWaiterQRCode(string color)
        {


            string codeValue = ServicesContextRuntime.NewWaiter();
            string SigBase64 = "";
#if DeviceDotNet
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };


            var bitmapMatrix = barcodeWriter.Encode(codeValue);
            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

            SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
            if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                fgColor = SkiaSharp.SKColors.Black;

            var pixels = qrCodeImage.Pixels;
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixels[k++] = fgColor;
                    else
                        pixels[k++] = SkiaSharp.SKColors.White;
                }
            }
            qrCodeImage.Pixels = pixels;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                d.SaveTo(ms);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + System.Convert.ToBase64String(byteImage);
            }
#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif


            return new NewUserCode() { QRCode = SigBase64, Code = codeValue };
        }


        /// <MetaDataID>{204d4e46-9188-44d6-80fc-d583ee024af2}</MetaDataID>
        public NewUserCode GetNewTakeAwayCashierQRCode(string color)
        {

            string codeValue = ServicesContextRuntime.NewTakeAwayCashier();
            string SigBase64 = "";
#if DeviceDotNet
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };


            var bitmapMatrix = barcodeWriter.Encode(codeValue);
            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

            SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
            if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                fgColor = SkiaSharp.SKColors.Black;

            var pixels = qrCodeImage.Pixels;
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixels[k++] = fgColor;
                    else
                        pixels[k++] = SkiaSharp.SKColors.White;
                }
            }
            qrCodeImage.Pixels = pixels;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                d.SaveTo(ms);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + System.Convert.ToBase64String(byteImage);
            }
#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif


            return new NewUserCode() { QRCode = SigBase64, Code = codeValue };

        }
        public DeviceAssignmentKey GetPreparationStationKeyQRCode(IPreparationStation preparationStation, string color)
        {

            string codeValue = preparationStation.PreparationStationIdentity;
            string SigBase64 = GetQRcodeBase64Img(color, codeValue);
            return new DeviceAssignmentKey() { QRCode = SigBase64, PreparationStationIdentity = codeValue, ShortIdentity = preparationStation.ShortIdentity };

        }

        public DeviceAssignmentKey GetPrintManagerDeviceAssignKeyQRCode(string color)
        {
            DeviceAssignKeyData deviceAssignKeyData = this.ServicesContextRuntime.PrintManager.GetPrintManagerNewCredentialKey();
            string codeValue = deviceAssignKeyData.PrintManagerDeviceAssignFullKey;
            string SigBase64 = GetQRcodeBase64Img(color, codeValue);

            return new DeviceAssignmentKey() { QRCode = SigBase64, PreparationStationIdentity = codeValue, ShortIdentity = deviceAssignKeyData.PrintManagerDeviceAssignShortKey };

        }

        private static string GetQRcodeBase64Img(string color, string codeValue)
        {
            string SigBase64 = "";
#if DeviceDotNet
                var barcodeWriter = new BarcodeWriterGeneric()
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 400,
                        Width = 400
                    }
                };


                var bitmapMatrix = barcodeWriter.Encode(codeValue);
                var width = bitmapMatrix.Width;
                var height = bitmapMatrix.Height;
                int[] pixelsImage = new int[width * height];
                SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

                SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
                if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                    fgColor = SkiaSharp.SKColors.Black;

                var pixels = qrCodeImage.Pixels;
                int k = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (bitmapMatrix[j, i])
                            pixels[k++] = fgColor;
                        else
                            pixels[k++] = SkiaSharp.SKColors.White;
                    }
                }
                qrCodeImage.Pixels = pixels;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                    d.SaveTo(ms);
                    byte[] byteImage = ms.ToArray();
                    SigBase64 = @"data:image/png;base64," + System.Convert.ToBase64String(byteImage);
                }
#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif
            return SigBase64;
        }

        public NewUserCode GetNewNativeUserQRCode(IWorkerPresentation worker, string color)
        {
            if (worker?.NativeUser == true)
            {
                string codeValue = this.ServicesContext.ServicesContextIdentity + ";" + worker.WorkerIdentity;
                string SigBase64 = "";
#if DeviceDotNet
                var barcodeWriter = new BarcodeWriterGeneric()
                {
                    Format = ZXing.BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Height = 400,
                        Width = 400
                    }
                };


                var bitmapMatrix = barcodeWriter.Encode(codeValue);
                var width = bitmapMatrix.Width;
                var height = bitmapMatrix.Height;
                int[] pixelsImage = new int[width * height];
                SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

                SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
                if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                    fgColor = SkiaSharp.SKColors.Black;

                var pixels = qrCodeImage.Pixels;
                int k = 0;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (bitmapMatrix[j, i])
                            pixels[k++] = fgColor;
                        else
                            pixels[k++] = SkiaSharp.SKColors.White;
                    }
                }
                qrCodeImage.Pixels = pixels;

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                    d.SaveTo(ms);
                    byte[] byteImage = ms.ToArray();
                    SigBase64 = @"data:image/png;base64," + System.Convert.ToBase64String(byteImage);
                }
#else
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

                using (System.IO.MemoryStream ms = new MemoryStream())
                {
                    qrCodeImage.Save(ms, ImageFormat.Png);
                    byte[] byteImage = ms.ToArray();
                    SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
                }
#endif


                return new NewUserCode() { QRCode = SigBase64, Code = codeValue };


            }
            else
                return null;
        }



        /// <MetaDataID>{efc09c4c-633b-45de-9a6c-212f03ae5d73}</MetaDataID>
        public NewUserCode GetNewCourierQRCode(string color)
        {
            string codeValue = ServicesContextRuntime.NewCourier();
            string SigBase64 = "";
#if DeviceDotNet
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400
                }
            };


            var bitmapMatrix = barcodeWriter.Encode(codeValue);
            var width = bitmapMatrix.Width;
            var height = bitmapMatrix.Height;
            int[] pixelsImage = new int[width * height];
            SkiaSharp.SKBitmap qrCodeImage = new SkiaSharp.SKBitmap(width, height);

            SkiaSharp.SKColor fgColor = SkiaSharp.SKColors.Black;
            if (!SkiaSharp.SKColor.TryParse(color, out fgColor))
                fgColor = SkiaSharp.SKColors.Black;

            var pixels = qrCodeImage.Pixels;
            int k = 0;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (bitmapMatrix[j, i])
                        pixels[k++] = fgColor;
                    else
                        pixels[k++] = SkiaSharp.SKColors.White;
                }
            }
            qrCodeImage.Pixels = pixels;

            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                SkiaSharp.SKData d = SkiaSharp.SKImage.FromBitmap(qrCodeImage).Encode(SkiaSharp.SKEncodedImageFormat.Png, 100);
                d.SaveTo(ms);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + System.Convert.ToBase64String(byteImage);
            }
#else
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(codeValue, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, color, "#FFFFFF", true);

            using (System.IO.MemoryStream ms = new MemoryStream())
            {
                qrCodeImage.Save(ms, ImageFormat.Png);
                byte[] byteImage = ms.ToArray();
                SigBase64 = @"data:image/png;base64," + Convert.ToBase64String(byteImage);
            }
#endif


            return new NewUserCode() { QRCode = SigBase64, Code = codeValue };
        }


    }
}
