using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Web.Http;
using System.Linq;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using FlavourBusinessFacade;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Web;
using FlavourBusinessManager;

using System.Web.Http.Cors;

namespace FlavourBusinessWorkerRole.Controllers
{
    /// <MetaDataID>{bb598870-44f7-4a8e-a106-400ea37fce4b}</MetaDataID>
    /// 
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RestApiMessagesController : ApiController
    {
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2-1" };
        }
        public string Get(int id)
        {
            return "value1";
        }


        public ResponseData Post([FromBody]RequestData request)
        {
            ResponseData responseData = null;
            try
            {
                string roleInstanceServerUrl = RemotingServices.InternalEndPointResolver.GetRoleInstanceServerUrl(request);

                //string roleInstanceServerUrl = null;
                //if (!string.IsNullOrWhiteSpace(request.InternalChannelUri))
                //{

                //    string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).CommunicationEndpoints
                //                      where endPoint.Name == "tcpinternal" 
                //                      select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
                //    roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);

                //}
                //else
                //{
                //    var computingResource =ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext);
                //    request.InternalChannelUri = StandardComputingContext.FlavourBusinessManagmenContext;
                //    string address = (from endPoint in computingResource.CommunicationEndpoints
                //                      where endPoint.Name == "tcpinternal"
                //                      select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
                //    roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);
                //    roleInstanceServerUrl = roleInstanceServerUrl.Trim();
                //}

                string x_Access_Token = null;
                string x_Auth_Token = null;

                #region gets authentication tokens
                if (ActionContext.Request != null && this.ActionContext.Request.Headers != null)
                {
                    if (this.ActionContext.Request.Headers.Contains("X-Access-Token"))
                        x_Access_Token = this.ActionContext.Request.Headers.GetValues("X-Access-Token").First();
                    if (this.ActionContext.Request.Headers.Contains("X-Auth-Token"))
                        x_Auth_Token = this.ActionContext.Request.Headers.GetValues("X-Auth-Token").First();
                }
                #endregion

                responseData= RemotingServices.Invoke(roleInstanceServerUrl, request, x_Auth_Token, x_Access_Token);

            }
            catch (Exception error)
            {
                responseData = new ResponseData() { details = error.Message };
                
            }
            return responseData;
        }

        //private string GetJavaScriptProxy(Type type)
        //{
        //    var classifier = OOAdvantech.MetaDataRepository.Classifier.GetClassifier(type);


        //    List<string> methodNames = (from method in classifier.GetFeatures(true).OfType<Operation>()
        //                                where method.Visibility == VisibilityKind.AccessPublic &&
        //                                classifier.IsHttpVisible(method)
        //                                select method.Name).Distinct().ToList();

        //    if (methodNames.Count == 0)
        //        return null;

        //    string javaScript = @"function(url) {
        //                          this.url = url;";
        //    foreach (string method in methodNames)
        //        javaScript += string.Format( "\nthis.{0} = function() {{ return invoke(this.url, \"{0}\", arguments); }};",method);
        //    javaScript += "\n}";

        //    return javaScript;
        //}
    }


}
