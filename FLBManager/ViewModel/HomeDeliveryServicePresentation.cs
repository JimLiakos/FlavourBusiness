using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.Json;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel
{
    [Transactional]//To force transaction mechanism to ask to save on close the changes
    /// <MetaDataID>{a2719e24-d3e7-4df3-acaa-e80226746b20}</MetaDataID>
    public class HomeDeliveryServicePresentation : MarshalByRefObject, INotifyPropertyChanged, IGeocodingPlaces, IExtMarshalByRefObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <exclude>Excluded</exclude>
        List<Coordinate> _ServiceAreaMap;
        [HttpVisible]
        public List<Coordinate> ServiceAreaMap
        {
            get => _ServiceAreaMap;
            set
            {
                bool hasChanges = true;
                if (_ServiceAreaMap != null && value != null && value.Count == _ServiceAreaMap.Count)
                {
                    if (value.All(x => _ServiceAreaMap.Any(y => y == x)))
                        hasChanges = false;


                }
                if (hasChanges)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ServiceAreaMap = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        Coordinate? _MapCenter;
        [HttpVisible]
        public Coordinate? MapCenter
        {
            get => _MapCenter;
            set
            {
                if (_MapCenter != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MapCenter = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        [HttpVisible]
        public void SetOpeningHoursForDate(DayOfWeek dayOfWeek, List<OpeningHours> openingHours)
        {
            var existingOpeningHours = GetOpeningHoursForDate(dayOfWeek);
            if (existingOpeningHours.Count != openingHours.Count || !openingHours.All(x => existingOpeningHours.Any(y => x.StartsAt == y.StartsAt && x.EndsAt == y.EndsAt)))
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    WeeklyDeliverySchedule[dayOfWeek] = openingHours;
                    stateTransition.Consistent = true;
                }

            }
        }



        //Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule=new Dictionary<DayOfWeek, List<OpeningHours>>();

        [HttpVisible]
        public List<OpeningHours> GetOpeningHoursForDate(DayOfWeek dayOfWeek)
        {
            List<OpeningHours> openingHours = null;
            if (WeeklyDeliverySchedule.TryGetValue(dayOfWeek, out openingHours))
                return openingHours;
            else
                return new List<OpeningHours>();
        }
        [HttpVisible]
        public Dictionary<DayOfWeek, List<OpeningHours>> GetWeeklyDeliverySchedule()
        {
            return WeeklyDeliverySchedule;
        }

        double _Zoom;
        [HttpVisible]
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

        bool _IsPolyline;
        [HttpVisible]
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

        /// <exclude>Excluded</exclude>
        decimal _MinimumOrderValue;
        [HttpVisible]
        [CachingDataOnClientSide]
        public decimal MinimumOrderValue
        {
            get
            {
                return _MinimumOrderValue;
            }
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
        decimal _ShippingCost;
        [HttpVisible]
        [CachingDataOnClientSide]
        public decimal ShippingCost
        {
            get
            {
                return _ShippingCost;
            }
            set
            {

                if (_ShippingCost != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _ShippingCost = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }

        /// <exclude>Excluded</exclude>
        decimal _FreeShippingMinimumOrderValue;
        [HttpVisible]
        [CachingDataOnClientSide]
        public decimal FreeShippingMinimumOrderValue
        {
            get
            {
                return _FreeShippingMinimumOrderValue;
            }
            set
            {

                if (_FreeShippingMinimumOrderValue != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _FreeShippingMinimumOrderValue = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        public HomeDeliveryServicePresentation(FlavourBusinessFacade.ServicesContextResources.IHomeDeliveryServicePoint homeDeliveryServicePoint, IFlavoursServicesContext servicesContext)
        {
            ServicesContext = servicesContext;

            HomeDeliveryServicePoint = homeDeliveryServicePoint;
            var placeOfDistribution = homeDeliveryServicePoint.PlaceOfDistribution;
            if (placeOfDistribution != null)
                _Places.Add(placeOfDistribution);
            _MapCenter = HomeDeliveryServicePoint.MapCenter;
            //if(MapCenter.Latitude==0&& MapCenter.Longitude==0&& placeOfDistribution)
            //    MapCenter

            _ServiceAreaMap = HomeDeliveryServicePoint.ServiceAreaMap;
            _IsPolyline = HomeDeliveryServicePoint.IsPolyline;
            _Zoom = HomeDeliveryServicePoint.Zoom;

            _MinimumOrderValue = HomeDeliveryServicePoint.MinimumOrderValue;
            _ShippingCost = HomeDeliveryServicePoint.ShippingCost;
            _FreeShippingMinimumOrderValue = HomeDeliveryServicePoint.FreeShippingMinimumOrderValue;

            WeeklyDeliverySchedule = HomeDeliveryServicePoint.WeeklyDeliverySchedule;



            BeforeTransactionCommitCommand = new RelayCommand((object sender) =>
            {
                if (_Places.Count == 0)
                    HomeDeliveryServicePoint.PlaceOfDistribution = null;
                else
                    HomeDeliveryServicePoint.PlaceOfDistribution = _Places[0];
                HomeDeliveryServicePoint.Update(HomeDeliveryServicePoint.PlaceOfDistribution, MapCenter, ServiceAreaMap, IsPolyline, Zoom, WeeklyDeliverySchedule,MinimumOrderValue,ShippingCost,FreeShippingMinimumOrderValue);
                var foodTypeTags = _FoodTypeTags.Where(x => x.Selected && !SelectedFoodTypes.Any(y => y.Uri == x.Uri)).Select(x => x.FoodTypeTag).ToList();
                ServicesContext.AddFoodTypes(foodTypeTags);

                foodTypeTags = _FoodTypeTags.Where(x => !x.Selected && SelectedFoodTypes.Any(y => y.Uri == x.Uri)).Select(x => x.FoodTypeTag).ToList();
                ServicesContext.RemoveFoodTypes(foodTypeTags);

            });
        }
        public HomeDeliveryServicePresentation()
        {

        }
        /// <exclude>Excluded</exclude>
        private List<FoodTypeTagPresentation> _FoodTypeTags;

        [HttpVisible]
        public Task<List<FoodTypeTagPresentation>> FoodTypeTags
        {
            get
            {
                return Task<List<FoodTypeTagPresentation>>.Run(() =>
                {
                    if (_FoodTypeTags == null)
                    {
                        SelectedFoodTypes = ServicesContext.FoodTypes;
                        string assemblyData = "FlavourBusinessManager, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";
                        string type = "FlavourBusinessManager.FlavoursServicesContextManagment";
                        string serverUrl = AzureServerUrl;
                        IFlavoursServicesContextManagment servicesContextManagment = OOAdvantech.Remoting.RestApi.RemotingServices.CastTransparentProxy<IFlavoursServicesContextManagment>(OOAdvantech.Remoting.RestApi.RemotingServices.CreateRemoteInstance(serverUrl, type, assemblyData));
                        _FoodTypeTags = servicesContextManagment.FoodTypeTags.Select(x => new FoodTypeTagPresentation(x, SelectedFoodTypes.Where(y => y.Uri == x.Uri).Count() > 0)).ToList();
                    }
                    return _FoodTypeTags;
                });
            }
        }
        [HttpVisible]
        public void AddFoodTypeTag(string foodTypeTagUri)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodTypeTags.Where(x => x.Uri == foodTypeTagUri).FirstOrDefault().Selected = true;

                stateTransition.Consistent = true;
            }

        }
        [HttpVisible]
        public void RemoveFoodTypeTag(string foodTypeTagUri)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodTypeTags.Where(x => x.Uri == foodTypeTagUri).FirstOrDefault().Selected = false;
                stateTransition.Consistent = true;
            }

        }


        public WPFUIElementObjectBind.RelayCommand BeforeTransactionCommitCommand { get; set; }

        OOAdvantech.Collections.Generic.Set<IPlace> _Places = new OOAdvantech.Collections.Generic.Set<IPlace>();

        [ImplementationMember(nameof(_Places))]
        public List<IPlace> Places => _Places.ToList();

        public IHomeDeliveryServicePoint HomeDeliveryServicePoint { get; }
        public List<IFoodTypeTag> SelectedFoodTypes { get; private set; }

        public void SavePlace(IPlace place)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Places.Clear();
                _Places.Add(place);
                place.Default = true;

                stateTransition.Consistent = true;
            }


        }

        public void RemovePlace(IPlace place)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Places.Clear();
                stateTransition.Consistent = true;
            }

        }


        Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule = new Dictionary<DayOfWeek, List<OpeningHours>>();
        private IFlavoursServicesContext ServicesContext;

        public void SetDefaultPlace(IPlace place)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                place.Default = true;
                HomeDeliveryServicePoint.PlaceOfDistribution = place;

                stateTransition.Consistent = true;
            }
        }

        Task<Coordinate?> IGeocodingPlaces.GetCurrentLocation()
        {
            return null;
        }
    }

    public class FoodTypeTagPresentation
    {
        public FoodTypeTagPresentation(IFoodTypeTag foodTypeTag, bool selected)
        {
            FoodTypeTag = foodTypeTag;
            Selected = selected;
        }
        public OOAdvantech.Multilingual MultilingualName
        {
            get
            {
                return FoodTypeTag?.MultilingualName;
            }
            set
            {
            }
        }

        public string Uri
        {
            get
            {
                return FoodTypeTag?.Uri;
            }
            set
            {
            }
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return FoodTypeTag?.Name;
            }
            set
            {
            }
        }
        public readonly IFoodTypeTag FoodTypeTag;

        public bool Selected { get; set; }
    }


}
