using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFUIElementObjectBind;

namespace Finance.ViewModel
{
    /// <MetaDataID>{18bb38f9-c1ee-40c4-974f-1c6874fd98ee}</MetaDataID>
    public class TaxOverrideViewModel :MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TaxOverrideViewModel()
        {

        }

        public readonly FinanceFacade.ITax Tax;
        FinanceFacade.ITaxesContext TaxesContext;
        public TaxOverrideViewModel(FinanceFacade.ITax tax, FinanceFacade.ITaxesContext taxesContext)
        {
            Tax = tax;
            TaxesContext = taxesContext;

            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
            });
        }

        public string Description
        {
            get
            {
                return Tax?.Description;
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
                if (Tax == null)
                    return default(double);
                return Tax.TaxRate * 100;
            }
            set
            {
                Tax.TaxRate = value / 100;
            }
        }

        //public string MaxMinImage
        //{
        //    get
        //    {
        //        "/MenuItemsEditor;component/Image/MaximizeWindow.png"
        //    }
        //}

        public ImageSource MaxMinImage
        {
            get
            {
                if (this.IsMaximized)
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FinanceWpf;component/Images/MinimizeWindow.png"));
                else
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FinanceWpf;component/Images/MaximizeWindow.png"));
            }
        }

        public string AccountID
        {
            get
            {
                return Tax?.AccountID;
            }
            set
            {
                Tax.AccountID = value;
            }
        }

        public bool IsMaximized { get; private set; }
        public bool IsMimized
        {
            get
            {
                return !IsMaximized;

            }
        }
    }
}
