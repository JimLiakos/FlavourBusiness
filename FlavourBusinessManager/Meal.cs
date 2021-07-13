using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{2a2adf89-8607-46c7-844d-d075fcf5d18b}</MetaDataID>
    [BackwardCompatibilityID("{2a2adf89-8607-46c7-844d-d075fcf5d18b}")]
    [Persistent()]
    public class Meal : IMeal
    {
        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{9dfc917f-c600-497d-a324-d1722058e811}</MetaDataID>
        string _MealTypeUri;

        /// <MetaDataID>{64183d95-2f41-4306-92fe-f877796dc446}</MetaDataID>
        [PersistentMember(nameof(_MealTypeUri))]
        [BackwardCompatibilityID("+4")]
        public string MealTypeUri
        {
            get => _MealTypeUri;
            set
            {

                if (_MealTypeUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealTypeUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{48e9e0d2-baf3-4f5a-8d87-b93c25ba690a}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+3")]
        public string Name
        {
            get => _Name;
            set
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
        OOAdvantech.Collections.Generic.Set<IMealCourse> _Courses = new OOAdvantech.Collections.Generic.Set<IMealCourse>();

        /// <MetaDataID>{5f5f90f7-bfae-4b34-8dc6-c6aa57297db5}</MetaDataID>
        [PersistentMember(nameof(_Courses))]
        [BackwardCompatibilityID("+2")]
        public List<IMealCourse> Courses => _Courses.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        OOAdvantech.Member<IFoodServiceSession> _Session = new OOAdvantech.Member<IFoodServiceSession>();

        /// <MetaDataID>{df1bef38-aa51-450a-a6c2-bdb6b6f960a5}</MetaDataID>
        [PersistentMember(nameof(_Session))]
        [BackwardCompatibilityID("+1")]
        public IFoodServiceSession Session => _Session.Value;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;




        /// <MetaDataID>{41cb3ae5-a95b-4809-bf04-5e4738fc3756}</MetaDataID>
        protected Meal()
        {

        }
        /// <MetaDataID>{1d0598fe-a193-451d-a1f4-38a330388d26}</MetaDataID>
        internal Meal(MenuModel.MealType mealType, List<ItemPreparation> mealItems)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Name = mealType.Name;
                _MealTypeUri = ObjectStorage.GetStorageOfObject(mealType).GetPersistentObjectUri(mealType);
                foreach (var mealCourseItems in (from mealItem in mealItems
                                                 group mealItem by mealItem.SelectedMealCourseTypeUri into mealCourseItems
                                                 select mealCourseItems))
                {
                    var mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => ObjectStorage.GetStorageOfObject(x)?.GetPersistentObjectUri(x) == mealCourseItems.Key).First();
                    MealCourse mealCourse = new MealCourse(mealCourseType, mealCourseItems.ToList());
                    _Courses.Add(mealCourse);
                }

                stateTransition.Consistent = true;
            }
        }
        /// <MetaDataID>{529127a8-8386-46da-a81b-1539bed5530b}</MetaDataID>
        object MealLock = new object();

        /// <MetaDataID>{340bccf9-c1ba-4185-851e-370a3286f0ee}</MetaDataID>
        Task MonitoringTask;
        /// <MetaDataID>{16c25ab9-e42f-4e19-b0de-7f0ea44de07c}</MetaDataID>
        internal void MonitoringRun()
        {
            lock (MealLock)
            {
                if (MonitoringTask != null && !MonitoringTask.IsCompleted)
                    return;
                MonitoringTask = Task.Run(() =>
                {
                    var sesionState = Session.SessionState;
                    while (sesionState == SessionState.MealMonitoring)
                    {
                        if (Courses[0].ServedAtForecast == null)
                        {
                            BuildMealTimePlan();
                        }
                        System.Threading.Thread.Sleep(1000);
                        sesionState = Session.SessionState;
                    }
                });

            }

        }

        /// <MetaDataID>{d71ac0eb-ed43-410f-80d8-ab8cce78f64d}</MetaDataID>
        private void BuildMealTimePlan()
        {


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                MealCourse headCourse = Courses[0] as MealCourse;
                if (headCourse.ServedAtForecast == null)
                {
                    var foodItemsPreparatioData = headCourse.FoodItems.OfType<ItemPreparation>().Select(x => new { foodItem = x, duration = ServicePointRunTime.PreparationStationRuntime.GetPreparationData(x).Duration }).OrderByDescending(x => x.duration).ToList();
                    headCourse.ServedAtForecast = System.DateTime.Now + foodItemsPreparatioData[0].duration;
                    foreach (var foodITem in foodItemsPreparatioData.Select(x => x.foodItem))
                    {
                        foodITem.State = ItemPreparationState.PreparationDelay;
                        foodITem.PreparedAtForecast = headCourse.ServedAtForecast;
                    }

                }

                foreach (MealCourse course in Courses)
                {
                    if (course== headCourse|| course.ServedAtForecast!=null)
                        continue;
                     
                    var foodItemsPreparatioData = course.FoodItems.OfType<ItemPreparation>().Select(x => new { foodItem = x, duration = ServicePointRunTime.PreparationStationRuntime.GetPreparationData(x).Duration }).OrderByDescending(x => x.duration).ToList();

                    DateTime shouldnotServedBefore = (Courses[Courses.IndexOf(course) - 1] as MealCourse).ServedAtForecast.Value + TimeSpan.FromMinutes((Courses[Courses.IndexOf(course) - 1] as MealCourse).DurationInMinutes);



                    course.ServedAtForecast = System.DateTime.Now + foodItemsPreparatioData[0].duration;
                    if (course.ServedAtForecast < shouldnotServedBefore)
                        course.ServedAtForecast = shouldnotServedBefore;

                    foreach (var foodITem in foodItemsPreparatioData.Select(x => x.foodItem))
                    {
                        foodITem.State = ItemPreparationState.PreparationDelay;
                        foodITem.PreparedAtForecast = course.ServedAtForecast;
                    }

                }
                stateTransition.Consistent = true;
            }


        }


        /// <MetaDataID>{4a767f54-cf13-46d7-8efd-7763ffcd80af}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        protected void BeforeCommitObjectState()
        {
            foreach (var course in Courses)
            {
                if (ObjectStorage.GetStorageOfObject(course) == null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(course);
            }
        }
        ///// <MetaDataID>{df1bef38-aa51-450a-a6c2-bdb6b6f960a5}</MetaDataID>
        //public IFoodServiceSession Session => throw new System.NotImplementedException();
    }
}