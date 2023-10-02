using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.MetaDataRepository;

namespace ComputationalResources
{
    /// <MetaDataID>{2f1c4a25-1d0f-438a-a383-2852ca355863}</MetaDataID>
    public class ComputingCluster : IComputingCluster
    {


        /// <MetaDataID>{fd787641-adab-4b4b-be6b-27bafc74b926}</MetaDataID>
        [Association("ClusterResourceAllocator", Roles.RoleA, "d6984cc2-c3fe-4eb7-98f9-038b984c2e2a")]
        [OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(1, 1)]
        public ResourceAllocator ResourceAllocator;

        /// <MetaDataID>{777feaf1-17f1-4af9-a127-e3adf7329cb3}</MetaDataID>
        public const string ComputingContextID = "055980081b674aec9e774e8403cdc972";

        /// <exclude>Excluded</exclude>
        static ComputingCluster _CurrentComputingCluster;
        /// <MetaDataID>{a84e56e7-07cd-4435-b324-d67e63e8cbd2}</MetaDataID>
        public static ComputingCluster CurrentComputingCluster
        {
            get
            {
                if (RoleEnvironment.IsAvailable)
                {
                    if (_CurrentComputingCluster == null)
                        _CurrentComputingCluster = new ComputingCluster();
                }
                return _CurrentComputingCluster;
            }
        }

        /// <MetaDataID>{3439dfe1-f4a8-4dc6-b9ce-bab44fd6dcab}</MetaDataID>
        System.Timers.Timer ResourcesAllocationTimer = new System.Timers.Timer(200);

        /// <MetaDataID>{62749b75-ad2b-4917-b030-95ca38108dc6}</MetaDataID>
        public System.Timers.Timer ComptingClusterAgentTimer = new System.Timers.Timer(200);
        /// <MetaDataID>{1afd83a5-8874-4033-84c3-96ee358aeff7}</MetaDataID>
        public void OnUpdateComputingClusterAgents(object source, System.Timers.ElapsedEventArgs e)
        {
            lock (this)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
                var isolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext.Refresh()).ToList();

                foreach (var computingResource in Resources)
                {
                    if (computingResource.ResourceID != RoleEnvironment.CurrentRoleInstance.Id)
                    {
                        try
                        {
                            //string address = (from endPoint in computingResource.CommunicationEndpoints
                            //                  where endPoint.Name == "tcpinternal"
                            //                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
                            ////string roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);


                            string address = (from endPoint in computingResource.CommunicationEndpoints
                                              where endPoint.Name == "httpInternal"
                                              select endPoint.Address + ":" + endPoint.Port).ToArray()[0];

                            string roleInstanceServerUrl = string.Format("ws://{0}/api/", address);
                            //ws://127.255.0.1:20000/api/WebSocketMessages
                            roleInstanceServerUrl = roleInstanceServerUrl.Trim() + "(httpInternal" + ComputingCluster.ComputingContextID + ")";
                            ComputingContextRunTime computingContextRunTime = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(roleInstanceServerUrl, typeof(ComputingContextRunTime).FullName, typeof(ComputingContextRunTime).Assembly.FullName) as ComputingContextRunTime;
                            var computingResourceEndPoints = computingContextRunTime.UpdateComputingClusterAgent();
                            computingResource.UpdateEndPoints(computingResourceEndPoints);
                        }
                        catch (Exception error)
                        {
                            (computingResource as ComputingResource).ReloadCommunicationEndpoints();

                            ComptingClusterAgentTimer.Start();
                        }

                    }
                }
            }

            //bool auto = true;
            //timer.AutoReset = auto;
        }


        /// <MetaDataID>{5bd7d052-5f81-4c2d-87d0-cbe0ae3b9641}</MetaDataID>
        public string GetRoleInstancePublicServerUrl(string contextID)
        {


            ////string address = (from endPoint in computingResource.CommunicationEndpoints
            ////                  where endPoint.Name == "tcpinternal"
            ////                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
            ////string roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);

            //IComputingResource computingResource = ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(contextID);

            //string address = (from endPoint in computingResource.CommunicationEndpoints
            //                  where endPoint.Name == "httpInternal"
            //                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
            //string roleInstanceServerUrl = string.Format("http://{0}/api/", address);

            string roleInstanceServerUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl;
            roleInstanceServerUrl = roleInstanceServerUrl.Trim() + "(" + contextID + ")";

            return roleInstanceServerUrl;
        }
        /// <MetaDataID>{1baf463f-f47e-453e-b24f-2c3226958cf9}</MetaDataID>
        public ComputingContextRunTime GetComputingContextRunTime(string contextID)
        {
            IComputingResource computingResource = ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(contextID);

            try
            {

                //string address = (from endPoint in computingResource.CommunicationEndpoints
                //                  where endPoint.Name == "tcpinternal"
                //                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
                //string roleInstanceServerUrl = string.Format("net.tcp://{0}/", address);


                string address = (from endPoint in computingResource.CommunicationEndpoints
                                  where endPoint.Name == "httpInternal"
                                  select endPoint.Address + ":" + endPoint.Port).ToArray()[0];
                string roleInstanceServerUrl = string.Format("http://{0}/api/", address);

                //roleInstanceServerUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl;
                roleInstanceServerUrl = roleInstanceServerUrl.Trim() + "(" + contextID + ")";
                ComputingContextRunTime computingContextRunTime = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(roleInstanceServerUrl, typeof(ComputingContextRunTime).FullName, typeof(ComputingContextRunTime).Assembly.FullName) as ComputingContextRunTime;
                return computingContextRunTime;
            }
            catch (Exception error)
            {

            }

            return null;
        }

        public static void WriteOnEventLog(string source, string message, EventLogEntryType error)
        {

        }



        /// <MetaDataID>{823631bf-fbae-4c22-b66f-076b37540ac2}</MetaDataID>
        public void UpdateComputingCluster()
        {

            //bool utd = true;
            //if (utd)
            //    return;

            //var objectStorage = OpenFlavourBusinessesResourcesStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
            List<IsolatedComputingContext> movedIsolatedComputingContexts = new List<IsolatedComputingContext>();
            lock (this)
            {
                if (IsolatedComputingContexts == null)
                {
                    IsolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext).ToList();
                    return;
                }
            }

            var isolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext.Refresh()).ToList();
            lock (IsolatedComputingContexts)
            {
                foreach (var isoContext in IsolatedComputingContexts)
                {

                    if (isoContext.ComputingResourceID != ComputingCluster.CurrentComputingResource.ResourceIndex && isoContext.ContextID != ComputingCluster.ComputingContextID)
                        movedIsolatedComputingContexts.Add(isoContext);

                    //var oldStateIsoContext = (from updateIsoContext in IsolatedComputingContexts
                    //            where updateIsoContext.ContextID == isoContext.ContextID
                    //            select updateIsoContext).FirstOrDefault();


                    //if(oldStateIsoContext

                    //if ((from updateIsoContext in IsolatedComputingContexts 
                    // where updateIsoContext.ContextID== isoContext.ContextID
                    // select updateIsoContext).FirstOrDefault()==null)
                    //{
                    //    MovedIsolatedComputingContexts.Add(isoContext);
                    //}
                    //else
                    //{
                    //    MovedIsolatedComputingContexts.Remove(isoContext);
                    //}
                }
                IsolatedComputingContexts = isolatedComputingContexts;
            }

            foreach (var movedIsolatedContext in movedIsolatedComputingContexts)
            {
                OOAdvantech.Remoting.RestApi.IsolatedContext.UnloadIsolatedContext(movedIsolatedContext.ContextID);
            }


            System.Diagnostics.Debug.WriteLine(string.Format("-ð- Update ComputingCluster  agent for {0}:", RoleEnvironment.CurrentRoleInstance.Id));

            //var sitems = (from foodItem in storage.GetObjectCollection<MenuModel.MenuItem>()
            //              select new { foodItem = foodItem.Refresh(foodItem.OptionsMenuItemSpecifics) }).ToArray();

        }


        /// <MetaDataID>{075ddc9d-6644-4c8f-bb5d-f3d3046080ec}</MetaDataID>
        public static ObjectStorage ClusterObjectStorage;

        /// <MetaDataID>{878bee7d-4b5b-4833-93da-754dda2e53d2}</MetaDataID>
        public void StartUp()
        {
            RoleEnvironment.Changed += OnChanged;
            RoleEnvironment.StatusCheck += RoleEnvironment_StatusCheck;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

            ComputationalResourcesContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                             where computingContext.ContextID == ComputingCluster.ComputingContextID
                                             select computingContext.Refresh()).FirstOrDefault();

            LastRefreshDateTime = System.DateTime.UtcNow;
            if (RoleEnvironment.CurrentRoleInstance.UpdateDomain == 0)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("-ð-ð- StartUp for {0}:", RoleEnvironment.CurrentRoleInstance.Id));
                if (ComputationalResourcesContext == null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        ComputationalResourcesContext = new IsolatedComputingContext();
                        ComputationalResourcesContext.ContextID = ComputingCluster.ComputingContextID;
                        ComputationalResourcesContext.Description = "Manage computing resources";
                        ComputationalResourcesContext.FixedComputingResource = true;
                        ComputationalResourcesContext.ComputingResourceID = 0;
                        ClusterObjectStorage.CommitTransientObjectState(ComputationalResourcesContext);

                        stateTransition.Consistent = true;
                    }
                    Resources[0].Assign(ComputationalResourcesContext, true);
                }
            }

            ResourcesAllocationTimer.Elapsed += ResourcesAllocationTimer_Elapsed;
            ResourcesAllocationTimer.Interval = TimeSpan.FromSeconds(1).TotalMilliseconds;
            ResourcesAllocationTimer.Start();

            if (ComputationalResourcesContext != null && ComputationalResourcesContext.ComputingResourceID == ComputingCluster.CurrentComputingResource.ResourceIndex)
            {
                ReAssignIsolatedComputingContext();
                ComptingClusterAgentTimer.Enabled = true;
                //ComptingClusterAgentTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnUpdateComputingClusterAgents);
                ComptingClusterAgentTimer.AutoReset = false;
            }
        }

        /// <MetaDataID>{81e944db-0054-4079-b5e6-8a6abd0466b8}</MetaDataID>
        private void RoleEnvironment_StatusCheck(object sender, RoleInstanceStatusCheckEventArgs e)
        {


        }

        /// <MetaDataID>{483d0c46-ec7c-43e3-bd5b-b8afe0e2cf7f}</MetaDataID>
        DateTime LastRefreshDateTime;

        DateTime LastResourceAllocationhDateTime;

        object ResourcesAllocationLock = new object();
        bool InResourcesAllocation = false;


        /// <MetaDataID>{84f9ac62-febc-47a0-b9ac-8dc4155928df}</MetaDataID>
        IsolatedComputingContext ComputationalResourcesContext;
        /// <MetaDataID>{8f55b78d-b294-4ef7-8207-010adfdcb6c9}</MetaDataID>
        private void ResourcesAllocationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            //bool utd = true;
            //if (utd)
            //    return;
            lock (ResourcesAllocationLock)
            {
                if (InResourcesAllocation)
                    return;
            }
            try
            {
                InResourcesAllocation = true;

                if (ComputationalResourcesContext.ComputingResourceID == CurrentComputingResource.ResourceIndex)
                {
                    if (((TimeSpan)(DateTime.UtcNow - LastResourceAllocationhDateTime)).TotalMinutes > 0.5)
                    {
                        OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

                        var computingResourceContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                                        where computingContext.ContextID == ComputingCluster.ComputingContextID
                                                        select computingContext.Refresh()).FirstOrDefault();

                        ComputationalResourcesAllocation();
                        LastResourceAllocationhDateTime = DateTime.UtcNow;
                    }

                }
                else if (((TimeSpan)(DateTime.UtcNow - LastRefreshDateTime)).TotalMinutes > 3)
                {
                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

                    var computingResourceContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                                    where computingContext.ContextID == ComputingCluster.ComputingContextID
                                                    select computingContext.Refresh()).FirstOrDefault();

                    LastRefreshDateTime = DateTime.UtcNow;
                }
            }
            finally
            {
                InResourcesAllocation = false;
            }
        }

        /// <MetaDataID>{7ffd0b9a-d415-4c18-83d2-36cccdb21175}</MetaDataID>
        void ComputationalResourcesAllocation()
        {
            ReAssignIsolatedComputingContext();
            ComptingClusterAgentTimer.Start();
        }



        /// <MetaDataID>{614e38ff-90f7-483f-b672-936be81e52c5}</MetaDataID>
        private void OnChanged(object sender, Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironmentChangedEventArgs e)
        {
            ComputationalResourcesAllocation();

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

        /// <MetaDataID>{d0e11629-b7b1-4d7f-9a7c-8b5fa08659ea}</MetaDataID>
        private void ReAssignIsolatedComputingContext()
        {

            if (ComputingCluster.CurrentComputingResource.ResourceIndex != 0)
                return;

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

            var isolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                             where !computingContext.FixedComputingResource //computingContext.ContextID != StandardComputingContext.ComputingContext && computingContext.ContextID != StandardComputingContext.FlavourBusinessManagmenContext
                                             orderby computingContext.ComputingResourceID
                                             select computingContext).ToList();


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var computingContext in isolatedComputingContexts.ToList())
                {
                    bool assigned = false;
                    #region assign computing context to previous computing resource if it is practicable
                    foreach (var computingResource in this.Resources.ToList())
                    {
                        //foreach (var computingContext in isolatedComputingContexts.ToList())
                        {
                            assigned = computingResource.Assign(computingContext);
                            if (assigned)
                            {
                                var theComputingResource = GetComputingResourceFor(computingContext.ContextID);
                                isolatedComputingContexts.Remove(computingContext);
                                break;
                            }
                            else
                            {
                            }
                        }
                    }
                    if (assigned)
                        break;
                    #endregion




                    #region assign remaining computing context to computing resource with hight resource availability.
                    //if (isolatedComputingContexts.Count > 0)
                    {
                        foreach (var computingResource in (from resurce in this.Resources orderby resurce.AvailableResources descending select resurce).ToList())
                        {
                            // foreach (var computingContext in isolatedComputingContexts.ToList())
                            {
                                computingContext.ResourceAllocationState = ResourceAllocationState.ReAssignState;
                                assigned = computingResource.Assign(computingContext);
                                if (assigned)
                                {
                                    var theComputingResource = GetComputingResourceFor(computingContext.ContextID);
                                    isolatedComputingContexts.Remove(computingContext);
                                    break;
                                }
                            }
                        }
                    }
                    if (assigned)
                        break;

                    #endregion

                    #region assign remaining computing context to computing resource.Allow low computing resources assignment. 
                    // if (isolatedComputingContexts.Count > 0)
                    {
                        foreach (var computingResource in (from resurce in this.Resources orderby resurce.AvailableResources descending select resurce).ToList())
                        {
                            // foreach (var computingContext in isolatedComputingContexts.ToList())
                            {
                                computingContext.ResourceAllocationState = ResourceAllocationState.ReAssignState;
                                assigned = computingResource.Assign(computingContext, true);
                                if (assigned)
                                {
                                    var theComputingResource = GetComputingResourceFor(computingContext.ContextID);
                                    isolatedComputingContexts.Remove(computingContext);
                                    break;
                                }
                            }
                        }
                    }
                    #endregion



                }
                stateTransition.Consistent = true;
            }


            isolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                             //where !computingContext.FixedComputingResource //computingContext.ContextID != StandardComputingContext.ComputingContext && computingContext.ContextID != StandardComputingContext.FlavourBusinessManagmenContext
                                         orderby computingContext.ComputingResourceID
                                         select computingContext).ToList();


            foreach (var isoContext in isolatedComputingContexts)
            {

                if (isoContext.ComputingResourceID != ComputingCluster.CurrentComputingResource.ResourceIndex)
                    OOAdvantech.Remoting.RestApi.IsolatedContext.UnloadIsolatedContext(isoContext.ContextID);

            }
        }


        /// <MetaDataID>{e8c69770-9de8-461b-b04c-bab2eb6fa0f3}</MetaDataID>
        static List<IsolatedComputingContext> IsolatedComputingContexts = null;



        /// <MetaDataID>{21073020-bfda-499e-a825-0b63adb7267e}</MetaDataID>
        public IComputingResource GetComputingResourceFor(string computingContextID)
        {
            lock (this)
            {
                if (IsolatedComputingContexts == null)
                {

                    OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
                    IsolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext).ToList();
                }
            }
            IsolatedComputingContext favourBusinessManagmentContext = null;
            lock (IsolatedComputingContexts)
            {
                favourBusinessManagmentContext = (from computingContext in IsolatedComputingContexts
                                                  where computingContext.ContextID == computingContextID
                                                  select computingContext).FirstOrDefault();
            }
            if (favourBusinessManagmentContext == null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
                IsolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext).ToList();
                lock (IsolatedComputingContexts)
                {
                    favourBusinessManagmentContext = (from computingContext in IsolatedComputingContexts
                                                      where computingContext.ContextID == computingContextID
                                                      select computingContext).FirstOrDefault();
                }
            }
            return Resources[favourBusinessManagmentContext.ComputingResourceID];
        }

        /// <exclude>Excluded</exclude>
        Dictionary<string, ComputingResource> _Resources = new Dictionary<string, ComputingResource>();
        /// <MetaDataID>{8f5b67f3-408d-437b-8db3-16d3b59e6ad9}</MetaDataID>
        public IList<IComputingResource> Resources
        {
            get
            {
                lock (_Resources)
                {
                    var removeResources = _Resources.Values.ToList();
                    List<IComputingResource> resources = new List<IComputingResource>();
                    foreach (var roleInstance in RoleEnvironment.CurrentRoleInstance.Role.Instances.OrderBy(x => x.Id))
                    {
                        ComputingResource resource = null;
                        if (!_Resources.TryGetValue(roleInstance.Id, out resource))
                        {
                            resource = new ComputingResource(roleInstance);
                            _Resources[roleInstance.Id] = resource;
                        }
                        else
                            removeResources.Remove(resource);
                        resources.Add(resource);
                    }
                    return resources;
                }
            }
        }

        /// <MetaDataID>{5cfe86b7-2dc8-4280-a53c-d1b9913e757b}</MetaDataID>
        public static IComputingResource CurrentComputingResource
        {
            get
            {
                return (from computingResource in CurrentComputingCluster.Resources
                        where computingResource.ResourceID == RoleEnvironment.CurrentRoleInstance.Id
                        select computingResource).FirstOrDefault();
            }
        }

        /// <MetaDataID>{3b657af4-0735-4687-9249-39318606e8df}</MetaDataID>
        public List<RoleInstanceInternalEndPoint> GetRoleInstancesInternalEndPoints(string endPointName)
        {
            List<RoleInstanceInternalEndPoint> instanceInternalEndPoints = new List<RoleInstanceInternalEndPoint>();
            try
            {
                foreach (var role in RoleEnvironment.Roles)
                {
                    foreach (var roleInstance in role.Value.Instances)
                    {
                        foreach (var instanceEndpointEntry in roleInstance.InstanceEndpoints)
                        {
                            if (instanceEndpointEntry.Key == endPointName)
                            {
                                instanceInternalEndPoints.Add(new RoleInstanceInternalEndPoint() { RoleInstanceID = roleInstance.Id, Protocol = instanceEndpointEntry.Value.Protocol, IPEndpoint = instanceEndpointEntry.Value.IPEndpoint.ToString() });
                                Trace.WriteLine(instanceEndpointEntry.Key + ": Instance endpoint IP address and port: " + instanceEndpointEntry.Value.IPEndpoint, "Information");
                            }
                        }
                    }
                }
            }
            catch (Exception error)
            {
            }
            return instanceInternalEndPoints;
        }

        /// <MetaDataID>{0f44dc10-b5b3-4e4a-8bba-9edb443d166d}</MetaDataID>
        public IIsolatedComputingContext NewIsolatedComputingContext(string contextID, string contextDescription)
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
            var computingResourceContext = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                            where computingContext.ContextID == contextID
                                            select computingContext).FirstOrDefault();
            if (computingResourceContext == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    computingResourceContext = new IsolatedComputingContext();
                    computingResourceContext.ContextID = contextID;
                    computingResourceContext.Description = contextDescription;
                    ClusterObjectStorage.CommitTransientObjectState(computingResourceContext);
                    stateTransition.Consistent = true;

                    if (IsolatedComputingContexts != null)
                    {
                        lock (IsolatedComputingContexts)
                        {
                            IsolatedComputingContexts.Add(computingResourceContext);
                        }
                    }
                    return computingResourceContext;
                }
            }
            else
                throw new Exception(string.Format("IsolatedComputingContext with contextID '{0}' already exist.", contextID));
        }
    }
}