using System.Collections.Generic;
using FlavourBusinessFacade.HumanResources;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{73dc3884-ac5c-45ed-af17-cb448bcc8b01}</MetaDataID>
    public class ServiceContextResources
    {
        /// <MetaDataID>{c8d48fa2-c36c-4c67-97dc-59487183d5b4}</MetaDataID>
        public IList<ICashierStation> CashierStations { get; set; }


        /// <MetaDataID>{22e9a2eb-3240-4163-b7d8-c2f1cbd5fdd8}</MetaDataID>
        public IList<IPreparationStation> PreparationStations { get; set; }


        /// <MetaDataID>{89beb1b4-cee5-42f6-ad62-7ad83bc0c720}</MetaDataID>
        public IList<ITakeAwayStation> TakeAwayStations { get; set; }
        /// <MetaDataID>{f209db6e-02f8-4929-8524-b9cafe311f10}</MetaDataID>
        public ICallerIDServer CallerIDServer { get; set; }

        /// <MetaDataID>{a195405b-5fdb-4244-a9f8-adb59ed76ea7}</MetaDataID>
        public IList<IServiceArea> ServiceAreas { get; set; }
        /// <MetaDataID>{4691fb51-6ffd-45a2-acc8-d51e847a6130}</MetaDataID>
        public IHomeDeliveryServicePoint DeliveryServicePoint { get; set; }
        /// <MetaDataID>{e49c336c-fa52-4519-b2c1-31591e4daca6}</MetaDataID>
        public List<IPaymentTerminal> PaymentTerminals { get; set; }
        /// <MetaDataID>{669df26e-21af-4835-8cf7-11b2dee2f70e}</MetaDataID>
        public List<IHomeDeliveryCallCenterStation> DeliveryCallCenterStations { get; set; }
    }
}