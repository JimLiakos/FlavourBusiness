using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;

namespace ComputationalResources
{
    /// <MetaDataID>{280f3e84-5733-4e47-8b3e-e00445b33fee}</MetaDataID>
    [BackwardCompatibilityID("{280f3e84-5733-4e47-8b3e-e00445b33fee}")]
    public interface IComputingResource
    {
         
        /// <MetaDataID>{40d57b15-0d00-4a68-afb0-289c519f2a81}</MetaDataID>
        int AvailableResources { get; }

        [Association("ComputingResourceConsumer", Roles.RoleA, "e4793b3e-a55d-4b5f-ab78-b8f6c2d8d77d")]
        System.Collections.Generic.IList<IIsolatedComputingContext> ComputingContexts { get; }

        /// <MetaDataID>{c1a8d90a-3f42-45f0-a2b7-dd20d5712a58}</MetaDataID>
        [Association("ComputingResourceEndPoint", Roles.RoleA, "60462fb6-e778-4f0e-b09e-635cca61bf67")]
        System.Collections.Generic.IList<EndPoint> CommunicationEndpoints { get; }

        /// <MetaDataID>{0d5fc5e7-2912-4b9b-9000-bf6c613dcf2d}</MetaDataID>
        string ResourceID { get; }


        /// <MetaDataID>{ccd5f29b-4074-46a2-a105-32bb5336773f}</MetaDataID>
        bool Assign(IIsolatedComputingContext computingContext, bool allowLowResourcesAssign = false);

        /// <MetaDataID>{a20c85a6-e37a-45b5-9bbc-17e4326ff873}</MetaDataID>
        int ResourceIndex { get; set; }

        /// <MetaDataID>{ecd59b42-7f57-4890-b36d-631a855c3cdb}</MetaDataID>
        void ReleaseComputingContext();
        /// <MetaDataID>{6b2b6b3f-9ed0-472c-a6bf-ace499238452}</MetaDataID>
        void UpdateEndPoints(IList<EndPoint> computingResourceEndPoints);

    }
}