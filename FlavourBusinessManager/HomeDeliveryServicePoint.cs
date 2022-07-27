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
        /// <MetaDataID>{f5213c16-1b33-465c-9c83-9ffb82f8259f}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+10")]
        private string MapCenterJson;

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
            get => _IsActive;
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


        /// <MetaDataID>{1c2670e3-8d2d-4b7c-aaed-1ec452eab8ee}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+7")]
        string PlaceOfDistributionJson = null;

        /// <exclude>Excluded</exclude>
        EndUsers.Place _PlaceOfDistribution;
        /// <MetaDataID>{67337dcd-a5e2-4ca9-8c70-077d6e594510}</MetaDataID>
        [BackwardCompatibilityID("+6")]
        public IPlace PlaceOfDistribution
        {
            get => _PlaceOfDistribution;
            set
            {
                if (_PlaceOfDistribution != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _PlaceOfDistribution = value as EndUsers.Place;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        Coordinate _MapCenter;
        /// <MetaDataID>{14a3ba03-5e0d-43c1-bbd1-eb01a8c246ea}</MetaDataID>
        public Coordinate MapCenter { get => _MapCenter; set => _MapCenter= value; }


        /// <exclude>Excluded</exclude>
        double _Zoom;

        /// <MetaDataID>{42346420-ec90-4b15-9070-2eac25381892}</MetaDataID>
        [PersistentMember(nameof(_Zoom))]
        [BackwardCompatibilityID("+8")]
        public double Zoom
        {
            get => _Zoom; 
            set
            {
                if (_Zoom != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Zoom = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <exclude>Excluded</exclude>
        bool _IsPolyline;
        /// <MetaDataID>{003f8343-7a42-40a2-a86e-b9940dcdddeb}</MetaDataID>
        [PersistentMember(nameof(_IsPolyline))]
        [BackwardCompatibilityID("+9")]
        public bool IsPolyline
        {
            get => _IsPolyline; 
            set
            {
                if (_IsPolyline != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsPolyline = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{1348a699-3c57-4358-a22a-845dcb992d0a}</MetaDataID>
        public void Update(IPlace placeOfDistribution, Coordinate mapCenter, List<Coordinate> serviceAreaMap, bool isPolyline, double zoom)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                PlaceOfDistribution = placeOfDistribution;
                MapCenter = mapCenter;
                ServiceAreaMap = serviceAreaMap;
                IsPolyline = IsPolyline;
                Zoom = zoom;

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{ae158781-fd71-4b27-8311-76438eaabc23}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+1")]
        string ServiceAreaMapJson = @"[{""Latitude"":38.000240262320936, ""Longitude"":23.750335054426756},{""Latitude"":37.99832108602798,""Longitude"":23.751161174803343},{ ""Latitude"":37.99534499527491,""Longitude"":23.749326543837157},{ ""Latitude"":37.996503317677686,""Longitude"":23.74603279116687},{ ""Latitude"":37.99605520972059,""Longitude"":23.743919210463133},{ ""Latitude"":37.99616512324652,""Longitude"":23.742170410185423},{ ""Latitude"":37.99491379029195,""Longitude"":23.736054973631468},{ ""Latitude"":37.995556369340605,""Longitude"":23.732074575453368},{ ""Latitude"":38.00013880932487,""Longitude"":23.7332332897478},{ ""Latitude"":38.00572696945966,""Longitude"":23.734713869124022},{ ""Latitude"":38.00665265229764,""Longitude"":23.734955267935362},{ ""Latitude"":38.00648357867907,""Longitude"":23.740719335108366},{ ""Latitude"":38.005760784563485,""Longitude"":23.741502540140715},{ ""Latitude"":38.00575655767636,""Longitude"":23.742610292463866},{ ""Latitude"":38.006542372746324,""Longitude"":23.743995032319887},{ ""Latitude"":38.00739196305551,""Longitude"":23.744778237352236},{ ""Latitude"":38.00814010159382,""Longitude"":23.744831881532534},{ ""Latitude"":38.00805164316818,""Longitude"":23.746637986944283},{ ""Latitude"":38.007451441510376,""Longitude"":23.747614311025703},{ ""Latitude"":38.00665680077053,""Longitude"":23.74774305705842},{ ""Latitude"":38.007299276929025,""Longitude"":23.7491807210904},{ ""Latitude"":38.00653844928516,""Longitude"":23.74970643405732},{ ""Latitude"":38.00627449090539,""Longitude"":23.74997551698178},{ ""Latitude"":38.006502740849264,""Longitude"":23.75049050111264},{ ""Latitude"":38.005302307259825,""Longitude"":23.751327350325287},{ ""Latitude"":38.00536148402404,""Longitude"":23.751627757734955},{ ""Latitude"":38.00283374825802,""Longitude"":23.751928165144623},{ ""Latitude"":38.00192619043664,""Longitude"":23.75025822604646},{ ""Latitude"":38.00165565502161,""Longitude"":23.750526446947948},{ ""Latitude"":38.00077640802997,""Longitude"":23.74972178424348},{ ""Latitude"":38.000240262320936,""Longitude"":23.750335054426756},{ ""Latitude"":38.000240262320936,""Longitude"":23.750335054426756}]";

    
        /// <MetaDataID>{23169d00-d175-494e-9d0f-ab960b760834}</MetaDataID>
        [PersistentMember()]
        [BackwardCompatibilityID("+2")]
        string WeeklyDeliveryScheduleJson;

        /// <MetaDataID>{41e6bdf1-c64e-4d58-ae16-5280acb3324a}</MetaDataID>
        [BeforeCommitObjectStateInStorageCall]
        void BeforeCommitObjectState()
        {
            ServiceAreaMapJson = OOAdvantech.Json.JsonConvert.SerializeObject(_ServiceAreaMap);
            MapCenterJson = OOAdvantech.Json.JsonConvert.SerializeObject(_MapCenter);
            WeeklyDeliveryScheduleJson = OOAdvantech.Json.JsonConvert.SerializeObject(_WeeklyDeliverySchedule);
            PlaceOfDistributionJson = OOAdvantech.Json.JsonConvert.SerializeObject(_PlaceOfDistribution);
            
        }
        /// <MetaDataID>{d602d811-8ffd-4679-a1ac-6511c47a057a}</MetaDataID>
        [ObjectActivationCall]
        void ObjectActivation()
        {
            if (!string.IsNullOrWhiteSpace(ServiceAreaMapJson))
                _ServiceAreaMap = OOAdvantech.Json.JsonConvert.DeserializeObject<List<Coordinate>>(ServiceAreaMapJson);

            if (!string.IsNullOrWhiteSpace(MapCenterJson))
                _MapCenter = OOAdvantech.Json.JsonConvert.DeserializeObject<Coordinate>(MapCenterJson);

            if (!string.IsNullOrWhiteSpace(WeeklyDeliveryScheduleJson))
                _WeeklyDeliverySchedule = OOAdvantech.Json.JsonConvert.DeserializeObject<Dictionary<DayOfWeek, List<OpeningHours>>>(WeeklyDeliveryScheduleJson);

            if (!string.IsNullOrWhiteSpace(PlaceOfDistributionJson))
                _PlaceOfDistribution = OOAdvantech.Json.JsonConvert.DeserializeObject<EndUsers.Place>(PlaceOfDistributionJson);


        }
    }
}