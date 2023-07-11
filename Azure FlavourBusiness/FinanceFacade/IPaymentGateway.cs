namespace FinanceFacade
{
    /// <MetaDataID>{0d547eab-ccbb-4482-964e-048b8dfa9f0c}</MetaDataID>
    public interface IPaymentGateway
    {
        void CreatePaymentGatewayOrder(FinanceFacade.IPayment payment, decimal tipAmount, string paramsJson);
    }
}