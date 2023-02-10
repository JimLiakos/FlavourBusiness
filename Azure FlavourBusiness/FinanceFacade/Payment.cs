
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Security;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace FinanceFacade
{
    /// <MetaDataID>{e777abd6-c8a8-45d4-921b-29c10a6c1f7e}</MetaDataID>
    [BackwardCompatibilityID("{e777abd6-c8a8-45d4-921b-29c10a6c1f7e}")]
    [Persistent()]
    public class Payment : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IPayment
    {

        /// <exclude>Excluded</exclude>
        string _PaymentGetwayRequestID;
        /// <MetaDataID>{c2e444aa-672e-4237-be55-27a58f5dad3a}</MetaDataID>
        [PersistentMember(nameof(_PaymentGetwayRequestID))]
        [BackwardCompatibilityID("+12")]
        public string PaymentGetwayRequestID
        {
            get => _PaymentGetwayRequestID;
            set
            {
                if (_PaymentGetwayRequestID!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentGetwayRequestID=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        static IPaymentFinder _PaymentFinder;
        /// <MetaDataID>{6ebc2d81-57f1-46a9-a663-e71620ab91ab}</MetaDataID>
        static public IPaymentFinder PaymentFinder
        {
            get
            {
                lock (PaymentProvidersLock)
                    return _PaymentFinder;
            }
            set
            {
                lock (PaymentProvidersLock)
                    _PaymentFinder = value;
            }
        }



        /// <MetaDataID>{66849d90-4ab5-4c1f-bc6b-23a1225e155c}</MetaDataID>
        static object PaymentProvidersLock = new object();

        /// <MetaDataID>{11024fee-9f82-4b21-bce8-33a625687522}</MetaDataID>
        public static void SetPaymentProvider(string providerName, IPaymentProvider paymentProvider)
        {
            lock (PaymentProvidersLock)
                PaymentProviders[providerName]=paymentProvider;
        }

        /// <MetaDataID>{7ecca2f0-e76d-4d57-8a35-9332d750d53c}</MetaDataID>
        static Dictionary<string, IPaymentProvider> PaymentProviders = new Dictionary<string, IPaymentProvider>();

        /// <MetaDataID>{4284efcf-ff9f-4949-b5e2-ebd47cc9f273}</MetaDataID>
        public static IPaymentProvider GetPaymentProvider(string providerName)
        {
            lock (PaymentProvidersLock)
            {
                if (PaymentProviders.ContainsKey(providerName))
                    return PaymentProviders[providerName];
                return null;
            }
        }

        /// <exclude>Excluded</exclude>
        string _PaymentGetwayID;

        /// <MetaDataID>{5fa3e801-b9aa-4782-990c-4f490dd4a464}</MetaDataID>
        [PersistentMember(nameof(_PaymentGetwayID))]
        [BackwardCompatibilityID("+11")]
        public string PaymentGetwayID
        {
            get => _PaymentGetwayID;
            set
            {

                if (_PaymentGetwayID!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentGetwayID=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        decimal _TipsAmount;

        /// <MetaDataID>{dea089ad-5a2c-450a-be8f-081f61c55b23}</MetaDataID>
        [PersistentMember(nameof(_TipsAmount))]
        [BackwardCompatibilityID("+8")]
        public decimal TipsAmount
        {
            get => _TipsAmount;
            set
            {
                if (_TipsAmount!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TipsAmount=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{fb7abb8e-43d0-40a3-9f4a-4c13bc0be5e0}</MetaDataID>
        public PaymentActionState ParseResponse(string response)
        {
            var paymentProvide = GetPaymentProvider(PaymentGetwayID);
            return paymentProvide.ParseResponse(this, response);
        }

        /// <MetaDataID>{5d873702-9a79-4fe1-ae82-02ad6c7d4652}</MetaDataID>
        [OOAdvantech.Json.JsonProperty]
        /// <MetaDataID>{4c7bdca1-0a7e-47cd-a10f-7e0306ba04fd}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        private string ItemsJson;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        decimal _Amount;
        /// <MetaDataID>{83e4b5ff-08c6-474c-a342-881f6e7919b1}</MetaDataID>
        [PersistentMember(nameof(_Amount))]
        [BackwardCompatibilityID("+1")]
        public decimal Amount
        {
            get => _Amount;
            set
            {
                if (_Amount != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Amount = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{6a08f486-7859-4ea3-ad1b-00e568d88fea}</MetaDataID>
        public void PaymentInProgress()
        {
            State = PaymentState.InProgress;
        }


        /// <MetaDataID>{12b56fcf-34ac-4b7d-b9dd-653a487afcec}</MetaDataID>
        public bool IsCompleted()
        {
            var task = Task<bool>.Run(() =>
            {
                int count = 5;
                do
                {
                    if (State==PaymentState.Completed)
                    {
                        return true;
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(PaymentGetwayID))
                        {
                            var paymentProvide = GetPaymentProvider(PaymentGetwayID);
                            if (paymentProvide!=null)
                                paymentProvide.CheckPaymentProgress(this);
                        }
                    }

                    System.Threading.Thread.Sleep(1000);
                    count--;
                    if (count<0)
                        return false;
                } while (true);

            });
            task.Wait();

            return task.Result;
        }


        /// <exclude>Excluded</exclude>
        PaymentType _PaymentType;

        /// <MetaDataID>{1b3ee769-e800-4f12-9b92-444c022e89f7}</MetaDataID>
        [PersistentMember(nameof(_PaymentType))]
        [BackwardCompatibilityID("+2")]
        public FinanceFacade.PaymentType PaymentType
        {
            get => _PaymentType;
            set
            {
                if (_PaymentType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        List<IItem> _Items = new System.Collections.Generic.List<IItem>();

        /// <MetaDataID>{f3f15640-076f-4d1e-b993-418ac5f6c521}</MetaDataID>
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("")]
        public List<IItem> Items => _Items;




        /// <MetaDataID>{762d1d71-4f66-4454-a3ed-b695f6ca611e}</MetaDataID>
        public Payment(string paymentIdentity, List<Item> paymentItems, string currency)
        {
            Identity = paymentIdentity;
            _Items = paymentItems.OfType<IItem>().ToList();
            _Currency = currency;
            _Amount = paymentItems.Sum(x => (x.Quantity * x.Price)-x.PaidAmount);


        }
        /// <MetaDataID>{a106846c-0a0d-49e0-80bd-d72f4067f22e}</MetaDataID>
        protected Payment()
        {

        }

        /// <MetaDataID>{e68605e1-f9d0-4000-a4eb-3e2e55176818}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public Payment(decimal amount, string currency, string Identity, string itemsJson, string paymentInfoFieldsJson)
        {
            _Amount = amount;
            _Currency = currency;
            _Identity = Identity;
            ItemsJson = itemsJson;
            PaymentInfoFieldsJson = paymentInfoFieldsJson;

        }


        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{eea11458-36b8-498e-868d-a56063322308}</MetaDataID>
        string _Currency;


        /// <MetaDataID>{cdab1bd4-351b-4d1e-bfd5-f69c49555adb}</MetaDataID>
        [PersistentMember(nameof(_Currency))]
        [BackwardCompatibilityID("+3")]
        public string Currency
        {
            get => _Currency;
            set
            {
                if (_Currency != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Currency = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _Identity;

        /// <MetaDataID>{05e7a9c4-2762-4283-b7d7-70319f271da3}</MetaDataID>
        [PersistentMember(nameof(_Identity))]
        [BackwardCompatibilityID("+4")]
        public string Identity
        {
            get => _Identity;
            set
            {
                if (_Identity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Identity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        PaymentState _State;

        /// <MetaDataID>{a4fbb956-f9d0-44c3-99d2-96e8de5c3e93}</MetaDataID>
        [PersistentMember(nameof(_State))]
        [BackwardCompatibilityID("+5")]
        public PaymentState State
        {
            get => _State;
            set
            {
                if (_State != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _State = value;

                        if (_State==PaymentState.Completed)
                            TransactionDate= DateTime.Now;

                        stateTransition.Consistent = true;
                    }


                }


                OOAdvantech.Transactions.Transaction.RunOnTransactionCompleted(() =>
                {
                    _ObjectChangeState?.Invoke(this, nameof(State));
                });


            }
        }

        /// <exclude>Excluded</exclude>
        string _PaymentProviderJson;
        /// <MetaDataID>{c2593fbd-309f-4205-808a-221920425787}</MetaDataID>
        [PersistentMember(nameof(_PaymentProviderJson))]
        [BackwardCompatibilityID("+9")]
        public string PaymentProviderJson
        {
            get => _PaymentProviderJson;
            set
            {
                if (_PaymentProviderJson != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentProviderJson = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        DateTime? _TransactionDate;

        /// <MetaDataID>{863c31da-f7e3-4efc-934c-4a44a02aec20}</MetaDataID>
        [PersistentMember(nameof(_TransactionDate))]
        [BackwardCompatibilityID("+10")]
        public System.DateTime? TransactionDate
        {
            get => _TransactionDate;
            private set
            {

                if (_TransactionDate!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TransactionDate=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        IPaymentSubject _Subject;

        /// <MetaDataID>{9d0d16c5-955d-4792-bccf-9bfafdf8f2f6}</MetaDataID>
        public IPaymentSubject Subject
        {
            get
            {
                if (_Subject==null&&!string.IsNullOrWhiteSpace(SubjectUri))
                {

                    _Subject = OOAdvantech.Remoting.RestApi.RemotingServices.GetPersistentObject<IPaymentSubject>(OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl, SubjectUri);
                }
                return _Subject;
            }
            set
            {
                if (_Subject != value)
                {
                    _Subject=value;
                    if (_Subject==null)
                        SubjectUri=null;
                    else
                    {

                        using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                        {
                            SubjectUri= OOAdvantech.Remoting.RestApi.RemotingServices.GetComputingContextPersistentUri(_Subject);
                            stateTransition.Consistent = true;
                        }
                    }

                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _PaymentOrderUrl;

        /// <MetaDataID>{36947fd2-48db-49a0-abd9-a3023f7c3f1f}</MetaDataID>
        [PersistentMember(nameof(_PaymentOrderUrl))]
        [BackwardCompatibilityID("+14")]
        public string PaymentOrderUrl
        {
            get => _PaymentOrderUrl;
            set
            {
                if (_PaymentOrderUrl!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PaymentOrderUrl=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <MetaDataID>{461a9ea2-bc16-407b-bb03-076c301d9bf8}</MetaDataID>
        [PersistentMember]
        [BackwardCompatibilityID("+13")]
        string SubjectUri;



        /// <MetaDataID>{c6400a9f-da1d-409f-860b-7d46c76643cd}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {

            if (!string.IsNullOrWhiteSpace(ItemsJson))
                _Items = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<Item>>(ItemsJson).OfType<IItem>().ToList();

            if (!string.IsNullOrWhiteSpace(PaymentInfoFieldsJson))
                PaymentInfoFields = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(PaymentInfoFieldsJson);
        }

        /// <MetaDataID>{a1551750-8abf-45bc-8c3f-ca6da68f7458}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            ItemsJson = OOAdvantech.Json.JsonConvert.SerializeObject(Items);
            PaymentInfoFieldsJson = OOAdvantech.Json.JsonConvert.SerializeObject(PaymentInfoFields);
        }

        /// <MetaDataID>{95ef39af-d230-4d08-b6e7-16855a0941e8}</MetaDataID>
        public void Update(List<Item> paymentItems)
        {
            _Items = paymentItems.OfType<IItem>().ToList();
            _Amount = paymentItems.Sum(x => (x.Quantity * x.Price)-x.PaidAmount);
        }



        [OOAdvantech.MetaDataRepository.HttpInVisible]
        event OOAdvantech.ObjectChangeStateHandle _ObjectChangeState;

        [GenerateEventConsumerProxy]
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState
        {
            add
            {
                _ObjectChangeState += value;
            }
            remove
            {
                _ObjectChangeState -= value;
            }
        }


        /// <MetaDataID>{d31c99a2-c628-437e-ba87-5e2cb197a2c3}</MetaDataID>
        public void CardPaymentCompleted(string cardType, string accountNumber, bool isDebit, string transactionID, decimal tipAmount)
        {
            if (State == PaymentState.Completed)
                return;


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                PaymentInfoFields["CardType"] = cardType;
                PaymentInfoFields["AccountNumber"] = accountNumber;
                PaymentInfoFields["TransactionID"] = transactionID;
                _TipsAmount = tipAmount;
                State = PaymentState.Completed;

                this.Subject.PaymentCompleted(this);

                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{185fe21f-7fb4-47aa-9ea1-31941c36d82a}</MetaDataID>
        Dictionary<string, string> PaymentInfoFields = new Dictionary<string, string>();


        /// <MetaDataID>{949b5e42-af19-4a82-a567-8e7982a4c23b}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+6")]
        string PaymentInfoFieldsJson;


        /// <MetaDataID>{da962a27-c80c-40a0-91e7-7ec87c017179}</MetaDataID>
        public void CashPaymentCompleted(decimal tipAmount)
        {
            if (State == PaymentState.Completed)
                throw new Exception("Payment already completed");

            if(this.Items.Where(x=>x.Quantity<0).Count()>0&&Amount<=0)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _TipsAmount = tipAmount;
                    //Normalize
                    NormalizeNettingItems();
                    
                    State = PaymentState.Completed;
                    stateTransition.Consistent = true;
                }


            }
            else if (Amount>0)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _TipsAmount = tipAmount;
                    State = PaymentState.Completed;
                    stateTransition.Consistent = true;
                }
            }
        }

        private void NormalizeNettingItems()
        {
            var itemsToPayAmount = Items.OfType<Item>().Where(x => x.Amount-x.PaidAmount>0).Sum(x => x.Amount-x.PaidAmount)+TipsAmount;

            var nettingItems = Items.OfType<Item>().Where(x => x.Amount<0).ToList();
            foreach (var nettingItem in nettingItems.ToList())
            {
                
                if ((-nettingItem.Amount)<=itemsToPayAmount)
                {
                    nettingItems.Remove(nettingItem);
                    itemsToPayAmount+=nettingItem.Amount;
                }else
                {
                    nettingItem.Quantity=-itemsToPayAmount/nettingItem.Price;
                    itemsToPayAmount=0;
                    nettingItems.Remove(nettingItem);
                }
            }
            _Amount = Items.OfType<Item>().Sum(x => (x.Quantity * x.Price)-x.PaidAmount);



        }

        /// <summary></summary>
        /// <param name="bankDescription"></param>
        /// <param name="bic"></param>
        /// <param name="iban"></param>
        /// <param name="checkID"></param>
        /// <param name="issuer"></param>
        /// <param name="issueDate"></param>
        /// <param name="checkNotes"></param>
        /// <param name="totalAmount"></param>
        /// <param name="tipAmount"></param>
        /// <MetaDataID>{3193cc4c-50f8-4c3c-9f1e-fd01255f5458}</MetaDataID>
        public void CheckPaymentCompleted(string bankDescription, string bic, string iban, string checkID, string issuer, DateTime issueDate, string checkNotes, decimal totalAmount, decimal tipAmount)
        {
            if (State == PaymentState.Completed)
                throw new Exception("Payment already completed");

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                PaymentInfoFields["BankDescription"] = bankDescription;
                PaymentInfoFields["BIC"] = bic;
                PaymentInfoFields["IBAN"] = iban;
                PaymentInfoFields["CheckID"] = checkID;
                PaymentInfoFields["Issuer"] = issuer;
                PaymentInfoFields["issueDateAsLong"] = issueDate.Ticks.ToString();
                PaymentInfoFields["CheckNotes"] = checkNotes;

                _TipsAmount = tipAmount;
                State = PaymentState.Completed;
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{c25bafa9-31da-465a-b9a1-801d415b82e9}</MetaDataID>
        public void PaymentRequestCanceled()
        {
            if (State == PaymentState.InProgress)
                State = PaymentState.New;

        }

        /// <MetaDataID>{37608630-70ad-42af-a372-b7ca81506a8b}</MetaDataID>
        public void Refund()
        {
            throw new NotImplementedException();
        }


    }
}