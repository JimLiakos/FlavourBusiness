using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Owin;
using OOAdvantech.Remoting.RestApi;

namespace FlavoursServicesWorkerRole.Controllers
{
    using System;
    using System.Net.WebSockets;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    //using WebSocketAccept = System.Action<
    //                          System.Collections.Generic.IDictionary<string, object>, // WebSocket Accept parameters
    //                          System.Func< // WebSocketFunc callback
    //                              System.Collections.Generic.IDictionary<string, object>, // WebSocket environment
    //                              System.Threading.Tasks.Task>>;
    //using WebSocketCloseAsync = System.Func<
    //                              int, // closeStatus
    //                              string, // closeDescription
    //                              System.Threading.CancellationToken, // cancel
    //                              System.Threading.Tasks.Task>;
    //// closeStatusDescription
    //using WebSocketReceiveResult = System.Tuple<int, bool, int>;

    //using WebSocketReceiveAsync = System.Func<
    //            System.ArraySegment<byte>, // data
    //            System.Threading.CancellationToken, // cancel
    //            System.Threading.Tasks.Task<
    //                System.Tuple< // WebSocketReceiveTuple
    //                    int, // messageType
    //                    bool, // endOfMessage
    //                    int>>>; // count

    //using WebSocketSendAsync = System.Func<
    //                                 System.ArraySegment<byte>, // data
    //                                 int, // message type
    //                                 bool, // end of message
    //                                 System.Threading.CancellationToken, // cancel
    //                                 System.Threading.Tasks.Task>;


    // http://owin.org/extensions/owin-WebSocket-Extension-v0.4.0.htm
    using WebSocketAccept = Action<IDictionary<string, object>, // options
        Func<IDictionary<string, object>, Task>>; // callback
    using WebSocketCloseAsync =
        Func<int /* closeStatus */,
            string /* closeDescription */,
            CancellationToken /* cancel */,
            Task>;
    using WebSocketReceiveAsync =
        Func<ArraySegment<byte> /* data */,
            CancellationToken /* cancel */,
            Task<Tuple<int /* messageType */,
                bool /* endOfMessage */,
                int /* count */>>>;
    using WebSocketSendAsync =
        Func<ArraySegment<byte> /* data */,
            int /* messageType */,
            bool /* endOfMessage */,
            CancellationToken /* cancel */,
            Task>;
    using WebSocketReceiveResult = Tuple<int, // type
        bool, // end of message?
        int>; // count

    using System.Web;
    using System.ServiceModel.Channels;
    using FlavoursServicesWorkerRole;
    using Microsoft.WindowsAzure.ServiceRuntime;
    using ComputationalResources;

    public class WebSocketMessagesController : ApiController
    {

        private string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }


        public HttpResponseMessage Get()
        {
            Type webSocket = typeof(Microsoft.ServiceModel.WebSockets.IWebSocket);

            webSocket = typeof(Microsoft.Web.WebSockets.WebSocketHandler);

            //IOwinContext owinContext = Request.GetOwinContext();
            IOwinContext owinContext = RequestContext.GetType().GetProperty("Context")?.GetValue(RequestContext) as IOwinContext;
             
        
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
                acceptToken(null, WebSocketEcho);
                //acceptToken(acceptOptions, ProcessSocketConnection);


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
            //var handler = new WebSocketHandler();
            var handler = new WebSocketServer(RequestUri, wsSendAsync, wsCloseAsync, CancellationToken.None, wsEnv);
            handler.OnOpen();
            var buffer = new ArraySegment<byte>(new byte[4096]);
            try
            {
                object status;
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                while (!wsEnv.TryGetValue("websocket.ClientCloseStatus", out status) || (int)status == 0)
                {

                    //try
                    //{

                    System.Threading.Thread.Sleep(2000);
                        WebSocketReceiveResult webSocketResultTuple = await wsRecieveAsync(buffer, CancellationToken.None);
                        if (webSocketResultTuple==null)
                        {

                        }
                        int count = webSocketResultTuple.Item3; //received bytes
                        memoryStream.Write(buffer.Array, 0, count);
                        if (webSocketResultTuple.Item2) //endOfMessage
                        {
                            memoryStream.Position = 0;
                            byte[] bytes = memoryStream.ToArray();
                            memoryStream.Dispose();
                            memoryStream = new System.IO.MemoryStream();
                            if (bytes.Length > 0&& webSocketResultTuple.Item1==1)
                                handler.OnMessage(Encoding.UTF8.GetString(bytes, 0, bytes.Length));

                            if (bytes.Length > 0 && webSocketResultTuple.Item1 == 2)
                                handler.OnData(bytes);


                        }
                    //}
                    //catch (System.Net.WebSockets.WebSocketException error)
                    //{

                        
                    //}
                }
            }
            catch (System.Net.WebSockets.WebSocketException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.ErrorCode == 258)
                {
                    await wsCloseAsync((int)WebSocketCloseStatus.EndpointUnavailable, "Closing", CancellationToken.None);
                    handler.OnClose();
                }
                else
                {
                    await wsCloseAsync((int)WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None);
                    handler.OnClose();
                }
                return;
            }


            handler.OnClose();
            await wsCloseAsync((int)WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }



        private Task UpgradeToWebSockets(IOwinContext context, Func<Task> next)
        {
            WebSocketAccept accept = context.Get<WebSocketAccept>("websocket.Accept");
            if (accept == null)
            {
                // Not a websocket request
                return next();
            }

            accept(null, WebSocketEcho);

            return Task.FromResult<object>(null);
        }

        private async Task WebSocketEcho(IDictionary<string, object> websocketContext)
        {
            var sendAsync = (WebSocketSendAsync)websocketContext["websocket.SendAsync"];
            var receiveAsync = (WebSocketReceiveAsync)websocketContext["websocket.ReceiveAsync"];
            var closeAsync = (WebSocketCloseAsync)websocketContext["websocket.CloseAsync"];
            var callCancelled = (CancellationToken)websocketContext["websocket.CallCancelled"];
            var webSocketContext = (System.Net.WebSockets.WebSocketContext)websocketContext["System.Net.WebSockets.WebSocketContext"];

            byte[] buffer = new byte[1024];


            var RequestUri = webSocketContext.RequestUri.AbsoluteUri;
            string roleInstanceID = Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;
            //var handler = new WebSocketHandler();
            var handler = new WebSocketServer(RequestUri, sendAsync, closeAsync, CancellationToken.None, websocketContext);

            try
            {

                WebSocketReceiveResult received = await receiveAsync(new ArraySegment<byte>(buffer), callCancelled);
                System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                object status;
                while (!websocketContext.TryGetValue("websocket.ClientCloseStatus", out status) || (int)status == 0)
                {
                    // Echo anything we receive
                    //await sendAsync(new ArraySegment<byte>(buffer, 0, received.Item3), received.Item1, received.Item2, callCancelled);
                    if (received==null)
                    {
                    }
                    int count = received.Item3; //received bytes
                    memoryStream.Write(buffer, 0, count);
                    if (received.Item2) //endOfMessage
                    {
                        memoryStream.Position = 0;
                        byte[] bytes = memoryStream.ToArray();
                        memoryStream.Dispose();
                        memoryStream = new System.IO.MemoryStream();
                        if (bytes.Length > 0&& received.Item1==1)
                            handler.OnMessage(Encoding.UTF8.GetString(bytes, 0, bytes.Length));

                        if (bytes.Length > 0 && received.Item1 == 2)
                            handler.OnData(bytes);
                    }
                    received = await receiveAsync(new ArraySegment<byte>(buffer), callCancelled);

                }
            }
            catch (System.Net.WebSockets.WebSocketException ex)
            {
                Console.WriteLine(ex.Message);
                if (ex.ErrorCode == 258)
                {
                    await closeAsync((int)WebSocketCloseStatus.EndpointUnavailable, "Closing", CancellationToken.None);
                    handler.OnClose();
                }
                else
                {
                    await closeAsync((int)WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None);
                    handler.OnClose();
                }
                return;
            }


            handler.OnClose();

            await closeAsync((int)websocketContext["websocket.ClientCloseStatus"], (string)websocketContext["websocket.ClientCloseDescription"], callCancelled);
        }

        T GetValue<T>(IDictionary<string, object> env, string key)
        {
            object value;
            return env.TryGetValue(key, out value) && value is T ? (T)value : default(T);
        }


    }

    public class WebSocketHandler
    {
        internal void OnClose()
        {

            
            LogMessage.WriteLog("websocket closed ");

            
        }

        internal void OnMessage(string message)
        {
        

            LogMessage.WriteLog("websocket message : " + message);

        }

        internal void OnOpen()
        {

          
            LogMessage.WriteLog("websocket opened ");

        }
    }
}
