using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade
{

    /// <MetaDataID>{fbb23ace-e52c-4b4d-bed9-8fd7221673ad}</MetaDataID>
    [ServiceContract]
    public interface IDemoService
    {

        /// <MetaDataID>{d95d7d59-ad0e-46c2-bb7c-68e95bded8ba}</MetaDataID>
        [OperationContract]
        string DoWork();

        //[OperationContract]
        //List<ComputingResources.RoleInstanceInternalEndPoint> GetRoleInstancesInternalEndPoints(string endPointName);

    }
   

}
