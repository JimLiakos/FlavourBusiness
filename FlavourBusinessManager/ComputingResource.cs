using System;
using System.Collections.Generic;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ComputingResources;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ComputingResources
{
    /// <MetaDataID>{bf3c2ca2-4b9c-480c-9230-106899672f1f}</MetaDataID>
    public class ComputingResource : IComputingResource
    {

        /// <MetaDataID>{92a79c88-b1fb-42f9-9b32-6be1e5d16aaf}</MetaDataID>
        public int ResourceIndex { get; set; }

        /// <MetaDataID>{689d2688-dd63-4c41-85db-96ae5ee338ac}</MetaDataID>
        private RoleInstance RoleInstance;

        /// <MetaDataID>{f6da11d2-ef22-4002-b211-10366c22411b}</MetaDataID>
        public ComputingResource(RoleInstance roleInstance)
        {
            this.RoleInstance = roleInstance;
            ResourceIndex = roleInstance.UpdateDomain;
        }

        IList<EndPoint> _CommunicationEndpoints;
        /// <MetaDataID>{4ffc62aa-10c8-431f-ad0d-1fa4ff783e00}</MetaDataID>
        public IList<EndPoint> CommunicationEndpoints
        {
            get
            {
                if (_CommunicationEndpoints == null)
                {
                    _CommunicationEndpoints = new List<EndPoint>();
                    foreach (var instanceEndpointEntry in RoleInstance.InstanceEndpoints)
                        _CommunicationEndpoints.Add(new EndPoint() { Name = instanceEndpointEntry.Key, Protocol = instanceEndpointEntry.Value.Protocol, Address = instanceEndpointEntry.Value.IPEndpoint.Address.ToString(), Port = instanceEndpointEntry.Value.IPEndpoint.Port });
                }
                return _CommunicationEndpoints;
            }
        }

        /// <MetaDataID>{cb3e213a-21fb-4cd2-b332-0790ba40dd8f}</MetaDataID>
        public string ResourceID
        {
            get
            {
                return RoleInstance.Id;
            }
        }

        public bool Update;


        List<IIsolatedComputingContext> _ComputingContexts = new List<IIsolatedComputingContext>();
        public IList<IIsolatedComputingContext> ComputingContexts
        {
            get
            {
                return _ComputingContexts.AsReadOnly();
            }
        }

        public int AvailableResources
        {
            get
            {
                if (ResourceIndex == 0)
                    return 28;
                if (this.ComputingContexts.Count == 0)
                    return 100;
                else
                    return 100 /( this.ComputingContexts.Count + 1);
            }
        }

        internal void ReloadCommunicationEndpoints()
        {
            _CommunicationEndpoints = null;
        }

        /// <MetaDataID>{c7cc7ea3-579b-437e-961a-9c2e7ad9dc7f}</MetaDataID>
        public bool Assign(IIsolatedComputingContext computingContext, bool allowLowResourcesAssign = false)
        {

            //if (ResourceIndex == 0 && (computingContext.ContextID == StandardComputingContext.ComputingContext || computingContext.ContextID == StandardComputingContext.FlavourBusinessManagmenContext))
            //{
            //    if (!_ComputingContexts.Contains(computingContext))
            //        _ComputingContexts.Add(computingContext);
            //    if (computingContext.ComputingResourceID != ResourceIndex)
            //        computingContext.ComputingResourceID = ResourceIndex;
            //    return true;
            //}
            //else

            if (computingContext.ComputingResourceID == ResourceIndex || computingContext.ComputingResourceID == -1)
                if (HasAvailableResourcesFor(computingContext) || allowLowResourcesAssign)
                {
                    computingContext.ComputingResourceID = ResourceIndex;
                    if (!_ComputingContexts.Contains(computingContext))
                        _ComputingContexts.Add(computingContext);

                    return true;
                }
            return false;

        }

        private bool HasAvailableResourcesFor(IIsolatedComputingContext computingContext)
        {
            if (AvailableResources < 30)
                return false;
            return true;
        }

        public void ReleaseComputingContext()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var computingContext in _ComputingContexts.ToList())
                {
                    if (computingContext.ContextID == StandardComputingContext.ComputingContext || computingContext.ContextID == StandardComputingContext.FlavourBusinessManagmenContext)
                        continue;

                    _ComputingContexts.Remove(computingContext);
                    computingContext.ComputingResourceID = -1;
                } 
                stateTransition.Consistent = true;
            }

        }

        public void UpdateEndPoints(IList<EndPoint> computingResourceEndPoints)
        {
            _CommunicationEndpoints = computingResourceEndPoints;
        }
    }
}