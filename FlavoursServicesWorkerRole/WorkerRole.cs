using Microsoft.Azure.Cosmos.Table;
using Microsoft.Owin.Hosting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
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
        public override bool OnStart()
        {
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
                logMessage.Message = "Fb WorkerRole has been started";

                TableOperation insertOperation = TableOperation.Insert(logMessage);
                var executeResult = logMessageTable.Execute(insertOperation);
            }
            catch (Exception error)
            {
            }


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
        public string Message { get; set; }
    }
}
