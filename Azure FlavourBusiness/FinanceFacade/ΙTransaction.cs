using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{4fba9d38-4340-4447-8008-acf24fa3ab39}</MetaDataID>
    public interface ITransaction
    {
        /// <MetaDataID>{a39e41f4-5c67-47ff-9787-56cd1de4c89c}</MetaDataID>
        Measurement.Quantity Amount { get; set; }

        [Association("TransactionTax", Roles.RoleA, "10fb597c-93b2-42f7-99b5-b2b6e6654102")]
        [RoleBMultiplicityRange(0, 1)]
        IList<TaxAmount> TransactionTaxes { get; }

        [Association("TransactionItem", Roles.RoleA, "7ccfc6ee-4f2d-4f52-bd22-b8c63e74d0f9")]
        IList<IItem> Items { get; }

        string PayeeRegistrationNumber { get; set; }


        string GetPropertyValue(string propertyName);

        void SetPropertyValue(string propertyName,string value);


        //
        // Summary:
        //     /// Description of what is being paid for. ///

        /// <MetaDataID>{51986f1a-f2d4-4809-a941-8a4df3bd7748}</MetaDataID>
        string Description { get; set; }
        //
        // Summary:
        //     /// invoice number to track this payment ///

        /// <MetaDataID>{cc41c482-497e-4c88-9f2e-8e9730a2ded3}</MetaDataID>
        string InvoiceNumber { get; set; }

    }
}