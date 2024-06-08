using FlavourBusinessManager.Infrastructure;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FlavoursServicesWorkerRole.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UniqueIdController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "Lora";
        }

        [HttpGet]
        [Route("api/UniqueId/{fullIdentity}")]
        public string ShortIdentity(string fullIdentity)
        {
            return GlobalResourceIdentity.GetGlobalResourceIdentity(fullIdentity, true).RowKey;
        }

        [HttpGet]
        [Route("api/ExpiringUniqueId/{fullIdentity}/{expiringTimeSpanInMinutes}")]
        public string ExpiringShortIdentity(string fullIdentity,int expiringTimeSpanInMinutes)
        {
            return GlobalResourceIdentity.GetExpiringGlobalResourceIdentity(fullIdentity, expiringTimeSpanInMinutes).RowKey;
        }



        [HttpGet]
        [Route("api/UniqueId/FullIDFor/{identity}")]
        public string FullIDFor(string identity)
        {
            return GlobalResourceIdentity.GetGlobalResourceIdentityFor(identity)?.ResourceFullIdentity;
        }

        [HttpGet]
        [Route("api/UniqueId/FullIDForExpiringKey/{identity}")]
        public string FullIDForExpiringKey(string identity)
        {
            return GlobalResourceIdentity.GetGlobalResourceIdentityForExpiringKey(identity)?.ResourceFullIdentity;
        }

    }
}
