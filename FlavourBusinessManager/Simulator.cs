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

namespace FlavourBusinessManager.RoomService
{



    public class Simulator
    {


        internal static double Velocity = 0.33;

        static Random _R = new Random();

        Task SimulationTask;

        bool EndOfSimulation = false;

        static List<List<PSItemsPattern>> PreparationStationSimulatorItems = new List<List<PSItemsPattern>>() {
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(1) , new PSItemsPattern(2) , new PSItemsPattern(0)},
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(0) , new PSItemsPattern(0) , new PSItemsPattern(2)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(2) , new PSItemsPattern(1) , new PSItemsPattern(0)},
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(0) , new PSItemsPattern(3) , new PSItemsPattern(0)},
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(2) , new PSItemsPattern(1) , new PSItemsPattern(1)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(0) , new PSItemsPattern(2) , new PSItemsPattern(1)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(2) , new PSItemsPattern(1) , new PSItemsPattern(1)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(2) , new PSItemsPattern(0) , new PSItemsPattern(1)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(3) , new PSItemsPattern(0) , new PSItemsPattern(1)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(5) , new PSItemsPattern(0) , new PSItemsPattern(0)},
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(0) , new PSItemsPattern(1) , new PSItemsPattern(2)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(0) , new PSItemsPattern(3) , new PSItemsPattern(0)},
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(0) , new PSItemsPattern(0) , new PSItemsPattern(3)  },
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(2) , new PSItemsPattern(0) , new PSItemsPattern(0)},
            new List<PSItemsPattern> { new PSItemsPattern(0), new PSItemsPattern(1) , new PSItemsPattern(0) , new PSItemsPattern(2)  },

        };

        /// <MetaDataID>{8574ee8e-dac2-40f7-acf2-ff4f27bc5f1e}</MetaDataID>
        internal void StartSimulator()
        {


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
                        var preparationSationItems = mainMealCourseMenuItems.Where(x => preparationStation.CanPrepareItem(x)).ToList();
                        if (preparationSationItems.Count > 0)
                            preparationStationsItems[preparationStation] = preparationSationItems;
                    }

                    IFoodServiceClientSession clientSession = null;


                    int i = 0;
                    while (!EndOfSimulation && servicePoints.Count > 0)
                    {

                        if (lastMealCourseAdded == null || (DateTime.UtcNow - lastMealCourseAdded.Value).TotalMinutes > 0.6)
                        {
                            List<List<PSItemsPattern>> preparationStationSimulatorItems = GetNextPreparationPatern(i++);

                            var freeServicePoints = servicePoints.Where(x => x.State == ServicePointState.Free).ToList();

                            if (freeServicePoints.Count > 0 && servicePoints.Where(x => x.State != ServicePointState.Free).Count() < 2)
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


        IList<IPreparationStation> SimulatorPreparationStations
        {
            get
            {
                var simulatorPreparationStations = ServicesContextRunTime.Current.PreparationStationRuntimes.Values.OrderBy(x => x.Description).OfType<IPreparationStation>().ToList();
                return simulatorPreparationStations;
            }
        }
        private List<List<PSItemsPattern>> GetNextPreparationPatern(int step)
        {
            List<List<PSItemsPattern>> preparationPaterns = new List<List<PSItemsPattern>>();

            //if (step == 0)
            //{
            //    preparationPaterns.Add(new List<PSItemsPattern> {
            //        new PSItemsPattern(0),
            //        new PSItemsPattern(0),
            //        PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,4.5),new ItemPattern(3.5,4.5))  ,
            //        PSItemsPattern.GetItemsPatterns(new ItemPattern(8.5,10.5),new ItemPattern(3.5,5.5)) });
            //    return preparationPaterns;
            //}
            //if (step == 1)
            //{
            //    preparationPaterns.Add(new List<PSItemsPattern> {
            //        new PSItemsPattern(0),
            //        new PSItemsPattern(0),
            //        PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,4.5),new ItemPattern(3.5,4.5)),
            //        new PSItemsPattern(0)});

            //    return preparationPaterns;
            //}

            if (step == 0)
            {
                preparationPaterns.Add(new List<PSItemsPattern> {
                    new PSItemsPattern(0),
                    new PSItemsPattern(0),
                    new PSItemsPattern(0),
                    PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,5.5),new ItemPattern(3.5,5.5),new ItemPattern(3.5,5.5)) 
                });
                return preparationPaterns;
            }
            if (step == 1)
            {
                preparationPaterns.Add(new List<PSItemsPattern> {
                    new PSItemsPattern(0),
                    new PSItemsPattern(0),
                    new PSItemsPattern(0),
                        PSItemsPattern.GetItemsPatterns(new ItemPattern(3.5,5.5),new ItemPattern(3.5,5.5))
                });
                return preparationPaterns;
            }
            if (step == 3)
            {
                preparationPaterns.Add(new List<PSItemsPattern> {
                    new PSItemsPattern(0),
                    new PSItemsPattern(0),
                    new PSItemsPattern(0),
                    new PSItemsPattern(3) });


                return preparationPaterns;
            }

            return PreparationStationSimulatorItems;
        }

        internal void DeleteSimulationData()
        {
            var objectStorage = ObjectStorage.GetStorageOfObject(ServicesContextRunTime.Current);
            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(objectStorage);

            var simulationClientSessions = (from clientSession in servicesContextStorage.GetObjectCollection<FoodServiceClientSession>()
                                            where clientSession.ClientDeviceID == "S_81000000296"
                                            select clientSession).ToList();

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
                                ObjectStorage.DeleteObject(mealCourse);
                            ObjectStorage.DeleteObject(meal);
                        }

                    }
                    clientSession.ServicePoint.State = ServicePointState.Free;
                    foreach (var itemPreparation in clientSession.FlavourItems)
                        ObjectStorage.DeleteObject(itemPreparation);
                    ObjectStorage.DeleteObject(clientSession);

                }
                stateTransition.Consistent = true;
            }
        }

        private IFoodServiceClientSession simulateClientSession(string mainMealCourseTypeUri, Dictionary<IPreparationStation, List<IMenuItem>> preparationStationsItems, List<List<PSItemsPattern>> preparationStationSimulatorItems, string servicesPointIdentity, string clientDeviceID, string clientName)
        {



            List<IItemPreparation> itemsToPrepare = new List<IItemPreparation>();
            var patern = preparationStationSimulatorItems[_R.Next(preparationStationSimulatorItems.Count - 1)].ToList();



            foreach (var psItemsPattern in patern)
            {
                if (psItemsPattern.NumberOfItems != null)
                {
                    while (psItemsPattern.NumberOfItems > 0)
                    {
                        var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[patern.IndexOf(psItemsPattern)]];
                        var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                        itemsToPrepare.Add(itemPreparation);
                        psItemsPattern.NumberOfItems = psItemsPattern.NumberOfItems - 1;
                    }
                }
                if (psItemsPattern.ItemsPatterns != null)
                {
                    foreach (var itemPatern in psItemsPattern.ItemsPatterns)
                    {
                        var preparationStation = preparationStationsItems.Keys.ToList()[patern.IndexOf(psItemsPattern)];
                        var preparationStationItems = preparationStationsItems[preparationStation];

                        preparationStationItems = preparationStationItems.Where(x => (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) >= itemPatern.MinDuration / 2 && (preparationStation as PreparationStation).GetPreparationTimeInMin(x as MenuItem) <= itemPatern.MaxDuration / 2).ToList();
                        var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
                        ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
                        itemsToPrepare.Add(itemPreparation);
                    }
                }


            }

            //while (patern[0].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[0]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[0].NumberOfItems = patern[0].NumberOfItems - 1;
            //}


            //while (patern[1].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[1]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[1].NumberOfItems = patern[1].NumberOfItems - 1;
            //}

            //while (patern[2].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[2]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[2].NumberOfItems = patern[2].NumberOfItems - 1;
            //}

            //while (patern[3].NumberOfItems > 0)
            //{
            //    var preparationStationItems = preparationStationsItems[preparationStationsItems.Keys.ToList()[3]];
            //    var menuItem = preparationStationItems[_R.Next(preparationStationItems.Count - 1)];
            //    ItemPreparation itemPreparation = new ItemPreparation(Guid.NewGuid().ToString("N"), ObjectStorage.GetStorageOfObject(menuItem).GetPersistentObjectUri(menuItem), menuItem.Name) { Quantity = 1, SelectedMealCourseTypeUri = mainMealCourseTypeUri };
            //    itemsToPrepare.Add(itemPreparation);
            //    patern[3].NumberOfItems = patern[3].NumberOfItems - 1;
            //}
            IFoodServiceClientSession clientSession = null;
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                clientSession = ServicesContextRunTime.Current.GetClientSession(servicesPointIdentity, null, clientName, clientDeviceID, null, ServicesContextRunTime.Current.OrganizationIdentity, ServicesContextRunTime.Current.GraphicMenus, true).FoodServiceClientSession;
                foreach (var itemToPrepare in itemsToPrepare)
                    clientSession.AddItem(itemToPrepare);


                stateTransition.Consistent = true;

            }
            clientSession.Commit(itemsToPrepare);
            return clientSession;

            //   clientSession.FoodServiceClientSession.Commit(itemsToPrepare);
        }

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
        public PSItemsPattern(int numberOfItems)
        {
            NumberOfItems = numberOfItems;
        }
        public PSItemsPattern(List<ItemPattern> itemsPatterns)
        {
            ItemsPatterns = itemsPatterns;
        }
        public List<ItemPattern> ItemsPatterns;

        public int? NumberOfItems;

        public static PSItemsPattern GetItemsPatterns(params ItemPattern[] paternParma)
        {
            return new PSItemsPattern(paternParma.ToList());
        }
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
}

