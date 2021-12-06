using System;
using FinanceFacade;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{f0a5e89f-1b4c-451f-b440-ee742c9044e6}</MetaDataID>
    [BackwardCompatibilityID("{f0a5e89f-1b4c-451f-b440-ee742c9044e6}")]
    [Persistent()]
    public class CashierStation : MarshalByRefObject, OOAdvantech.Remoting.IExtMarshalByRefObject, ICashierStation, ICashiersStationRuntime
    {
        /// <exclude>Excluded</exclude>
        string _CashierStationIdentity;
        /// <MetaDataID>{57fea597-c836-44a8-a747-e6b9885eacc1}</MetaDataID>
        [PersistentMember(nameof(_CashierStationIdentity))]
        [BackwardCompatibilityID("+5")]
        public string CashierStationIdentity
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_CashierStationIdentity))
                {

                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
                        _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
                        while (_DeviceCredentialKeyAbbreviation == null)
                        {
                            _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
                            _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
                        }

                        stateTransition.Consistent = true;
                    }
                }

                return _CashierStationIdentity;
            }
            private set
            {

                if (_CashierStationIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _CashierStationIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _DeviceCredentialKeyAbbreviation;

        /// <MetaDataID>{0ee76ad6-4cbb-4e40-88ca-b3e12020d582}</MetaDataID>
        [PersistentMember(nameof(_DeviceCredentialKeyAbbreviation))]
        [BackwardCompatibilityID("+4")]
        public string DeviceCredentialKeyAbbreviation
        {
            get => _DeviceCredentialKeyAbbreviation;
            private set
            {

                if (_DeviceCredentialKeyAbbreviation != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DeviceCredentialKeyAbbreviation = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        IFisicalParty _Issuer;
        /// <MetaDataID>{128db8e9-4c7a-4d52-a542-cfd6edea863d}</MetaDataID>
        [PersistentMember(nameof(_Issuer))]
        [BackwardCompatibilityID("+3")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        public IFisicalParty Issuer
        {
            get => _Issuer;
            set
            {
                if (_Issuer != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        if (value != null)
                            _Issuer = ObjectStorage.GetObjectFromUri<FisicalParty>((value as FisicalParty).FisicalPartyUri);
                        else
                            _Issuer = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        string _Description;
        /// <MetaDataID>{59c73ec1-a4d4-4504-8225-d975e74d15ce}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+1")]
        public string Description
        {
            get
            {
                return _Description;
            }

            set
            {
                if (_Description != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Description = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        protected CashierStation()
        {

        }
        public CashierStation(string servicesContextIdentity)
        {
            _ServicesContextIdentity = servicesContextIdentity;
            _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
            _DeviceCredentialKeyAbbreviation= DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
            while(_DeviceCredentialKeyAbbreviation==null)
            {
                _CashierStationIdentity = _ServicesContextIdentity + "_" + Guid.NewGuid().ToString("N");
                _DeviceCredentialKeyAbbreviation = DeviceIDAbbreviation.GetAbbreviation(_CashierStationIdentity);
            }
        }

        /// <exclude>Excluded</exclude>
        string _ServicesContextIdentity;

     

        /// <MetaDataID>{b214da78-22cc-49ee-9200-8e27feaf7bf7}</MetaDataID>
        [PersistentMember(nameof(_ServicesContextIdentity))]
        [BackwardCompatibilityID("+2")]
        public string ServicesContextIdentity
        {
            get
            {
                return _ServicesContextIdentity;
            }

            set
            {

                if (_ServicesContextIdentity != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicesContextIdentity = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


    }
}