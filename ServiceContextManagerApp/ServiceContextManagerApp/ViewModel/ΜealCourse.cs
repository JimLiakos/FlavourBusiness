﻿using FlavourBusinessFacade.RoomService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Json;
using UIBaseEx;

using FlavourBusinessFacade.EndUsers;
using CourierApp.ViewModel;
using FlavourBusinessFacade.Shipping;
using System.Runtime.InteropServices.ComTypes;

namespace FlavourBusinessManager.RoomService.ViewModel
{

    public delegate void MealCourseUpdatedHandle(MealCourse mealCourse);
    public delegate void MealCoursesUpdatedHandle(IList<MealCourse> mealCourses);


    /// <MetaDataID>{e2ff48b0-ff9f-41df-b754-a190bfc34d1e}</MetaDataID>
    public class MealCourse
    {
        /// <MetaDataID>{a3a9913d-4df3-42a1-9424-7fbd636669ac}</MetaDataID>

        [JsonIgnore]
        public IMealCourse ServerSideMealCourse { get; }

        public SessionType SessionType { get; set; }


        public string Description { get; private set; }
        public IList<ItemsPreparationContext> FoodItemsInProgress { get; set; }
        public long StateTimestamp { get; private set; }
        public DontWaitApp.MenuData MenuData { get; set; }

        public string MealCourseUri { get; set; }
        public ItemPreparationState PreparationState { get; set; }

        IMealsController MealsController;



        /// <MetaDataID>{f2cb7dc5-4e40-4f3a-a09a-dda9dcd27a0b}</MetaDataID>
        public MealCourse(IMealCourse serverSideMealCourse, IMealsController mealsController)
        {
            MealsController = mealsController;
            MealCourseUri = OOAdvantech.Remoting.RestApi.RemotingServices.GetPersistentUri(serverSideMealCourse);
            ServerSideMealCourse = serverSideMealCourse;
            FoodItemsInProgress = serverSideMealCourse.FoodItemsInProgress;

          

            StateTimestamp = serverSideMealCourse.StateTimestamp;

            PartiallyUnderServingProcess = serverSideMealCourse.PartiallyUnderServingProcess;
            UnderServingProcess = serverSideMealCourse.UnderServingProcess;


            var sessionData = ServerSideMealCourse.SessionData;

            SessionType = sessionData.SessionType;

            Description = sessionData.Description + " - " + ServerSideMealCourse.Name;


            var storeRef = sessionData.Menu;
#if !DeviceDotNet

            storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif
            string menuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1);
            string menuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1);


            MenuData = new DontWaitApp.MenuData()
            {
                MenuName = storeRef.Name,
                MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                DefaultMealTypeUri = sessionData.DefaultMealTypeUri
            };

            MealsController.MealCourseItemsStateChanged += ServerSideMealCourse_ItemsStateChanged;
            MealsController.MealCourseChangeState += ServerSideMealCourse_ObjectChangeState;
            //serverSideMealCourse.ItemsStateChanged += ServerSideMealCourse_ItemsStateChanged;
            //serverSideMealCourse.ObjectChangeState += ServerSideMealCourse_ObjectChangeState;


            PreparationState = serverSideMealCourse.PreparationState;
        }


        public event MealCourseUpdatedHandle MealCourseUpdated;
        public event ItemsStateChangedHandle ItemsStateChanged;



        private void ServerSideMealCourse_ObjectChangeState(IMealCourse mealCourse, string member)
        {
            if (mealCourse == this.ServerSideMealCourse)
            {

                if (member == nameof(IMealCourse.FoodItems))
                {
                    FoodItemsInProgress = ServerSideMealCourse.FoodItemsInProgress;
                    var items = FoodItemsInProgress.SelectMany(x => x.PreparationItems).ToList();
                    UnderServingProcess = this.ServerSideMealCourse.UnderServingProcess;
                    PartiallyUnderServingProcess = this.ServerSideMealCourse.PartiallyUnderServingProcess;
                    MealCourseUpdated?.Invoke(this);
                }

                if (member == nameof(IMealCourse.ServingBatches))
                {
                    var underServingProcess = this.ServerSideMealCourse.UnderServingProcess;
                    var partiallyUnderServingProcess = this.ServerSideMealCourse.PartiallyUnderServingProcess;

                    try
                    {
                        var dfdd=mealCourse.Fetching(mc => mc.Caching(x => new { x.UnderServingProcess, x.PartiallyUnderServingProcess }));
                    }
                    catch (Exception error)
                    {
                        
                    }
                    UnderServingProcess = this.ServerSideMealCourse.UnderServingProcess;
                    PartiallyUnderServingProcess = this.ServerSideMealCourse.PartiallyUnderServingProcess;

                    MealCourseUpdated?.Invoke(this);
                }

                if (member == nameof(IMealCourse.Meal))
                {
                    var sessionData = ServerSideMealCourse.SessionData;
                    Description = sessionData.Description + " - " + ServerSideMealCourse.Name;
                    SessionType = sessionData.SessionType;
                    MealCourseUpdated?.Invoke(this);

                }

                if (member == nameof(IMealCourse.SessionData))
                {

                    var sessionData = ServerSideMealCourse.SessionData;
                    var storeRef = sessionData.Menu;
#if !DeviceDotNet

                    storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif
                    string menuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1);
                    string menuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1);


                    MenuData = new DontWaitApp.MenuData()
                    {
                        MenuName = storeRef.Name,
                        MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                        MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                        DefaultMealTypeUri = sessionData.DefaultMealTypeUri
                    };

                }
            }
        }

        private void ServerSideMealCourse_ItemsStateChanged(IMealCourse mealCourse, Dictionary<string, ItemPreparationState> newItemsState)
        {
            if (mealCourse == this.ServerSideMealCourse)
            {
                if (newItemsState.Any(x => x.Value.IsIntheSameOrFollowingState(ItemPreparationState.OnRoad)))
                {

                }

                foreach (var preparationItem in FoodItemsInProgress.SelectMany(x => x.PreparationItems))
                {
                    if (newItemsState.ContainsKey(preparationItem.uid))
                        preparationItem.State = newItemsState[preparationItem.uid];
                }



                var preparationState = mealCourse.PreparationState;
                if (PreparationState != preparationState)
                {
                    PreparationState = preparationState;
                    this.MealCourseUpdated?.Invoke(this);
                }
                else
                    ItemsStateChanged?.Invoke(newItemsState);

            }

        }

        internal MealCourse MealCourseUpdate(IMealCourse serverSideMealCourse)
        {

            if(StateTimestamp != serverSideMealCourse.StateTimestamp)
            {
                
                FoodItemsInProgress = serverSideMealCourse.FoodItemsInProgress;

                StateTimestamp = serverSideMealCourse.StateTimestamp;

                PartiallyUnderServingProcess = serverSideMealCourse.PartiallyUnderServingProcess;
                UnderServingProcess = serverSideMealCourse.UnderServingProcess;


                var sessionData = ServerSideMealCourse.SessionData;

                SessionType = sessionData.SessionType;

                Description = sessionData.Description + " - " + ServerSideMealCourse.Name;


                var storeRef = sessionData.Menu;
#if !DeviceDotNet

            storeRef.StorageUrl = "https://dev-localhost/" + storeRef.StorageUrl.Substring(storeRef.StorageUrl.IndexOf("devstoreaccount1"));
#endif
                string menuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1);
                string menuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1);


                MenuData = new DontWaitApp.MenuData()
                {
                    MenuName = storeRef.Name,
                    MenuRoot = storeRef.StorageUrl.Substring(0, storeRef.StorageUrl.LastIndexOf("/") + 1),
                    MenuFile = storeRef.StorageUrl.Substring(storeRef.StorageUrl.LastIndexOf("/") + 1),
                    DefaultMealTypeUri = sessionData.DefaultMealTypeUri
                };

                MealsController.MealCourseItemsStateChanged += ServerSideMealCourse_ItemsStateChanged;
                MealsController.MealCourseChangeState += ServerSideMealCourse_ObjectChangeState;
                PreparationState = serverSideMealCourse.PreparationState;

                //MealCourseUpdated?.Invoke(this);
            }

            return this;

        }


        /// <summary>
        /// Some of the items are under the process of serving / delivery
        /// </summary>
        public bool PartiallyUnderServingProcess { get; private set; }

        /// <summary>
        /// All items are under the process of serving / delivery
        /// </summary>
        public bool UnderServingProcess { get; private set; }

    }
}