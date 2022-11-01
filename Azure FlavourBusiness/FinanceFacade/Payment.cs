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
    public class Payment : IPayment
    {
        /// <exclude>Excluded</exclude>
        string _ItemsJson;

        /// <MetaDataID>{4c7bdca1-0a7e-47cd-a10f-7e0306ba04fd}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        private string ItemsJson;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        double _Amount;
        /// <MetaDataID>{83e4b5ff-08c6-474c-a342-881f6e7919b1}</MetaDataID>
        [PersistentMember(nameof(_Amount))]
        [BackwardCompatibilityID("+1")]
        public double Amount
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
            this.Identity = paymentIdentity;
            _Items = paymentItems.OfType<IItem>().ToList();
            _Currency = currency;
        }


        /// <MetaDataID>{e68605e1-f9d0-4000-a4eb-3e2e55176818}</MetaDataID>
        public Payment()
        {

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
        }
    }
}