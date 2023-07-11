namespace FinanceFacade
{
    /// <MetaDataID>{0d547eab-ccbb-4482-964e-048b8dfa9f0c}</MetaDataID>
    public interface IPaymentGateway
    {

        /// <summary>
        /// Creates a payment order for the items o payment parameter
        /// </summary>
        /// <param name="payment">
        /// The payment parameter specifies a list of items that are pending payment
        /// </param>
        /// <param name="tipAmount">
        /// Defines the tip amount for  the service person 
        /// </param>
        void CreatePaymentOrder(FinanceFacade.IPayment payment, decimal tipAmount, string paramsJson);
    }
}