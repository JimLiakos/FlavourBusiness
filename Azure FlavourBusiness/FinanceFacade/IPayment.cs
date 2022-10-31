namespace FinanceFacade
{
    /// <MetaDataID>{3836664e-5f1a-4e75-9bbb-4c9d6f963fe0}</MetaDataID>
    public interface IPayment
    {
        /// <MetaDataID>{45d235f7-ea8f-476a-a5b7-93112d49437f}</MetaDataID>
        double Amount { get; set; }
        /// <MetaDataID>{3df25632-5ef6-420a-95bf-a342f7e3588a}</MetaDataID>
        PaymentType PaymentType { get; set; }

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
}