
using ComputationalResources;
using FlavourBusinessManager;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting.RestApi;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FlavoursServicesWorkerRole
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("FlavoursServicesWorkerRole is running");
            FlavoursServicesContextManagment.Init();

            string message = "Role instance run ";
            try
            {
                var roleName = Environment.MachineName;
                if (RoleEnvironment.IsAvailable)
                    roleName = RoleEnvironment.CurrentRoleInstance.Id;
                message += ": " + roleName;
            }
            catch (Exception error)
            {
            }

            LogMessage.WriteLog(message);


            try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
            }
            finally
            {
                this.runCompleteEvent.Set();
            }
        }
        private IDisposable _publicApp = null;
        private IDisposable _internalApp = null;


        static string _AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);
        internal static string AzureServerUrl
        {
            get
            {

                return _AzureServerUrl;
            }
        }


        public override bool OnStart()
        {

            try
            {
#if DEBUG && !DeviceDotNet
                RemotingServices.SetDebugLeaseTime();
#else
                RemotingServices.SetProductionLeaseTime();
#endif
                IsolatedContext.AssignAppDomain(ComputingCluster.ComputingContextID, AppDomain.CurrentDomain);
                IsolatedContext.AssignAppDomain("httpInternal" + ComputingCluster.ComputingContextID, AppDomain.CurrentDomain);


                string serverPublicUrl = "meridian-services.northeurope.cloudapp.azure.com:8090/api/";
                serverPublicUrl = AzureServerUrl;
                RemotingServices.ServerPublicUrl = serverPublicUrl;


                string output = string.Format("- ð  ð - {1} RoleEnvironment.Changing for {0}:", RoleEnvironment.CurrentRoleInstance.Id, System.Diagnostics.Process.GetCurrentProcess().Id);
                System.Diagnostics.Debug.WriteLine(output);

                RemotingServices.RunInAzureRole = true;
                System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[]
                {
                    typeof(MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem).Assembly,
                    typeof(MenuModel.MenuItem).Assembly,
                    typeof(OOAdvantech.DotNetMetaDataRepository.Assembly).Assembly,
                    typeof(OOAdvantech.PersistenceLayerRunTime.ObjectStorage).Assembly,
                    typeof(OOAdvantech.RDBMSMetaDataRepository.Storage).Assembly,
                    typeof(OOAdvantech.MetaDataLoadingSystem.Storage).Assembly,
                    typeof(OOAdvantech.WindowsAzureTablesPersistenceRunTime.Storage).Assembly
                };
                OOAdvantech.ObjectsContext.Init(assemblies);


                FlavourBusinessManagerApp.CloudTableStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;
                //FlavourBusinessManagerApp.CloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);

                FlavourBusinessManagerApp.CloudBlobStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount;
                //FlavourBusinessManagerApp.CloudBlobStorageAccount = new Microsoft.Azure.Storage.CloudStorageAccount(new Microsoft.Azure.Storage.Auth.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);
                //FlavourBusinessManagerApp.RootContainer = "$web";

                FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName;
                //FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName = "angularhost";
                //FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey = "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==";
                //FlavourBusinessManagerApp.FlavourBusinessStoragesLocation = "angularhost";

                //OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider.SetImplicitOpenStorageCredentials("angularhost", new StorageCredentials(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey));

                ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "", "");
                //ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==", "angularhost");





                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ComputingCluster.ClusterObjectStorage);
                var computingResourceContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                                select computingContext).FirstOrDefault();

                //if (computingResourceContext != null)
                //    LogMessage.WriteLog("computingResourceContext : " + computingResourceContext.Description);

                //LogMessage.WriteLog("computingResourceContext");
            }
            catch (Exception error)
            {
                LogMessage.WriteLog("Error : " + error.Message);

            }


            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("FlavoursServicesWorkerRole has been started");
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

            //CreateServiceHost(); // server for net.tcp:// internal channel

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
            Trace.TraceInformation("FlavoursServicesWorkerRole is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("FlavoursServicesWorkerRole has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }




    class InternalEndPointResolver : IInternalEndPointResolver
    {

        public InternalEndPointResolver()
        {

            if (ComputingCluster.CurrentComputingResource.ResourceIndex == 0)
            {
                var objectsStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey);
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
                if (string.IsNullOrWhiteSpace(address))
                {

                }
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

            System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[]
              {
                    typeof(MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem).Assembly,
                    typeof(MenuModel.MenuItem).Assembly,
                    typeof(OOAdvantech.DotNetMetaDataRepository.Assembly).Assembly,
                    typeof(OOAdvantech.PersistenceLayerRunTime.ObjectStorage).Assembly,
                    typeof(OOAdvantech.RDBMSMetaDataRepository.Storage).Assembly,
                    typeof(OOAdvantech.MetaDataLoadingSystem.Storage).Assembly,
                    typeof(OOAdvantech.WindowsAzureTablesPersistenceRunTime.Storage).Assembly
              };

            OOAdvantech.PersistenceLayer.ObjectStorage.Init(assemblies);

            string serverPublicUrl = "http://localhost:8090/api/";
            serverPublicUrl = WorkerRole.AzureServerUrl;

            RemotingServices.ServerPublicUrl = serverPublicUrl;

            FlavourBusinessManagerApp.Init(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "", "", null);

            //var start = DateTime.Now;
            //FlavourBusinessManagerApp.Init("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==", "angularhost", "$web");
            //LogMessage.WriteLog("Load time span : " + (DateTime.Now - start).ToString());
            //OOAdvantech.WindowsAzureTablesPersistenceRunTime.StorageProvider.SetImplicitOpenStorageCredentials("angularhost", new Microsoft.Azure.Cosmos.Table.StorageCredentials(FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName, FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey));



            RemotingServices.InternalEndPointResolver = new InternalEndPointResolver();

            ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "", "");
            //ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==", "angularhost");





            Authentication.InitializeFirebase("demomicroneme");

            IsolatedComputingContext.CurrentContextID = contextID;

            OOAdvantech.PersistenceLayer.StorageServerInstanceLocator.AddStorageLocatorExtender(this);
            return true;
        }
    }





    public class ErrorLog : IErrorLog
    {
        public void WriteError(string message)
        {
            LogMessage.WriteLog(message);
        }
    }
    public class LogMessage : Microsoft.Azure.Cosmos.Table.TableEntity
    {


        public static CloudStorageAccount CloudTableStorageAccount { get; private set; }
        public static CloudTableClient TableClient { get; private set; }
        public string Message { get; set; }


        internal static void WriteLog(string message)
        {
            try
            {
                if (CloudTableStorageAccount == null)
                    CloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);

                if (TableClient == null)
                    TableClient = CloudTableStorageAccount.CreateCloudTableClient();

                CloudTable logMessageTable = TableClient.GetTableReference("LogMessage");
                if (!logMessageTable.Exists())
                    logMessageTable.CreateIfNotExists();

                LogMessage logMessage = new LogMessage();
                logMessage.PartitionKey = "AAA";
                logMessage.RowKey = Guid.NewGuid().ToString();
                logMessage.Message = message;


                TableOperation insertOperation = TableOperation.Insert(logMessage);
                var executeResult = logMessageTable.Execute(insertOperation);
            }
            catch (Exception error)
            {
            }



        }
    }
}
