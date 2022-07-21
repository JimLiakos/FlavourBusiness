using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.Remoting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{a2719e24-d3e7-4df3-acaa-e80226746b20}</MetaDataID>
    public class HomeDeliveryServicePresentation : MarshalByRefObject, INotifyPropertyChanged, IGeocodingPlaces, IExtMarshalByRefObject
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public HomeDeliveryServicePresentation(FlavourBusinessFacade.ServicesContextResources.IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {
            HomeDeliveryServicePoint = homeDeliveryServicePoint;
            var placeOfDistribution = homeDeliveryServicePoint.PlaceOfDistribution;
            if (placeOfDistribution != null)
                _Places.Add(placeOfDistribution);
        }
        public HomeDeliveryServicePresentation()
        {

        }
        public WPFUIElementObjectBind.RelayCommand SaveCommand { get; set; }

        List<IPlace> _Places = new List<IPlace>();
        public List<IPlace> Places => _Places.ToList();

        public IHomeDeliveryServicePoint HomeDeliveryServicePoint { get; }

        public void SavePlace(IPlace place)
        {
            _Places.Clear();
            _Places.Add(place);
            place.Default = true;
            HomeDeliveryServicePoint.PlaceOfDistribution = place;
            
        }

        public void RemovePlace(IPlace place)
        {
            _Places.Clear();
            HomeDeliveryServicePoint.PlaceOfDistribution = null;
        }

   

   

        public void SetDefaultPlace(IPlace place)
        {
            place.Default = true;
            HomeDeliveryServicePoint.PlaceOfDistribution = place;
        }

        Task<Coordinate?> IGeocodingPlaces.GetCurrentLocation()
        {
            return null;
        }
    }
}
