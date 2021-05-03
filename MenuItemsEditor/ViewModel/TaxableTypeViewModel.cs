using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuItemsEditor.ViewModel
{
    /// <MetaDataID>{64e21e03-560a-46a9-95ec-d25b6c3443ce}</MetaDataID>
    public class TaxableTypeViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public readonly FinanceFacade.ITaxableType TaxableType;
        public TaxableTypeViewModel(FinanceFacade.ITaxableType taxableType)
        {
            TaxableType = taxableType;
        }

        public string Description
        {
            get
            {
                return TaxableType.Description;
            }
            set
            {
                TaxableType.Description = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
