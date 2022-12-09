using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{3836664e-5f1a-4e75-9bbb-4c9d6f963fe0}</MetaDataID>
    [BackwardCompatibilityID("{3836664e-5f1a-4e75-9bbb-4c9d6f963fe0}")]
    [HttpVisible]
    [GenerateFacadeProxy]
    public interface IPayment
    {
        /// <MetaDataID>{2a5b9053-54e1-4a3c-825e-49199b184b3b}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        [CachingDataOnClientSide]
        decimal TipsAmount { get; }

        /// <MetaDataID>{da05e520-55c7-42ea-9102-bd74b4f6394b}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        [CachingDataOnClientSide]
        PaymentState State
        {
            set;
            get;
        }
        /// <MetaDataID>{2925c6e1-ba42-496b-85b0-fc322d435c4d}</MetaDataID>
        string PaymentProviderJson { get; set; }

        /// <MetaDataID>{144808de-5908-4b69-bea9-0195b13f7f20}</MetaDataID>
        [BackwardCompatibilityID("+7")]
        [CachingDataOnClientSide]
        System.DateTime? TransactionDate { get; }

        /// <MetaDataID>{23bd0625-6451-4298-b1b2-ecc990469eb7}</MetaDataID>
        void CardPaymentCompleted(string cardType, string accountNumber, bool isDebit, string transactionID, decimal tipAmount);

        /// <MetaDataID>{28e03d9b-bf11-400b-add0-e052213aaf05}</MetaDataID>
        void CashPaymentCompleted(decimal tipAmount);

        /// <MetaDataID>{971787bb-c85e-4e5c-bd80-1bacc148db44}</MetaDataID>
        void CheckPaymentCompleted(string bankDescription, string bic, string iban, string checkID, string issuer, System.DateTime issueDate, string checkNotes, decimal totalAmount, decimal tipAmount);

        /// <MetaDataID>{b81d6b69-d293-4ec4-b108-65eae0dde1be}</MetaDataID>
        void Refund();

        /// <MetaDataID>{83e36bd4-23e3-4f71-a261-9abd85134dac}</MetaDataID>
        [Association("PaymentItem", Roles.RoleA, "d4adad7e-5b24-4d4c-9785-07c27b196a3f")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        [CachingDataOnClientSide]
        System.Collections.Generic.List<IItem> Items { get; }




        /// <MetaDataID>{bf901995-1915-47bc-bbae-cb1643677b3e}</MetaDataID>
        void PaymentInProgress();

        /// <MetaDataID>{ff361440-c40d-42a2-a5d5-9946ced65f08}</MetaDataID>
        void PaymentRequestCanceled();
        




        /// <MetaDataID>{45d235f7-ea8f-476a-a5b7-93112d49437f}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        [CachingDataOnClientSide]
        decimal Amount { get; set; }


        /// <MetaDataID>{3df25632-5ef6-420a-95bf-a342f7e3588a}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        [CachingDataOnClientSide]
        PaymentType PaymentType { get; set; }

        /// <summary>
        /// Defines ISO Currency Symbol 
        /// </summary>
        /// <MetaDataID>{c25d9c8d-f279-4a72-85fd-1f53655030c8}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [CachingDataOnClientSide]
        string Currency { get; set; }


        /// <MetaDataID>{def3eb8b-4fc3-41a3-9d56-8a4c83b95e0f}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        [CachingDataOnClientSide]
        string Identity { get; set; }

    }

    /// <MetaDataID>{c29b8985-3d96-4fea-91b6-7b3040b4d715}</MetaDataID>
    public enum PaymentType
    {
        None,
        Cash,
        DebitCard,
        CreditCard,
        Check
    }

    /// <MetaDataID>{f3fd7c47-599e-4736-9f69-1d823cfbf1da}</MetaDataID>
    public enum PaymentState
    {
        New,
        InProgress,
        Completed
    }
}