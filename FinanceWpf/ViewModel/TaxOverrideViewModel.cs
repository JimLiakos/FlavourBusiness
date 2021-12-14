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
    public class TaxOverrideViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TaxOverrideViewModel()
        {

        }

        public readonly FinanceFacade.ITax Tax;

        private FinanceFacade.ITaxOverride TaxOverride;
        FinanceFacade.ITaxesContext TaxesContext;
        public TaxOverrideViewModel(FinanceFacade.ITax tax, FinanceFacade.ITaxesContext taxesContext)
        {

            TaxOverride = taxesContext.GetTaxOverride(tax);

            Tax = tax;
            TaxesContext = taxesContext;

            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
            });

            del
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

        public RelayCommand DeleteCommand { get; set; }

        public double TaxRate
        {
            get
            {
                if (TaxOverride != null)
                    return TaxOverride.TaxRate * 100;

                if (Tax == null)
                    return default(double);
                return Tax.TaxRate * 100;
            }
            set
            {
                if (Tax.TaxRate != value / 100)
                {
                    if (TaxOverride == null)
                        TaxOverride = TaxesContext.GetTaxOverride(Tax,true);
                    TaxOverride.TaxRate = value / 100;
                }
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

        double _AccountIDErrorBorder = 0;

        public double AccountIDErrorBorder
        {
            get
            {
                return _AccountIDErrorBorder;
            }
            set
            {
                _AccountIDErrorBorder = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountIDErrorBorder)));
            }

        }

        public string AccountID
        {
            get
            {
                return TaxOverride?.AccountID;
            }
            set
            {
                if (Tax.AccountID != value)
                {
                    AccountIDErrorBorder = 0;
                    if (TaxOverride == null)
                        TaxOverride = TaxesContext.GetTaxOverride(Tax,true);
                    TaxOverride.AccountID = value;
                }
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

        internal bool Validate()
        {
            if (TaxOverride != null && (TaxOverride.TaxRate == Tax.TaxRate || string.IsNullOrWhiteSpace(TaxOverride.AccountID)))
            {
                IsMaximized = true;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                AccountIDErrorBorder = 1;
                return false;
            }
            else
                return true;
        }
    }
}
