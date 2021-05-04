using System;
using System.Collections.Generic;
using OOAdvantech.Collections.Generic;
using System.Linq;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{f4fdf9fe-3c45-48fd-b958-c16677d2397c}</MetaDataID>
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

        System.Collections.Generic.Dictionary<string, string> TransactionProperies = new System.Collections.Generic.Dictionary<string, string>();
        public string GetPropertyValue(string propertyName)
        {
            return TransactionProperies[propertyName];
        }

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
        [PersistentMember(nameof(_Items))]
        [BackwardCompatibilityID("+4")]
        public System.Collections.Generic.IList<FinanceFacade.IItem> Items
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
        [PersistentMember(nameof(_TransactionTaxes))]
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