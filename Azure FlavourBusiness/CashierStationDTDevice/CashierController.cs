using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice
{
    /// <MetaDataID>{82be5194-83d3-4038-b878-d6225ab658cb}</MetaDataID>
    public class CashierController
    {
        /// <MetaDataID>{af4a528e-ef65-4e03-b887-e0ba7e094e13}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        ICashiersStationRuntime CashiersStation;
        /// <MetaDataID>{d76d5629-dbbc-4583-8610-6cd79f7f3c2c}</MetaDataID>
        public void Start()
        {
            string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
            string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
            string serverUrl = AzureServerUrl;
            if (string.IsNullOrWhiteSpace(ApplicationSettings.Current.CommunicationCredentialKey))
                return;
            IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
            CashiersStation = servicesContextManagment.GetCashiersStationRuntime(ApplicationSettings.Current.CommunicationCredentialKey);
            try
            {




                CashiersStation.OpenTransactions += CashiersStation_OpenTransactions;
                var transctios = CashiersStation.GetOpenTransactions(null);
                var sdsd = transctios.FirstOrDefault()?.Items;


            }
            catch (Exception error)
            {

                if (error.HResult == 800)
                {
                    if (!System.Diagnostics.EventLog.SourceExists("CashierStationDevice", "."))
                        System.Diagnostics.EventLog.CreateEventSource("CashierStationDevice", "Microneme");
                    System.Diagnostics.EventLog myLog = new System.Diagnostics.EventLog();
                    myLog.Source = "CashierStationDevice";
                    myLog.WriteEntry("There is already attached device to the cashier station", System.Diagnostics.EventLogEntryType.Error);

                    throw new CashierStationDeviceException("Only one cashier station device allowed to subscribe");
                }
            }
        }

        private void CashiersStation_OpenTransactions(ICashiersStationRuntime sender, string deviceUpdateEtag)
        {
            var transctios = CashiersStation.GetOpenTransactions(deviceUpdateEtag);
        }
    }
}
