using FinanceFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FlavourBusinessManager.PaymentProviders
{

    /// <MetaDataID>{9977de77-c743-4074-8e65-61eff156cdc7}</MetaDataID>
    public static class VivaIPaymentExtMethods
    {
        public static PaymentOrder GetPaymentOrder(this IPayment payment)
        {
            if (string.IsNullOrEmpty(payment?.PaymentProviderJson))
                return null;

            var paymentOrderResponse = OOAdvantech.Json.JsonConvert.DeserializeObject<PaymentOrder>(payment.PaymentProviderJson);

//#if !DeviceDotNet
//            (payment as Payment).PaymentProvider="Viva";
//#endif
            return paymentOrderResponse;
        }

        public static void SetPaymentOrder(this IPayment payment, PaymentOrder paymentOrder)
        {
            if (paymentOrder!=null)
                payment.PaymentProviderJson = OOAdvantech.Json.JsonConvert.SerializeObject(paymentOrder);
            else
                payment.PaymentProviderJson= null;

        }

    }

    public class PaymentOrder
    {
        public long orderCode { get; set; }
        public long expiring { get; set; }
        public string TransactionId { get; set; }
    }
}
