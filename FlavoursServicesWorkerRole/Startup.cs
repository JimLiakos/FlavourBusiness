using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace FlavoursServicesWorkerRole
{
    using WebSocketAccept = System.Action<
         System.Collections.Generic.IDictionary<string, object>, // WebSocket Accept parameters
         System.Func< // WebSocketFunc callback
             System.Collections.Generic.IDictionary<string, object>, // WebSocket environment
             System.Threading.Tasks.Task>>;

    using WebSocketSendAsync = System.Func<
                   System.ArraySegment<byte>, // data
                   int, // message type
                   bool, // end of message
                   System.Threading.CancellationToken, // cancel
                   System.Threading.Tasks.Task>;
    // closeStatusDescription

    using WebSocketReceiveAsync = System.Func<
                System.ArraySegment<byte>, // data
                System.Threading.CancellationToken, // cancel
                System.Threading.Tasks.Task<
                    System.Tuple< // WebSocketReceiveTuple
                        int, // messageType
                        bool, // endOfMessage
                        int?, // count
                        int?, // closeStatus
                        string>>>; // closeStatusDescription

    using WebSocketCloseAsync = System.Func<
                int, // closeStatus
                string, // closeDescription
                System.Threading.CancellationToken, // cancel
                System.Threading.Tasks.Task>;
    using System.Net.WebSockets;
    using System.Threading;
    using System.Diagnostics;
    using System.Net.Http.Formatting;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    
    using System.Xml.XPath;

    class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
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


            appBuilder.UseWebApi(config);
            appBuilder.Use(UpgradeToWebSockets);
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
            var accept = context.Get<WebSocketAccept>("websocket.Accept");
            if (accept == null)
            {
                // Not a websocket request
                return next();
            }

            accept(null, WebSocketEcho);

            return Task.FromResult<object>(null);
        }

        private async Task WebSocketEcho(IDictionary<string, object> wsEnv)
        {
            var wsSendAsync = (WebSocketSendAsync)wsEnv["websocket.SendAsync"];
            var wsRecieveAsync = (WebSocketReceiveAsync)wsEnv["websocket.ReceiveAsync"];
            var wsCloseAsync = (WebSocketCloseAsync)wsEnv["websocket.CloseAsync"];
            var wsVersion = (string)wsEnv["websocket.Version"];
            var wsCallCancelled = (CancellationToken)wsEnv["websocket.CallCancelled"];

            // note: make sure to catch errors when calling sendAsync, receiveAsync and closeAsync
            // for simiplicity this code does not handle errors
            var buffer = new ArraySegment<byte>(new byte[6]);
            while (true)
            {
                var webSocketResultTuple = await wsRecieveAsync(buffer, wsCallCancelled);
                int wsMessageType = webSocketResultTuple.Item1;
                bool wsEndOfMessge = webSocketResultTuple.Item2;
                int? count = webSocketResultTuple.Item3;
                int? closeStatus = webSocketResultTuple.Item4;
                string closeStatusDescription = webSocketResultTuple.Item5;

                Debug.Write(Encoding.UTF8.GetString(buffer.Array, 0, count.Value));

                await wsSendAsync(new ArraySegment<byte>(buffer.Array, 0, count.Value), 1, wsEndOfMessge, wsCallCancelled);

                if (wsEndOfMessge)
                    break;
            }

            await wsCloseAsync((int)WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
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
