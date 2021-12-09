using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace Finance.ViewModel
{
    /// <MetaDataID>{283b34d8-6d5c-4dc8-af79-a05945c11bbc}</MetaDataID>
    public class TaxViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public readonly FinanceFacade.ITax Tax;
        public TaxViewModel(FinanceFacade.ITax tax)
        {
            Tax = tax;
            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
            });
        }

        public string Description
        {
            get
            {
                return Tax.Description;
            }
            set
            {
                Tax.Description = value;
            }
        }
        public RelayCommand MaximizeCommand { get; set; }

        public double TaxRate
        {
            get
            {
                return Tax.TaxRate;
            }
            set
            {
                Tax.TaxRate = value;
            }
        }

        public string AccountID
        {
            get
            {
                return Tax.AccountID;
            }
            set
            {
                Tax.AccountID = value;
            }
        }

        public bool IsMaximized { get; private set; }
    }
}
