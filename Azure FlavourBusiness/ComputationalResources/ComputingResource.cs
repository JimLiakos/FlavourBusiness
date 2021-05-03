using System;
using System.Collections.Generic;

using Microsoft.WindowsAzure.ServiceRuntime;
using System.Linq;
using OOAdvantech.Transactions;

namespace ComputationalResources
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

        /// <MetaDataID>{037f1c23-df24-4f74-80f1-3996fcadaf04}</MetaDataID>
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

        /// <MetaDataID>{b399d63e-6d49-46b5-87a2-a5012338cf4e}</MetaDataID>
        public bool Update;


        /// <MetaDataID>{35f95e0b-ff3e-4e5b-afcc-3d1704426f52}</MetaDataID>
        List<IIsolatedComputingContext> _ComputingContexts = new List<IIsolatedComputingContext>();
        /// <MetaDataID>{6ee8a519-836d-463c-8a8c-2d52207c986d}</MetaDataID>
        public IList<IIsolatedComputingContext> ComputingContexts
        {
            get
            {
                return _ComputingContexts.AsReadOnly();
            }
        }

        /// <MetaDataID>{a0866363-805f-4117-b5fa-8a06e7fff82e}</MetaDataID>
        public int AvailableResources
        {
            get
            {
                if (ResourceIndex == 0)
                    return 28;
                if (this.ComputingContexts.Count == 0)
                    return 100;
                else
                    return 100 / (this.ComputingContexts.Count + 1);
            }
        }

        /// <MetaDataID>{6cac9cce-4a49-4103-9d94-ccc63ce673fc}</MetaDataID>
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

            if (computingContext.ComputingResourceID == ResourceIndex || computingContext.ResourceAllocationState==ResourceAllocationState.ReAssignState)
                if (HasAvailableResourcesFor(computingContext) || allowLowResourcesAssign)
                {
                    computingContext.ComputingResourceID = ResourceIndex;
                    if (!_ComputingContexts.Contains(computingContext))
                        _ComputingContexts.Add(computingContext);

                    computingContext.ResourceAllocationState = ResourceAllocationState.OperationState;

                    return true;
                }
            return false;

        }

        /// <MetaDataID>{e1ab0a69-2073-44e3-bd87-4b11baa0449f}</MetaDataID>
        private bool HasAvailableResourcesFor(IIsolatedComputingContext computingContext)
        {
            if (AvailableResources < 30)
                return false;
            return true;
        }

        /// <MetaDataID>{dba260cf-8bcc-4992-b1ec-a2fd15c7b69d}</MetaDataID>
        public void ReleaseComputingContext()
        {

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {
                foreach (var computingContext in _ComputingContexts.Where(x => !x.FixedComputingResource).ToList())
                {
                    //if (computingContext.FixedComputingResource)//.ContextID == StandardComputingContext.ComputingContext || computingContext.ContextID == StandardComputingContext.FlavourBusinessManagmenContext)
                    //    continue;

                    _ComputingContexts.Remove(computingContext);
                    computingContext.ResourceAllocationState = ResourceAllocationState.ReAssignState;
                }
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{08959826-16af-40c4-90e3-a7ad18658e8e}</MetaDataID>
        public void UpdateEndPoints(IList<EndPoint> computingResourceEndPoints)
        {
            _CommunicationEndpoints = computingResourceEndPoints;
        }
    }
}