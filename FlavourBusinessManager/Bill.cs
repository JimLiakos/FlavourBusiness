using FinanceFacade;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace FlavourBusinessManager.EndUsers
{
    /// <MetaDataID>{0a80e52c-fc44-4bc3-8e84-970570d12727}</MetaDataID>
    public class Bill : MarshalByRefObject, FlavourBusinessFacade.RoomService.IBill, OOAdvantech.Remoting.IExtMarshalByRefObject
    {
        public Bill(List<IPayment> payments)
        {
            Payments=payments;

        }
        [CachingDataOnClientSide]
        public List<IPayment> Payments { get; }

        public FinanceFacade.IPayment OpenPayment
        {
            get
            {
                return Payments.Where(x => x.State!=PaymentState.Completed).FirstOrDefault();
            }
        }


        internal static List<FinanceFacade.Item> GetUnpaidItems(FoodServiceClientSession foodServiceClientSession, List<Payment> payments, List<ItemPreparation> flavourItems)
        {

            //FinanceFacade.Payment payment = null;

            Dictionary<ItemPreparation, decimal> paidItemsAmounts = new Dictionary<ItemPreparation, decimal>();
            //List<Payment> payments = sessionsPayments[foodServiceClientSession];

            #region sort by transaction date
            var tmpPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).OrderBy(x => x.TransactionDate.Value).ToList();
            tmpPayments.AddRange(payments.Where(x => x.State != FinanceFacade.PaymentState.Completed));
            payments = tmpPayments;
            #endregion

            List<FinanceFacade.Item> paymentItems = new List<FinanceFacade.Item>();


            foreach (var flavourItem in flavourItems)
            {
                decimal paidAmount = 0;
                paidAmount=flavourItem.GetPaidAmount(foodServiceClientSession, payments );
                //decimal paidQuantity = itemPayments.Sum(paidItem => paidItem.Quantity);
                FinanceFacade.Item item = null;
                decimal quantity = (decimal)flavourItem.Quantity / flavourItem.NumberOfShares;
                decimal amount = (decimal)flavourItem.ModifiedItemPrice * quantity;

                string quantityDescription = flavourItem.Quantity.ToString() + "/" + flavourItem.NumberOfShares.ToString();
                if (quantity==(int)quantity)
                    quantityDescription=((int)quantity).ToString();
                #region remove  useless decimals
                if (((decimal)(int)quantity) == quantity)
                    quantity = (int)quantity;
                #endregion
                paidItemsAmounts[flavourItem] = paidAmount;
                if (amount - paidAmount > 0)
                {
                    if (flavourItem.NumberOfShares > 1)
                        item = new Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = paidAmount };
                    else
                        item = new Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.GetItemUid(foodServiceClientSession), QuantityDescription = flavourItem.Quantity.ToString(), PaidAmount = paidAmount };
                    paymentItems.Add(item);
                }
                if (amount - paidAmount < 0)
                {
                    //if (flavourItem.NumberOfShares > 1)
                    //    item = new FinanceFacade.Item() { Name = flavourItem.FullName, Quantity = quantity, Price = itemPrice, uid = flavourItem.uid, QuantityDescription = quantityDescription, PaidAmount = paidAmount };
                    //else
                    //    item = new FinanceFacade.Item() { Name = flavourItem.FullName, Quantity = quantity, Price = itemPrice, uid = flavourItem.uid, QuantityDescription = flavourItem.Quantity.ToString(), PaidAmount = paidAmount };
                    //paymentItems.Add(item);
                }


            }


            var flavourItemsUids = flavourItems.Where(x => x.State!=ItemPreparationState.Canceled).Select(x => x.uid).ToList();



            List<Item> paidItems = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).OfType<Item>().Where(x=>x.HasPayAmountFor(foodServiceClientSession)).ToList();
            var canceledItems = paidItems.Where(x => x.Quantity > 0/*ignore netting items*/ && !flavourItemsUids.Contains(x.GetItemPreparationUid())).OfType<FinanceFacade.Item>().ToList();

            foreach (var canceledItem in canceledItems)
            {
                if (payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid == canceledItem.GetItemPreparationUid() && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity > 0)
                {
                    var nettingQuantity = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid ==canceledItem.GetItemPreparationUid() && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity;
                    var nettingItem = new FinanceFacade.Item() { Name = canceledItem.Name, Quantity = -nettingQuantity, Price = canceledItem.Price, uid = canceledItem.uid, QuantityDescription = canceledItem.QuantityDescription, PaidAmount = canceledItem.PaidAmount };
                    paymentItems.Add(nettingItem);
                }
            }
            foreach (var paidItem in paidItemsAmounts.Where(x => ((decimal)(x.Key.ModifiedItemPrice *(x.Key.Quantity/x.Key.NumberOfShares))) < x.Value))
            {

                decimal refundAmount = paidItem.Value - (decimal)(paidItem.Key.ModifiedItemPrice * paidItem.Key.Quantity/paidItem.Key.NumberOfShares);
                var nettingQuantity = refundAmount / (decimal)paidItem.Key.ModifiedItemPrice;
                string quantityDescription = nettingQuantity.ToString();// + "/" + flavourItem.NumberOfShares.ToString();
                var nettingItem = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = -nettingQuantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = 0 };
                paymentItems.Add(nettingItem);
                var item = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = (decimal)paidItem.Key.Quantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.uid, QuantityDescription = quantityDescription, PaidAmount = (decimal)(paidItem.Key.Quantity * paidItem.Key.ModifiedItemPrice) };
                paymentItems.Add(item);


                //paidItem.ModifiedItemPrice* paidItem.Key.Quantity
                //var nettingQuantity=
            }

            return paymentItems;
        }


    

    

        internal static IBill GetBillFor(FoodServiceClientSession foodServiceClientSession)
        {
            var flavourItems = foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServiceClientSession.SharedItems.OfType<ItemPreparation>()).ToList();

            string paymentIdentity = GetPaymentIdentity(foodServiceClientSession);

            List<FinanceFacade.Payment> payments = foodServiceClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments == null)
                payments = new List<FinanceFacade.Payment>();

          //  Dictionary<FoodServiceClientSession, List<Payment>> sessionsPayments = payments.GroupBy(x => foodServiceClientSession.MainSession.PartialClientSessions.OfType<FoodServiceClientSession>().Where(y => GetPaymentIdentity(y)== x.Identity).First()).ToDictionary(g => g.Key, g => g.ToList());



            List<FinanceFacade.Item> paymentItems = Bill.GetUnpaidItems(foodServiceClientSession, payments, flavourItems);

            payments = payments.OrderBy(x => x.TransactionDate).ToList();

            var payment = payments.Where(x => x.State == FinanceFacade.PaymentState.New || x.State == FinanceFacade.PaymentState.InProgress).FirstOrDefault();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                if (payment == null)
                {
                    payment = new FinanceFacade.Payment(paymentIdentity, paymentItems, foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().First().ISOCurrencySymbol);

                    ObjectStorage.GetStorageOfObject(foodServiceClientSession).CommitTransientObjectState(payment);
                    if (foodServiceClientSession.MainSession== null)
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(foodServiceClientSession);

                    foodServiceClientSession.MainSession.AddPayment(payment);
                    payments.Add(payment);

                }
                else
                {
                    payment.Update(paymentItems);
                    //move to end of list;
                    payments.Remove(payment);
                    payments.Add(payment);
                }

                stateTransition.Consistent = true;
            }
            payment.Subject = foodServiceClientSession.MainSession as FinanceFacade.IPaymentSubject;

            return new Bill(payments.OfType<FinanceFacade.IPayment>().ToList());
        }

        private static string GetPaymentIdentity(FoodServiceClientSession foodServiceClientSession)
        {
            return foodServiceClientSession.ServicesContextRunTime.ServicesContextIdentity + ";" + ObjectStorage.GetStorageOfObject(foodServiceClientSession).GetPersistentObjectUri(foodServiceClientSession);
        }

        internal static decimal GetPaidAmount(FoodServiceClientSession foodServiceClientSession, ItemPreparation flavourItem)
        {
            List<FinanceFacade.Payment> payments = foodServiceClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments  == null)
                payments  = new List<FinanceFacade.Payment>();
            return flavourItem.GetPaidAmount(foodServiceClientSession, payments);
        }



        internal static IBill GetBillFor(List<SessionItemPreparationAbbreviation> itemPreparations)
        {

            List<RoomService.ItemPreparation> itemsForTransfer = new List<RoomService.ItemPreparation>();


            foreach (var sessionitemsEntry in (from itemPreparation in itemPreparations
                                               group itemPreparation by itemPreparation.SessionID into SessionItems
                                               select SessionItems))
            {
                var itemsUids = sessionitemsEntry.Select(x => x.uid).ToList();
                var foodServiceClientSession = ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions.OfType<EndUsers.FoodServiceClientSession>().Where(x => x.SessionID == sessionitemsEntry.Key).First();
                var flavourItems = foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServiceClientSession.SharedItems.OfType<ItemPreparation>()).Where(x => itemsUids.Contains(x.uid)).ToList();


                var uids = flavourItems.Select(x => new { x.Name, uid = x.GetItemUid() }).ToArray();


            }
            return null;

        }
    }

    static class PaymentExtension
    {
        public static string GetItemPreparationUid(this Item item)
        {
            return item.uid.Split('?')[0];
        }

    

        public static decimal GetPaidAmount(this ItemPreparation flavourItem, FoodServiceClientSession foodServiceClientSession, List<Payment> payments )
        {
            decimal paidAmount;
            var itemPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).OfType<Item>().Where(x => x.GetItemPreparationUid() == flavourItem.uid).OfType<FinanceFacade.Item>().ToList();

            itemPayments= itemPayments.Where(x => x.HasPayAmountFor(foodServiceClientSession)).ToList();


            paidAmount = itemPayments.Sum(paidItem => paidItem.Amount - paidItem.PaidAmount);
            return paidAmount;
        }


        public static bool HasPayAmountFor(this Item item, FoodServiceClientSession foodServiceClientSession)
        {
            if(foodServiceClientSession?.IsWaiterSession==true)
                return true;
            else
                return item.uid.Contains(foodServiceClientSession.SessionID);

            //return item.uid.Split('?')[0];
        }
        public static string GetItemUid( this ItemPreparation flavourItem, FoodServiceClientSession foodServiceClientSession = null)
        {
            if (foodServiceClientSession != null&&!foodServiceClientSession.IsWaiterSession)
                return flavourItem.uid+"?"+foodServiceClientSession.SessionID;
            else
            {
                string sessionsIDS = null;
                flavourItem.SharedInSessions.Select(x => sessionsIDS==null ? sessionsIDS="?"+x : sessionsIDS+=",&"+x).ToList();
                return flavourItem.uid+sessionsIDS;
            }
        }

    }


}