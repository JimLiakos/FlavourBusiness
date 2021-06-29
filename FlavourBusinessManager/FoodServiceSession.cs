using System;
using System.Collections.Generic;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.EndUsers;
using System.Threading.Tasks;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{a44c2bf7-1fd4-4e5a-b831-f9f7a1c381ac}</MetaDataID>
    [BackwardCompatibilityID("{a44c2bf7-1fd4-4e5a-b831-f9f7a1c381ac}")]
    [Persistent()]
    public class FoodServiceSession : IFoodServiceSession
    {

        /// <MetaDataID>{37452f52-1155-4bbb-87c5-f621b27c624c}</MetaDataID>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;
        /// <exclude>Excluded</exclude>
        DateTime _SessionEnds;

        /// <MetaDataID>{40f071e7-8255-4f31-b2b2-8a774e1fa835}</MetaDataID>
        [PersistentMember(nameof(_SessionEnds))]
        [OOAdvantech.MetaDataRepository.BackwardCompatibilityID("+2")]
        public System.DateTime SessionEnds
        {
            get
            {
                return _SessionEnds;
            }

            set
            {

                if (_SessionEnds.ToUniversalTime() != value.ToUniversalTime())
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionEnds = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        IServicePoint _ServicePoint;

        /// <MetaDataID>{f1464bf3-fe2a-4468-b8b1-0cd936988636}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public FlavourBusinessFacade.ServicesContextResources.IServicePoint ServicePoint
        {
            get
            {
                return _ServicePoint;
            }
            set
            {
                _ServicePoint = value;
            }
        }

        /// <exclude>Excluded</exclude>
        DateTime _SessionStarts;
        /// <MetaDataID>{4bb0bd71-bead-41de-a054-9e612487691e}</MetaDataID>
        [PersistentMember(nameof(_SessionStarts))]
        [BackwardCompatibilityID("+3")]
        public System.DateTime SessionStarts
        {
            get
            {
                return _SessionStarts;
            }

            set
            {
                _SessionStarts = value;
            }
        }

        /// <exclude>Excluded</exclude> 
        OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession> _PartialClientSessions = new OOAdvantech.Collections.Generic.Set<IFoodServiceClientSession>();


        /// <MetaDataID>{0aa1ebef-0011-45f0-9176-eba5e8afd63a}</MetaDataID>
        [PersistentMember(nameof(_PartialClientSessions))]
        [BackwardCompatibilityID("+4")]
        public IList<IFoodServiceClientSession> PartialClientSessions => _PartialClientSessions.AsReadOnly();




        ///// <MetaDataID>{0aa1ebef-0011-45f0-9176-eba5e8afd63a}</MetaDataID>
        //[BackwardCompatibilityID("+4")]
        //public System.Collections.Generic.IList<IFoodServiceClientSession> PartialClientSessions
        //{
        //    get
        //    {
        //        return _PartialClientSessions.AsReadOnly();
        //    }
        //}

        /// <MetaDataID>{40eff473-e07d-4486-97f2-f44bcd37c877}</MetaDataID>
        public void AddPartialSession(IFoodServiceClientSession partialSession)
        {
            if (!PartialClientSessions.Contains(partialSession))
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _PartialClientSessions.Add(partialSession);

                    stateTransition.Consistent = true;
                }
                partialSession.ObjectChangeState += PartialSession_ObjectChangeState;
                StateMachineMonitoring();
            }

        }

        /// <MetaDataID>{93821822-e7d6-4ef6-b28c-07ca8f1fa291}</MetaDataID>
        private void PartialSession_ObjectChangeState(object _object, string member)
        {
            if (member == nameof(IFoodServiceClientSession.SessionState))
                StateMachineMonitoring();
        }

        /// <MetaDataID>{fbe827bb-85e5-4ac5-a527-028149b7e116}</MetaDataID>
        object StateMachineLock = new object();

        /// <MetaDataID>{dd202b7b-1e56-45fe-a4a9-f3f26dcc645a}</MetaDataID>
        private void StateMachineMonitoring()
        {

            var partialClientSessions = PartialClientSessions;
            if (partialClientSessions.Count > 0)
            {
                //One of the messmates commits event
                if (SessionState == SessionState.Conversation && partialClientSessions.Where(
                    x => x.SessionState == ClientSessionState.Conversation || x.SessionState == ClientSessionState.ConversationStandy || x.SessionState == ClientSessionState.UrgesToDecide).Count() > 0
                    && partialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited).Count() > 0)
                {
                    SessionState = SessionState.UrgesToDecide;
                    //UrgesToDecideStateRun();
                }

                lock (StateMachineLock)
                {
                    if (SessionState == SessionState.MealValidationDelay && partialClientSessions.Where(x => x.SessionState != ClientSessionState.ItemsCommited).Count() > 0)
                        SessionState = SessionState.UrgesToDecide;
                }

                lock (StateMachineLock)
                {
                    //There aren't messmates in the ItemsCommit state
                    if (SessionState == SessionState.UrgesToDecide && partialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited).Count() == 0)
                        SessionState = SessionState.Conversation;
                }


                //All messmates are in committed state for specific timespan event
                if (partialClientSessions.Where(x => x.SessionState == ClientSessionState.ItemsCommited || x.SessionState == ClientSessionState.Inactive).Count() == partialClientSessions.Count)
                {
                    bool mealValidationDelaySessionState = false;

                    lock (StateMachineLock)
                    {
                        if (SessionState != SessionState.MealValidationDelay)
                        {
                            mealValidationDelaySessionState = true;
                            SessionState = SessionState.MealValidationDelay;
                        }
                    }

                    if(mealValidationDelaySessionState)
                        MealValidationDelayRun();
                    

                }
            }
            else
                SessionState = SessionState.Conversation;

        }

        /// <MetaDataID>{598f4be7-bfaf-40ae-b1e2-dc80d367a54f}</MetaDataID>
        private void MealValidationDelayRun()
        {
            Task.Run(() =>
            {
                var allMessmetesCommitedTimeSpanInSeconds = ServicePointRunTime.ServicesContextRunTime.Current.AllMessmetesCommitedTimeSpan;

                while (allMessmetesCommitedTimeSpanInSeconds > 0)
                {
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    allMessmetesCommitedTimeSpanInSeconds--;
                    if (SessionState != SessionState.MealValidationDelay)
                        return;
                }

                if (SessionState == SessionState.MealValidationDelay)
                {
                    SessionState = SessionState.MealMonitoring;
                    CreateAndInitMeal();
                }
            });
        }

        /// <MetaDataID>{93093ff3-2928-40af-a748-5be7660d9f39}</MetaDataID>
        private void UrgesToDecideStateRun()
        {
            //Task.Run(() =>
            //{
            //    while (SessionState == SessionState.UrgesToDecide)
            //    {
            //        var items = (from partialSession in PartialClientSessions
            //                     from itemPreparation in partialSession.FlavourItems
            //                     select itemPreparation).ToList();

            //        var serivcePoint = (from clientSession in this.PartialClientSessions
            //                            select clientSession.ServicePoint).FirstOrDefault();

            //        #region items conversations monitoring
            //        //items In Conversation
            //        var itemsInConversation = items.Where(x => x.State == FlavourBusinessFacade.RoomService.ItemPreparationState.New).ToList();
            //        var commitedItems = items.Where(x => x.State == ItemPreparationState.Committed).ToList();

            //        var itemsInConversationSessions = itemsInConversation.Select(x => x.ClientSession).Distinct().ToList();
            //        var commitedItemsSessions = commitedItems.Where(x => !itemsInConversationSessions.Contains(x.ClientSession)).Select(x => x.ClientSession).Distinct().ToList();
            //        //commitedItemsSessions defines the sessions which has not items in conversation
            //        if (commitedItemsSessions.Count > 0)
            //        {
            //            //
            //            foreach (var conversationSession in itemsInConversationSessions.OfType<FoodServiceClientSession>())
            //                conversationSession.YouMustDecide(commitedItemsSessions);
            //        }
            //        else
            //        {
            //            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //            {
            //                foreach (var conversationSession in itemsInConversationSessions.OfType<FoodServiceClientSession>())
            //                    conversationSession.YouMustDecideMessagesNumber = 0;

            //                stateTransition.Consistent = true;
            //            }
            //        }

            //        System.Threading.Thread.Sleep(5000);

            //        #endregion
            //    }
            //});
        }

        /// <MetaDataID>{0d5343f3-0c56-4e22-b66d-e4357dbd8a75}</MetaDataID>
        MenuModel.MealType MealType
        {
            get
            {
                return null;
            }
        }

        /// <MetaDataID>{1b995301-4937-4889-a8b1-be54049ff16e}</MetaDataID>
        private void CreateAndInitMeal()
        
        {


            var itemsToPrepare = (from clientSession in PartialClientSessions
                                  from itemPreparation in clientSession.FlavourItems
                                  select itemPreparation).OfType<ItemPreparation>().ToList();

            var mealType= OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri<MenuModel.IMealCourseType>((itemsToPrepare[0] as ItemPreparation).SelectedMealCourseTypeUri).Meal as MenuModel.MealType;



            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Meal = new Meal(mealType, itemsToPrepare);
                OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(_Meal);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{f7125663-4378-4d70-868d-4c90af7c98fd}</MetaDataID>
        public static TimeSpan YouMustDecideFromStartOfSession
        {
            get
            {
                return TimeSpan.FromMinutes(6);
            }
        }
        /// <MetaDataID>{3e61ac19-61f9-45cd-bc8a-a58b62a7bbcf}</MetaDataID>
        public static TimeSpan YouMustDecideFromFirstPreparationItemCommit
        {
            get
            {
                return TimeSpan.FromMinutes(2);
            }
        }

        /// <MetaDataID>{d9669276-6fef-49a6-a5db-3d56880a9958}</MetaDataID>
        public static TimeSpan YouMustDecideWhenDeviceIsIdle
        {
            get
            {
                return TimeSpan.FromMinutes(0.5);
            }
        }


        /// <MetaDataID>{115bab53-101d-444f-b364-e8f27d160e71}</MetaDataID>
        public static TimeSpan MealConversetionTime
        {
            get
            {
                return TimeSpan.FromMinutes(10);
            }
        }

        /// <exclude>Excluded</exclude>
        SessionState _SessionState;

        /// <MetaDataID>{992bbbb9-bddc-4b7d-89f9-90360d6fe4d1}</MetaDataID>
        [PersistentMember(nameof(_SessionState))]
        [BackwardCompatibilityID("+5")]
        public SessionState SessionState
        {
            get => _SessionState;
            set
            {
                if (_SessionState != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _SessionState = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>     
        IMeal _Meal;

        /// <MetaDataID>{8d538032-0365-42f6-a71a-176cf8a85f38}</MetaDataID>
        [PersistentMember(nameof(_Meal))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+6")]
        public IMeal Meal
        {
            get => _Meal; 
            set
            {
                if (_Meal != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Meal = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }




        /// <MetaDataID>{ce6107e5-bd9c-4f9f-bc82-e2070d163bf6}</MetaDataID>
        public void RemovePartialSession(IFoodServiceClientSession partialSession)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PartialClientSessions.Remove(partialSession);
                stateTransition.Consistent = true;
            }
            StateMachineMonitoring();
            partialSession.ObjectChangeState -= PartialSession_ObjectChangeState;
        }
        /// <MetaDataID>{9c60f720-9c83-4a06-b80d-687ff2517dc1}</MetaDataID>
        [ObjectActivationCall]
        internal void OnActivated()
        {

            foreach (var partialSession in PartialClientSessions)
                partialSession.ObjectChangeState += PartialSession_ObjectChangeState;

        }

        /// <MetaDataID>{a9c1e448-ba0d-4491-8520-0dc47dfdc530}</MetaDataID>
        public void MonitorTick()
        {


        }




        /// <MetaDataID>{1b4da94a-7178-4595-a8d4-5084488ee46b}</MetaDataID>
        internal void ReassignSharedItem(ItemPreparation flavourItem)
        {
            if (flavourItem.SharedInSessions.Count != 0)
            {
                var sessionID = flavourItem.SharedInSessions[0];
                var clientSession = this.PartialClientSessions.Where(x => x.SessionID == sessionID).FirstOrDefault();
                clientSession.AddItem(flavourItem);

            }
        }

    }
}