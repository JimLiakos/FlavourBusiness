using OOAdvantech.MetaDataRepository;

namespace ComputationalResources
{
    /// <MetaDataID>{e533d4a1-c8ac-4dbc-8cf1-01b08cdb03fc}</MetaDataID>
    public class ResourceAllocator
    {
        /// <MetaDataID>{3ddc8eaf-dc0b-4ed5-ab49-17207d30036d}</MetaDataID>
        [Association("ClusterResourceAllocator", Roles.RoleB, "d6984cc2-c3fe-4eb7-98f9-038b984c2e2a")]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public ComputingCluster ComputingCluster;
    }
}