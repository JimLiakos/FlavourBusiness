using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace FlavourBusinessWorkerRole
{

    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Any)]
    public class DemoService : IDemoService
    {
        public string DoWork()
        {
     
            return "Hello World !!";
        }

      
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
    }
}
