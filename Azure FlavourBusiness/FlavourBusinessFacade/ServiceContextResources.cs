using System.Collections.Generic;
using FlavourBusinessFacade.HumanResources;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{73dc3884-ac5c-45ed-af17-cb448bcc8b01}</MetaDataID>
    public class ServiceContextResources
    {
        /// <MetaDataID>{c8d48fa2-c36c-4c67-97dc-59487183d5b4}</MetaDataID>
        public IList<ICashierStation> CashierStations { get; set; }


        public IList<IPreparationStation> PreparationStations { get; set; }


        /// <MetaDataID>{f209db6e-02f8-4929-8524-b9cafe311f10}</MetaDataID>
        public ICallerIDServer CallerIDServer { get; set; }

        public IList<IServiceArea> ServiceAreas { get; set; }

    }
}