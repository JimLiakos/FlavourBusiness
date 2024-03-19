using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance.ViewModel
{
    /// <MetaDataID>{4362711b-13f2-458a-8a95-e1388b8668d1}</MetaDataID>
    public class TaxesContextViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TaxesContextViewModel()
        {
        }

        public readonly FinanceFacade.ITaxesContext TaxesContext;

        public TaxesContextViewModel(FinanceFacade.ITaxesContext taxesContext)
        {
            TaxesContext = taxesContext;
        }



        bool _Edit;
        public bool Edit
        {
            get
            {
                return _Edit;
            }
            set
            {
                if (_Edit != value)
                {
                    _Edit = value;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
        }

        public string Description
        {
            get
            {
                if(TaxesContext == null)
                    return Properties.Resources.NameForDefaultTaxes;

                return TaxesContext?.Description;
            }
            set
            {
                if (TaxesContext != null)
                {
                    TaxesContext.Description = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Description)));
                }
            }
        }

    }
}
