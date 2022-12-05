using DeviceMessageServer;
using Microsoft.ServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebhooksToLocalServer
{
    public class Webhookservice
    {
        public ServiceHost DeviceMessageServerServiceBusProxy { get; private set; }

        public void Start()
        {
            //https://meridianmessagingservice.azurewebsites.net/api/WoltHook/NewOrder/CB848610-B5A5-4E21-8ED4-E6F470BA997A
            try
            {
                string serviceBusID = "CB848610-B5A5-4E21-8ED4-E6F470BA997A";
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

            var doc = XDocument.Parse(xmlMessage);
            return "";


        }
    }


}
namespace DeviceMessageServer
{
    [ServiceContract(Namespace = "urn:ps")]
    public interface IDeviceMessageServer
    {
        [OperationContract]
        string SendMessage(string xmlMessage);


        [OperationContract]
        string SendCallBackEventMessage(string xmlMessage);

    }

}