using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationDevice.ViewModel
{
    /// <MetaDataID>{2262dee6-0251-4960-843c-99e57dfb66d1}</MetaDataID>
    public class CashierStationPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{3c259bbc-16a9-456f-81f6-a3c53fd4de2f}</MetaDataID>
        public readonly ICashierStation CashierStation;

        /// <MetaDataID>{0365cb2d-6a98-4f10-bf4c-a349e1335711}</MetaDataID>
        public CashierStationPresentation(ICashierStation cashierStation)
        {
            CashierStation = cashierStation;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{6b8e3355-15f1-4596-9542-f9469901dbb0}</MetaDataID>
        public string Name
        {
            get
            {
                return CashierStation.Description;
            }
        }


    }
}
