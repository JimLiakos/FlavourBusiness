using DeviceMessageServer;
using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace WebhooksToLocalServer
{
    /// <MetaDataID>{951718dd-4fa5-49b3-946a-ae1e25913e1b}</MetaDataID>
    public class Webhookservice
    {
        public ServiceHost DeviceMessageServerServiceBusProxy { get; private set; }

        public void Start(string serviceBusID)
        {
            //https://meridianmessagingservice.azurewebsites.net/api/WoltHook/NewOrder/CB848610-B5A5-4E21-8ED4-E6F470BA997A
            try
            {
                //string serviceBusID = "CB848610-B5A5-4E21-8ED4-E6F470BA997A";
                string sbUri = null;
                string rootManageSharedAccessKeyValue = "f9tTGFYjW2Og77pIqt3pJRUbODquGh4NYVa4bCbSx9I=";
                string servicebusName = "arionsoftware";

                sbUri = ServiceBusEnvironment.CreateServiceUri("sb", servicebusName, serviceBusID).ToString();

                DeviceMessageServerServiceBusProxy = new ServiceHost(typeof(DeviceMessageServer));
                DeviceMessageServerServiceBusProxy.AddServiceEndpoint(typeof(IDeviceMessageServer), new NetTcpRelayBinding(), sbUri)
                    .Behaviors.Add(new TransportClientEndpointBehavior
                    {
                        TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", rootManageSharedAccessKeyValue)
                    });

                DeviceMessageServerServiceBusProxy.Open();
            }
            catch (AddressAccessDeniedException error)
            {
            }
            catch (AddressAlreadyInUseException error)
            {
            }
            catch (Exception error)
            {

            }
        }
    }

    /// <MetaDataID>{f4d1226d-0ef4-4a1c-96f5-9216853ca0e2}</MetaDataID>
    [ServiceBehavior(Name = "DeviceMessageServer", Namespace = "https://samples.microsoft.com/ServiceModel/Relay/")]
    class DeviceMessageServer : IDeviceMessageServer
    {
        static string _lock = "lock";

        Task<string> PhoneCallerIdTask;
        Task<string> PreparationStationTask;


        public string SendCallBackEventMessage(string xmlMessage)
        {

            return "";
        }

        public string SendMessage(string xmlMessage)
        {

            var messageDoc = XDocument.Parse(xmlMessage);

            string method = messageDoc.Root.Attribute("Method").Value;
            string headersjSON = messageDoc.Root.Attribute("Headers").Value;
            string content = messageDoc.Root.Attribute("Content").Value;
            string webHookName = messageDoc.Root.Attribute("WebHookName").Value;
            var headers = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(headersjSON);

            var response = FlavourBusinessManager.ServicePointRunTime.ServicesContextRunTime.Current.WebHook(method, webHookName, headers, content);

            headersjSON = OOAdvantech.Json.JsonConvert.SerializeObject(response.Headers);

            var responseDoc = XDocument.Parse("<Main/>");
            responseDoc.Root.SetAttributeValue("StatusCode", ((int)response.StatusCode).ToString());
            responseDoc.Root.SetAttributeValue("Headers", headersjSON);
            responseDoc.Root.SetAttributeValue("Content", response.Content);
            return responseDoc.ToString();


        }
    }


}
namespace DeviceMessageServer
{
    /// <MetaDataID>{f6dc7ce9-d648-41d5-a378-1380fd97f130}</MetaDataID>
    [ServiceContract(Namespace = "urn:ps")]
    public interface IDeviceMessageServer
    {
        [OperationContract]
        string SendMessage(string xmlMessage);


        [OperationContract]
        string SendCallBackEventMessage(string xmlMessage);

    }
}
