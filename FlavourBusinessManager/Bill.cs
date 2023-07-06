using FinanceFacade;
using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using FlavourBusinessManager.ServicesContextResources;
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

        internal static List<FinanceFacade.Item> GetPayItems(FoodServiceClientSession foodServiceClientSession, List<Payment> payments, List<ItemPreparation> flavourItems)
        {



            Dictionary<ItemPreparation, decimal> paidItemsAmounts = new Dictionary<ItemPreparation, decimal>();


            #region sort by transaction date
            var tmpPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).OrderBy(x => x.TransactionDate.Value).ToList();
            tmpPayments.AddRange(payments.Where(x => x.State != FinanceFacade.PaymentState.Completed));
            payments = tmpPayments;
            #endregion

            List<FinanceFacade.Item> paymentItems = new List<FinanceFacade.Item>();


            foreach (var flavourItem in flavourItems)
            {

                decimal paidAmount = flavourItem.GetPaidAmount(foodServiceClientSession, payments);
                paidItemsAmounts[flavourItem] = paidAmount;
                FinanceFacade.Item item = null;

                int numberOfShares = flavourItem.NumberOfShares;
                if (foodServiceClientSession==null|| foodServiceClientSession.IsWaiterSession)
                    numberOfShares=1;

                decimal quantity = (decimal)flavourItem.Quantity / numberOfShares;
                decimal amount = (decimal)flavourItem.ModifiedItemPrice * quantity;

                string quantityDescription = flavourItem.Quantity.ToString() + "/" + numberOfShares.ToString();
                if (quantity==(int)quantity)
                    quantityDescription=((int)quantity).ToString();
                #region remove  useless decimals
                if (((decimal)(int)quantity) == quantity)
                    quantity = (int)quantity;
                #endregion

                if (amount - paidAmount >= 0)
                {
                    if (numberOfShares > 1)
                    {
                        item = new Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = paidAmount };
                        paymentItems.Add(item);
                    }
                    else
                    {
                        item = new Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.GetItemUid(foodServiceClientSession), QuantityDescription = flavourItem.Quantity.ToString(), PaidAmount = paidAmount };
                        paymentItems.Add(item);
                    }
                }
            }

            List<string> sessionAllFlavourItemsUids = foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServiceClientSession.SharedItems.OfType<ItemPreparation>()).Where(x => x.State!=ItemPreparationState.Canceled).Select(x => x.uid).ToList();

            if(foodServiceClientSession.IsWaiterSession)
                sessionAllFlavourItemsUids=foodServiceClientSession.MainSession.PartialClientSessions.SelectMany(x=>x.FlavourItems).Select(x => x.uid).ToList();

            List<Item> paidItems = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).OfType<Item>().Where(x => x.HasPayAmountFor(foodServiceClientSession)).ToList();
            List<Item> canceledItems = paidItems.Where(x => x.Quantity > 0/*ignore netting items*/ && !sessionAllFlavourItemsUids.Contains(x.GetItemPreparationUid())).OfType<FinanceFacade.Item>().ToList();

            //The items where canceled
            foreach (var canceledItem in canceledItems)
            {
                if (payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid == canceledItem.GetItemPreparationUid() && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity > 0)
                {
                    var nettingQuantity = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid ==canceledItem.GetItemPreparationUid() && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity;
                    var nettingItem = new FinanceFacade.Item() { Name = canceledItem.Name, Quantity = -nettingQuantity, Price = canceledItem.Price, uid = canceledItem.uid, QuantityDescription = canceledItem.QuantityDescription, PaidAmount = canceledItem.PaidAmount };
                    paymentItems.Add(nettingItem);
                }
            }

            //The items with price less than price where paid
            foreach (var paidItem in paidItemsAmounts.Where(paidItemEntry => ((decimal)(paidItemEntry.Key.ModifiedItemPrice *(paidItemEntry.Key.Quantity/paidItemEntry.Key.NumberOfShares))) < paidItemEntry.Value))
            {

                decimal refundAmount = paidItem.Value - (decimal)(paidItem.Key.ModifiedItemPrice * paidItem.Key.Quantity/paidItem.Key.NumberOfShares);
                var nettingQuantity = refundAmount / (decimal)paidItem.Key.ModifiedItemPrice;
                string quantityDescription = nettingQuantity.ToString();// + "/" + numberOfShares.ToString();
                var nettingItem = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = -nettingQuantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = 0 };
                paymentItems.Add(nettingItem); // entry with  refund amount "netting item" 

                //Adds the paid item 
                quantityDescription=FlavourBusinessToolKit.Fraction.RealToFraction(paidItem.Key.Quantity, 0.1).ToString();
                var item = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = (decimal)paidItem.Key.Quantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = (decimal)(paidItem.Key.Quantity * paidItem.Key.ModifiedItemPrice) };
                paymentItems.Add(item);


                //paidItem.ModifiedItemPrice* paidItem.Key.Quantity
                //var nettingQuantity=
            }

            return paymentItems;
        }

        //internal static List<FinanceFacade.Item> GetPayItems(FoodServiceClientSession foodServiceClientSession, List<Payment> payments, List<ItemPreparation> flavourItems)
        //{



        //    Dictionary<ItemPreparation, decimal> paidItemsAmounts = new Dictionary<ItemPreparation, decimal>();


        //    #region sort by transaction date
        //    var tmpPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).OrderBy(x => x.TransactionDate.Value).ToList();
        //    tmpPayments.AddRange(payments.Where(x => x.State != FinanceFacade.PaymentState.Completed));
        //    payments = tmpPayments;
        //    #endregion

        //    List<FinanceFacade.Item> paymentItems = new List<FinanceFacade.Item>();


        //    foreach (var flavourItem in flavourItems)
        //    {

        //        decimal paidAmount = flavourItem.GetPaidAmount(foodServiceClientSession, payments);
        //        paidItemsAmounts[flavourItem] = paidAmount;
        //        FinanceFacade.Item item = null;

        //        int numberOfShares = flavourItem.NumberOfShares;
        //        if (foodServiceClientSession==null|| foodServiceClientSession.IsWaiterSession)
        //            numberOfShares=1;

        //        decimal quantity = (decimal)flavourItem.Quantity / numberOfShares;
        //        decimal amount = (decimal)flavourItem.ModifiedItemPrice * quantity;

        //        string quantityDescription = flavourItem.Quantity.ToString() + "/" + numberOfShares.ToString();
        //        if (quantity==(int)quantity)
        //            quantityDescription=((int)quantity).ToString();
        //        #region remove  useless decimals
        //        if (((decimal)(int)quantity) == quantity)
        //            quantity = (int)quantity;
        //        #endregion

        //        if (amount - paidAmount >= 0)
        //        {
        //            if (numberOfShares > 1)
        //            {
        //                item = new Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = paidAmount };
        //                paymentItems.Add(item);
        //            }
        //            else
        //            {
        //                item = new Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.GetItemUid(foodServiceClientSession), QuantityDescription = flavourItem.Quantity.ToString(), PaidAmount = paidAmount };
        //                paymentItems.Add(item);

        //            }

        //        }
        //    }
        //    var sessionAllFlavourItemsUids = foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServiceClientSession.SharedItems.OfType<ItemPreparation>()).Where(x => x.State!=ItemPreparationState.Canceled).Select(x => x.uid).ToList();



        //    List<Item> paidItems = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).OfType<Item>().Where(x => x.HasPayAmountFor(foodServiceClientSession)).ToList();
        //    List<Item> canceledItems = paidItems.Where(x => x.Quantity > 0/*ignore netting items*/ && !sessionAllFlavourItemsUids.Contains(x.GetItemPreparationUid())).OfType<FinanceFacade.Item>().ToList();

        //    //The items where canceled
        //    foreach (var canceledItem in canceledItems)
        //    {
        //        if (payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid == canceledItem.GetItemPreparationUid() && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity > 0)
        //        {
        //            var nettingQuantity = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid ==canceledItem.GetItemPreparationUid() && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity;
        //            var nettingItem = new FinanceFacade.Item() { Name = canceledItem.Name, Quantity = -nettingQuantity, Price = canceledItem.Price, uid = canceledItem.uid, QuantityDescription = canceledItem.QuantityDescription, PaidAmount = canceledItem.PaidAmount };
        //            paymentItems.Add(nettingItem);
        //        }
        //    }

        //    //The items with price less than price where paid
        //    foreach (var paidItem in paidItemsAmounts.Where(paidItemEntry => ((decimal)(paidItemEntry.Key.ModifiedItemPrice *(paidItemEntry.Key.Quantity/paidItemEntry.Key.NumberOfShares))) < paidItemEntry.Value))
        //    {

        //        decimal refundAmount = paidItem.Value - (decimal)(paidItem.Key.ModifiedItemPrice * paidItem.Key.Quantity/paidItem.Key.NumberOfShares);
        //        var nettingQuantity = refundAmount / (decimal)paidItem.Key.ModifiedItemPrice;
        //        string quantityDescription = nettingQuantity.ToString();// + "/" + numberOfShares.ToString();
        //        var nettingItem = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = -nettingQuantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = 0 };
        //        paymentItems.Add(nettingItem); // entry with  refund amount "netting item" 

        //        //Adds the paid item 
        //        quantityDescription=FlavourBusinessToolKit.Fraction.RealToFraction(paidItem.Key.Quantity, 0.1).ToString();
        //        var item = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = (decimal)paidItem.Key.Quantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.GetItemUid(foodServiceClientSession), QuantityDescription = quantityDescription, PaidAmount = (decimal)(paidItem.Key.Quantity * paidItem.Key.ModifiedItemPrice) };
        //        paymentItems.Add(item);


        //        //paidItem.ModifiedItemPrice* paidItem.Key.Quantity
        //        //var nettingQuantity=
        //    }

        //    return paymentItems;
        //}






        internal static IBill GetBillFor(FoodServiceClientSession foodServiceClientSession)
        {
            var flavourItems = foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServiceClientSession.SharedItems.OfType<ItemPreparation>()).ToList();

            string paymentIdentity = foodServiceClientSession.GetPaymentIdentity();

            List<FinanceFacade.Payment> payments = foodServiceClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments == null)
                payments = new List<FinanceFacade.Payment>();

            //  Dictionary<FoodServiceClientSession, List<Payment>> sessionsPayments = payments.GroupBy(x => foodServiceClientSession.MainSession.PartialClientSessions.OfType<FoodServiceClientSession>().Where(y => y.GetPaymentIdentity()== x.Identity).First()).ToDictionary(g => g.Key, g => g.ToList());



            List<FinanceFacade.Item> paymentItems = Bill.GetPayItems(foodServiceClientSession, payments, flavourItems);

            payments = payments.OrderBy(x => x.TransactionDate).ToList();

            var payment = payments.Where(x => (x.State == FinanceFacade.PaymentState.New || x.State == FinanceFacade.PaymentState.InProgress)&&x.Identity==paymentIdentity).FirstOrDefault();


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

                if (foodServiceClientSession.MainSession.ServicePoint is HallServicePoint)
                {
                    if (!string.IsNullOrWhiteSpace(foodServiceClientSession.UserLanguageCode))
                        payment.Description=string.Format(Properties.Resources.ResourceManager.GetString("TablePaymentDescription", System.Globalization.CultureInfo.GetCultureInfo(foodServiceClientSession.UserLanguageCode)), foodServiceClientSession.MainSession.ServicePoint.Description);
                    else
                        payment.Description=string.Format(Properties.Resources.TablePaymentDescription, foodServiceClientSession.MainSession.ServicePoint.Description);
                }

                stateTransition.Consistent = true;
            }
            payment.Subject = foodServiceClientSession.MainSession as FinanceFacade.IPaymentSubject;

            return new Bill(payments.OfType<FinanceFacade.IPayment>().Where(x => x.State==PaymentState.Completed||x.Identity==paymentIdentity).ToList());
        }



        internal static decimal GetPaidAmount(FoodServiceClientSession foodServiceClientSession, ItemPreparation flavourItem)
        {
            List<FinanceFacade.Payment> payments = foodServiceClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments  == null)
                payments  = new List<FinanceFacade.Payment>();
            return flavourItem.GetPaidAmount(foodServiceClientSession, payments);
        }

        internal static IBill GetBillFor(List<SessionItemPreparationAbbreviation> itemPreparations, FoodServiceClientSession waiterFoodServicesClientSession)
        {

            List<RoomService.ItemPreparation> itemsForTransfer = new List<RoomService.ItemPreparation>();
            //List<ItemPreparation> flavourItems = new List<ItemPreparation>();
            string paymentIdentity = waiterFoodServicesClientSession.GetPaymentIdentity();

            List<FinanceFacade.Payment> payments = waiterFoodServicesClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments == null)
                payments = new List<FinanceFacade.Payment>();

            List<FinanceFacade.Item> paymentItems = new List<Item>();

            //foreach (var sessionitemsEntry in (from itemPreparation in itemPreparations
            //                                   group itemPreparation by itemPreparation.SessionID into SessionItems
            //                                   select SessionItems))
            //{
            //    var itemsUids = sessionitemsEntry.Select(x => x.uid).ToList();
            //    var foodServicesClientSession = ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions.OfType<EndUsers.FoodServiceClientSession>().Where(x => x.SessionID == sessionitemsEntry.Key).First();
            //    var sessionFlavourItems = foodServicesClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServicesClientSession.SharedItems.OfType<ItemPreparation>()).Where(x => itemsUids.Contains(x.uid)).ToList();
            //    flavourItems=flavourItems.Union(sessionFlavourItems).ToList();
            //    foreach (var item in Bill.GetPayItems(foodServicesClientSession, payments, sessionFlavourItems))
            //    {
            //        var itemPreparation = sessionitemsEntry.Where(x => x.uid==item.GetItemPreparationUid()).FirstOrDefault();
            //        if (itemPreparation==null)
            //            continue;
            //        if (itemPreparation.Quantity!=null)
            //        {
            //            decimal unpaidAmount = item.Amount-item.PaidAmount;
            //            decimal billItemAmount = ((decimal)itemPreparation.Quantity.Value*item.Price);
            //            decimal billItemPaidAmount = 0;
            //            if (billItemAmount>unpaidAmount)
            //                billItemPaidAmount=billItemAmount-unpaidAmount;
            //            var newItem = new Item() { Name = item.Name, Quantity =(decimal)itemPreparation.Quantity.Value, Price = item.Price, uid = item.uid+="&"+waiterFoodServicesClientSession.SessionID, QuantityDescription = FlavourBusinessToolKit.Fraction.RealToFraction(itemPreparation.Quantity.Value, 0.1).ToString(), PaidAmount = billItemPaidAmount };
            //            paymentItems.Add(newItem);
            //        }
            //        else
            //        {
            //            item.uid+="&"+waiterFoodServicesClientSession.SessionID;
            //            paymentItems.Add(item);
            //        }
            //    }

            //}
            var itemsUids = itemPreparations.Select(x => x.uid).ToList();
            var flavourItems = waiterFoodServicesClientSession.MainSession.PartialClientSessions.SelectMany(x=>x.FlavourItems).OfType<ItemPreparation>().Where(x => itemsUids.Contains(x.uid)).ToList();
            foreach (var item in Bill.GetPayItems(waiterFoodServicesClientSession, payments, flavourItems))
            {
                   
                var itemPreparation = itemPreparations.Where(x => x.uid==item.GetItemPreparationUid()).FirstOrDefault();
                if (itemPreparation==null)
                    continue;
                if (itemPreparation.Quantity!=null)
                {
                    decimal unpaidAmount = item.Amount-item.PaidAmount;
                    decimal billItemAmount = ((decimal)itemPreparation.Quantity.Value*item.Price);
                    decimal billItemPaidAmount = 0;
                    if (billItemAmount>unpaidAmount)
                        billItemPaidAmount=billItemAmount-unpaidAmount;
                    var newItem = new Item() { Name = item.Name, Quantity =(decimal)itemPreparation.Quantity.Value, Price = item.Price, uid = item.uid+="&"+waiterFoodServicesClientSession.SessionID, QuantityDescription = FlavourBusinessToolKit.Fraction.RealToFraction(itemPreparation.Quantity.Value, 0.1).ToString(), PaidAmount = billItemPaidAmount };
                    paymentItems.Add(newItem);
                }
                else
                {
                    item.uid+="&"+waiterFoodServicesClientSession.SessionID;
                    paymentItems.Add(item);
                }
            }



            payments=payments.Where(x => x.State==PaymentState.Completed||x.Identity==paymentIdentity).ToList();
            payments = payments.OrderBy(x => x.TransactionDate).ToList();

            var payment = payments.Where(x => (x.State == FinanceFacade.PaymentState.New || x.State == FinanceFacade.PaymentState.InProgress)&&x.Identity==paymentIdentity).FirstOrDefault();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                if (payment == null)
                {
                    payment = new FinanceFacade.Payment(paymentIdentity, paymentItems, flavourItems.OfType<ItemPreparation>().First().ISOCurrencySymbol);

                    ObjectStorage.GetStorageOfObject(waiterFoodServicesClientSession).CommitTransientObjectState(payment);
                    if (waiterFoodServicesClientSession.MainSession== null)
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(waiterFoodServicesClientSession);

                    waiterFoodServicesClientSession.MainSession.AddPayment(payment);
                    payments.Add(payment);

                }
                else
                {
                    payment.Update(paymentItems);
                    //move to end of list;
                    payments.Remove(payment);
                    payments.Add(payment);
                }

                if (waiterFoodServicesClientSession.MainSession.ServicePoint is HallServicePoint)
                {
                    if (!string.IsNullOrWhiteSpace(waiterFoodServicesClientSession.UserLanguageCode))
                        payment.Description=string.Format(Properties.Resources.ResourceManager.GetString("TablePaymentDescription", System.Globalization.CultureInfo.GetCultureInfo(waiterFoodServicesClientSession.UserLanguageCode)), waiterFoodServicesClientSession.MainSession.ServicePoint.Description);
                    else
                        payment.Description=string.Format(Properties.Resources.TablePaymentDescription, waiterFoodServicesClientSession.MainSession.ServicePoint.Description);
                }


                stateTransition.Consistent = true;
            }
            payment.Subject = waiterFoodServicesClientSession.MainSession as FinanceFacade.IPaymentSubject;

            return new Bill(payments.OfType<FinanceFacade.IPayment>().ToList());

        }


        internal static IBill GetBillFor_old(List<SessionItemPreparationAbbreviation> itemPreparations, FoodServiceClientSession waiterFoodServicesClientSession)
        {

            List<RoomService.ItemPreparation> itemsForTransfer = new List<RoomService.ItemPreparation>();
            List<ItemPreparation> flavourItems = new List<ItemPreparation>();
            string paymentIdentity = waiterFoodServicesClientSession.GetPaymentIdentity();

            List<FinanceFacade.Payment> payments = waiterFoodServicesClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments == null)
                payments = new List<FinanceFacade.Payment>();

            List<FinanceFacade.Item> paymentItems = new List<Item>();

            foreach (var sessionitemsEntry in (from itemPreparation in itemPreparations
                                               group itemPreparation by itemPreparation.SessionID into SessionItems
                                               select SessionItems))
            {
                var itemsUids = sessionitemsEntry.Select(x => x.uid).ToList();
                var foodServicesClientSession = ServicePointRunTime.ServicesContextRunTime.Current.OpenClientSessions.OfType<EndUsers.FoodServiceClientSession>().Where(x => x.SessionID == sessionitemsEntry.Key).First();
                var sessionFlavourItems = foodServicesClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServicesClientSession.SharedItems.OfType<ItemPreparation>()).Where(x => itemsUids.Contains(x.uid)).ToList();
                flavourItems=flavourItems.Union(sessionFlavourItems).ToList();
                foreach (var item in Bill.GetPayItems(foodServicesClientSession, payments, sessionFlavourItems))
                {
                    var itemPreparation = sessionitemsEntry.Where(x => x.uid==item.GetItemPreparationUid()).FirstOrDefault();
                    if (itemPreparation==null)
                        continue;
                    if (itemPreparation.Quantity!=null)
                    {
                        decimal unpaidAmount = item.Amount-item.PaidAmount;
                        decimal billItemAmount = ((decimal)itemPreparation.Quantity.Value*item.Price);
                        decimal billItemPaidAmount = 0;
                        if (billItemAmount>unpaidAmount)
                            billItemPaidAmount=billItemAmount-unpaidAmount;
                        var newItem = new Item() { Name = item.Name, Quantity =(decimal)itemPreparation.Quantity.Value, Price = item.Price, uid = item.uid+="&"+waiterFoodServicesClientSession.SessionID, QuantityDescription = FlavourBusinessToolKit.Fraction.RealToFraction(itemPreparation.Quantity.Value, 0.1).ToString(), PaidAmount = billItemPaidAmount };
                        paymentItems.Add(newItem);
                    }
                    else
                    {
                        item.uid+="&"+waiterFoodServicesClientSession.SessionID;
                        paymentItems.Add(item);
                    }
                }

            }

            payments=payments.Where(x => x.State==PaymentState.Completed||x.Identity==paymentIdentity).ToList();
            payments = payments.OrderBy(x => x.TransactionDate).ToList();

            var payment = payments.Where(x => (x.State == FinanceFacade.PaymentState.New || x.State == FinanceFacade.PaymentState.InProgress)&&x.Identity==paymentIdentity).FirstOrDefault();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                if (payment == null)
                {
                    payment = new FinanceFacade.Payment(paymentIdentity, paymentItems, flavourItems.OfType<ItemPreparation>().First().ISOCurrencySymbol);

                    ObjectStorage.GetStorageOfObject(waiterFoodServicesClientSession).CommitTransientObjectState(payment);
                    if (waiterFoodServicesClientSession.MainSession== null)
                        (ServicesContextRunTime.Current.MealsController as MealsController).AutoMealParticipation(waiterFoodServicesClientSession);

                    waiterFoodServicesClientSession.MainSession.AddPayment(payment);
                    payments.Add(payment);

                }
                else
                {
                    payment.Update(paymentItems);
                    //move to end of list;
                    payments.Remove(payment);
                    payments.Add(payment);
                }

                if (waiterFoodServicesClientSession.MainSession.ServicePoint is HallServicePoint)
                {
                    if (!string.IsNullOrWhiteSpace(waiterFoodServicesClientSession.UserLanguageCode))
                        payment.Description=string.Format(Properties.Resources.ResourceManager.GetString("TablePaymentDescription", System.Globalization.CultureInfo.GetCultureInfo(waiterFoodServicesClientSession.UserLanguageCode)), waiterFoodServicesClientSession.MainSession.ServicePoint.Description);
                    else
                        payment.Description=string.Format(Properties.Resources.TablePaymentDescription, waiterFoodServicesClientSession.MainSession.ServicePoint.Description);
                }


                stateTransition.Consistent = true;
            }
            payment.Subject = waiterFoodServicesClientSession.MainSession as FinanceFacade.IPaymentSubject;

            return new Bill(payments.OfType<FinanceFacade.IPayment>().ToList());

        }




    }

    /// <MetaDataID>{cc28b19f-f926-4a56-8dd4-50e94d5419f2}</MetaDataID>
    static class PaymentExtension
    {
        public static string GetItemPreparationUid(this Item item)
        {
            return item.uid.Split('?')[0];
        }



        public static decimal GetPaidAmount(this ItemPreparation flavourItem, FoodServiceClientSession foodServiceClientSession, List<Payment> payments)
        {
            decimal paidAmount;
            var itemPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).OfType<Item>().Where(x => x.GetItemPreparationUid() == flavourItem.uid).OfType<FinanceFacade.Item>().ToList();

            itemPayments= itemPayments.Where(x => x.HasPayAmountFor(foodServiceClientSession)).ToList();


            paidAmount = itemPayments.Sum(paidItem => paidItem.Amount - paidItem.PaidAmount);
            return paidAmount;
        }

    

        public static bool HasPayAmountFor(this Item item, FoodServiceClientSession foodServiceClientSession)
        {
            if (foodServiceClientSession?.IsWaiterSession==true)
                return true;
            else
                return item.uid.Contains(foodServiceClientSession.SessionID);

            //return item.uid.Split('?')[0];
        }
        public static string GetItemUid(this ItemPreparation flavourItem, FoodServiceClientSession foodServiceClientSession)
        {
            if (foodServiceClientSession != null&&!foodServiceClientSession.IsWaiterSession)
                return flavourItem.uid+"?"+foodServiceClientSession.SessionID;
            else
            {
                string sessionsIDS = null;
                flavourItem.SharedInSessions.Select(x => sessionsIDS==null ? sessionsIDS="?"+foodServiceClientSession.SessionID+"&"+x : sessionsIDS+="&"+x).ToList();
                return flavourItem.uid+sessionsIDS;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="foodServiceClientSession">
        /// </param>
        /// <returns></returns>
        public static List<Payment> GetPayments(this FoodServiceClientSession foodServiceClientSession)
        {
            List<FinanceFacade.Payment> payments = foodServiceClientSession.MainSession?.BillingPayments.OfType<FinanceFacade.Payment>().ToList();
            if (payments == null)
                payments = new List<FinanceFacade.Payment>();
            string paymentIdentity = foodServiceClientSession.GetPaymentIdentity();
            return payments.Where(x => x.Identity==paymentIdentity).ToList();

        }
        public static string GetPaymentIdentity(this FoodServiceClientSession foodServiceClientSession)
        {
            return foodServiceClientSession.ServicesContextRunTime.ServicesContextIdentity + ";" + ObjectStorage.GetStorageOfObject(foodServiceClientSession).GetPersistentObjectUri(foodServiceClientSession);
        }


    }
}