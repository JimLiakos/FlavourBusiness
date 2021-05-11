using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.Threading;
using System.Threading.Tasks;
using ComputationalResources;
using FlavourBusinessFacade;
using FlavourBusinessManager;

using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Transactions;

namespace FlavourBusinessWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {

        //System.Timers.Timer FlavourBusinessesResourcesInitiatorTimer = new System.Timers.Timer(500);

        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);
        private IDisposable _publicApp = null;
        private IDisposable _internalApp = null;
        public override void Run()
        {
            FlavoursServicesContextManagment.Init();
            //FlavourBusinessesResourcesInitiatorTimer.Elapsed += FlavourBusinessesResourcesInitiatorTimer_Elapsed;
            //FlavourBusinessesResourcesInitiatorTimer.Interval = TimeSpan.FromSeconds(0.8).TotalMilliseconds;
            //FlavourBusinessesResourcesInitiatorTimer.Start();


            Trace.TraceInformation("FlavourBusinessWorkerRole is running");

            try
            {

                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }

        //private void FlavourBusinessesResourcesInitiatorTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    try
        //    {

        //        FlavourBusinessesResourcesInitiatorTimer.Stop();
        //    }
        //    catch (Exception error)
        //    {
        //    }

        //}

        //static string _AzureServerUrl = "http://localhost:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.2:8090/api/";
        //static string _AzureServerUrl = "http://192.168.2.7:8090/api/";

        static string _AzureServerUrl = "http://192.168.2.2:8090/api/";//Braxati
        //static string _AzureServerUrl = "http://192.168.2.8:8090/api/";//org
        //static string _AzureServerUrl = "http://10.0.0.8:8090/api/";//work



        internal static string AzureServerUrl
        {
            get
            {
                string azureStorageUrl = OOAdvantech.Remoting.RestApi.RemotingServices.GetLocalIPAddress();
                if (azureStorageUrl == null)
                    azureStorageUrl = _AzureServerUrl;
                else
                    azureStorageUrl = "http://" + azureStorageUrl + ":8090/api/";

                return azureStorageUrl;
            }
        }
        public override bool OnStart()
        {

#if DEBUG && !DeviceDotNet
            RemotingServices.SetDebugLeaseTime();
#else
            RemotingServices.SetProductionLeaseTime();
#endif

            IsolatedContext.AssignAppDomain(ComputingCluster.ComputingContextID, AppDomain.CurrentDomain);
            IsolatedContext.AssignAppDomain("httpInternal" + ComputingCluster.ComputingContextID, AppDomain.CurrentDomain);

            string serverPublicUrl = "http://localhost:8090/api/";
            serverPublicUrl = AzureServerUrl;
            RemotingServices.ServerPublicUrl = serverPublicUrl;


            string output = string.Format("- ð  ð - {1} RoleEnvironment.Changing for {0}:", RoleEnvironment.CurrentRoleInstance.Id, System.Diagnostics.Process.GetCurrentProcess().Id);
            System.Diagnostics.Debug.WriteLine(output);

            RemotingServices.RunInAzureRole = true;
            System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[] { typeof(MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem).Assembly, typeof(MenuModel.MenuItem).Assembly };
            OOAdvantech.PersistenceLayer.ObjectStorage.Init(assemblies);
            ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "");

            //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ComputingCluster.ClusterObjectStorage);
            //var isolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
            //                               //  where !computingContext.FixedComputingResource //computingContext.ContextID != StandardComputingContext.ComputingContext && computingContext.ContextID != StandardComputingContext.FlavourBusinessManagmenContext
            //                                 orderby computingContext.ComputingResourceID
            //                                 select computingContext).ToList();


            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            //{
            //    foreach (var ss in isolatedComputingContexts)
            //    {
            //        ss.FixedComputingResource = true;
            //    }
            //    stateTransition.Consistent = true;
            //}


            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("FlavourBusinessWorkerRole has been started");
            IsolatedContext.AppDomainInitializeType = typeof(AppDomainInitializer);


            #region Initialize communication end point

            var endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["httpInternal"];
            string baseUri = String.Format("{0}://{1}", endpoint.Protocol, endpoint.IPEndpoint);

            Trace.TraceInformation(String.Format("Starting OWIN at {0}", baseUri),
                "Information");
            _internalApp = WebApp.Start<Startup>(new StartOptions(url: baseUri));

            endpoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["httpPublic"];
            baseUri = String.Format("{0}://{1}", endpoint.Protocol, endpoint.IPEndpoint);

            Trace.TraceInformation(String.Format("Starting OWIN at {0}", baseUri),
                "Information");
            _publicApp = WebApp.Start<Startup>(new StartOptions(url: baseUri));

            #endregion

            #region initialize internal communication

            RemotingServices.InternalEndPointResolver = new InternalEndPointResolver();
            Authentication.InitializeFirebase("demomicroneme");
            CreateServiceHost();

            #endregion


            //FlavourBusinessManager.FlavourBusinessManagerApp.Init();


            ComputingCluster.CurrentComputingCluster.StartUp();

            RoleEnvironment.Changed += OnChanged;




            return result;
        }

        private void OnChanged(object sender, RoleEnvironmentChangedEventArgs e)
        {
            //if ((e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange)))
            //{
            //    e.Cancel = true;
            //}

            if (RoleEnvironment.IsEmulated)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("-ð- RoleEnvironment.Changing for {0}:", RoleEnvironment.CurrentRoleInstance.Id));
                //var traceSource = CreateTraceSource();
                //traceSource.TraceInformation("-ð- RoleEnvironment.Changing for {0}:", RoleEnvironment.CurrentRoleInstance.Id);

                foreach (RoleEnvironmentChange change in e.Changes)
                {
                    System.Diagnostics.Debug.WriteLine(" > {0}", change.GetType());
                }
            }

        }
        public override void OnStop()
        {
            Trace.TraceInformation("FlavourBusinessWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("FlavourBusinessWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {



            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(5000);
            }


        }
        private ServiceHost serviceHost;
        private void CreateServiceHost()
        {


            {
                serviceHost = new ServiceHost(typeof(OOAdvantech.Remoting.RestApi.WCFMessageDispatcher));
                NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
                //RoleInstanceEndpoint externalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["tcpinput"];
                //string endpoint = String.Format("net.tcp://{0}/WCFMessageDispatcher", externalEndPoint.IPEndpoint);
                //serviceHost.AddServiceEndpoint(typeof(OOAdvantech.Remoting.RestApi.IMessageDispatcher), binding, endpoint);

                RoleInstanceEndpoint endPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["tcpinternal"];
                string endpoint = String.Format("net.tcp://{0}/WCFMessageDispatcher", endPoint.IPEndpoint);
                serviceHost.AddServiceEndpoint(typeof(OOAdvantech.Remoting.RestApi.IMessageDispatcher), binding, endpoint);
                serviceHost.Open();
            }

            //serviceHost = new ServiceHost(typeof(DemoService));

            //NetTcpBinding binding = new NetTcpBinding(SecurityMode.None);
            //RoleInstanceEndpoint externalEndPoint =RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["tcpinput"];
            //string endpoint = String.Format("net.tcp://{0}/DemoService", externalEndPoint.IPEndpoint);
            //serviceHost.AddServiceEndpoint(typeof(IDemoService), binding, endpoint);

            //externalEndPoint = RoleEnvironment.CurrentRoleInstance.InstanceEndpoints["tcpinternal"];
            //endpoint = String.Format("net.tcp://{0}/DemoService", externalEndPoint.IPEndpoint);
            //serviceHost.AddServiceEndpoint(typeof(IDemoService), binding, endpoint);



            //serviceHost.Open();
        }
    }

    class InternalEndPointResolver : IInternalEndPointResolver
    {

        public InternalEndPointResolver()
        {

            if (ComputingCluster.CurrentComputingResource.ResourceIndex == 0)
            {
                var objectsStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "");
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectsStorage);


                var flavourBusinessManagmentContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                                       where computingContext.ContextID == StandardComputingContext.FlavourBusinessManagmenContext
                                                       select computingContext).FirstOrDefault();

                if (flavourBusinessManagmentContext == null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        flavourBusinessManagmentContext = new IsolatedComputingContext();
                        flavourBusinessManagmentContext.ContextID = StandardComputingContext.FlavourBusinessManagmenContext;
                        flavourBusinessManagmentContext.Description = "Manage flavour business";
                        objectsStorage.CommitTransientObjectState(flavourBusinessManagmentContext);
                        flavourBusinessManagmentContext.ComputingResourceID = 0;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        public bool CanBeResolvedLocal(TransportData request)
        {
            if (request.InternalChannelUri != null && request.InternalChannelUri.IndexOf("httpInternal") == 0)
                return true;

            if (!string.IsNullOrWhiteSpace(request.InternalChannelUri))
            {

                if (ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).ResourceID == RoleEnvironment.CurrentRoleInstance.Id)
                    return true;
                else
                    return false;

            }
            else
            {
                if (ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext).ResourceID == RoleEnvironment.CurrentRoleInstance.Id)
                {

                    request.ChannelUri = request.PublicChannelUri + "(" + StandardComputingContext.FlavourBusinessManagmenContext + ")";
                    return true;
                }
                else
                {
                    request.ChannelUri = request.PublicChannelUri + "(" + StandardComputingContext.FlavourBusinessManagmenContext + ")";
                    return false;
                }
            }
        }

        //public bool CanBeResponseResolvedLocal(TransportData request)
        //{
        //     if (!string.IsNullOrWhiteSpace(request.InternalChannelUri))
        //    {

        //        if (ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).ResourceID == RoleEnvironment.CurrentRoleInstance.Id)
        //            return true;
        //        else
        //            return false;

        //    }
        //    else
        //    {
        //        if (ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext).ResourceID == RoleEnvironment.CurrentRoleInstance.Id)
        //        {

        //            request.ChannelUri= request.PublicChannelUri+"("+ StandardComputingContext.FlavourBusinessManagmenContext+")";
        //            return true;
        //        }
        //        else
        //            return false;
        //    }
        //}


        public string GetInternalEndPointUrl(string contextID)
        {
            //var computingResource = ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(contextID);

            //string address = (from endPoint in computingResource.CommunicationEndpoints
            //                  where endPoint.Name == "tcpinternal"
            //                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
            //string roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);
            //roleInstanceServerUrl = roleInstanceServerUrl.Trim();
            //return roleInstanceServerUrl;


            string roleInstanceServerUrl = null;
            string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(contextID).CommunicationEndpoints
                              where endPoint.Name == "httpInternal"
                              select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
            roleInstanceServerUrl = string.Format("ws://{0}/api/", address);


            return roleInstanceServerUrl;
        }

        public string GetRoleInstanceServerUrl(string contextID)
        {
            string roleInstanceServerUrl = null;
            string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(contextID).CommunicationEndpoints
                              where endPoint.Name == "httpInternal"
                              select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
            roleInstanceServerUrl = string.Format("ws://{0}/api/", address);


            return roleInstanceServerUrl;
        }

        public string GetRoleInstanceServerUrl(TransportData request)
        {

            string roleInstanceServerUrl = null;
            if (!string.IsNullOrWhiteSpace(request.InternalChannelUri))
            {

                //string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).CommunicationEndpoints
                //                  where endPoint.Name == "tcpinternal"
                //                  select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
                //roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);

                string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).CommunicationEndpoints
                                  where endPoint.Name == "httpInternal"
                                  select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
                roleInstanceServerUrl = string.Format("ws://{0}/api/", address);



            }
            else
            {

                //request.InternalChannelUri = StandardComputingContext.FlavourBusinessManagmenContext;
                //string address = (from endPoint in computingResource.CommunicationEndpoints
                //                  where endPoint.Name == "tcpinternal"
                //                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
                //roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);

                var computingResource = ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext);
                string address = (from endPoint in computingResource.CommunicationEndpoints
                                  where endPoint.Name == "httpInternal"
                                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
                roleInstanceServerUrl = string.Format("ws://{0}/api/", address);

            }
            return roleInstanceServerUrl;

        }

        public string TranslateToPublic(string channelUri)
        {
            string publicChannelUri = null;
            string internalchannelUri = null;
            ObjRef.GetChannelUriParts(channelUri, out publicChannelUri, out internalchannelUri);
            if (internalchannelUri == null)
                return channelUri;
            if (GetInternalEndPointUrl(internalchannelUri).ToLower() == publicChannelUri.ToLower())
                channelUri = ComputingCluster.CurrentComputingCluster.GetRoleInstancePublicServerUrl(internalchannelUri);

            return channelUri;

        }


        //public string GetRoleInstanceServerUrl(ResponseData request)
        //{

        //    string roleInstanceServerUrl = null;
        //    if (!string.IsNullOrWhiteSpace(request.InternalChannelUri))
        //    {

        //        //string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).CommunicationEndpoints
        //        //                  where endPoint.Name == "tcpinternal"
        //        //                  select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
        //        //roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);

        //        string address = (from endPoint in ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(request.InternalChannelUri).CommunicationEndpoints
        //                          where endPoint.Name == "httpInternal"
        //                          select endPoint.Address + ":" + endPoint.Port).FirstOrDefault();
        //        roleInstanceServerUrl = string.Format("ws://{0}/api/", address);

        //        //RoleEnvironment.CurrentRoleInstance.InstanceEndpoints

        //    }
        //    else
        //    {
        //        var computingResource = ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(StandardComputingContext.FlavourBusinessManagmenContext);
        //        //request.InternalChannelUri = StandardComputingContext.FlavourBusinessManagmenContext;
        //        //string address = (from endPoint in computingResource.CommunicationEndpoints
        //        //                  where endPoint.Name == "tcpinternal"
        //        //                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
        //        //roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);
        //        string address = (from endPoint in computingResource.CommunicationEndpoints
        //                          where endPoint.Name == "httpInternal"
        //                          select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
        //        roleInstanceServerUrl = string.Format("ws://{0}/api/", address);
        //        //roleInstanceServerUrl = roleInstanceServerUrl.Trim();
        //    }
        //    return roleInstanceServerUrl;

        //}


    }

    public class AppDomainInitializer : IAppDomainInitializer, OOAdvantech.PersistenceLayer.IStorageLocatorEx
    {
        public StorageMetaData GetSorageMetaData(string storageIdentity)
        {
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            string serverUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.Substring(0, OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl.IndexOf("/api"));
            OOAdvantech.PersistenceLayer.StoragesClient storagesClient = new OOAdvantech.PersistenceLayer.StoragesClient(httpClient);

            storagesClient.BaseUrl = serverUrl;
            var task = storagesClient.GetAsync(storageIdentity);
            if (task.Wait(TimeSpan.FromSeconds(2)))
                return task.Result;
            else
                return new StorageMetaData();
        }

        public bool OnStart(string contextID)
        {
            RemotingServices.RunInAzureRole = true;

            System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[] { typeof(MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem).Assembly, typeof(MenuModel.MenuItem).Assembly };


            OOAdvantech.PersistenceLayer.ObjectStorage.Init(assemblies);

            string serverPublicUrl = "http://localhost:8090/api/";
            serverPublicUrl = WorkerRole.AzureServerUrl;

            RemotingServices.ServerPublicUrl = serverPublicUrl;
            RemotingServices.InternalEndPointResolver = new InternalEndPointResolver();

            ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "");

            FlavourBusinessManagerApp.Init(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "");

            Authentication.InitializeFirebase("demomicroneme");

            IsolatedComputingContext.CurrentContextID = contextID;

            OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.AddStorageLocatorExtender(this);
            return true;
        }
    }
}
