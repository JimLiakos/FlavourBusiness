using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Azure.Documents.SystemFunctions;
using Microsoft.Owin;
using OOAdvantech.Remoting.RestApi;
using Owin;

namespace FlavoursServicesWorkerRole
{
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


    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.Use(UpgradeToWebSockets);
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "Default",
                "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            //config.Formatters.Clear();
            //config.Formatters.Add(new JsonMediaTypeFormatter());
            //config.Formatters.JsonFormatter.SerializerSettings =
            //new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //};
            //CorsMiddlewareOptions corsOptions = new CorsMiddlewareOptions();
            //corsOptions.AllowedOrigins = "*";
            //corsOptions.AllowedRequestMethods = "POST, PUT, DELETE";
            //corsOptions.AllowedRequestHeaders = "Content-Type, Accept,X-Auth-Token,X-Access-Token,X-Auth-UserID";
            //corsOptions.MaxPreflightedRequestCacheDuration = "1728000";
            //corsOptions.AllowedRequestCredentials = "true";
            //app.Use<CorsMiddleware>(corsOptions);
            config.EnableCors();
            appBuilder.UseWebApi(config);


            
            
            //config.EnableSwagger(c =>
            //{

            //    c.SingleApiVersion("v1", "A title for your API");
            //    c.IncludeXmlComments(GetXmlCommentsPath());

            //}).EnableSwaggerUi();


        }


        private string GetXmlCommentsPath()
        {
            return AppDomain.CurrentDomain.BaseDirectory + @"\FlavourBusinessWorkerRole.XML";
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
            catch (System.Exception ex)
            {
                await closeAsync((int)WebSocketCloseStatus.InternalServerError, "Closing", CancellationToken.None);
                handler.OnClose();
                return;

            }


            handler.OnClose();

            await closeAsync((int)websocketContext["websocket.ClientCloseStatus"], (string)websocketContext["websocket.ClientCloseDescription"], callCancelled);
        }


        //private Task UpgradeToWebSockets(IOwinContext context, Func<Task> next)
        //{
        //    var accept = context.Get<WebSocketAccept>("websocket.Accept");
        //    if (accept == null)
        //    {
        //        // Not a websocket request
        //        return next();
        //    }

        //    accept(null, WebSocketEcho);

        //    return Task.FromResult<object>(null);
        //}

        //private async Task WebSocketEcho(IDictionary<string, object> wsEnv)
        //{
        //    var wsSendAsync = (WebSocketSendAsync)wsEnv["websocket.SendAsync"];
        //    var wsRecieveAsync = (WebSocketReceiveAsync)wsEnv["websocket.ReceiveAsync"];
        //    var wsCloseAsync = (WebSocketCloseAsync)wsEnv["websocket.CloseAsync"];
        //    var wsVersion = (string)wsEnv["websocket.Version"];
        //    var wsCallCancelled = (CancellationToken)wsEnv["websocket.CallCancelled"];

        //    // note: make sure to catch errors when calling sendAsync, receiveAsync and closeAsync
        //    // for simiplicity this code does not handle errors
        //    var buffer = new ArraySegment<byte>(new byte[6]);
        //    while (true)
        //    {
        //        var webSocketResultTuple = await wsRecieveAsync(buffer, wsCallCancelled);
        //        int wsMessageType = webSocketResultTuple.Item1;
        //        bool wsEndOfMessge = webSocketResultTuple.Item2;
        //        int? count = webSocketResultTuple.Item3;
        //        int? closeStatus = webSocketResultTuple.Item4;
        //        string closeStatusDescription = webSocketResultTuple.Item5;

        //        Debug.Write(Encoding.UTF8.GetString(buffer.Array, 0, count.Value));

        //        await wsSendAsync(new ArraySegment<byte>(buffer.Array, 0, count.Value), 1, wsEndOfMessge, wsCallCancelled);

        //        if (wsEndOfMessge)
        //            break;
        //    }

        //    await wsCloseAsync((int)WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        //}
    }


    public class CorsMiddleware : OwinMiddleware
    {
        CorsMiddlewareOptions _options;

        public CorsMiddleware(OwinMiddleware next, CorsMiddlewareOptions options)
            : base(next)
        {
            _options = options;
        }

        public override Task Invoke(IOwinContext context)
        {
            //ORIGIN
            var origin = context.Request.Headers.Get("Origin");
            if (!String.IsNullOrEmpty(origin) && _options.AllowedOrigins.Contains(origin))
            {
                context.Response.Headers.Add("Access-Control-Allow-Origin", new[] { origin });
            }

            if (!String.IsNullOrEmpty(_options.AllowedRequestCredentials))
                context.Response.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Credentials", _options.AllowedRequestCredentials);

            //OPTIONS
            if (context.Request.Method == "OPTIONS")
            {
                #region Methods

                var accessControlRequestMethod = context.Request.Headers.Get("Access-Control-Request-Method");

                if (_options.AllowedRequestMethods.Contains(accessControlRequestMethod))
                {
                    context.Response.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Methods", accessControlRequestMethod);
                }

                #endregion

                #region Headers

                var accessControlRequestHeaders = context.Request.Headers.Get("Access-Control-Request-Headers");

                if (!String.IsNullOrEmpty(accessControlRequestHeaders))
                {
                    var requestedHeaders = accessControlRequestHeaders.Split(',');
                    var allowedHeaders = requestedHeaders.Where((h) => _options.AllowedRequestHeaders.Contains(h.Trim())).ToArray();

                    if (allowedHeaders.Length == requestedHeaders.Length)
                    {
                        context.Response.Headers.AppendCommaSeparatedValues("Access-Control-Allow-Headers", allowedHeaders);
                    }
                }

                #endregion

                #region Preflight Cache

                context.Response.Headers.AppendCommaSeparatedValues("Access-Control-Max-Age", _options.MaxPreflightedRequestCacheDuration);

                #endregion

                return Task.FromResult<IOwinContext>(context);
            }
            else
            {
                return base.Next.Invoke(context);
            }
        }
    }

    public class CorsMiddlewareOptions
    {
        public string AllowedOrigins { get; set; }
        public string AllowedRequestMethods { get; set; }
        public string AllowedRequestHeaders { get; set; }
        public string MaxPreflightedRequestCacheDuration { get; set; }
        public string AllowedRequestCredentials { get; set; }
    }
}
