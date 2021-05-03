using System.Runtime.Serialization;
using OOAdvantech.MetaDataRepository;

namespace ComputationalResources
{
    /// <MetaDataID>{7b0444bf-940e-4f22-b3cf-e59a966b512d}</MetaDataID>
    public interface IComputingCluster
    {
        [Association("ComputingClusterResource", Roles.RoleA, "fe34eb36-f31c-45bc-8481-14094e84aee3")]
        [RoleAMultiplicityRange(1)]
        System.Collections.Generic.IList<IComputingResource> Resources { get; }

         
         
        /// <MetaDataID>{1d363fbf-bc8b-4c88-961b-ae3021466790}</MetaDataID>
        System.Collections.Generic.List<RoleInstanceInternalEndPoint> GetRoleInstancesInternalEndPoints(string endPointName);

        /// <MetaDataID>{0e65cf74-e33c-4f90-b75f-4cf0c87b96e7}</MetaDataID>
        IIsolatedComputingContext NewIsolatedComputingContext(string contextID, string contextDescription);
    }

    /// <MetaDataID>{f3e29619-94c6-4b74-a775-ee4510065df7}</MetaDataID>
    [DataContract]
    public class RoleInstanceInternalEndPoint
    {

        /// <MetaDataID>{68ed4ffd-d26e-4b34-8913-32ceb9e4ee6f}</MetaDataID>
        [DataMember]
        public string RoleInstanceID { get; set; }

        /// <MetaDataID>{0befefb3-98db-4c20-9aa8-5a12c2c98a9b}</MetaDataID>
        [DataMember]
        public string IPEndpoint { get; set; }

        /// <MetaDataID>{364a51ca-3b94-4a8a-a5d7-0c78d1fd5bb6}</MetaDataID>
        [DataMember]
        public string Protocol { get; set; }

    }
}