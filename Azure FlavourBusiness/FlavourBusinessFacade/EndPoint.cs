using System.Net;

namespace FlavourBusinessFacade.ComputingResources
{
    /// <MetaDataID>{38d0468f-ec27-4515-8087-dfe1bf7d3665}</MetaDataID>
    public class EndPoint
    {
        /// <MetaDataID>{5d6ecf35-63bb-4d1c-b499-8df087fac9d7}</MetaDataID>
        public string Name { get; set; }

        /// <MetaDataID>{3b1534bc-44eb-4285-b5e1-f2be054caa72}</MetaDataID>
        public string Address { get; set; }

        /// <MetaDataID>{537bd9d9-af69-4bf4-ab16-6c4b13311925}</MetaDataID>
        public string Protocol { get; set; }

        /// <MetaDataID>{02b1cca9-86cf-4766-8ee2-e855751e009b}</MetaDataID>
        public int Port { get; set; }


        //public static string Server = "192.168.2.20";//Braxati

        //public static string Server = "192.168.2.8";//org

        static string _Server;

        public static string Server
        {
            get
            {
                try
                {
                    if (_Server == null)
                    {
                        _Server = "192.168.2.8";//work
                        if (_Server == null)
                        {
                            using (WebClient wc = new WebClient())
                            {
                                var json = wc.DownloadString("https://angularhost.z16.web.core.windows.net/usersfolder/ServerAddress.json");
                                _Server = OOAdvantech.Json.JsonConvert.DeserializeObject<string>(json);
                                return _Server;
                            }
                        }
                        else
                            return _Server;
                    }
                    else
                        return _Server;

                }
                catch (System.Exception error)
                {


                }

                return "10.0.0.13";//work
            }
        }
        //public static string Server = "meridian-services.northeurope.cloudapp.azure.com";

        //Sudo /Library/Application\\ Support/VMware\\ Tools/vmware-resolutionSet 1920 1080
    }
}
