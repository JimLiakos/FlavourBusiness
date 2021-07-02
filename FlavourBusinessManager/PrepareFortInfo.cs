using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{5a7f0187-b1e8-4b95-9fe8-e79da0833aff}</MetaDataID>
    [BackwardCompatibilityID("{5a7f0187-b1e8-4b95-9fe8-e79da0833aff}")]
    [Persistent()]
    public class PreparationForInfo : System.MarshalByRefObject, FlavourBusinessFacade.ServicesContextResources.IPreparationForInfo
    {

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <MetaDataID>{ceca1c79-04a4-4df9-ab43-e17bc52de5e3}</MetaDataID>
        public PreparationForInfo()
        {

        }


        /// <exclude>Excluded</exclude>
        PreparationForInfoType _PreparationForInfoType;

        /// <MetaDataID>{3e9af4db-dd7c-4c3f-8cae-30833a99aad4}</MetaDataID>
        [PersistentMember(nameof(_PreparationForInfoType))]
        [BackwardCompatibilityID("+3")]
        public PreparationForInfoType PreparationForInfoType
        {
            get => _PreparationForInfoType;
            set
            {
                if (_PreparationForInfoType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PreparationForInfoType = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
        /// <exclude>Excluded</exclude>
        string _Description;

        /// <MetaDataID>{a4a0166a-3480-4ea9-9713-f73f6abb1a18}</MetaDataID>
        [PersistentMember(nameof(_Description))]
        [BackwardCompatibilityID("+2")]
        public string Description
        {
            get => _Description;
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

        /// <exclude>Excluded</exclude>
        string _ServicePointsInfoObjectUri;

        /// <MetaDataID>{c33b309b-8ccf-4023-824d-d8c951ca7e2a}</MetaDataID>
        [PersistentMember(nameof(_ServicePointsInfoObjectUri))]
        [BackwardCompatibilityID("+1")]
        public string ServicePointsInfoObjectUri
        {
            get => _ServicePointsInfoObjectUri;
            set
            {
                if (_ServicePointsInfoObjectUri != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServicePointsInfoObjectUri = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        IServicePoint _ServicePoint;


        /// <exclude>Excluded</exclude>
        IServiceArea _ServiceArea;



        public IServicePoint ServicePoint
        {
            get
            {
                return _ServicePoint;
            }
            set
            {
                _ServicePoint = value;
                ServicePointsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_ServiceArea)?.GetPersistentObjectUri(_ServicePoint);
            }
        }
        public IServiceArea ServiceArea
        {
            get
            {
                if (_ServiceArea == null && _ServicePoint == null)
                    LoadServicePointsInfo();

                return _ServiceArea;
            }
            set
            {
                _ServiceArea = value;
                ServicePointsInfoObjectUri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(_ServiceArea)?.GetPersistentObjectUri(_ServiceArea);

            }
        }

        private void LoadServicePointsInfo()
        {
            var servicPointsObjects= OOAdvantech.PersistenceLayer.ObjectStorage.GetObjectFromUri(ServicePointsInfoObjectUri);
            if (servicPointsObjects is IServicePoint)
                _ServicePoint = servicPointsObjects as IServicePoint;
            if (servicPointsObjects is IServiceArea)
                _ServiceArea = servicPointsObjects as IServiceArea;

        }
    }
}