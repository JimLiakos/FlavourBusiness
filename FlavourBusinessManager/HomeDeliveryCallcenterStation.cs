using FlavourBusinessFacade.ServicesContextResources;
using System.Collections.Generic;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{eacd7ffb-4107-4ce3-ae6b-154b56792f54}</MetaDataID>
    public class HomeDeliveryCallcenterStation : IHomeDeliveryCallcenterStation
    {
        /// <MetaDataID>{e3266bfc-45a7-4348-8a96-b44092eb9ae7}</MetaDataID>
        public List<IHomeDeliveryServicePoint> HomeDeliveryServicePoints => throw new System.NotImplementedException();

        /// <MetaDataID>{5c6d7b59-3a2b-40b6-90f0-392978df84ee}</MetaDataID>
        public string Description { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        /// <MetaDataID>{ce07ba48-ffb9-4383-b9cb-e61fc01386b1}</MetaDataID>
        public string ServicesContextIdentity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        /// <MetaDataID>{519ff5fa-ec0c-40b7-9131-6c9d156bf632}</MetaDataID>
        public string CallcenterStationIdentity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        /// <MetaDataID>{ff543df5-ff7a-4610-99df-47046b84bce3}</MetaDataID>
        public void AddHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {
            throw new System.NotImplementedException();
        }

        /// <MetaDataID>{06f8a5e7-45a5-4ab8-97fd-3e75409531fa}</MetaDataID>
        public void RemoveHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {
            throw new System.NotImplementedException();
        }
    }
}