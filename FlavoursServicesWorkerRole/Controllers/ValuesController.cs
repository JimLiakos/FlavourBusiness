using ComputationalResources;
using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
//using System.Web.Http.Cors;

namespace FlavoursServicesWorkerRole.Controllers
{
    // [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ValuesController : ApiController
    {
        public string[] Get()
        {

             
                LogMessage .WriteLog( " Valuse :");
            return new string[] { "value 1.85", "value2--" };

            //return new HttpResponseMessage() { Version = new Version("1.1.1.3") } ;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
