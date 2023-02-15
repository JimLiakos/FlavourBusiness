using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;

namespace FinanceFacade
{
    /// <MetaDataID>{3836664e-5f1a-4e75-9bbb-4c9d6f963fe0}</MetaDataID>
    [BackwardCompatibilityID("{3836664e-5f1a-4e75-9bbb-4c9d6f963fe0}")]
    [HttpVisible]
    [GenerateFacadeProxy]
    public interface IPayment
    {
        /// <MetaDataID>{d6eb7ce8-cc7c-4b94-bb55-abf72e5e3c30}</MetaDataID>
        string PaymentOrderUrl { get;  }

#if !DeviceDotNet
        [Association("SubjectOfPayment", Roles.RoleA, "1fb62be2-4d9b-4618-9c9f-7142f4031744")]
        IPaymentSubject Subject { get; set; }
#endif

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
        void CardPaymentCompleted(string cardType, string accountNumber, bool isDebit, string transactionID, decimal? tipAmount);

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

        [GenerateEventConsumerProxy]
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;


        /// <MetaDataID>{221a8131-7c53-40e0-9ef7-1e3d03c6f2b9}</MetaDataID>
        bool IsCompleted();

        /// <MetaDataID>{6bd54dfb-5044-4c07-83d4-ebfeb2390358}</MetaDataID>
        PaymentActionState ParseResponse(string response);


    }

    public class PaymentException : System.Exception
    {
        public readonly PaymentFailure PaymentFailure;

        public PaymentException(string message, PaymentFailure zeroAmount,int hresult ) : base(message)
        {
            this.PaymentFailure=zeroAmount;
            this.HResult=hresult;
        }

    }

    public enum PaymentFailure
    {
        Failed,
        ZeroAmount
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

    /// <MetaDataID>{4f5e2e3e-64be-40b0-b120-46c8371bf532}</MetaDataID>
    public enum PaymentActionState
    {
        Continue = 1,
        Succeeded = 2,
        Canceled = 3
    }

    /// <MetaDataID>{f3fd7c47-599e-4736-9f69-1d823cfbf1da}</MetaDataID>
    public enum PaymentState
    {
        New,
        InProgress,
        Completed
    }

#if !DeviceDotNet
    /// <MetaDataID>{ad13180b-ad36-4b4e-a3ec-9d7b69a180ae}</MetaDataID>
    public interface IPaymentProvider
    {
        /// <MetaDataID>{3445fdf2-6747-462d-b219-c50b65776fec}</MetaDataID>
        void CheckPaymentProgress(IPayment payment);
        /// <MetaDataID>{b9bed520-85ef-4952-938a-c94cf2243c27}</MetaDataID>
        PaymentActionState ParseResponse(Payment payment, string response);
        /// <MetaDataID>{c8d1c483-1045-481c-88aa-3ba8b506d651}</MetaDataID>
        HookRespnose WebHook(string method, string webHookName, System.Collections.Generic.Dictionary<string, string> headers, string content);

    }
#endif  


}