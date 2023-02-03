using FinanceFacade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace FlavourBusinessManager
{
    internal class ServiceContextPaymentFinder : IPaymentFinder
    {
        public IPayment FindPayment(string paymentGetwayID, string paymentGetwayRequestID)
        {


            return (from foodServiceSession in ServicePointRunTime.ServicesContextRunTime.Current.OpenSessions
                    from payment in foodServiceSession.BillingPayments.OfType<Payment>().Where(x => x.PaymentGetwayID==paymentGetwayID&&x.PaymentGetwayRequestID==paymentGetwayRequestID)
                    select payment).FirstOrDefault();
        }
    }
}
