using ComputationalResources;
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
        //public string[] Get()
        //{


        //        LogMessage .WriteLog( " Valuse :");
        //    return new string[] { "value 1.85", "value2--" };

        //    //return new HttpResponseMessage() { Version = new Version("1.1.1.3") } ;
        //}

        public HttpResponseMessage Get(string id)
        {

            string html = @"<p id=""demo""></p><script> location.href='https://play.google.com/store/apps/details?id=com.arion.deliveries';</script>";

            var response = this.Request.CreateResponse(HttpStatusCode.OK);
            //response.Content = new StringContent(@"<head><meta http-equiv=""Refresh"" content=""0; URL = ""https://www.google.com/"" ></head> ", System.Text.Encoding.UTF8, "text/HTML");
            response.Content = new StringContent(html, System.Text.Encoding.UTF8, "text/HTML");
            return response;
        }

        //// GET api/values/5
        //public string Get(int id)
        //{
        //    return "value";
        //}

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
