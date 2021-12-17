using System;
using System.Collections.Generic;
using OOAdvantech.Collections.Generic;
using System.Linq;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{f4fdf9fe-3c45-48fd-b958-c16677d2397c}</MetaDataID>
    [BackwardCompatibilityID("{f4fdf9fe-3c45-48fd-b958-c16677d2397c}")]
    [Persistent()]
    public class Transaction : ITransaction
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <MetaDataID>{69518e16-81af-4aee-805c-b85507378551}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        public void BeforeCommitObjectState()
        {
            TaxesJson = OOAdvantech.Json.JsonConvert.SerializeObject(_TransactionTaxes);
            ItemsJson = OOAdvantech.Json.JsonConvert.SerializeObject(Items);
            TransactionProperiesJson = OOAdvantech.Json.JsonConvert.SerializeObject(TransactionProperies);
        }




        /// <MetaDataID>{3f9250f2-d7ff-4391-a58d-b594ef71479b}</MetaDataID>
        [ObjectActivationCall]
        public void ObjectActivation()
        {
            _TransactionTaxes = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<TaxAmount>>(TaxesJson);
            _Items = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.List<IItem>>(ItemsJson);
            TransactionProperies = OOAdvantech.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, string>>(TransactionProperiesJson);

        }



        /// <exclude>Excluded</exclude>
        Measurement.Quantity _Amount;

        /// <MetaDataID>{a73db1df-3024-41b4-9b3e-b9f33eb8dba9}</MetaDataID>
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
        [PersistentMember()]
        [BackwardCompatibilityID("+9")]
        string ItemsJson;

        /// <MetaDataID>{eca8e8e8-575c-4abb-a3f8-2116112ae81e}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+8")]
        string TaxesJson;


        /// <MetaDataID>{f4315f4c-6dca-4db6-93f8-4200df91241c}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+9")]
        string TransactionProperiesJson;

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{c2f54715-50cf-4554-b74a-555edc4948b2}</MetaDataID>
        public void AddItem(Item item)
        {
            _Items.Add(item);
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
            TransactionProperies[propertyName] = value;
        }

        /// <MetaDataID>{37cb0b91-e4b3-465e-9211-01644466a8f9}</MetaDataID>
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
        [BackwardCompatibilityID("+5")]
        public System.Collections.Generic.IList<FinanceFacade.TaxAmount> TransactionTaxes
        {
            get
            {
                lock (_TransactionTaxes)
                {
                    return _TransactionTaxes.ToList();
                }
            }
        }


        /// <exclude>Excluded</exclude> 
        string _PayeeRegistrationNumber;

        /// <MetaDataID>{89200097-a871-44e8-a73c-29db104b8668}</MetaDataID>
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