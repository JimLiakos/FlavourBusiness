using FinanceFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PaymentProviders
{

    /// <MetaDataID>{9977de77-c743-4074-8e65-61eff156cdc7}</MetaDataID>
    public static class VivaIPaymentExtMethods
    {
        /// <MetaDataID>{c5ef43db-90fb-4904-ae7f-f87f6b702764}</MetaDataID>
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

        /// <MetaDataID>{47140e14-2bba-4da7-ac1b-78d85b97f55f}</MetaDataID>
        public static void SetPaymentOrder(this IPayment payment, PaymentOrder paymentOrder)
        {
            if (paymentOrder!=null)
                payment.PaymentProviderJson = OOAdvantech.Json.JsonConvert.SerializeObject(paymentOrder);
            else
                payment.PaymentProviderJson= null;

        }

    }

    /// <MetaDataID>{338bb92d-3830-4ece-88f0-6f7c959df2e4}</MetaDataID>
    public class PaymentOrder
    {
        /// <MetaDataID>{30858d57-833a-4461-ab22-164b2b8ebfb7}</MetaDataID>
        public long orderCode { get; set; }
        /// <MetaDataID>{c4cd896e-9ede-47b2-a2a6-984e49913f2a}</MetaDataID>
        public long expiring { get; set; }
        /// <MetaDataID>{90d5e6af-e124-498a-9497-c94ef52edb91}</MetaDataID>
        public string TransactionId { get; set; }
    }
}
