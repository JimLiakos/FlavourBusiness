using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.WebSockets;

namespace FlavourBusinessWorkerRole.Handler {
    using FlavourBusinessManager;
    using FlavourBusinessManager.ComputingResources;
    using OOAdvantech.Remoting.RestApi;
    using WebSocketSendAsync = Func<ArraySegment<byte>, int, bool, CancellationToken, Task>;
    public class WebSocketMessagesHandler : WebSocketHandler, IEventCallBackChannel
    {
      //private static WebSocketCollection connections = new WebSocketCollection();
      private WebSocketSendAsync _sendFunc;
      private CancellationToken _token;

      public WebSocketMessagesHandler(WebSocketSendAsync sendFunc, CancellationToken token) {
         _sendFunc = sendFunc;
         _token = token;
      }

      public override void OnOpen() {
         //connections.Add(this);
      }

      public override void OnClose() {
         //connections.Remove(this);
      }

      public override void OnMessage(string message) {

            Task.Run(() =>
            {
                MessageDispatch(message);
            });
        }

        public async Task SendMessage(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            var buffer = new ArraySegment<byte>(new byte[bytes.Length]);
            Array.Copy(bytes, 0, buffer.Array, 0, bytes.Length);
            await _sendFunc(new ArraySegment<byte>(buffer.Array, 0, bytes.Length), 1, true, _token);
        }

        

        string ClientProcessIdentity;
        public async void MessageDispatch(string messageData)
        {
            RequestData request = Newtonsoft.Json.JsonConvert.DeserializeObject<RequestData>(messageData);
            if (ClientProcessIdentity == null)
            {
                ClientProcessIdentity = request.ClientProcessIdentity;
                WebSocketClient .WebSocketConnections[ClientProcessIdentity] = this;
            }

            ResponseData responseData = null;
            try
            {
                string roleInstanceServerUrl = RemotingServices.InternalEndPointResolver.GetRoleInstanceServerUrl(request);

                bool localResolveRequest = RemotingServices.InternalEndPointResolver.CanBeRequestResolvedLocal(request);
                if (localResolveRequest)
                {
                    

                    try
                    {
                        System.Runtime.Remoting.Messaging.CallContext.SetData("internalChannelUri", request.InternalChannelUri);
                        System.Runtime.Remoting.Messaging.CallContext.SetData("PublicChannelUri", request.PublicChannelUri);
                        try
                        {
                            request.EventCallBackChannel = this;
                            responseData = MessageDispatcher.MessageDispatch(request);
                            responseData.BidirectionalChannel = true;
                        }
                        finally
                        {
                            System.Runtime.Remoting.Messaging.CallContext.SetData("internalChannelUri", null);
                            System.Runtime.Remoting.Messaging.CallContext.SetData("PublicChannelUri", null);
                        }
                    }
                    catch (Exception error)
                    {
                        responseData = new ResponseData() { RequestID = request.RequestID, ClientProcessIdentity = request.ClientProcessIdentity,  details = error.Message };

                    }
                }
                else
                {

                    string x_Access_Token = null;
                    string x_Auth_Token = null;

                    RequestData forwordRequest = new RequestData() { ClientProcessIdentity = request.ClientProcessIdentity, PublicChannelUri = request.PublicChannelUri, InternalChannelUri = request.InternalChannelUri, details = request.details };

                    responseData = RemotingServices.Invoke(roleInstanceServerUrl, forwordRequest, x_Auth_Token, x_Access_Token);
                    responseData.RequestID = request.RequestID;
                    responseData.ClientProcessIdentity = request.ClientProcessIdentity;
                }

            }
            catch (Exception error)
            {
                responseData = new ResponseData() { details = error.Message };

            }
            
            string responseDatajson = Newtonsoft.Json.JsonConvert.SerializeObject(responseData);
           await SendMessage(responseDatajson);


        }
   }

}
