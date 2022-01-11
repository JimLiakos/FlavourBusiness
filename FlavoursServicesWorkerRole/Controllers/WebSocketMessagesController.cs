using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin;

namespace FlavoursServicesWorkerRole.Controllers
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    

    using WebSocketAccept = System.Action<
                              System.Collections.Generic.IDictionary<string, object>, // WebSocket Accept parameters
                              System.Func< // WebSocketFunc callback
                                  System.Collections.Generic.IDictionary<string, object>, // WebSocket environment
                                  System.Threading.Tasks.Task>>;
    using WebSocketCloseAsync = System.Func<
                                  int, // closeStatus
                                  string, // closeDescription
                                  System.Threading.CancellationToken, // cancel
                                  System.Threading.Tasks.Task>;
    // closeStatusDescription
    using WebSocketReceiveResult = System.Tuple<int, bool, int>;

    using WebSocketReceiveAsync = System.Func<
                System.ArraySegment<byte>, // data
                System.Threading.CancellationToken, // cancel
                System.Threading.Tasks.Task<
                    System.Tuple< // WebSocketReceiveTuple
                        int, // messageType
                        bool, // endOfMessage
                        int>>>; // count

    using WebSocketSendAsync = System.Func<
                                     System.ArraySegment<byte>, // data
                                     int, // message type
                                     bool, // end of message
                                     System.Threading.CancellationToken, // cancel
                                     System.Threading.Tasks.Task>;
    using System.Web;
    using System.ServiceModel.Channels;
    using Microsoft.ServiceModel.WebSockets;
    using Microsoft.Azure.Cosmos.Table;

    public class WebSocketMessagesController : ApiController
    {

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(System.ServiceModel.Channels.RemoteEndpointMessageProperty.Name))
            {
                
               RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
    

    public HttpResponseMessage Get()
        {
            Type webSocket = typeof(IWebSocket);

            webSocket = typeof(Microsoft.Web.WebSockets.WebSocketHandler);

            IOwinContext owinContext = Request.GetOwinContext();

            

            WebSocketAccept acceptToken = owinContext.Get<WebSocketAccept>("websocket.Accept");
            if (acceptToken != null)
            {
                var requestHeaders = GetValue<IDictionary<string, string[]>>(owinContext.Environment, "owin.RequestHeaders");

                Dictionary<string, object> acceptOptions = null;
                string[] subProtocols;
                if (requestHeaders.TryGetValue("Sec-WebSocket-Protocol", out subProtocols) && subProtocols.Length > 0)
                {
                    acceptOptions = new Dictionary<string, object>();
                    // Select the first one from the client
                    acceptOptions.Add("websocket.SubProtocol", subProtocols[0].Split(',').First().Trim());
                }
                //acceptOptions["Uri"] = owinContext.Request.Uri;
                //acceptOptions["RemoteIpAddress"] = owinContext.Request.RemoteIpAddress;

                acceptToken(acceptOptions, ProcessSocketConnection);


            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
            return new HttpResponseMessage(HttpStatusCode.SwitchingProtocols);
        }

        private async Task ProcessSocketConnection(IDictionary<string, object> wsEnv)
        {
            var wsSendAsync = (WebSocketSendAsync)wsEnv["websocket.SendAsync"];
            var wsCloseAsync = (WebSocketCloseAsync)wsEnv["websocket.CloseAsync"];
            var wsCallCancelled = (CancellationToken)wsEnv["websocket.CallCancelled"];
            var wsRecieveAsync = (WebSocketReceiveAsync)wsEnv["websocket.ReceiveAsync"];
            var webSocketContext = (System.Net.WebSockets.WebSocketContext)wsEnv["System.Net.WebSockets.WebSocketContext"];
            var RequestUri = webSocketContext.RequestUri.AbsoluteUri;
            string roleInstanceID = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;
            //var handler = new NoteSocketHandler(wsSendAsync, CancellationToken.None);
            ////var handler = new WebSocketServer(RequestUri,wsSendAsync, wsCloseAsync, CancellationToken.None,wsEnv);
            //handler.OnOpen();

            try
            {
                CloudStorageAccount cloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);

                CloudTableClient tableClient = cloudTableStorageAccount.CreateCloudTableClient();
                CloudTable logMessageTable = tableClient.GetTableReference("LogMessage");
                if (!logMessageTable.Exists())
                    logMessageTable.CreateIfNotExists();

                LogMessage logMessage = new LogMessage();
                logMessage.PartitionKey = "AAA";
                logMessage.RowKey = Guid.NewGuid().ToString();
                logMessage.Message = " websocket Open WorkerRole has been started";

                TableOperation insertOperation = TableOperation.Insert(logMessage);
                var executeResult = logMessageTable.Execute(insertOperation);
            }
            catch (Exception error)
            {
            }

            var buffer = new ArraySegment<byte>(new byte[4096]);
            try
            {
                object status;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                while (!wsEnv.TryGetValue("websocket.ClientCloseStatus", out status) || (int)status == 0)
                {
                    

                    WebSocketReceiveResult webSocketResultTuple = await wsRecieveAsync(buffer, CancellationToken.None);
                    int count = webSocketResultTuple.Item3;
                    memoryStream.Write(buffer.Array, 0, count);
                    if (webSocketResultTuple.Item2)
                    {
                        memoryStream.Position = 0;
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Dispose();
                        memoryStream = new System.IO.MemoryStream();
                        if (bytes.Length > 0)
                        {
                            string message = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                            //handler.OnMessage(message);

                            try
                            {
                                CloudStorageAccount cloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);

                                CloudTableClient tableClient = cloudTableStorageAccount.CreateCloudTableClient();
                                CloudTable logMessageTable = tableClient.GetTableReference("LogMessage");
                                if (!logMessageTable.Exists())
                                    logMessageTable.CreateIfNotExists();

                                LogMessage logMessage = new LogMessage();
                                logMessage.PartitionKey = "AAA";
                                logMessage.RowKey = Guid.NewGuid().ToString();
                                logMessage.Message = "websocket receive message :"+ message;

                                TableOperation insertOperation = TableOperation.Insert(logMessage);
                                var executeResult = logMessageTable.Execute(insertOperation);
                            }
                            catch (Exception error)
                            {
                            }
                        }



                    }
                }
            }
            catch (System.Net.WebSockets.WebSocketException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.ErrorCode == 258)
                {
                    await wsCloseAsync((int)WebSocketCloseStatus.EndpointUnavailable, "Closing", CancellationToken.None);
                    //handler.OnClose();
                }
                else
                {
                    await wsCloseAsync((int)WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None);
                    //handler.OnClose();
                }
                return;
            }


            //handler.OnClose();
            await wsCloseAsync((int)WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }

        T GetValue<T>(IDictionary<string, object> env, string key)
        {
            object value;
            return env.TryGetValue(key, out value) && value is T ? (T)value : default(T);
        }


    }
}
