
using ComputationalResources;
using FlavourBusinessManager;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using OOAdvantech.Remoting.RestApi;
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
                LogMessage.WriteLog("OnStart");
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
                System.Reflection.Assembly[] assemblies = new System.Reflection.Assembly[] { typeof(MenuPresentationModel.MenuCanvas.MenuCanvasFoodItem).Assembly, typeof(MenuModel.MenuItem).Assembly };
                OOAdvantech.ObjectsContext.Init(assemblies);


                //FlavourBusinessManagerApp.CloudTableStorageAccount = Microsoft.Azure.Cosmos.Table.CloudStorageAccount.DevelopmentStorageAccount;

                FlavourBusinessManagerApp.CloudTableStorageAccount = new Microsoft.Azure.Cosmos.Table.CloudStorageAccount(new Microsoft.Azure.Cosmos.Table.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="),true);

                //FlavourBusinessManagerApp.CloudBlobStorageAccount = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount;

                FlavourBusinessManagerApp.CloudBlobStorageAccount = new Microsoft.Azure.Storage.CloudStorageAccount(new Microsoft.Azure.Storage.Auth.StorageCredentials("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw=="), true);
                FlavourBusinessManagerApp.RootContainer = "$web";

                //FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName = Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName;
                LogMessage.WriteLog("computingResourceContext 1");
                FlavourBusinessManagerApp.FlavourBusinessStoragesAccountName = "angularhost";
                FlavourBusinessManagerApp.FlavourBusinessStoragesAccountkey = "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==";
                FlavourBusinessManagerApp.FlavourBusinessStoragesLocation = "angularhost";

                LogMessage.WriteLog("computingResourceContext 2");
                //ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage(Microsoft.Azure.Storage.CloudStorageAccount.DevelopmentStorageAccount.Credentials.AccountName, "", "");
                ComputingCluster.ClusterObjectStorage = FlavourBusinessManagerApp.OpenFlavourBusinessesResourcesStorage("angularhost", "YxNQAvlMWX7e7Dz78w/WaV3Z9VlISStF+Xp2DGigFScQmEuC/bdtiFqKqagJhNIwhsgF9aWHZIcpnFHl4bHHKw==", "angularhost");

                ComputingCluster.ClusterObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage(storageName,
                                                          storageLocation,
                                                          storageType, accountName, acountkey);

                LogMessage.WriteLog("computingResourceContext 3");
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ComputingCluster.ClusterObjectStorage);
                LogMessage.WriteLog("computingResourceContext 4");
                var computingResourceContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                                select computingContext).FirstOrDefault();


                LogMessage.WriteLog("computingResourceContext");
            }
            catch (Exception error)
            {
                LogMessage.WriteLog("Error : "+error.Message);

            }

            LogMessage.WriteLog("OnStart");

            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("FlavoursServicesWorkerRole has been started");


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





            return result;
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
