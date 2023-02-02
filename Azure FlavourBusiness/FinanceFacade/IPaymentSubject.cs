namespace FinanceFacade
{
    /// <MetaDataID>{470c761f-5748-4805-a7fd-1860264ab8cb}</MetaDataID>
    public interface IPaymentSubject
    {
        /// <MetaDataID>{91cddc2c-f5ff-4b52-8e43-58781b885692}</MetaDataID>
        void PaymentCompleted(IPayment payment);
    }
}