namespace FinanceFacade
{
    /// <MetaDataID>{807c1d65-5ce4-41a7-b78b-87a118310810}</MetaDataID>
    public interface IPaymentFinder
    {
        /// <MetaDataID>{3e68b462-42b9-4bdc-9f0c-97867e0bc5a9}</MetaDataID>
        IPayment FindPayment(string paymentGetwayID, string paymentGetwayRequestID);
    }
}