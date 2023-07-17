using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicePointRunTime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{eacd7ffb-4107-4ce3-ae6b-154b56792f54}</MetaDataID>
    [BackwardCompatibilityID("{eacd7ffb-4107-4ce3-ae6b-154b56792f54}")]
    [Persistent()]
    public class HomeDeliveryCallCenterStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, IHomeDeliveryCallCenterStation
    {

        public HomeDeliveryCallCenterStation(ServicesContextRunTime servicesContextRunTime)

        {
            _CallcenterStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }


        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<IHomeDeliveryServicePoint> _HomeDeliveryServicePoints = new OOAdvantech.Collections.Generic.Set<IHomeDeliveryServicePoint>();

        /// <MetaDataID>{e3266bfc-45a7-4348-8a96-b44092eb9ae7}</MetaDataID>
        [PersistentMember(nameof(_HomeDeliveryServicePoints))]
        [BackwardCompatibilityID("+1")]
        public List<IHomeDeliveryServicePoint> HomeDeliveryServicePoints => _HomeDeliveryServicePoints.ToThreadSafeList();

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{5c6d7b59-3a2b-40b6-90f0-392978df84ee}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get => _Description; set
            {
                if (_Description!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;


        /// <MetaDataID>{ce07ba48-ffb9-4383-b9cb-e61fc01386b1}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+3")]
        public string ServicesContextIdentity
        {
            get => _ServicesContextIdentity; set
            {
                if (_ServicesContextIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }


        /// <exclude>Excluded</exclude>
        string _CallcenterStationIdentity;
        /// <MetaDataID>{519ff5fa-ec0c-40b7-9131-6c9d156bf632}</MetaDataID>
        [PersistentMember(nameof(_CallcenterStationIdentity))]
        [BackwardCompatibilityID("+4")]
        public string CallcenterStationIdentity
        {
            get => _CallcenterStationIdentity; set
            {
                if (_CallcenterStationIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CallcenterStationIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        string _GraphicMenuStorageIdentity;

        /// <MetaDataID>{4e8369d3-e6a4-45ac-8a03-e79a3c051eff}</MetaDataID>
        [PersistentMember(nameof(_GraphicMenuStorageIdentity))]
        [BackwardCompatibilityID("+6")]
        public string GraphicMenuStorageIdentity
        {
            get => _GraphicMenuStorageIdentity; set
            {
                if (_GraphicMenuStorageIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _GraphicMenuStorageIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{ff543df5-ff7a-4610-99df-47046b84bce3}</MetaDataID>
        public void AddHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HomeDeliveryServicePoints.Add(homeDeliveryServicePoint);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{06f8a5e7-45a5-4ab8-97fd-3e75409531fa}</MetaDataID>
        public void RemoveHomeDeliveryServicePoint(IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _HomeDeliveryServicePoints.Remove(homeDeliveryServicePoint);
                stateTransition.Consistent = true;
            }

        }
    }
}