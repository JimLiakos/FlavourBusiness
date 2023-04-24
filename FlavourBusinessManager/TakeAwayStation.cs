using FlavourBusinessManager.ServicePointRunTime;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{99f0adb9-6986-412c-9f26-aa956ec96f18}</MetaDataID>
    [BackwardCompatibilityID("{99f0adb9-6986-412c-9f26-aa956ec96f18}")]
    [Persistent()]
    public class TakeAwayStation : ServicePoint, OOAdvantech.Remoting.IExtMarshalByRefObject, FlavourBusinessFacade.ServicesContextResources.ITakeAwayStation
    {
        /// <exclude>Excluded</exclude>
        string _GraphicMenuStorageIdentity;
        /// <MetaDataID>{0301f3d5-d132-499d-aef0-c04561c4dc80}</MetaDataID>
        [PersistentMember(nameof(_GraphicMenuStorageIdentity))]
        [BackwardCompatibilityID("+4")]
        public string GraphicMenuStorageIdentity
        {
            get => _GraphicMenuStorageIdentity;
            set
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





        ///// <exclude>Excluded</exclude>
        //OOAdvantech.ObjectStateManagerLink StateManagerLink;
        ///// <exclude>Excluded</exclude>
        //string _Description;

        ///// <MetaDataID>{ddd8d4f1-14bf-4a64-935d-0d097b9cf192}</MetaDataID>
        //[PersistentMember(nameof(_Description))]
        //[BackwardCompatibilityID("+3")]
        //public string Description
        //{
        //    get => _Description;
        //    set
        //    {
        //        if (_Description!=value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _Description=value;
        //                stateTransition.Consistent = true;
        //            }
        //        }

        //    }
        //}


        ///// <exclude>Excluded</exclude>
        //string _ServicesContextIdentity;


        ///// <MetaDataID>{f5ed2bd1-4318-494e-b105-be918048d738}</MetaDataID>
        //[PersistentMember(nameof(_ServicesContextIdentity))]
        //[BackwardCompatibilityID("+2")]
        //public string ServicesContextIdentity
        //{
        //    get => _ServicesContextIdentity;
        //    set
        //    {

        //        if (_ServicesContextIdentity!=value)
        //        {
        //            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
        //            {
        //                _ServicesContextIdentity=value;
        //                stateTransition.Consistent = true;
        //            }
        //        }

        //    }
        //}

        /// <exclude>Excluded</exclude>
        string _TakeAwayStationIdentity;

        /// <MetaDataID>{bb90f771-ef43-4bfa-94a5-6bb976474110}</MetaDataID>
        [PersistentMember(nameof(_TakeAwayStationIdentity))]
        [BackwardCompatibilityID("+1")]
        public string TakeAwayStationIdentity
        {
            get => _TakeAwayStationIdentity;
            set
            {

                if (_TakeAwayStationIdentity!=value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _TakeAwayStationIdentity=value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <MetaDataID>{57170463-54e1-47ae-bec8-bf45b89aee41}</MetaDataID>
        public TakeAwayStation()
        {

        }

        /// <MetaDataID>{e010fb08-4247-46e9-a957-d9788f3bff6a}</MetaDataID>
        public TakeAwayStation(ServicesContextRunTime servicesContextRunTime)
        {
            _TakeAwayStationIdentity = servicesContextRunTime.ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            ServicesContextIdentity = servicesContextRunTime.ServicesContextIdentity;
        }
    }
}