using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceFacade;
using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.RoomService;
using FlavourBusinessManager.ServicePointRunTime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{f0a5e89f-1b4c-451f-b440-ee742c9044e6}</MetaDataID>
    [BackwardCompatibilityID("{f0a5e89f-1b4c-451f-b440-ee742c9044e6}")]
    [Persistent()]
    public class CashierStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICashierStation, ICashiersStationRuntime
    {
        /// <exclude>Excluded</exclude>
        string _CashierStationIdentity;
        /// <MetaDataID>{57fea597-c836-44a8-a747-e6b9885eacc1}</MetaDataID>
        [PersistentMember(nameof(_CashierStationIdentity))]
        [BackwardCompatibilityID("+5")]
        public string CashierStationIdentity
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CashierStationIdentity))
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
                        _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
                        while (_DeviceCredentialKeyAbbreviation == null)
                        {
                            _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
                            _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
                        }

                        stateTransition.Consistent = true;
                    }
                }

                return _CashierStationIdentity;
            }
            private set
            {

                if (_CashierStationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CashierStationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _DeviceCredentialKeyAbbreviation;

        /// <MetaDataID>{0ee76ad6-4cbb-4e40-88ca-b3e12020d582}</MetaDataID>
        [PersistentMember(nameof(_DeviceCredentialKeyAbbreviation))]
        [BackwardCompatibilityID("+4")]
        public string DeviceCredentialKeyAbbreviation
        {
            get => _DeviceCredentialKeyAbbreviation;
            private set
            {

                if (_DeviceCredentialKeyAbbreviation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceCredentialKeyAbbreviation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{dcddcd6e-8523-46fa-b078-4ea706a3d471}</MetaDataID>
        List<ServicePointPreparationItems> ServicePointsPreparationItems = new List<ServicePointPreparationItems>();

        /// <MetaDataID>{516fa955-dc1a-4115-ba39-01eba844f1f9}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {

            Task.Run(() =>
            {

                foreach (var servicePointPreparationItems in (from openSession in ServicesContextRunTime.Current.OpenSessions
                                                          where openSession.CashierStation == this && openSession.Meal != null
                                                          //from sessionPart in openSession.PartialClientSessions
                                                          from mealCourse in openSession.Meal.Courses
                                                          from itemPreparation in mealCourse.FoodItems
                                                          orderby itemPreparation.PreparedAtForecast
                                                          group itemPreparation by mealCourse into ServicePointItems
                                                          select ServicePointItems))


            {
                var preparationItems = new System.Collections.Generic.List<IItemPreparation>();
                foreach (var item in servicePointPreparationItems.OfType<RoomService.ItemPreparation>())
                {
                    if (item.MenuItem == null)
                        item.LoadMenuItem();


                    preparationItems.Add(item);
                    item.ObjectChangeState += FlavourItem_ObjectChangeState;
                }
                ServicePointsPreparationItems.Add(new ServicePointPreparationItems(servicePointPreparationItems.Key, preparationItems));

            }


            if (!string.IsNullOrWhiteSpace(PrintReceiptsConditionsJson))
                PrintReceiptsConditions = OOAdvantech.Json.JsonConvert.DeserializeObject<List<PrintReceiptCondition>>(PrintReceiptsConditionsJson);

            if (GetPrintReceiptCondition(ServicePointType.Delivery) == null)
                PrintReceiptsConditions.Add(new PrintReceiptCondition() { ServicePointType = ServicePointType.Delivery, ItemState = ItemPreparationState.OnRoad });
            if (GetPrintReceiptCondition(ServicePointType.HallServicePoint) == null)
                PrintReceiptsConditions.Add(new PrintReceiptCondition() { ServicePointType = ServicePointType.HallServicePoint, ItemState = ItemPreparationState.OnRoad });
            if (GetPrintReceiptCondition(ServicePointType.TakeAway) == null)
                PrintReceiptsConditions.Add(new PrintReceiptCondition() { ServicePointType = ServicePointType.TakeAway, ItemState = ItemPreparationState.OnRoad });

            foreach (var transaction in _Transactions.OfType<FinanceFacade.Transaction>())
            {
                transaction.ObjectChangeState += Transaction_ObjectChangeState;
            }
         
                System.Threading.Thread.Sleep(500);
                foreach (var preparationItem in (from servicePointPreparationItems in ServicePointsPreparationItems
                                                 from preparationItem in servicePointPreparationItems.PreparationItems.OfType<ItemPreparation>()
                                                 where string.IsNullOrWhiteSpace(preparationItem.TransactionUri)
                                                 select preparationItem).OfType<ItemPreparation>())
                {
                    PrintReceiptCheck(preparationItem);
                }
                CashierStationDeviceMonitoring();


            });


        }

        private void Transaction_ObjectChangeState(object _object, string member)
        {

            if (member == nameof(FinanceFacade.Transaction.PrintAgain))
            {
                lock (DeviceUpdateLock)
                {
                    if (DeviceUpdateEtag == null)
                        DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _DeviceUpdateEtag;
        /// <MetaDataID>{c78c2bbe-02f2-4094-9f11-6b1660e1f47c}</MetaDataID>
        /// <summary>
        /// Device update mechanism operates asynchronously
        /// When the state of preparation station change the change marked as timestamp
        /// The device update mechanism raise event after 3 seconds.
        /// The device catch the event end gets the changes for timestamp (DeviceUpdateEtag) 
        /// the PreparationStationRuntime clear DeviceUpdateEtag 
        /// </summary>
        [BackwardCompatibilityID("+10")]
        [PersistentMember(nameof(_DeviceUpdateEtag))]

        public string DeviceUpdateEtag
        {
            get => _DeviceUpdateEtag;
            set
            {
                if (_DeviceUpdateEtag != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceUpdateEtag = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        DateTime? RaiseEventTimeStamp;


        /// <MetaDataID>{97143314-8688-4311-a99d-c87acb729de5}</MetaDataID>
        private void CashierStationDeviceMonitoring()
        {

            Task.Run(() =>
            {

                while (true)
                {
                    lock (DeviceUpdateLock)
                    {
                        if (DeviceUpdateEtag != null)
                        {
                            long numberOfTicks = 0;
                            if (long.TryParse(DeviceUpdateEtag, out numberOfTicks))
                            {
                                DateTime myDate = new DateTime(numberOfTicks);
                                if ((DateTime.Now - myDate).TotalSeconds > 0.5)
                                {
                                    if (RaiseEventTimeStamp == null || (DateTime.UtcNow - RaiseEventTimeStamp.Value).TotalSeconds > 30)
                                    {
                                        _OpenTransactions?.Invoke(this, DeviceUpdateEtag);
                                        RaiseEventTimeStamp = DateTime.UtcNow;
                                    }
                                }
                            }
                        }
                    }
                    System.Threading.Thread.Sleep(500);
                }
            });


        }

        /// <MetaDataID>{d0089674-75cc-4000-bbc1-7e5995a43b40}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            PrintReceiptsConditionsJson = OOAdvantech.Json.JsonConvert.SerializeObject(PrintReceiptsConditions);
        }

        /// <MetaDataID>{b68665e9-c01c-421b-97fe-5673e7691267}</MetaDataID>
        private void FlavourItem_ObjectChangeState(object _object, string member)
        {
            ItemPreparation itemPreparation = (_object as ItemPreparation);

            PrintReceiptCheck(itemPreparation);
        }

        /// <MetaDataID>{5fb73605-0c1b-4106-9abc-26c71c953de7}</MetaDataID>
        private void PrintReceiptCheck(ItemPreparation itemPreparation)
        {
            if (string.IsNullOrWhiteSpace(itemPreparation.TransactionUri))
            {
                var printReceiptCondition = PrintReceiptsConditions.Where(x => x.ServicePointType == itemPreparation.ClientSession.MainSession.ServicePoint.ServicePointType).FirstOrDefault();
                if (printReceiptCondition.ItemState != null && itemPreparation.IsIntheSameOrFollowingState(printReceiptCondition.ItemState.Value))
                {
                    if (itemPreparation.ServedInTheBatch.PreparedItems.OfType<ItemPreparation>().All(x => x.IsIntheSameOrFollowingState(printReceiptCondition.ItemState.Value)))
                        PrintReceipt(itemPreparation.ServedInTheBatch.PreparedItems);

                }
            }
        }

        /// <MetaDataID>{d71448c3-59f6-4ad4-9636-bcc68c045e36}</MetaDataID>
        static object PrintReceiptLock = new object();
        /// <MetaDataID>{7a14c632-b037-4b38-8722-2d631ffbe80b}</MetaDataID>
        private void PrintReceipt(List<IItemPreparation> receiptItems)
        {


            //FinanceFacade.Transaction transaction = new FinanceFacade.Transaction();
            //var Spinach = new FinanceFacade.Item() { Name = "Spinach", Quantity = 1, Price = 7.5m };

            //Spinach.AddTax(new FinanceFacade.TaxAmount() { AccountID = "c1", Amount = 1.45m });
            //transaction.AddItem(Spinach);
            //var pasta = new FinanceFacade.Item() { Name = "Πένες Μπολονέζ", Quantity = 1, Price = 6.5m };
            //pasta.AddTax(new FinanceFacade.TaxAmount() { AccountID = "c1", Amount = 1.3m });
            //transaction.AddItem(pasta);
            //var beer = new FinanceFacade.Item() { Name = "Μπύρα Μύθος", Quantity = 1, Price = 2.5m };
            //beer.AddTax(new FinanceFacade.TaxAmount() { AccountID = "c1", Amount = 0.48m });
            //transaction.AddItem(beer);


            lock (PrintReceiptLock)
            {
                ObjectStorage objectStorage = null;
                FinanceFacade.Transaction transaction = null;
                receiptItems = receiptItems.OfType<ItemPreparation>().Where(x => string.IsNullOrEmpty(x.TransactionUri)).OfType<IItemPreparation>().ToList();

                if (receiptItems.Count > 0)
                {
                    lock (DeviceUpdateLock)
                    {
                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {

                            if (objectStorage == null)
                                objectStorage = ObjectStorage.GetStorageOfObject(this);
                            transaction = new FinanceFacade.Transaction();
                            objectStorage.CommitTransientObjectState(transaction);
                            transaction.PayeeRegistrationNumber = Issuer.VATNumber;

                            string servingBatchUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(receiptItems[0].ServedInTheBatch)?.GetPersistentObjectUri(receiptItems[0].ServedInTheBatch);

                            transaction.SetPropertyValue("ServingBatchUri", servingBatchUri);

                            _Transactions.Add(transaction);
                            (transaction as FinanceFacade.Transaction).ObjectChangeState += Transaction_ObjectChangeState;
                            stateTransition.Consistent = true;
                        }
                        try
                        {
                            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                            {
                                ITaxAuthority taxAuthority = null;
                                foreach (var receiptItem in receiptItems.OfType<ItemPreparation>().Where(x => string.IsNullOrEmpty(x.TransactionUri)))
                                {
                                    string transactionUri = ObjectStorage.GetStorageOfObject(transaction)?.GetPersistentObjectUri(transaction);
                                    taxAuthority = (receiptItem.MenuItem as MenuModel.MenuItem).Menu.TaxAuthority;
                                    var transactionItem = new Item() { Name = receiptItem.FullName, Quantity = (decimal)receiptItem.Quantity, Price = (decimal)receiptItem.Price, uid = receiptItem.uid };
                                    decimal amount = transactionItem.Amount;
                                    if ((receiptItem.MenuItem as MenuModel.MenuItem).TaxableType != null)
                                    {
                                        foreach (var tax in (receiptItem.MenuItem as MenuModel.MenuItem).TaxableType.Taxes)
                                            transactionItem.AddTax(new TaxAmount() { AccountID = tax.AccountID, Amount = (transactionItem.Amount / (1 + (decimal)tax.TaxRate)) * (decimal)tax.TaxRate, TaxRate = tax.TaxRate });
                                    }
                                    else
                                    {
                                    }
                                    transaction.AddItem(transactionItem);
                                    receiptItem.TransactionUri = transactionUri;
                                }
                                stateTransition.Consistent = true;
                            }
                            if (DeviceUpdateEtag == null)
                                DeviceUpdateEtag = System.DateTime.Now.Ticks.ToString();

                        }
                        catch (Exception error)
                        {

                            ObjectStorage.DeleteObject(transaction);
                        }
                    }
                }

            }

        }



        /// <exclude>Excluded</exclude>
        IFisicalParty _Issuer;
        /// <MetaDataID>{128db8e9-4c7a-4d52-a542-cfd6edea863d}</MetaDataID>
        [PersistentMember(nameof(_Issuer))]
        [BackwardCompatibilityID("+3")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IFisicalParty Issuer
        {
            get => _Issuer;
            set
            {
                if (_Issuer != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (value != null)
                            _Issuer = ObjectStorage.GetObjectFromUri<FisicalParty>((value as FisicalParty).FisicalPartyUri);
                        else
                            _Issuer = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{59c73ec1-a4d4-4504-8225-d975e74d15ce}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <MetaDataID>{ed608efa-fc7b-435e-b16f-a579836c0bf5}</MetaDataID>
        protected CashierStation()
        {
            SetPrintReceiptCondition(ServicePointType.Delivery, new PrintReceiptCondition() { ItemState = ItemPreparationState.OnRoad });
            SetPrintReceiptCondition(ServicePointType.HallServicePoint, new PrintReceiptCondition() { ItemState = ItemPreparationState.OnRoad });
            SetPrintReceiptCondition(ServicePointType.TakeAway, new PrintReceiptCondition() { ItemState = ItemPreparationState.PendingPreparation });
        }
        /// <MetaDataID>{57957487-b853-444c-ad61-3388b00695ef}</MetaDataID>
        public CashierStation(string servicesContextIdentity) : this()
        {
            _ServicesContextIdentity = servicesContextIdentity;
            _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
            while (_DeviceCredentialKeyAbbreviation == null)
            {
                _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
                _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;



        /// <MetaDataID>{b214da78-22cc-49ee-9200-8e27feaf7bf7}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+2")]
        public string ServicesContextIdentity
        {
            get
            {
                return _ServicesContextIdentity;
            }

            set
            {

                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <MetaDataID>{7d0f3477-7cf2-4c22-a788-4869819fe9b9}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        string PrintReceiptsConditionsJson;

        /// <MetaDataID>{00b7f6c6-e4c1-4272-9a21-3ed25cc674ae}</MetaDataID>
        public object DeviceUpdateLock = new object();

        /// <MetaDataID>{b9ca7f04-6dc4-41cd-b071-762066864cc8}</MetaDataID>
        internal void AssignItemPreparation(ItemPreparation flavourItem)
        {

            lock (DeviceUpdateLock)
            {
                var servicePointPreparationItems = ServicePointsPreparationItems.Where(x => x.ServicePoint == flavourItem.MealCourse.Meal.Session.ServicePoint).FirstOrDefault();
                if (servicePointPreparationItems == null || !servicePointPreparationItems.PreparationItems.Contains(flavourItem))
                {
                    flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;
                    if (servicePointPreparationItems == null)
                        ServicePointsPreparationItems.Add(new ServicePointPreparationItems(flavourItem.MealCourse, new List<IItemPreparation>() { flavourItem }));
                    else
                        servicePointPreparationItems.AddPreparationItem(flavourItem);
                }
            }
        }


        /// <MetaDataID>{3947afe1-bd2c-4829-b2d9-3270f40e74b0}</MetaDataID>
        public void SetPrintReceiptCondition(ServicePointType servicePointType, PrintReceiptCondition itemState)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                itemState.ServicePointType = servicePointType;
                var printReceiptsCondition = PrintReceiptsConditions.Where(x => x.ServicePointType == servicePointType).FirstOrDefault();
                if (printReceiptsCondition != null)
                {
                    printReceiptsCondition.IsPaid = itemState.IsPaid;
                    printReceiptsCondition.ItemState = itemState.ItemState;
                }
                else
                    PrintReceiptsConditions.Add(itemState);


                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{141d39a7-be70-420f-a5ae-94936e07f83d}</MetaDataID>
        public PrintReceiptCondition GetPrintReceiptCondition(ServicePointType servicePointType)
        {

            return PrintReceiptsConditions.Where(x => x.ServicePointType == servicePointType).FirstOrDefault();
        }

        /// <MetaDataID>{84c983b6-bc2f-4f5d-834b-2cfcb5efffa5}</MetaDataID>
        public List<ITransaction> GetOpenTransactions(string deviceUpdateEtag)
        {
            if (deviceUpdateEtag == DeviceUpdateEtag)
            {
                lock (DeviceUpdateLock)
                {
                    DeviceUpdateEtag = null;
                    RaiseEventTimeStamp = null;
                }
            }
            return this.Transactions.Where(x => x.InvoiceNumber == null || x.PrintAgain).ToList();
        }

        public void TransactionCommited(ITransaction transaction)
        {
            lock (DeviceUpdateLock)
            {
                bool _throw = false;
                if (_throw)
                    throw new Exception("error");

                var servertrasaction = ObjectStorage.GetObjectFromUri<FinanceFacade.Transaction>(transaction.Uri);
                servertrasaction.Update(transaction as FinanceFacade.Transaction);
                DeviceUpdateEtag = null;
                RaiseEventTimeStamp = null;
            }
        }

        public IServingBatch GetServingBatch(string servingBatchUri)
        {
            return ObjectStorage.GetObjectFromUri<IServingBatch>(servingBatchUri);
        }




        //ItemPreparationState GetPrintReceiptCondition(ServicePointType servicePointType);
        /// <MetaDataID>{4c0b9461-e4fc-422b-9f41-f60179819e3d}</MetaDataID>
        List<PrintReceiptCondition> PrintReceiptsConditions { get; set; } = new List<PrintReceiptCondition>();

        /// <exclude>Excluded</exclude>
        string _CashierStationDeviceData;

        /// <exclude>Excluded</exclude>
        event OpenTransactionsHandle _OpenTransactions;

        public event OpenTransactionsHandle OpenTransactions
        {
            add
            {
                if (_OpenTransactions != null && _OpenTransactions.GetInvocationList().Length >= 1)
                    throw new CashierStationDeviceException("Only one cashier station device allowed to subscribe");

                _OpenTransactions += value;

            }
            remove
            {
                _OpenTransactions -= value;
            }
        }

        /// <MetaDataID>{e38a5d74-b31d-44db-b669-50eca5862c8d}</MetaDataID>
        [PersistentMember(nameof(_CashierStationDeviceData))]
        [BackwardCompatibilityID("+8")]
        public string CashierStationDeviceData
        {
            get => _CashierStationDeviceData;
            set
            {

                if (_CashierStationDeviceData != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CashierStationDeviceData = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<ITransaction> _Transactions = new OOAdvantech.Collections.Generic.Set<ITransaction>();

        /// <MetaDataID>{e8489b63-60d4-4723-9d63-07af652c6bdb}</MetaDataID>
        [PersistentMember(nameof(_Transactions))]
        [BackwardCompatibilityID("+9")]
        public List<ITransaction> Transactions => _Transactions.ToThreadSafeList();



    }




}