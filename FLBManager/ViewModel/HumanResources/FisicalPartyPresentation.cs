using FinanceFacade;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLBManager.ViewModel.HumanResources
{
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
        }
        public string VATNumber
        {
            get
            {
                return FisicalParty.VATNumber;
            }
        }
    }
}
