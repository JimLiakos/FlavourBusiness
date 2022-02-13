using System;
using System.Collections.Generic;
using System.Text;
using FlavourBusinessManager.RoomService;
using OOAdvantech.Json;
using System.Linq;
namespace DontWaitApp
{
    /// <MetaDataID>{47a4f14f-cb85-4ae5-ad57-619bc3fb319e}</MetaDataID>
    public class Messmate
    { 

        [JsonIgnore]
        public readonly FlavourBusinessFacade.EndUsers.IFoodServiceClientSession ClientSession;
        public Messmate(FlavourBusinessFacade.EndUsers.IFoodServiceClientSession clientSession, List<ItemPreparation> sessionOrderItems)
        {
            ClientSession = clientSession;
            Name = ClientSession.ClientName;
            DateTimeOfLastRequest = ClientSession.DateTimeOfLastRequest;
            ClientSessionID = ClientSession.SessionID;
            MainSessionID = ClientSession.MainSession?.SessionID;
            WaiterSession = ClientSession.IsWaiterSession;
            MealConversationTimeout = clientSession.MealConversationTimeout;
            foreach (var preparationItem in ClientSession.FlavourItems.OfType<ItemPreparation>().ToList())
            {
                var sessionOrderItem = sessionOrderItems.Where(x => x.uid == preparationItem.uid).FirstOrDefault();

                if (sessionOrderItem!=null)
                    OrderItems[preparationItem.uid] = sessionOrderItem;
                else
                    OrderItems[preparationItem.uid] = preparationItem;
            }


            foreach (var preparationItem in ClientSession.SharedItems.OfType<ItemPreparation>().ToList())
            {
                var sessionOrderItem = sessionOrderItems.Where(x => x.uid == preparationItem.uid).FirstOrDefault();

                if (sessionOrderItem!=null)
                    OrderItems[preparationItem.uid] = sessionOrderItem;
                else
                    OrderItems[preparationItem.uid] = preparationItem;

                //OrderItems[preparationItem.uid] = preparationItem;
            }



        }



        public void Refresh(Dictionary<string, ItemPreparation> sessionOrderItems)
        {
            OrderItems = new Dictionary<string, ItemPreparation>();

            foreach (var preparationItem in ClientSession.FlavourItems.OfType<ItemPreparation>().ToList())
            {
                if (sessionOrderItems.ContainsKey(preparationItem.uid))
                    OrderItems[preparationItem.uid] = sessionOrderItems[preparationItem.uid];
                else
                    OrderItems[preparationItem.uid] = preparationItem;
            }


            foreach (var preparationItem in ClientSession.SharedItems.OfType<ItemPreparation>().ToList())
            {
                if (sessionOrderItems.ContainsKey(preparationItem.uid))
                    OrderItems[preparationItem.uid] = sessionOrderItems[preparationItem.uid];
                else
                    OrderItems[preparationItem.uid] = preparationItem;

                //OrderItems[preparationItem.uid] = preparationItem;
            }


        }
        public Messmate()
        {

        }

        public void AddPreparationItem(ItemPreparation itemPreparation)
        {
            if (!OrderItems.ContainsKey(itemPreparation.uid))
                OrderItems[itemPreparation.uid] = itemPreparation;
            else
                OrderItems[itemPreparation.uid].Update(itemPreparation);
        }
        public void RemovePreparationItem(ItemPreparation itemPreparation)
        {
            if (OrderItems.ContainsKey(itemPreparation.uid))
                OrderItems.Remove(itemPreparation.uid);
        }
        Dictionary<string, ItemPreparation> OrderItems = new Dictionary<string, ItemPreparation>();
        public List<ItemPreparation> PreparationItems
        {
            get
            {
                return OrderItems.Values.ToList();

            }
        }
        /// <exclude>Excluded</exclude>

        public string Name { get; set; }
        public DateTime DateTimeOfLastRequest { get; set; }
        public string ClientSessionID { get; set; }

        public string MainSessionID { get; set; }

        public bool WaiterSession { get; set; }
        public bool MealConversationTimeout { get; private set; }

        public IList<ItemPreparation> Items
        {
            get
            {
                return new List<ItemPreparation>();
            }
        }

        internal bool HasItemWithUid(string itemUid)
        {
            if (this.PreparationItems.Where(x => x.uid == itemUid).Count() > 0)
                return true;
            else
                return false;
        }

        internal void UpdateMealConversationTimeout()
        {
            MealConversationTimeout = ClientSession.MealConversationTimeout;
        }
    }
}
