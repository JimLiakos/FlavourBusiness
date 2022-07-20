using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using MenuModel;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;

namespace FlavourBusinessManager.ServicesContextResources
{
    /// <MetaDataID>{6412585a-256a-42d3-8bc5-e28467ff30b2}</MetaDataID>
    [BackwardCompatibilityID("{6412585a-256a-42d3-8bc5-e28467ff30b2}")]
    [Persistent()]
    public class HomeDeliveryServicePoint : ServicePoint, IHomeDeliveryServicePoint
    {

        /// <exclude>Excluded</exclude> 
        decimal _MinimumOrderValue;
        /// <MetaDataID>{ad37bdfb-0a9f-4753-9723-51fdd393e086}</MetaDataID>
        [PersistentMember(nameof(_MinimumOrderValue))]
        [BackwardCompatibilityID("+4")]
        public decimal MinimumOrderValue
        {
            get => _MinimumOrderValue;
            set
            {
                if (_MinimumOrderValue != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MinimumOrderValue = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _IsActive;
        /// <MetaDataID>{a59cb9b3-7a94-4d32-8548-bbf64f058696}</MetaDataID>
        [PersistentMember(nameof(_IsActive))]
        [BackwardCompatibilityID("+5")]
        internal bool IsActive
        {
            get=>_IsActive;
            set
            {

                if (_IsActive != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsActive = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude> 
        Dictionary<DayOfWeek, List<OpeningHours>> _WeeklyDeliverySchedule = new Dictionary<DayOfWeek, List<OpeningHours>>();

        /// <MetaDataID>{b7115410-3a04-441e-a776-36d3bc068202}</MetaDataID>
        public Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule
        {
            get => _WeeklyDeliverySchedule;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _WeeklyDeliverySchedule = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        List<Coordinate> _ServiceAreaMap = new List<Coordinate>();
        /// <MetaDataID>{28d81c75-938c-4dba-8c5a-c2f456e725b5}</MetaDataID>
        public List<Coordinate> ServiceAreaMap
        {
            get => _ServiceAreaMap;
            set
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _ServiceAreaMap = value;
                    stateTransition.Consistent = true;
                }
            }
        }
        /// <exclude>Excluded</exclude>
        decimal _FreeShippingMinimumOrderValue;

        /// <MetaDataID>{962cc95f-7c44-456b-9817-831fd6928525}</MetaDataID>
        public decimal FreeShippingMinimumOrderValue { get => _FreeShippingMinimumOrderValue; set => throw new NotImplementedException(); }

        /// <exclude>Excluded</exclude>
        decimal _MinimumShippingFee;

        /// <MetaDataID>{48980b23-8e25-47c9-906c-0ced1401a34e}</MetaDataID>
        public decimal MinimumShippingFee { get => _MinimumShippingFee; set => throw new NotImplementedException(); }

        /// <MetaDataID>{ae158781-fd71-4b27-8311-76438eaabc23}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        string ServiceAreaMapJson;

        /// <MetaDataID>{23169d00-d175-494e-9d0f-ab960b760834}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        string WeeklyDeliveryScheduleJson;

        /// <MetaDataID>{41e6bdf1-c64e-4d58-ae16-5280acb3324a}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            ServiceAreaMapJson = OOAdvantech.Json.JsonConvert.SerializeObject(_ServiceAreaMap);
            WeeklyDeliveryScheduleJson = OOAdvantech.Json.JsonConvert.SerializeObject(_WeeklyDeliverySchedule);
        }
        /// <MetaDataID>{d602d811-8ffd-4679-a1ac-6511c47a057a}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {
            if (!string.IsNullOrWhiteSpace(ServiceAreaMapJson))
                _ServiceAreaMap = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Coordinate>>(ServiceAreaMapJson);

            if (!string.IsNullOrWhiteSpace(WeeklyDeliveryScheduleJson))
                _WeeklyDeliverySchedule = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<DayOfWeek, List<OpeningHours>>>(WeeklyDeliveryScheduleJson);


        }
    }
}