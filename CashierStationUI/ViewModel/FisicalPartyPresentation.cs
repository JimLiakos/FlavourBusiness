using FinanceFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashierStationUI.ViewModel
{
    /// <MetaDataID>{b7f325f2-25bc-4e58-b4f7-1405eb262d1f}</MetaDataID>
    public class FisicalPartyPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public FisicalPartyPresentation()
        {

        }
        public IFisicalParty FisicalParty { get; }
        public FisicalPartyPresentation(IFisicalParty fisicalPart)
        {
            FisicalParty = fisicalPart;
        }

        public string Name
        {
            get
            {
                return FisicalParty.Name;
            }
            set
            {
                FisicalParty.Name = value;
            }



        }
        public string VATNumber
        {
            get
            {
                return FisicalParty.VATNumber;
            }
            set
            {
                FisicalParty.VATNumber = value;
            }
        }
    }
}
