using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlavourBusinessFacade
{
    /// <MetaDataID>{35815fe5-6439-414c-8d2b-417faeec12b1}</MetaDataID>
    public static class GlobalResourcesIdentities
    {
        public static string GetShortIdentity(string resourceFullIdentity)
        {
            string uniqueIdsUrl = string.Format("http://{0}:8090/api/UniqueId/{1}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, resourceFullIdentity);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                var json = wc.DownloadString(uniqueIdsUrl);
                var shortIdentity = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(json);

                return shortIdentity;
            }
        }

        public static string GetResourceFullIdentity(string shortIdentity)
        {
            string uniqueIdsUrl = string.Format("http://{0}:8090/api/UniqueId/FullIDFor/{1}", FlavourBusinessFacade.ComputingResources.EndPoint.Server, shortIdentity);
            using (System.Net.WebClient wc = new System.Net.WebClient())
            {
                var json = wc.DownloadString(uniqueIdsUrl);
                var resourceFullIdentity = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(json);

                return resourceFullIdentity;
            }
        }
    }
}
