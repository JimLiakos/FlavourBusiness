using System;
using System.Collections.Generic;

using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using OOAdvantech.Remoting.RestApi.Serialization;
using OOAdvantech.Json;
using JsonSerializerSettings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings;

namespace FinanceFacade
{
    /// <MetaDataID>{f4fdf9fe-3c45-48fd-b958-c16677d2397c}</MetaDataID>
    [BackwardCompatibilityID("{f4fdf9fe-3c45-48fd-b958-c16677d2397c}")]
    [Persistent()]
    public class Transaction : ITransaction
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        string _Uri;
        public string Uri
        {
            get
            {
                if (_Uri == null)
                    _Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this)?.GetPersistentObjectUri(this);

                return _Uri;
            }
        }


        /// <MetaDataID>{69518e16-81af-4aee-805c-b85507378551}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {

            ItemsJson = OOAdvantech.Json.JsonConvert.SerializeObject(Items);
            TransactionProperiesJson = OOAdvantech.Json.JsonConvert.SerializeObject(TransactionProperies);
        }

        [JsonConstructor]
        public Transaction(string uri, string description,List<IItem> items, List<FinanceFacade.TaxAmount> transactionTaxes, Measurement.Quantity amount, string invoiceNumber, string payeeRegistrationNumber)
        {
            _Uri = uri;
            _Description = description;
            _Items = items;
            _TransactionTaxes = transactionTaxes;
            _Amount = amount;
            _InvoiceNumber = invoiceNumber;
            
        }
        public Transaction()
        {

        }

        /// <MetaDataID>{3f9250f2-d7ff-4391-a58d-b594ef71479b}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            
            if (!string.IsNullOrWhiteSpace(ItemsJson))
                _Items = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<Item>>(ItemsJson).OfType<IItem>().ToList();
            
            if (!string.IsNullOrWhiteSpace(TransactionProperiesJson))
                TransactionProperies = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(TransactionProperiesJson);

        }



        /// <exclude>Excluded</exclude>
        Measurement.Quantity _Amount;

        /// <MetaDataID>{a73db1df-3024-41b4-9b3e-b9f33eb8dba9}</MetaDataID>
        [CachingDataOnClientSide]
        [PersistentMember(nameof(_Amount))]
        [BackwardCompatibilityID("+6")]
        public Measurement.Quantity Amount
        {
            get
            {
                return _Amount;
            }

            set
            {
                _Amount = value;
            }
        }

        /// <MetaDataID>{18309797-10bc-4866-b603-f31c1db2ac33}</MetaDataID>
        [JsonIgnore]
        [PersistentMember()]
        [BackwardCompatibilityID("+9")]
        string ItemsJson;




        /// <MetaDataID>{f4315f4c-6dca-4db6-93f8-4200df91241c}</MetaDataID>
        [JsonIgnore]
        [PersistentMember()]
        [BackwardCompatibilityID("+10")]
        string TransactionProperiesJson;


        /// <MetaDataID>{c2f54715-50cf-4554-b74a-555edc4948b2}</MetaDataID>
        public void AddItem(Item item)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Items.Add(item); 
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{11230707-d626-44c3-b0d8-19ae7c0910c5}</MetaDataID>
        public void RemoveItem(Item item)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Items.Remove(item);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{dd608129-7e91-4e0b-a6a2-8be1c701baa6}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        System.Collections.Generic.Dictionary<string, string> TransactionProperies = new System.Collections.Generic.Dictionary<string, string>();
        /// <MetaDataID>{1cf01a7e-9061-4c02-8ed0-e89e9588d776}</MetaDataID>
        public string GetPropertyValue(string propertyName)
        {
            return TransactionProperies[propertyName];
        }

        /// <MetaDataID>{4a9c2476-2b3a-4b04-8ae4-b2ecb76330f5}</MetaDataID>
        public void SetPropertyValue(string propertyName, string value)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                TransactionProperies[propertyName] = value; 
                stateTransition.Consistent = true;
            }

        }
        
        /// <exclude>Excluded</exclude>
        string _Description;


        /// <MetaDataID>{37cb0b91-e4b3-465e-9211-01644466a8f9}</MetaDataID>
        [CachingDataOnClientSide]
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                _Description = value;
            }
        }

        /// <exclude>Excluded</exclude>
        string _InvoiceNumber;
        /// <MetaDataID>{36c58bba-1602-4ea3-ba13-7428a5b1f717}</MetaDataID>
        [PersistentMember(nameof(_InvoiceNumber))]
        [BackwardCompatibilityID("+3")]
        public string InvoiceNumber
        {
            get
            {
                return _InvoiceNumber;
            }

            set
            {
                _InvoiceNumber = value;
            }
        }

        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<IItem> _Items = new System.Collections.Generic.List<IItem>();
        /// <MetaDataID>{414777ff-5a14-4bba-a10a-fa9900d41e71}</MetaDataID>
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+4")]
        public IList<IItem> Items
        {
            get
            {
                lock (_Items)
                {
                    return _Items.ToList();
                }
            }
        }
        /// <exclude>Excluded</exclude>
        System.Collections.Generic.List<TaxAmount> _TransactionTaxes = new System.Collections.Generic.List<TaxAmount>();

        /// <MetaDataID>{d83595b4-e005-4d29-878a-fa02bb297519}</MetaDataID>
        [CachingDataOnClientSide]
        [BackwardCompatibilityID("+5")]
        public System.Collections.Generic.IList<FinanceFacade.TaxAmount> TransactionTaxes
        {
            get
            {
                lock (_TransactionTaxes)
                {
                    
                    
                        _TransactionTaxes = (from item in _Items
                                             from tax in item.Taxes
                                             group tax by tax.AccountID into taxes
                                             select new FinanceFacade.TaxAmount { AccountID = taxes.Key, Amount = taxes.Sum(x => x.Amount) }).ToList();

                    return _TransactionTaxes;
                    
                    
                }
            }
        }


        /// <exclude>Excluded</exclude> 
        string _PayeeRegistrationNumber;

        /// <MetaDataID>{89200097-a871-44e8-a73c-29db104b8668}</MetaDataID>
        [CachingDataOnClientSide]
        public string PayeeRegistrationNumber
        {
            get
            {
                return _PayeeRegistrationNumber;
            }

            set
            {
                _PayeeRegistrationNumber = value;
            }
        }
    }
}