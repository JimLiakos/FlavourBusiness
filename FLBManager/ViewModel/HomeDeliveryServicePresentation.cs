using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
using Microsoft.Win32;
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
    /// <MetaDataID>{3bb76272-bf34-40af-82d9-591baa4a3ec3}</MetaDataID>
    [Transactional]//To force transaction mechanism to ask to save on close the changes
    /// <MetaDataID>{a2719e24-d3e7-4df3-acaa-e80226746b20}</MetaDataID>
    public class HomeDeliveryServicePresentation : MarshalByRefObject, INotifyPropertyChanged, IGeocodingPlaces, IExtMarshalByRefObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <exclude>Excluded</exclude>
        List<Coordinate> _ServiceAreaMap;
        /// <MetaDataID>{aa267801-8b19-4ff8-9600-3669bc90d741}</MetaDataID>
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
        /// <exclude>Excluded</exclude>
        string _LogoBackgroundImageUrl;
        [HttpVisible]
        public string LogoBackgroundImageUrl
        {
            get
            {

                return _LogoBackgroundImageUrl;
            }
        }
        /// <exclude>Excluded</exclude>
        string _LogoImageUrl;
        [HttpVisible]
        public string LogoImageUrl
        {
            get
            {

                return _LogoImageUrl;
            }
        }
        /// <MetaDataID>{dd4bdd27-a407-4e8d-97d6-245a4872274e}</MetaDataID>
        Coordinate? _MapCenter;
        /// <MetaDataID>{c449766c-97e2-49da-8173-95c38e41568d}</MetaDataID>
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

        /// <MetaDataID>{8c95dd8b-1410-47aa-8812-5fdbb4b0b167}</MetaDataID>
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

        /// <MetaDataID>{7421346e-70bf-4db6-91a4-eb9897b7acd8}</MetaDataID>
        [HttpVisible]
        public List<OpeningHours> GetOpeningHoursForDate(DayOfWeek dayOfWeek)
        {
            List<OpeningHours> openingHours = null;
            if (WeeklyDeliverySchedule.TryGetValue(dayOfWeek, out openingHours))
                return openingHours;
            else
                return new List<OpeningHours>();
        }
        /// <MetaDataID>{3e1eb674-8138-48d7-bcad-9e979a4dcef6}</MetaDataID>
        [HttpVisible]
        public Dictionary<DayOfWeek, List<OpeningHours>> GetWeeklyDeliverySchedule()
        {
            return WeeklyDeliverySchedule;
        }

        /// <MetaDataID>{bf9ca391-0021-4d67-b182-8ec945604968}</MetaDataID>
        double _Zoom;
        /// <MetaDataID>{82500503-30db-4404-bf45-e250d18a311b}</MetaDataID>
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

        /// <MetaDataID>{c8ec3392-369f-4d24-8cb3-fa5bc360f441}</MetaDataID>
        bool _IsPolyline;
        /// <MetaDataID>{313376a8-1de4-4363-b666-48a64859231f}</MetaDataID>
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
        /// <MetaDataID>{a04d5a4c-f9e3-440f-b0c1-96a80b34f68f}</MetaDataID>
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
        /// <MetaDataID>{106619ff-22cb-46b1-8a7b-ecf095308e29}</MetaDataID>
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
        [HttpVisible]
        public void UploadLogoImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image png files (*.png)|*.png";
            openFileDialog.Title = "Upload Logo";
            if (openFileDialog.ShowDialog() == true)
            {
                string BackgroundLogoImagePath = openFileDialog.FileName;

                var uploadSlot = this.HomeDeliveryServicePoint.GetUploadSlotForLogoImage();
                var imageStream = System.IO.File.OpenRead(BackgroundLogoImagePath);
                FlavourBusinessToolKit.RestApiBlobFileManager.Upload(imageStream, uploadSlot, "image/png");
                _LogoImageUrl = this.HomeDeliveryServicePoint.LogoImageUrl;
#if DEBUG
                if (!string.IsNullOrWhiteSpace(_LogoImageUrl))
                    _LogoImageUrl = "https://dev-localhost/" + _LogoImageUrl.Substring(_LogoImageUrl.IndexOf("devstoreaccount1"));
#endif
            }
        }

        [HttpVisible]
        public void UploadBackgroundLogoImage()
        {
            //if (UploadBackgroundLogoImageTask != null && UploadBackgroundLogoImageTask.Status == TaskStatus.Running)
            //    return UploadBackgroundLogoImageTask;
            //UploadBackgroundLogoImageTask = Task.Run(() =>
            //{
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "image avif files (*.avif)|*.avif";
            openFileDialog.Title = "Upload Logo background";
            if (openFileDialog.ShowDialog() == true)
            {
                string BackgroundLogoImagePath = openFileDialog.FileName;

                var uploadSlot = this.HomeDeliveryServicePoint.GetUploadSlotForLogoBackgroundImage();
                var imageStream = System.IO.File.OpenRead(BackgroundLogoImagePath);
                FlavourBusinessToolKit.RestApiBlobFileManager.Upload(imageStream, uploadSlot, "image/avif");
                _LogoBackgroundImageUrl = this.HomeDeliveryServicePoint.LogoBackgroundImageUrl;
#if DEBUG
                if (!string.IsNullOrWhiteSpace(_LogoBackgroundImageUrl))
                    _LogoBackgroundImageUrl = "https://dev-localhost/" + _LogoBackgroundImageUrl.Substring(_LogoBackgroundImageUrl.IndexOf("devstoreaccount1"));
#endif

            }
            //});



            //return UploadBackgroundLogoImageTask;
        }

        /// <exclude>Excluded</exclude>
        decimal _FreeShippingMinimumOrderValue;
        /// <MetaDataID>{40aea926-fbdc-4d4a-8881-6cbfeeefacd4}</MetaDataID>
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

        /// <exclude>Excluded</exclude>
        string _BrandName;
        [HttpVisible]
        [CachingDataOnClientSide]
        public string BrandName
        {
            get
            {
                return _BrandName;
            }
            set
            {

                if (_BrandName != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _BrandName = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
        /// <MetaDataID>{9cdef6b4-18ce-4b6e-a3cb-16217f2f4728}</MetaDataID>
        static string AzureServerUrl = string.Format("http://{0}:8090/api/", FlavourBusinessFacade.ComputingResources.EndPoint.Server);

        /// <MetaDataID>{4c0137a5-0923-4453-b323-f1411b8ac3a1}</MetaDataID>
        public HomeDeliveryServicePresentation(IHomeDeliveryServicePoint homeDeliveryServicePoint, IFlavoursServicesContext servicesContext)
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
            _BrandName = HomeDeliveryServicePoint.BrandName;
            WeeklyDeliverySchedule = HomeDeliveryServicePoint.WeeklyDeliverySchedule;

            _LogoBackgroundImageUrl = this.HomeDeliveryServicePoint.LogoBackgroundImageUrl;
#if DEBUG
            if (!string.IsNullOrWhiteSpace(_LogoBackgroundImageUrl))
                _LogoBackgroundImageUrl = "https://dev-localhost/" + _LogoBackgroundImageUrl.Substring(_LogoBackgroundImageUrl.IndexOf("devstoreaccount1"));
#endif

            _LogoImageUrl = this.HomeDeliveryServicePoint.LogoImageUrl;
#if DEBUG
            if (!string.IsNullOrWhiteSpace(_LogoImageUrl))
                _LogoImageUrl = "https://dev-localhost/" + _LogoImageUrl.Substring(_LogoImageUrl.IndexOf("devstoreaccount1"));
#endif
            BeforeTransactionCommitCommand = new RelayCommand((object sender) =>
            {
                if (_Places.Count == 0)
                    HomeDeliveryServicePoint.PlaceOfDistribution = null;
                else
                    HomeDeliveryServicePoint.PlaceOfDistribution = _Places[0];
                HomeDeliveryServicePoint.Update(BrandName,HomeDeliveryServicePoint.PlaceOfDistribution, MapCenter, ServiceAreaMap, IsPolyline, Zoom, WeeklyDeliverySchedule, MinimumOrderValue, ShippingCost, FreeShippingMinimumOrderValue);
                var foodTypeTags = _FoodTypeTags.Where(x => x.Selected && !SelectedFoodTypes.Any(y => y.Uri == x.Uri)).Select(x => x.FoodTypeTag).ToList();
                ServicesContext.AddFoodTypes(foodTypeTags);

                foodTypeTags = _FoodTypeTags.Where(x => !x.Selected && SelectedFoodTypes.Any(y => y.Uri == x.Uri)).Select(x => x.FoodTypeTag).ToList();
                ServicesContext.RemoveFoodTypes(foodTypeTags);

            });
        }
        /// <MetaDataID>{3f46a732-871b-4c71-be57-96ceb64ea27d}</MetaDataID>
        public HomeDeliveryServicePresentation()
        {

        }
        /// <exclude>Excluded</exclude>
        private List<FoodTypeTagPresentation> _FoodTypeTags;

        /// <MetaDataID>{468a861b-0f50-41b7-b0f6-4c6737fdf4d1}</MetaDataID>
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
        /// <MetaDataID>{518cd2d0-b666-49bd-9767-4ed6ffeda120}</MetaDataID>
        [HttpVisible]
        public void AddFoodTypeTag(string foodTypeTagUri)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodTypeTags.Where(x => x.Uri == foodTypeTagUri).FirstOrDefault().Selected = true;

                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{6c2a2280-4cb5-4905-b824-9a4b618a23c8}</MetaDataID>
        [HttpVisible]
        public void RemoveFoodTypeTag(string foodTypeTagUri)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _FoodTypeTags.Where(x => x.Uri == foodTypeTagUri).FirstOrDefault().Selected = false;
                stateTransition.Consistent = true;
            }

        }


        /// <MetaDataID>{0a491e1f-877b-40a2-a4d6-b51c83a6e9ea}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand BeforeTransactionCommitCommand { get; set; }

        /// <MetaDataID>{1489f795-b34f-4e16-8209-59fa43747ede}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<IPlace> _Places = new OOAdvantech.Collections.Generic.Set<IPlace>();

        /// <MetaDataID>{ee983561-4ec7-4200-8634-2710c20e134b}</MetaDataID>
        [ImplementationMember(nameof(_Places))]
        public List<IPlace> Places => _Places.ToList();

        /// <MetaDataID>{43eabe0e-e369-44c8-a3ef-4162707b65d8}</MetaDataID>
        public IHomeDeliveryServicePoint HomeDeliveryServicePoint { get; }
        /// <MetaDataID>{b2f35440-7e40-4026-b00d-2ec7507a1cd2}</MetaDataID>
        public List<IFoodTypeTag> SelectedFoodTypes { get; private set; }

        /// <MetaDataID>{d738e215-64b3-4735-ba0e-87b907f9dbb4}</MetaDataID>
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

        /// <MetaDataID>{8007c282-65f3-4d10-b4c4-d582112d52ae}</MetaDataID>
        public void RemovePlace(IPlace place)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Places.Clear();
                stateTransition.Consistent = true;
            }

        }
         

        /// <MetaDataID>{0fc6dea2-0872-488c-9b25-f319adea9d86}</MetaDataID>
        Dictionary<DayOfWeek, List<OpeningHours>> WeeklyDeliverySchedule = new Dictionary<DayOfWeek, List<OpeningHours>>();
        /// <MetaDataID>{5200a30e-3bd6-4e2e-a8d1-677d443f1726}</MetaDataID>
        private IFlavoursServicesContext ServicesContext;

        /// <MetaDataID>{d21a1c47-fc59-4620-b915-d0ee55708850}</MetaDataID>
        public void SetDefaultPlace(IPlace place)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                place.Default = true;
                HomeDeliveryServicePoint.PlaceOfDistribution = place;

                stateTransition.Consistent = true;
            }
        }

        /// <MetaDataID>{830bd0bc-8f19-4870-8336-0cac97a6c776}</MetaDataID>
        Task<Coordinate?> IGeocodingPlaces.GetCurrentLocation()
        {
            return null;
        }
    }

    /// <MetaDataID>{56f7e1a5-9026-44f0-becc-4865a3c9cfea}</MetaDataID>
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
