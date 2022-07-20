using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{a2719e24-d3e7-4df3-acaa-e80226746b20}</MetaDataID>
    public class HomeDeliveryServicePresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public HomeDeliveryServicePresentation(FlavourBusinessFacade.ServicesContextResources.IHomeDeliveryServicePoint homeDeliveryServicePoint)
        {

        }
        public HomeDeliveryServicePresentation()
        {

        }
        public WPFUIElementObjectBind.RelayCommand SaveCommand { get; set; }

    }
}
