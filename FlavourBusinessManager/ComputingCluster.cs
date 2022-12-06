using System;
using System.Collections.Generic;
using System.Diagnostics;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ComputingResources;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using OOAdvantech.Transactions;
using OOAdvantech.PersistenceLayer;

namespace FlavourBusinessManager.ComputingResources
{
    /// <MetaDataID>{2f1c4a25-1d0f-438a-a383-2852ca355863}</MetaDataID>
    public class ComputingCluster : IComputingCluster
    {
        static ComputingCluster _CurrentComputingCluster;
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

        System.Timers.Timer ResourcesAllocationTimer = new System.Timers.Timer(200);

        public System.Timers.Timer ComptingClusterAgentTimer = new System.Timers.Timer(200);
        /// <MetaDataID>{1afd83a5-8874-4033-84c3-96ee358aeff7}</MetaDataID>
        public void OnUpdateComputingClusterAgents(object source, System.Timers.ElapsedEventArgs e)
        {
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
                        roleInstanceServerUrl = roleInstanceServerUrl.Trim();
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

            //bool auto = true;
            //timer.AutoReset = auto;
        }

        /// <MetaDataID>{1baf463f-f47e-453e-b24f-2c3226958cf9}</MetaDataID>
        public ComputingContextRunTime GetComputingContextRunTime(string contextID)
        {
            IComputingResource computingResource = ComputingResources.ComputingCluster.CurrentComputingCluster.GetComputingResourceFor(contextID);

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


                roleInstanceServerUrl = OOAdvantech.Remoting.RestApi.RemotingServices.ServerPublicUrl;
                roleInstanceServerUrl = roleInstanceServerUrl.Trim() + "(" + contextID + ")";
                ComputingContextRunTime computingContextRunTime = OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(roleInstanceServerUrl, typeof(ComputingContextRunTime).FullName, typeof(ComputingContextRunTime).Assembly.FullName) as ComputingContextRunTime;
                return computingContextRunTime;
            }
            catch (Exception error)
            {

            }

            return null;
        }



        /// <MetaDataID>{823631bf-fbae-4c22-b66f-076b37540ac2}</MetaDataID>
        public void UpdateComputingCluster()
        {
            //var objectStorage = OpenFlavourBusinessesResourcesStorage();
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

            IsolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext.Refresh()).ToList();

            System.Diagnostics.Debug.WriteLine(string.Format("-ð- Update ComputingCluster  agent for {0}:", RoleEnvironment.CurrentRoleInstance.Id));

            //var sitems = (from foodItem in storage.GetObjectCollection<MenuModel.MenuItem>()
            //              select new { foodItem = foodItem.Refresh(foodItem.OptionsMenuItemSpecifics) }).ToArray();

        }


        public static ObjectStorage ClusterObjectStorage;

        /// <MetaDataID>{878bee7d-4b5b-4833-93da-754dda2e53d2}</MetaDataID>
        public void StartUp()
        {
            RoleEnvironment.Changed += OnChanged;
            RoleEnvironment.StatusCheck += RoleEnvironment_StatusCheck;
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

            ComputationalResourcesContext = (from computingContext in storage.GetObjectCollection<ComputingResources.IsolatedComputingContext>()
                                             where computingContext.ContextID == StandardComputingContext.ComputingContext
                                             select computingContext.Refresh()).FirstOrDefault();

            LastRefreshDateTime = System.DateTime.UtcNow;
            if (RoleEnvironment.CurrentRoleInstance.UpdateDomain == 0)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("-ð-ð- StartUp for {0}:", RoleEnvironment.CurrentRoleInstance.Id));
                if (ComputationalResourcesContext == null)
                {
                    using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                    {
                        ComputationalResourcesContext = new ComputingResources.IsolatedComputingContext();
                        ComputationalResourcesContext.ContextID = StandardComputingContext.ComputingContext;
                        ComputationalResourcesContext.Description = "Manage computing resources";
                        ClusterObjectStorage.CommitTransientObjectState(ComputationalResourcesContext);
                        stateTransition.Consistent = true;
                    }
                    Resources[0].Assign(ComputationalResourcesContext, true);
                }
            }

            ResourcesAllocationTimer.Elapsed += ResourcesAllocationTimer_Elapsed;
            ResourcesAllocationTimer.Interval = TimeSpan.FromMinutes(1).TotalMilliseconds;
            ResourcesAllocationTimer.Start();

            if (ComputationalResourcesContext != null && ComputationalResourcesContext.ComputingResourceID == ComputingCluster.CurrentComputingResource.ResourceIndex)
            {
                ReAssignIsolatedComputingContext();
                ComptingClusterAgentTimer.Enabled = true;
                ComptingClusterAgentTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnUpdateComputingClusterAgents);
                ComptingClusterAgentTimer.AutoReset = false;
            }
        }

        private void RoleEnvironment_StatusCheck(object sender, RoleInstanceStatusCheckEventArgs e)
        {
            
            
        }

        System.DateTime LastRefreshDateTime;
        IsolatedComputingContext ComputationalResourcesContext;
        private void ResourcesAllocationTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (ComputationalResourcesContext.ComputingResourceID == CurrentComputingResource.ResourceIndex)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

                var computingResourceContext = (from computingContext in storage.GetObjectCollection<ComputingResources.IsolatedComputingContext>()
                                                where computingContext.ContextID == StandardComputingContext.ComputingContext
                                                select computingContext.Refresh()).FirstOrDefault();

                ComputationalResourcesAllocation();

            }
            else if (((TimeSpan)(DateTime.UtcNow - LastRefreshDateTime)).TotalMinutes > 3)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

                var computingResourceContext = (from computingContext in storage.GetObjectCollection<ComputingResources.IsolatedComputingContext>()
                                                where computingContext.ContextID == StandardComputingContext.ComputingContext
                                                select computingContext.Refresh()).FirstOrDefault();

                LastRefreshDateTime = DateTime.UtcNow;
            }

        }

        void ComputationalResourcesAllocation()
        {
            ReAssignIsolatedComputingContext();
            ComptingClusterAgentTimer.Start();
        }



        /// <MetaDataID>{614e38ff-90f7-483f-b672-936be81e52c5}</MetaDataID>
        private void OnChanged(object sender, RoleEnvironmentChangedEventArgs e)
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

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);

            var isolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>()
                                             where computingContext.ContextID != StandardComputingContext.ComputingContext && computingContext.ContextID != StandardComputingContext.FlavourBusinessManagmenContext
                                             orderby computingContext.ComputingResourceID
                                             select computingContext).ToList();


            #region assign computing context to previous computing resource if it is practicable
            foreach (var computingResource in this.Resources.ToList())
            {
                foreach (var computingContext in isolatedComputingContexts.ToList())
                {
                    bool assigned = computingResource.Assign(computingContext);
                    if (assigned)
                    {
                        var theComputingResource = GetComputingResourceFor(computingContext.ContextID);
                        isolatedComputingContexts.Remove(computingContext);
                    }
                }
            }
            #endregion


            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                #region assign remaining computing context to computing resource with hight resource availability.
                if (isolatedComputingContexts.Count > 0)
                {
                    foreach (var computingResource in (from resurce in this.Resources orderby resurce.AvailableResources descending select resurce).ToList())
                    {
                        foreach (var computingContext in isolatedComputingContexts.ToList())
                        {
                            computingContext.ComputingResourceID = -1;
                            bool assigned = computingResource.Assign(computingContext);
                            if (assigned)
                            {
                                var theComputingResource = GetComputingResourceFor(computingContext.ContextID);
                                isolatedComputingContexts.Remove(computingContext);
                            }
                        }
                    }
                }

                #endregion

                #region assign remaining computing context to computing resource.Allow low computing resources assignment. 
                if (isolatedComputingContexts.Count > 0)
                {
                    foreach (var computingResource in (from resurce in this.Resources orderby resurce.AvailableResources descending select resurce).ToList())
                    {
                        foreach (var computingContext in isolatedComputingContexts.ToList())
                        {
                            computingContext.ComputingResourceID = -1;
                            bool assigned = computingResource.Assign(computingContext, true);
                            if (assigned)
                            {
                                var theComputingResource = GetComputingResourceFor(computingContext.ContextID);
                                isolatedComputingContexts.Remove(computingContext);
                            }
                        }
                    }
                }
                #endregion

                stateTransition.Consistent = true;
            }


        }


        static List<IsolatedComputingContext> IsolatedComputingContexts;
        /// <MetaDataID>{21073020-bfda-499e-a825-0b63adb7267e}</MetaDataID>
        public IComputingResource GetComputingResourceFor(string computingContextID)
        {
            if (IsolatedComputingContexts == null)
            {
                OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
                IsolatedComputingContexts = (from computingContext in storage.GetObjectCollection<IsolatedComputingContext>() select computingContext).ToList();
            }

            var favourBusinessManagmentContext = (from computingContext in IsolatedComputingContexts
                                                  where computingContext.ContextID == computingContextID
                                                  select computingContext).FirstOrDefault();
            return Resources[favourBusinessManagmentContext.ComputingResourceID];
        }

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

        public IIsolatedComputingContext NewIsolatedComputingContext(string contextID, string contextDescription)
        {

            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(ClusterObjectStorage);
            var computingResourceContext = (from computingContext in storage.GetObjectCollection<FlavourBusinessManager.ComputingResources.IsolatedComputingContext>()
                                            where computingContext.ContextID == contextID
                                            select computingContext).FirstOrDefault();
            if (computingResourceContext == null)
            {
                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
                {
                    computingResourceContext = new ComputingResources.IsolatedComputingContext();
                    computingResourceContext.ContextID = contextID;
                    computingResourceContext.Description = contextDescription;
                    ClusterObjectStorage.CommitTransientObjectState(computingResourceContext);
                    stateTransition.Consistent = true;
                    return computingResourceContext;
                }
            }
            else
                throw new Exception(string.Format("IsolatedComputingContext with contextID '{0}' already exist.", contextID));
        }
    }
}