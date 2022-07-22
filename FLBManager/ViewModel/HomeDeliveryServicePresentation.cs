using FlavourBusinessFacade;
using FlavourBusinessFacade.EndUsers;
using FlavourBusinessFacade.ServicesContextResources;
using FlavourBusinessManager.ServicesContextResources;
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
    [OOAdvantech.Transactions.Transactional]
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

            BeforeTransactionCommitCommand = new RelayCommand((object sender) =>
            {
                if (_Places.Count == 0)
                    HomeDeliveryServicePoint.PlaceOfDistribution = null;
                else
                    HomeDeliveryServicePoint.PlaceOfDistribution = _Places[0];

            });
        }
        public HomeDeliveryServicePresentation()
        {
            
        }
        public WPFUIElementObjectBind.RelayCommand BeforeTransactionCommitCommand { get; set; }

        OOAdvantech.Collections.Generic.Set <IPlace> _Places = new OOAdvantech.Collections.Generic.Set<IPlace>();
        
        [ImplementationMember(nameof(_Places))]
        public List<IPlace> Places => _Places.ToList();

        public IHomeDeliveryServicePoint HomeDeliveryServicePoint { get; }

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
}
