using FlavourBusinessFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.PriceList
{
    /// <MetaDataID>{8713b97f-bcb9-43ff-a2cb-2f9309c704b4}</MetaDataID>
    public class MenuPriceList : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly OrganizationStorageRef PriceListStorageRef;
        public MenuPriceList(OrganizationStorageRef priceListStorageRef)
        {
            PriceListStorageRef = priceListStorageRef;
        }

        public string Name
        {
            get
            {
                if (PriceListStorageRef != null)
                    return PriceListStorageRef.Name;
                else
                    return Properties.Resources.NonePriceListName;
            }
        }

    }
}
