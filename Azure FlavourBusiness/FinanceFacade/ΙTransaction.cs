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

        /// <MetaDataID>{61e00694-8dce-4da4-8ece-6f3485fb4ae5}</MetaDataID>
        string PayeeRegistrationNumber { get; set; }


        /// <MetaDataID>{bc1f1de8-1e9d-4efb-bb54-27a7f499d22d}</MetaDataID>
        string GetPropertyValue(string propertyName);

        /// <MetaDataID>{d55ce88a-4440-4845-b6d2-5c8b8ed9bd9f}</MetaDataID>
        void SetPropertyValue(string propertyName, string value);


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

        /// <MetaDataID>{d9728973-cc2b-475f-a811-ad7f3b17098c}</MetaDataID>
        string Uri { get; }


        /// <MetaDataID>{695b925f-94e5-4667-968e-a3af4d321260}</MetaDataID>
        decimal DiscountAmount { get; set; }

        /// <MetaDataID>{4452d442-ca30-4e53-8dda-7d199562ec89}</MetaDataID>
        double DiscountRate { get; set; }


        /// <MetaDataID>{717ccd3d-77b6-4f17-baf1-dd3c07b09633}</MetaDataID>
        bool PrintAgain { get; set; }

    }
}