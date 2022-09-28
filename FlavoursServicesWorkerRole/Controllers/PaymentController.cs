using Braintree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FlavoursServicesWorkerRole.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentController : ApiController
    {

        [Route("api/Payment/BraintreeClientToken")]

        public string GetBraintreeClientToken()
        {
            return BraintreeGateway.ClientToken.Generate();
        }


        static BraintreeGateway _BraintreeGateway;
        BraintreeGateway BraintreeGateway
        {
            get
            {
                if (_BraintreeGateway == null)
                {

                    _BraintreeGateway = new BraintreeGateway
                    {
                        Environment = Braintree.Environment.SANDBOX,
                        MerchantId = "nyqdtchc77jt6z52",
                        PublicKey = "cwy55c3by7zppkwq",
                        PrivateKey = "010a3da02008d58cd90be24a1b7343c0"
                    };
                }
                return _BraintreeGateway;
            }
        }
    }
}
