using FinanceFacade;
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


        internal static List<FinanceFacade.Item> GetUnpaidItems(List<FinanceFacade.Payment> payments, List<ItemPreparation> flavourItems)
        {

            //FinanceFacade.Payment payment = null;

            Dictionary<ItemPreparation, decimal> paidItemsAmounts = new Dictionary<ItemPreparation, decimal>();


            #region sort by transaction date
            var tmpPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).OrderBy(x => x.TransactionDate.Value).ToList();
            tmpPayments.AddRange(payments.Where(x => x.State != FinanceFacade.PaymentState.Completed));
            payments = tmpPayments;
            #endregion

            List<FinanceFacade.Item> paymentItems = new List<FinanceFacade.Item>();


            foreach (var flavourItem in flavourItems)
            {
                decimal paidAmount = 0;
                paidAmount=GetPaidAmount(payments, flavourItem);
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
                        item = new FinanceFacade.Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.uid, QuantityDescription = quantityDescription, PaidAmount = paidAmount };
                    else
                        item = new FinanceFacade.Item() { Name = flavourItem.FullName, Quantity = quantity, Price = (decimal)flavourItem.ModifiedItemPrice, uid = flavourItem.uid, QuantityDescription = flavourItem.Quantity.ToString(), PaidAmount = paidAmount };
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



            List<FinanceFacade.IItem> paidItems = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).ToList();
            var canceledItems = paidItems.Where(x => x.Quantity > 0/*ignore netting items*/ && !flavourItemsUids.Contains(x.uid)).OfType<FinanceFacade.Item>().ToList();

            foreach (var canceledItem in canceledItems)
            {

                if (payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid == canceledItem.uid && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity > 0)
                {
                    var nettingQuantity = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid == canceledItem.uid && x.Quantity < 0 /*netting item*/).Sum(x => x.Quantity) + canceledItem.Quantity;
                    var nettingItem = new FinanceFacade.Item() { Name = canceledItem.Name, Quantity = -nettingQuantity, Price = canceledItem.Price, uid = canceledItem.uid, QuantityDescription = canceledItem.QuantityDescription, PaidAmount = canceledItem.PaidAmount };
                    paymentItems.Add(nettingItem);
                }
            }
            foreach (var paidItem in paidItemsAmounts.Where(x => ((decimal)(x.Key.ModifiedItemPrice *(x.Key.Quantity/x.Key.NumberOfShares))) < x.Value))
            {

                decimal refundAmount = paidItem.Value - (decimal)(paidItem.Key.ModifiedItemPrice * paidItem.Key.Quantity/paidItem.Key.NumberOfShares);
                var nettingQuantity = refundAmount / (decimal)paidItem.Key.ModifiedItemPrice;
                string quantityDescription = nettingQuantity.ToString();// + "/" + flavourItem.NumberOfShares.ToString();
                var nettingItem = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = -nettingQuantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.uid, QuantityDescription = quantityDescription, PaidAmount = 0 };
                paymentItems.Add(nettingItem);
                var item = new FinanceFacade.Item() { Name = paidItem.Key.Name, Quantity = (decimal)paidItem.Key.Quantity, Price = (decimal)paidItem.Key.ModifiedItemPrice, uid = paidItem.Key.uid, QuantityDescription = quantityDescription, PaidAmount = (decimal)(paidItem.Key.Quantity * paidItem.Key.ModifiedItemPrice) };
                paymentItems.Add(item);


                //paidItem.ModifiedItemPrice* paidItem.Key.Quantity
                //var nettingQuantity=
            }

            return paymentItems;
        }

        private static decimal GetPaidAmount(List<Payment> payments, ItemPreparation flavourItem)
        {
            decimal paidAmount;
            var itemPayments = payments.Where(x => x.State == FinanceFacade.PaymentState.Completed).SelectMany(x => x.Items).Where(x => x.uid == flavourItem.uid).OfType<FinanceFacade.Item>().ToList();
            paidAmount = itemPayments.Sum(paidItem => paidItem.Amount - paidItem.PaidAmount);
            return paidAmount;
        }

        internal static IBill GetBillFor(FoodServiceClientSession foodServiceClientSession)
        {
            var flavourItems = foodServiceClientSession.FlavourItems.OfType<ItemPreparation>().Union(foodServiceClientSession.SharedItems.OfType<ItemPreparation>()).ToList();

            string paymentIdentity = foodServiceClientSession.ServicesContextRunTime.ServicesContextIdentity + ";" + ObjectStorage.GetStorageOfObject(foodServiceClientSession).GetPersistentObjectUri(foodServiceClientSession);
            List<FinanceFacade.Payment> payments = foodServiceClientSession.MainSession?.BillingPayments.Where(x => x.Identity == paymentIdentity).OfType<FinanceFacade.Payment>().ToList();
            if (payments == null)
                payments = new List<FinanceFacade.Payment>();
            List<FinanceFacade.Item> paymentItems = Bill.GetUnpaidItems(payments, flavourItems);

            payments = payments.OrderBy(x => x.TransactionDate).ToList();

            //if (paymentItems.Count > 0)
            {
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
            }
            //payments sorted by TransactionDate

            return new Bill(payments.OfType<FinanceFacade.IPayment>().ToList());
        }

        internal static decimal GetPaidAmount(FoodServiceClientSession foodServiceClientSession, ItemPreparation flavourItem)
        {
            string paymentIdentity = foodServiceClientSession.ServicesContextRunTime.ServicesContextIdentity + ";" + ObjectStorage.GetStorageOfObject(foodServiceClientSession).GetPersistentObjectUri(foodServiceClientSession);
            List<FinanceFacade.Payment> foodServiceClientSessionPayments = foodServiceClientSession.MainSession?.BillingPayments.Where(x => x.Identity == paymentIdentity).OfType<FinanceFacade.Payment>().ToList();
            if (foodServiceClientSessionPayments == null)
                foodServiceClientSessionPayments = new List<FinanceFacade.Payment>();
            return GetPaidAmount(foodServiceClientSessionPayments, flavourItem);
        }
    }

}