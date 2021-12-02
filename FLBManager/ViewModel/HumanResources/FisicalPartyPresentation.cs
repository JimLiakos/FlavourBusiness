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
        IFisicalParty FisicalPart;
        public FisicalPartyPresentation(IFisicalParty fisicalPart)
        {
            FisicalPart = fisicalPart;
        }
    }
}
