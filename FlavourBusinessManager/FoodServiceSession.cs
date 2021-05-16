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

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _PartialClientSessions.Add(partialSession);
                stateTransition.Consistent = true;
            }

        }

        public static TimeSpan YouMustDecideFromStartOfSession
        {
            get
            {
                return TimeSpan.FromMinutes(6);
            }
        }
        public static TimeSpan YouMustDecideFromFirstPreparationItemCommit
        {
            get
            {
                return TimeSpan.FromMinutes(2);
            }
        }

        public static TimeSpan YouMustDecideWhenDeviceIsIdle
        {
            get
            {
                return TimeSpan.FromMinutes(0.5);
            }
        }


        public static TimeSpan MealConversetionTime
        {
            get
            {
                return TimeSpan.FromMinutes(10);
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

        }

        /// <MetaDataID>{a9c1e448-ba0d-4491-8520-0dc47dfdc530}</MetaDataID>
        public void MonitorTick()
        {

            var items = (from partialSession in PartialClientSessions
                         from itemPreparation in partialSession.FlavourItems
                         select itemPreparation).ToList();

            var serivcePoint = (from clientSession in this.PartialClientSessions
                                select clientSession.ServicePoint).FirstOrDefault();

            #region items conversations monitoring
            //items In Conversation
            var itemsInConversation = items.Where(x => x.State == FlavourBusinessFacade.RoomService.ItemPreparationState.New).ToList();
            var commitedItems = items.Where(x => x.State == ItemPreparationState.Committed).ToList();
            AssignToMealCourse(commitedItems);


            var itemsInConversationSessions = itemsInConversation.Select(x => x.ClientSession).Distinct().ToList();
            var commitedItemsSessions = commitedItems.Where(x => !itemsInConversationSessions.Contains(x.ClientSession)).Select(x => x.ClientSession).Distinct().ToList();
            //commitedItemsSessions defines the sessions which has not items in conversation
            if (commitedItemsSessions.Count > 0)
            {
                //
                foreach (var conversationSession in itemsInConversationSessions.OfType<FoodServiceClientSession>())
                    conversationSession.YouMustDecide(commitedItemsSessions);
            }
            else
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    foreach (var conversationSession in itemsInConversationSessions.OfType<FoodServiceClientSession>())
                        conversationSession.YouMustDecideMessagesNumber = 0;

                    stateTransition.Consistent = true;
                }
            }

            #endregion
        }

        private void AssignToMealCourse(List<IItemPreparation> commitedItems)
        {
            
        }

        internal void ReassignSharedItem(ItemPreparation flavourItem)
        {
            if (flavourItem.SharedInSessions.Count != 0)
            {
                var sessionID = flavourItem.SharedInSessions[0];
                var clientSession = this.PartialClientSessions.Where(x => x.SessionID == sessionID).FirstOrDefault();
                clientSession.AddItem(flavourItem);

            }
        }

        public enum SessionState
        {
            Conversation=0,
            PromptsUserToDecide=1
        }
    }
}