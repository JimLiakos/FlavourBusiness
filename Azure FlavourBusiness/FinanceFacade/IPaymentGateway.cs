using OOAdvantech.MetaDataRepository;

namespace FinanceFacade
{
    /// <MetaDataID>{0d547eab-ccbb-4482-964e-048b8dfa9f0c}</MetaDataID>
    [GenerateFacadeProxy]
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
        /// <MetaDataID>{b3f0889b-64f4-460f-a0f2-e0b98ac63c95}</MetaDataID>
        void CreatePaymentOrder(FinanceFacade.IPayment payment, decimal tipAmount, string paramsJson);
    }
}