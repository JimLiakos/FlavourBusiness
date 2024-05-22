using System;
using FlavourBusinessFacade;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Linq;
using System.Linq;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using System.Collections.Generic;
using OOAdvantech;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessFacade.EndUsers;
using System.Xml.Linq;
using FlavourBusinessManager.ServicesContextResources;
using Microsoft.Azure.Storage;
using FlavourBusinessManager.EndUsers;
using FlavourBusinessFacade.HumanResources;
using System.Threading.Tasks;
using FlavourBusinessToolKit;
using System.IO;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using FinanceFacade;
using MenuModel;
using FlavourBusinessManager.ServicePointRunTime;
using System.ServiceModel.Configuration;
using System.Net;


namespace FlavourBusinessManager.RoomService
{



    /// <MetaDataID>{c99be1be-17c0-435d-87ae-81b2bf5ec133}</MetaDataID>
    public class Simulator : MarshalByRefObject, IExtMarshalByRefObject, FlavourBusinessFacade.UnitTest.IFoodServicesSessionsSimulator
    {


        /// <MetaDataID>{5639f6bd-def5-49c5-ba3d-8990fb6a4d47}</MetaDataID>
        internal static double Velocity = 0.33;

        /// <MetaDataID>{319a4f32-3817-4f74-8619-939616b7f9bb}</MetaDataID>
        static Random _R = new Random();

        /// <MetaDataID>{514b3cf9-13f4-4dad-9300-0b08c70a65df}</MetaDataID>
        Task SimulationTask;

        /// <MetaDataID>{90970a4a-6f80-4c2f-92ea-71f2cafab3eb}</MetaDataID>
        bool EndOfSimulation = false;

        /// <MetaDataID>{888ee274-4101-4e5d-b91b-7602d7e07844}</MetaDataID>
        static List<ClientSessionPattern> PreparationStationSimulatorItems;
        public event FlavourBusinessFacade.UnitTest.NewSimulateSessionHandler NewSimulateSession;

        /// <MetaDataID>{8574ee8e-dac2-40f7-acf2-ff4f27bc5f1e}</MetaDataID>
        internal void StartSimulator(ServicesContextRunTime servicesContextRunTime)
        {
            return;

            var preparationStations = servicesContextRunTime.PreparationStations.ToList();

            if (PreparationStationSimulatorItems == null)
            {
                PreparationStationSimulatorItems = new List<ClientSessionPattern>() {
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],1) , new PSItemsPattern(preparationStations[0],2) , new PSItemsPattern(preparationStations[0],0)} },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],0) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],2)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],2) , new PSItemsPattern(preparationStations[2],1) , new PSItemsPattern(preparationStations[3],0)} },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],0) , new PSItemsPattern(preparationStations[2],3) , new PSItemsPattern(preparationStations[3],0)} },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],2) , new PSItemsPattern(preparationStations[2],1) , new PSItemsPattern(preparationStations[3],1)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],0) , new PSItemsPattern(preparationStations[2],2) , new PSItemsPattern(preparationStations[3],1)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],2) , new PSItemsPattern(preparationStations[2],1) , new PSItemsPattern(preparationStations[3],1)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],2) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],1)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],3) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],1)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],5) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],0)} },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],0) , new PSItemsPattern(preparationStations[2],1) , new PSItemsPattern(preparationStations[3],2)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],0) , new PSItemsPattern(preparationStations[2],3) , new PSItemsPattern(preparationStations[3],0)} },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],0) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],3)  } },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],2) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],0)} },
            new ClientSessionPattern(){PreparationStations= new List<PSItemsPattern> { new PSItemsPattern(preparationStations[0],0), new PSItemsPattern(preparationStations[1],1) , new PSItemsPattern(preparationStations[2],0) , new PSItemsPattern(preparationStations[3],2)  } }};


                var tmp = OOAdvantech.Json.JsonConvert.SerializeObject(PreparationStationSimulatorItems);

                using (WebClient wc = new WebClient())
                {
                    var json = wc.DownloadString($"http://127.0.0.1:10000/devstoreaccount1/usersfolder/{servicesContextRunTime.OrganizationIdentity}/RestaurantSimulationPettern.json");
                    PreparationStationSimulatorItems = OOAdvantech.Json.JsonConvert.DeserializeObject<List<ClientSessionPattern>>(json);
                }
            }


            if (SimulationTask == null || SimulationTask.Status != TaskStatus.Running)
            {
                SimulationTask = Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(50000);
                    DateTime? lastMealCourseAdded = null;

                    var servicePoints = (from serviceArea in ServicesContextRunTime.Current.ServiceAreas
                                         from ServicePoint in serviceArea.ServicePoints
                                         select ServicePoint).ToList();

                    List<IMenuItem> menuItems = GetMenuItems(ServicesContextRunTime.Current.OperativeRestaurantMenu.RootCategory);

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(ServicesContextRunTime.Current.OperativeRestaurantMenu));
                    var twoCoursesMealType = (from fixedMealType in storage.GetObjectCollection<FixedMealType>()
                                              select fixedMealType).ToList().Where(x => x.Courses.Count == 2).FirstOrDefault();
                    //var clientSession = GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, null, OrganizationIdentity, GraphicMenus, true);

                    var mainMealCourseType = twoCoursesMealType.Courses.Where(x => x.IsDefault).FirstOrDefault();
                    var mainMealCourseMenuItems = menuItems.Where(x => x.PartofMeals.Any(y => y.MealCourseType == mainMealCourseType)).ToList();
                    string mainMealCourseTypeUri = ObjectStorage.GetStorageOfObject(mainMealCourseType).GetPersistentObjectUri(mainMealCourseType);
                    Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems = new Dictionary<IPreparationStation, List<IMenuItem>>();




                    foreach (var preparationStation in SimulatorPreparationStations.ToList())
                    {
                        var preparationStationItems = mainMealCourseMenuItems.Where(x => preparationStation.CanPrepareItem(x)).ToList();
                        if (preparationStationItems.Count > 0)
                            preparationStationsItems[preparationStation] = preparationStationItems;
                    }

                    IFoodServiceClientSession clientSession = null;


                    int i = 0;
                    while (!EndOfSimulation && servicePoints.Count > 0)
                    {

                        if (lastMealCourseAdded == null || (DateTime.UtcNow - lastMealCourseAdded.Value).TotalMinutes > 0.6)
                        {
                            List<ClientSessionPattern> preparationStationSimulatorItems = GetNextPreparationPattern(i++);

                            var freeServicePoints = servicePoints.Where(x => x.State == ServicePointState.Free).ToList();

                            if (freeServicePoints.Count > 0 && servicePoints.Where(x => x.State != ServicePointState.Free).Count() < 5)
                            {

                                string servicesPointIdentity = freeServicePoints[_R.Next(freeServicePoints.Count - 1)].ServicesPointIdentity;
                                string clientDeviceID = "S_81000000296";
                                string clientName = "Jimmy Garson";
                                clientSession = simulateClientSession(mainMealCourseTypeUri, preparationStationsItems, preparationStationSimulatorItems, servicesPointIdentity, clientDeviceID, clientName);
                                lastMealCourseAdded = DateTime.UtcNow;
                            }
                            else
                            {

                            }
                            ////DeleteSimulationData();
                        }

                        //PreparationStations[0].
                        //clientSession.FoodServiceClientSession.AddItem
                        System.Threading.Thread.Sleep(10000);

                    }
                });
            }
            else
            {

            }
            //clientDeviceID="S_81000000296"
            //clientName="clientName"

        }


        public void StartClientSideSimulation(SessionType sessionType)
        {
            string defaultMealTypeUri = null;

            if (sessionType == SessionType.Hall)
                defaultMealTypeUri = ServicePointRunTime.ServicesContextRunTime.Current.ServiceAreas.FirstOrDefault().ServesMealTypesUris.FirstOrDefault();


            if (sessionType == SessionType.HomeDeliveryGuest || sessionType == SessionType.HomeDelivery)
                defaultMealTypeUri = ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri;

            if (sessionType == SessionType.Takeaway)
                defaultMealTypeUri = ServicesContextRunTime.Current.GetOneCoursesMealType().MealTypeUri;



            if (SimulationTask == null || SimulationTask.Status != TaskStatus.Running)
            {
                SimulationTask = Task.Run(() =>
                {
                    System.Threading.Thread.Sleep(5000);
                    DateTime? lastMealCourseAdded = null;

                    var servicePoints = (from serviceArea in ServicesContextRunTime.Current.ServiceAreas
                                         from ServicePoint in serviceArea.ServicePoints
                                         select ServicePoint).ToList();

                    List<IMenuItem> menuItems = GetMenuItems(ServicesContextRunTime.Current.OperativeRestaurantMenu.RootCategory);

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(ServicesContextRunTime.Current.OperativeRestaurantMenu));
                    var mealType = (from fixedMealType in storage.GetObjectCollection<FixedMealType>()
                                    select fixedMealType).ToList().Where(x => x.MealTypeUri == defaultMealTypeUri).FirstOrDefault();
                    //var clientSession = GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, null, OrganizationIdentity, GraphicMenus, true);

                    var mainMealCourseType = mealType.Courses.Where(x => x.IsDefault).FirstOrDefault();
                    var mainMealCourseMenuItems = menuItems.Where(x => x.PartofMeals.Any(y => y.MealCourseType == mainMealCourseType)).ToList();
                    if (mainMealCourseMenuItems.Count == 0)
                        mainMealCourseMenuItems = menuItems;

                    string mainMealCourseTypeUri = ObjectStorage.GetStorageOfObject(mainMealCourseType).GetPersistentObjectUri(mainMealCourseType);
                    Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems = new Dictionary<IPreparationStation, List<IMenuItem>>();




                    foreach (var preparationStation in SimulatorPreparationStations.ToList())
                    {
                        var preparationStationItems = mainMealCourseMenuItems.Where(x => preparationStation.CanPrepareItem(x)).ToList();
                        if (preparationStationItems.Count > 0)
                            preparationStationsItems[preparationStation] = preparationStationItems;
                    }



                    int i = 0;
                    while (!EndOfSimulation && servicePoints.Count > 0)
                    {

                        if (lastMealCourseAdded == null || (DateTime.UtcNow - lastMealCourseAdded.Value).TotalMinutes > 0.6)
                        {
                            List<ClientSessionPattern> preparationStationSimulatorItems = GetClientSideNextPreparationPatern(i++);

                            var freeServicePoints = servicePoints.Where(x => x.State == ServicePointState.Free).ToList();

                            if (freeServicePoints.Count > 0 && servicePoints.Where(x => x.State != ServicePointState.Free).Count() < 5)
                            {

                                string servicesPointIdentity = freeServicePoints[_R.Next(freeServicePoints.Count - 1)].ServicesPointIdentity;
                                simulateClientSideSession(mainMealCourseTypeUri, preparationStationsItems, preparationStationSimulatorItems);
                                lastMealCourseAdded = DateTime.UtcNow;
                                break;
                            }
                            else
                            {

                            }
                            ////DeleteSimulationData();
                        }

                        //PreparationStations[0].
                        //clientSession.FoodServiceClientSession.AddItem
                        System.Threading.Thread.Sleep(10000);

                    }
                });
            }
            else
            {

            }

        }


        /// <MetaDataID>{f035b325-b1ac-464b-bd62-f8010d4c36a8}</MetaDataID>
        IList<IPreparationStation> SimulatorPreparationStations
        {
            get
            {
                var simulatorPreparationStations = ServicesContextRunTime.Current.PreparationStationRuntimes.Values.OrderBy(x => x.Description).OfType<IPreparationStation>().ToList();
                return simulatorPreparationStations;
            }
        }
        /// <MetaDataID>{836c7b95-b4d8-4c87-ac3b-466eb54bf6b1}</MetaDataID>
        private List<ClientSessionPattern> GetNextPreparationPattern(int step)
        {

            //http://127.0.0.1:10000/devstoreaccount1/usersfolder/ykCR5c6aHVUUpGJ8J7ZqpLLY97i1/RestaurantSimulationPettern.json




            List<ClientSessionPattern> preparationPatterns = new List<ClientSessionPattern>();

            if (step == 0)
            {

                //var preparationStationsPatterns = new List<PSItemsPattern> {
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],0),
                //    PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,4.5),new ItemPattern(3.5,4.5))  ,
                //    PSItemsPattern.GetItemsPatterns(new ItemPattern(8.5,10.5),new ItemPattern(3.5,5.5)) };
                preparationPatterns.Add(PreparationStationSimulatorItems[0]);
                return preparationPatterns;
            }
            if (step == 1)
            {
                //var preparationStationsPatterns = new List<PSItemsPattern> {
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],0),
                //    PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,4.5),new ItemPattern(3.5,4.5)),
                //    new PSItemsPattern(preparationStations[0],0)};

                preparationPatterns.Add(PreparationStationSimulatorItems[1]);
                return preparationPatterns;
            }

            //if (step == 0)
            //{
            //    var preparationStationsPatterns = new List<PSItemsPattern> {
            //        new PSItemsPattern(preparationStations[0],0),
            //        new PSItemsPattern(preparationStations[0],0),
            //        new PSItemsPattern(preparationStations[0],0),
            //        PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,5.5),new ItemPattern(3.5,5.5),new ItemPattern(3.5,5.5))
            //    };
            //    return new ClientSessionPattern() { PreparationStations = preparationStationsPatterns };

            //}
            //if (step == 1)
            //{
            //    var preparationStationsPatterns = new List<PSItemsPattern> {
            //        new PSItemsPattern(preparationStations[0],0),
            //        new PSItemsPattern(preparationStations[0],0),
            //        new PSItemsPattern(preparationStations[0],0),
            //            PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,5.5),new ItemPattern(3.5,5.5))
            //    };
            //    return new ClientSessionPattern() { PreparationStations = preparationStationsPatterns };

            //}
            if (step == 2)
            {
                //var preparationStationsPatterns = new List<PSItemsPattern> {
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],3) };
                //preparationPatterns.Add(new ClientSessionPattern() { PreparationStations = preparationStationsPatterns });
                preparationPatterns.Add(PreparationStationSimulatorItems[2]);
                return preparationPatterns;

            }
            if (step == 3)
            {
                //var preparationStationsPatterns = new List<PSItemsPattern> {
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],0),
                //    new PSItemsPattern(preparationStations[0],3) };
                //preparationPatterns.Add(new ClientSessionPattern() { PreparationStations = preparationStationsPatterns });

                preparationPatterns.Add(PreparationStationSimulatorItems[3]);

                return preparationPatterns;

            }
            return PreparationStationSimulatorItems;
        }


        private List<ClientSessionPattern> GetClientSideNextPreparationPatern(int step)
        {
            List<ClientSessionPattern> preparationPatterns = new List<ClientSessionPattern>();

            if (step == 0)
            {
                preparationPatterns.Add(PreparationStationSimulatorItems[3]);
            }
            if (step == 1)
            {
                preparationPatterns.Add(PreparationStationSimulatorItems[3]);
            }

            if (step == 0)
            {
                preparationPatterns.Add(PreparationStationSimulatorItems[0]);
            }
            if (step == 1)
            {
                preparationPatterns.Add(PreparationStationSimulatorItems[1]);
            }
            if (step == 2)
            {
                preparationPatterns.Add(PreparationStationSimulatorItems[2]);
            }
            if (step == 3)
            {
                preparationPatterns.Add(PreparationStationSimulatorItems[3]);
            }
            return PreparationStationSimulatorItems;
        }


        /// <MetaDataID>{04391a35-b2a6-4cea-ad53-6d18922a9380}</MetaDataID>
        internal void DeleteSimulationData()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(ServicesContextRunTime.Current);
            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

            var simulationClientSessions = (from clientSession in servicesContextStorage.GetObjectCollection<FoodServiceClientSession>()
                                            select clientSession).ToList();

            simulationClientSessions = simulationClientSessions.Where(x => x.ClientDeviceID == "S_81000000296" || x.ClientDeviceID.IndexOf("org_client_sim_") == 0).ToList();
            Dictionary<string, FinanceFacade.Transaction> deletedTransactions = new Dictionary<string, FinanceFacade.Transaction>();



            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var clientSession in simulationClientSessions)
                {
                    var mainSession = clientSession.MainSession;


                    if (mainSession != null)
                    {
                        ObjectStorage.DeleteObject(mainSession);
                        var meal = mainSession.Meal as Meal;
                        if (meal != null)
                        {
                            meal.StopMonitoring();
                            foreach (var mealCourse in meal.Courses)
                            {
                                ObjectStorage.DeleteObject(mealCourse);
                            }
                            ObjectStorage.DeleteObject(meal);
                        }

                    }
                    clientSession.ServicePoint.State = ServicePointState.Free;
                    foreach (var itemPreparation in clientSession.FlavourItems)
                    {
                        ObjectStorage.DeleteObject(itemPreparation);
                        var transactionUri = (itemPreparation as ItemPreparation).TransactionUri;
                        if (!string.IsNullOrWhiteSpace(transactionUri) && !deletedTransactions.ContainsKey(transactionUri))
                        {
                            var transaction = ObjectStorage.GetObjectFromUri<FinanceFacade.Transaction>(transactionUri);
                            deletedTransactions[transactionUri] = transaction;
                            ObjectStorage.DeleteObject(transaction);
                        }

                    }
                    ObjectStorage.DeleteObject(clientSession);

                }
                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{5addbfd2-b4b6-4392-a8ba-27638786d741}</MetaDataID>
        private IFoodServiceClientSession simulateClientSession(string mainMealCourseTypeUri, Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems, List<ClientSessionPattern> clientSessionPatterns, string servicesPointIdentity, string clientDeviceID, string clientName)
        {



            List<IItemPreparation> itemsToPrepare = new List<IItemPreparation>();
            var clientSessionPattern = clientSessionPatterns[_R.Next(clientSessionPatterns.Count - 1)];


            IFoodServiceClientSession clientSession = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                clientSession = ServicesContextRunTime.Current.GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, DeviceType.Phone, null, ServicesContextRunTime.Current.OrganizationIdentity, ServicesContextRunTime.Current.GraphicMenus, true, true).FoodServiceClientSession;

                foreach (var psItemsPattern in clientSessionPattern.PreparationStations)
                {
                    var preparationStation = preparationStationsItems.Keys.Where(x => x.ShortIdentity == psItemsPattern.PreparationStationIdentity).First();
                    if (psItemsPattern.NumberOfItems != null)
                    {
                        int tries = 15;
                        while (psItemsPattern.NumberOfItems > 0 && tries > 0)
                        {
                            var preparationStationItems = preparationStationsItems[preparationStation];
                            if (preparationStationItems.Count > 0)
                            {
                                var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                                var menuCanvasItem = (clientSession as FoodServiceClientSession).GraphicMenu.GetMenuCanvasFoodItem(ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem));
                                if (menuCanvasItem != null)
                                {
                                    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                                    itemsToPrepare.Add(itemPreparation);
                                    psItemsPattern.NumberOfItems = psItemsPattern.NumberOfItems - 1;
                                }
                                else
                                    tries--;
                            }
                            else
                                tries--;

                        }
                    }
                    if (psItemsPattern.ItemsPatterns != null)
                    {
                        foreach (var itemPattern in psItemsPattern.ItemsPatterns)
                        {
                            int tries = 15;
                            while (tries > 0)
                            {


                                var preparationStationItems = preparationStationsItems[preparationStation];

                                preparationStationItems = preparationStationItems.Where(x => (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) >= itemPattern.MinDuration / 2 && (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) <= itemPattern.MaxDuration / 2).ToList();
                                if (preparationStationItems.Count > 0)
                                {
                                    IMenuItem menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                                    var menuCanvasItem = (clientSession as FoodServiceClientSession).GraphicMenu.GetMenuCanvasFoodItem(ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem));
                                    if (menuCanvasItem != null)
                                    {
                                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                                        itemsToPrepare.Add(itemPreparation);
                                        break;
                                    }
                                    else
                                        tries--;
                                }
                                else
                                    tries--;
                            }
                        }
                    }


                }




                itemsToPrepare = itemsToPrepare.OfType<ItemPreparation>().Where(x => (clientSession as FoodServiceClientSession).GraphicMenu.GetMenuCanvasFoodItem(x.MenuItemUri) != null).OfType<IItemPreparation>().ToList();
                if (itemsToPrepare.Count > 0)
                {

                    foreach (var itemToPrepare in itemsToPrepare)
                        clientSession.AddItem(itemToPrepare);
                    stateTransition.Consistent = true;
                }
                else
                    stateTransition.Consistent = false;

            }
            clientSession.Commit(itemsToPrepare);
            return clientSession;

            //   clientSession.FoodServiceClientSession.Commit(itemsToPrepare);
        }



        private void simulateClientSideSession(string mainMealCourseTypeUri, Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems, List<ClientSessionPattern> preparationStationSimulatorItems)
        {

            List<ItemPreparation> itemsToPrepare = new List<ItemPreparation>();
            ClientSessionPattern pattern = preparationStationSimulatorItems[_R.Next(preparationStationSimulatorItems.Count - 1)].Clone() as ClientSessionPattern;


            foreach (var psItemsPattern in pattern.PreparationStations)
            {
                if (psItemsPattern.NumberOfItems != null)
                {
                    while (psItemsPattern.NumberOfItems > 0)
                    {
                        var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[pattern.PreparationStations.IndexOf(psItemsPattern)]];
                        var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri, ModifiedItemPrice = (double)menuItem.MenuItemPrice.Price, ISOCurrencySymbol = "EUR" };
                        itemsToPrepare.Add(itemPreparation);
                        psItemsPattern.NumberOfItems = psItemsPattern.NumberOfItems - 1;
                    }
                }
                if (psItemsPattern.ItemsPatterns != null)
                {
                    foreach (var itemPatern in psItemsPattern.ItemsPatterns)
                    {
                        var preparationStation = preparationStationsItems.Keys.ToList()[pattern.PreparationStations.IndexOf(psItemsPattern)];
                        var preparationStationItems = preparationStationsItems[preparationStation];

                        preparationStationItems = preparationStationItems.Where(x => (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) >= itemPatern.MinDuration / 2 && (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) <= itemPatern.MaxDuration / 2).ToList();
                        var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri, ModifiedItemPrice = (double)menuItem.MenuItemPrice.Price };
                        itemsToPrepare.Add(itemPreparation);
                    }
                }


            }

            NewSimulateSession?.Invoke(itemsToPrepare.OfType<IItemPreparation>().ToList());


            //IFoodServiceClientSession clientSession = null;


            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{
            //    clientSession = ServicesContextRunTime.Current.GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, DeviceType.Phone, null, ServicesContextRunTime.Current.OrganizationIdentity, ServicesContextRunTime.Current.GraphicMenus, true, true).FoodServiceClientSession;
            //    foreach (var itemToPrepare in itemsToPrepare)
            //        clientSession.AddItem(itemToPrepare);
            //    stateTransition.Consistent = true;
            //}
            //clientSession.Commit(itemsToPrepare);
            //return clientSession;

            //   clientSession.FoodServiceClientSession.Commit(itemsToPrepare);
        }



        /// <MetaDataID>{538da042-5afb-4e85-bf8c-e467b434fbfc}</MetaDataID>
        private List<IMenuItem> GetMenuItems(IItemsCategory rootCategory)
        {
            var menuItems = rootCategory.MenuItems.ToList();
            foreach (var subCategory in rootCategory.SubCategories)
                menuItems.AddRange(GetMenuItems(subCategory));

            return menuItems;
        }
    }



    /// <MetaDataID>{9ec89e47-dba6-4e95-ab4d-a2cc995af903}</MetaDataID>
    class PSItemsPattern
    {
        public PSItemsPattern(IPreparationStation preparationStation, int numberOfItems)
        {
            NumberOfItems = numberOfItems;
            this.PreparationStationIdentity = preparationStation.ShortIdentity;
            this.PreparationStationName = preparationStation.Description;

        }
        public PSItemsPattern(List<ItemPattern> itemsPatterns)
        {
            ItemsPatterns = itemsPatterns;
        }
        public PSItemsPattern()
        {

        }
        public List<ItemPattern> ItemsPatterns;

        public int? NumberOfItems;



        public string PreparationStationIdentity { get; set; }
        public string PreparationStationName { get; set; }
    }

    /// <MetaDataID>{f9ba4335-70f0-49f8-a541-b9f1736629c6}</MetaDataID>
    class ItemPattern
    {
        public ItemPattern(double min, double max)
        {
            MinDuration = min;
            MaxDuration = max;
        }
        public double MinDuration;
        public double MaxDuration;
    }


    /// <MetaDataID>{147267b7-ceae-4c37-8526-d41ea718f1f7}</MetaDataID>
    class ClientSessionPattern
    {
        public List<PSItemsPattern> PreparationStations;

        internal ClientSessionPattern Clone()
        {
            return new ClientSessionPattern() { PreparationStations = PreparationStations.ToList() };
        }
    }
}

