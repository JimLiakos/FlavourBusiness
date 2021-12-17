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
            OOAdvantech.Linq.Storage servicesContextStorage = new OOAdvantech.Linq.Storage(ObjectStorage.GetStorageOfObject(this));



            foreach (var servicePointPreparationItems in (from openSession in ServicesContextRunTime.Current.OpenSessions
                                                          where openSession.CashierStation == this
                                                          from sessionPart in openSession.PartialClientSessions
                                                          from itemPreparation in sessionPart.FlavourItems
                                                          orderby itemPreparation.PreparedAtForecast
                                                          group itemPreparation by openSession into ServicePointItems
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


            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                foreach (var preparationItem in (from servicePointPreparationItems in ServicePointsPreparationItems
                                                 from preparationItem in servicePointPreparationItems.PreparationItems
                                                 select preparationItem).OfType<ItemPreparation>())
                {
                    PrintReceiptCheck(preparationItem);
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

        private void PrintReceiptCheck(ItemPreparation itemPreparation)
        {
            var printReceiptCondition = PrintReceiptsConditions.Where(x => x.ServicePointType == itemPreparation.ClientSession.MainSession.ServicePoint.ServicePointType).FirstOrDefault();
            if (printReceiptCondition.ItemState != null && itemPreparation.IsInFollowingState(printReceiptCondition.ItemState.Value))
            {
                if (itemPreparation.IsInFollowingState(ItemPreparationState.Serving))
                {
                    if (itemPreparation.ServedInTheBatch.PreparedItems.Where(x => x.State == ItemPreparationState.OnRoad).Count() == itemPreparation.ServedInTheBatch.PreparedItems.Count())
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
                FinanceFacade.Transaction transaction = null;
                receiptItems = receiptItems.OfType<ItemPreparation>().Where(x => string.IsNullOrEmpty(x.TransactionUri)).OfType<IItemPreparation>().ToList();

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                {
                    ITaxAuthority taxAuthority = null;

                    foreach (var receiptItem in receiptItems.OfType<ItemPreparation>())
                    {
                        if (transaction == null)
                            transaction = new FinanceFacade.Transaction();

                        taxAuthority = (receiptItem.MenuItem as MenuModel.MenuItem).Menu.TaxAuthority;
                        var transactionItem = new Item() { Name = receiptItem.Name, Quantity = (decimal)receiptItem.Quantity, Price = (decimal)receiptItem.Price, uid = receiptItem.uid };
                        decimal amount = transactionItem.Amount;
                        foreach (var tax in (receiptItem.MenuItem as MenuModel.MenuItem).TaxableType.Taxes)
                            transactionItem.AddTax(new TaxAmount() { AccountID = tax.AccountID, Amount = transactionItem.Amount / (1 + (decimal)tax.TaxRate) });
                    }
                    stateTransition.Consistent = true;
                }

                if (transaction != null)
                {
                    string transactionUri = ObjectStorage.GetStorageOfObject(transaction)?.GetPersistentObjectUri(transaction);

                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        foreach (var receiptItem in receiptItems.OfType<ItemPreparation>())
                            receiptItem.TransactionUri = transactionUri;

                        stateTransition.Consistent = true;
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
                var servicePointPreparationItems = ServicePointsPreparationItems.Where(x => x.ServicePoint == flavourItem.ClientSession.ServicePoint).FirstOrDefault();
                if (!servicePointPreparationItems.PreparationItems.Contains(flavourItem))
                {
                    flavourItem.ObjectChangeState += FlavourItem_ObjectChangeState;
                    if (servicePointPreparationItems == null)
                        ServicePointsPreparationItems.Add(new ServicePointPreparationItems(flavourItem.ClientSession.MainSession, new List<IItemPreparation>() { flavourItem }));
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


        //ItemPreparationState GetPrintReceiptCondition(ServicePointType servicePointType);
        /// <MetaDataID>{4c0b9461-e4fc-422b-9f41-f60179819e3d}</MetaDataID>
        List<PrintReceiptCondition> PrintReceiptsConditions { get; set; } = new List<PrintReceiptCondition>();

    }



}