using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanceFacade
{
    /// <MetaDataID>{e777abd6-c8a8-45d4-921b-29c10a6c1f7e}</MetaDataID>
    [BackwardCompatibilityID("{e777abd6-c8a8-45d4-921b-29c10a6c1f7e}")]
    [Persistent()]
    public class Payment : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IPayment
    {
  


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
        protected Payment()
        {

        }

        /// <MetaDataID>{e68605e1-f9d0-4000-a4eb-3e2e55176818}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public Payment(decimal amount,string currency,string Identity, string itemsJson)
        {
            _Amount = amount;
            _Currency = currency;
            _Identity = Identity;
            ItemsJson = itemsJson;

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
        }

        /// <MetaDataID>{a1551750-8abf-45bc-8c3f-ca6da68f7458}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            ItemsJson = OOAdvantech.Json.JsonConvert.SerializeObject(Items);
        }

        /// <MetaDataID>{95ef39af-d230-4d08-b6e7-16855a0941e8}</MetaDataID>
        public void Update(List<Item> paymentItems)
        {
            _Items = paymentItems.OfType<IItem>().ToList();
            _Amount = paymentItems.Sum(x => x.Quantity * x.Price);
        }

        /// <MetaDataID>{d31c99a2-c628-437e-ba87-5e2cb197a2c3}</MetaDataID>
        public void CardPaymentCompleted(string cardType, string accountNumber, bool isDebit, string transactionID, decimal tipAmount)
        {

        }

        /// <MetaDataID>{da962a27-c80c-40a0-91e7-7ec87c017179}</MetaDataID>
        public void CashPaymentCompleted(decimal tipAmount)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        /// <MetaDataID>{c25bafa9-31da-465a-b9a1-801d415b82e9}</MetaDataID>
        public void PaymentRequestCanceled()
        {
            if(State == PaymentState.InProgress)
                State = PaymentState.New;

        }

        public void Refund()
        {
            throw new NotImplementedException();
        }
    }
}