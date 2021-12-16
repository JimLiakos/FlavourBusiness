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

        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{c2f54715-50cf-4554-b74a-555edc4948b2}</MetaDataID>
        public void AddItem(Item item)
        {
            _Items.Add(item);
        }

        /// <MetaDataID>{ed7e9605-e1fd-440e-a3f7-6ea6505e3cb0}</MetaDataID>
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
        Set<IItem> _Items = new Set<IItem>();
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
        Set<TaxAmount> _TransactionTaxes = new Set<TaxAmount>();

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