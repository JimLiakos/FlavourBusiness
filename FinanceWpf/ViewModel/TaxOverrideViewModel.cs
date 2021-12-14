using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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

            DeleteCommand = new RelayCommand((object sender) =>
             {
                 var ovrs = Tax.TaxOverrides.ToArray();

                 taxesContext.RemoveTaxOverride(TaxOverride);
                 TaxOverride = null;

                 ovrs = Tax.TaxOverrides.ToArray();

                 IsMaximized = false;
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TaxRate)));
                 PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AccountID)));

             });
        }

        public Visibility DeleteVisibility
        {
            get
            {
                 return TaxOverride != null?Visibility.Visible:Visibility.Collapsed;
            }
        }

        public double OverridenOpacity
        {
            get
            {
                return TaxOverride != null ? 1 : 0.5;
            }

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
                    {

                        TaxOverride = TaxesContext.GetTaxOverride(Tax, true);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));

                    }
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

        /// <exclude>Excluded</exclude>
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
                    {
                        TaxOverride = TaxesContext.GetTaxOverride(Tax, true);
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OverridenOpacity)));
                        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DeleteVisibility)));
                    }
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
            {
                AccountIDErrorBorder = 1;
                return true;
            }
        }
    }
}
