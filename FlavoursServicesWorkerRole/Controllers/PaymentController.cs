using Azure.Core;
using Braintree;
using FlavourBusinessFacade;
using FlavourBusinessManager;
using Microsoft.ServiceBus;
using OOAdvantech.RDBMSMetaDataRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.UI.WebControls;
using System.Xml.Linq;

namespace FlavoursServicesWorkerRole.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentController : ApiController
    {


        [HttpPost]
        [Route("api/WebHook/{webHookName}/{serviceContextIdentity}")]
        public HttpResponseMessage WebHookPost(string webHookName, string serviceContextIdentity)
        {

            var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());//.Select(x => new { Name = x.Key, Value=x.Value });// x.).n.AllKeys.Select(x => new { Name = x, Value = HttpContext.Current.Request.Headers.Get(x) }).ToList();
            string headersjSON = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
            Stream body = Request.Content.ReadAsStreamAsync().GetAwaiter().GetResult();

            StreamReader streamReader = new StreamReader(body);
            string Json = streamReader.ReadToEnd();


            IFlavoursServicesContextRuntime flavoursServicesContextRuntime = null;
            if (!FlavoursServicesContextRuntimes.TryGetValue(serviceContextIdentity, out flavoursServicesContextRuntime))
            {
                flavoursServicesContextRuntime = FlavoursServicesContext.GetServicesContextRuntime(serviceContextIdentity);
                FlavoursServicesContextRuntimes[serviceContextIdentity] = flavoursServicesContextRuntime;
            }
            var hookRespnose = flavoursServicesContextRuntime.WebHook("POST", webHookName, headers, Json);
            //var hookRespnose = ServiceBusWebHook(serviceContextIdentity, "POST", webHookName, headers, Json);  

            var response = Request.CreateResponse(hookRespnose.StatusCode, hookRespnose.Content);
            foreach (var headerEntry in hookRespnose.Headers)
                response.Headers.Add(headerEntry.Key, headerEntry.Value);

            return response;




        }
        struct Header
        {
            string Name { get; set; }
            string Value { get; set; }
        }

        [HttpGet]
        [Route("api/WebHook/{webHookName}/{serviceContextIdentity}")]
        public HttpResponseMessage WebHookGet(string webHookName, string serviceContextIdentity)
        {
            var headers = Request.Headers.ToDictionary(x => x.Key, x => x.Value.FirstOrDefault());
            string headersjSON = Newtonsoft.Json.JsonConvert.SerializeObject(headers);
            Stream body = Request.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
            StreamReader streamReader = new StreamReader(body);
            string Json = streamReader.ReadToEnd();

            IFlavoursServicesContextRuntime flavoursServicesContextRuntime = null;
            if (!FlavoursServicesContextRuntimes.TryGetValue(serviceContextIdentity, out flavoursServicesContextRuntime))
            {
                flavoursServicesContextRuntime = FlavoursServicesContext.GetServicesContextRuntime(serviceContextIdentity);
                FlavoursServicesContextRuntimes[serviceContextIdentity] = flavoursServicesContextRuntime;
            }

            //var hookRespnose = ServiceBusWebHook(serviceContextIdentity,"GET", webHookName, headers, Json);


            var hookRespnose = flavoursServicesContextRuntime.WebHook("GET", webHookName, headers, Json);

            var response = Request.CreateResponse(hookRespnose.StatusCode, hookRespnose.Content);
            foreach (var headerEntry in hookRespnose.Headers)
                response.Headers.Add(headerEntry.Key, headerEntry.Value);



            //var response = Request.CreateResponse(HttpStatusCode.OK, @"{""key"":""1234335""}");
            //response.Headers.Add("test-header", "value");
            return response;

        }

        private static HookRespnose ServiceBusWebHook(string serviceContextIdentity, string method, string webHookName, Dictionary<string, string> headers, string Json)
        {

            string headersjSON = OOAdvantech.Json.JsonConvert.SerializeObject(headers);

            var cf = new ChannelFactory<IDeviceMessageServerChannel>(
                     new NetTcpRelayBinding(),
                     new EndpointAddress(ServiceBusEnvironment.CreateServiceUri("sb", "arionsoftware", serviceContextIdentity)));

            var messageDoc = XDocument.Parse("<Main/>");
            messageDoc.Root.SetAttributeValue("Method", method);
            messageDoc.Root.SetAttributeValue("WebHookName", webHookName);
            messageDoc.Root.SetAttributeValue("Headers", headersjSON);
            messageDoc.Root.SetAttributeValue("Content", Json);
            cf.Endpoint.Behaviors.Add(new TransportClientEndpointBehavior
            {
                TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "f9tTGFYjW2Og77pIqt3pJRUbODquGh4NYVa4bCbSx9I=")
                //TokenProvider = TokenProvider.CreateSharedSecretTokenProvider("owner", "PghQZBj0CbCVceSsYI/tXocz/ekQFay0FMj/oEtS7yg=")
            });
            cf.Endpoint.Binding.SendTimeout = new TimeSpan(0, 0, 2, 0);
            cf.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 0, 2, 0);

            using (IDeviceMessageServerChannel orderManager = cf.CreateChannel())
            {
                if ((orderManager as ICommunicationObject).State == CommunicationState.Faulted)
                {
                    (orderManager as ICommunicationObject).Abort();
                    (orderManager as ICommunicationObject).Open(new TimeSpan(0, 0, 2, 0));
                }
                var responseXML = orderManager.SendMessage(messageDoc.ToString());

                var responseDoc = XDocument.Parse(responseXML);
                HookRespnose hookRespnose = new HookRespnose();

                hookRespnose.Headers = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(responseDoc.Root.Attribute("Headers").Value);
                hookRespnose.Content = responseDoc.Root.Attribute("Content").Value;
                hookRespnose.StatusCode = (HttpStatusCode)int.Parse(responseDoc.Root.Attribute("StatusCode").Value);
                return hookRespnose;
            }
        }

        Dictionary<string, IFlavoursServicesContextRuntime> FlavoursServicesContextRuntimes = new Dictionary<string, IFlavoursServicesContextRuntime>();

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

    /// <MetaDataID>{a5db5674-2a7c-4e6f-bd23-2abd6f7d4287}</MetaDataID>
    [ServiceContract(Namespace = "urn:ps")]
    public interface IDeviceMessageServer
    {
        [OperationContract]
        string SendMessage(string xmlMessage);


        [OperationContract]
        string SendCallBackEventMessage(string xmlMessage);

    }
    public interface IDeviceMessageServerChannel : IDeviceMessageServer, IClientChannel { }
}
