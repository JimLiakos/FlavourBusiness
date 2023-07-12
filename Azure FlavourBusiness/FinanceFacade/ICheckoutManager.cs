namespace FinanceFacade
{
    /// <MetaDataID>{d814eb42-0c33-4143-8682-85840141c7e8}</MetaDataID>
    public interface ICheckoutManager
    {
        /// <MetaDataID>{ba5a4589-f470-4fc1-a8f7-f3ff207d3e03}</MetaDataID>
        void Pay(IPayment payment, PaymentMethod paymentMethod, decimal tipAmount);
    }

    /// <MetaDataID>{88ab68cf-f4e1-4093-8c40-5e940f581637}</MetaDataID>
    public enum PaymentMethod
    {
        Cash = 1,
        Card = 2,
        PaymentGateway = 3
        

    }
}