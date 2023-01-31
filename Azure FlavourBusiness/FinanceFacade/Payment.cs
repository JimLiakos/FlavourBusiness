
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.AccessControl;

namespace FinanceFacade
{
    /// <MetaDataID>{e777abd6-c8a8-45d4-921b-29c10a6c1f7e}</MetaDataID>
    [BackwardCompatibilityID("{e777abd6-c8a8-45d4-921b-29c10a6c1f7e}")]
    [Persistent()]
    public class Payment : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IPayment
    {

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
                _TipsAmount=value;
            }
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
            _Amount = paymentItems.Sum(x => x.Quantity * x.Price);


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
                    OOAdvantech.Transactions.Transaction.RunOnTransactionCompleted(() =>
                    {
                        ObjectChangeState?.Invoke(this, nameof(State));
                    });
                }
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
            _Amount = paymentItems.Sum(x => x.Quantity * x.Price);
        }
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
        /// <MetaDataID>{d31c99a2-c628-437e-ba87-5e2cb197a2c3}</MetaDataID>
        public void CardPaymentCompleted(string cardType, string accountNumber, bool isDebit, string transactionID, decimal tipAmount)
        {
            if (State == PaymentState.Completed)
                throw new Exception("Payment already completed");


            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                PaymentInfoFields["CardType"] = cardType;
                PaymentInfoFields["AccountNumber"] = accountNumber;
                PaymentInfoFields["TransactionID"] = transactionID;
                _TipsAmount = tipAmount;

                State = PaymentState.Completed;
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
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _TipsAmount = tipAmount;
                State = PaymentState.Completed;
                stateTransition.Consistent = true;
            }
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