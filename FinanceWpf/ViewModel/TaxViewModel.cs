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
    /// <MetaDataID>{283b34d8-6d5c-4dc8-af79-a05945c11bbc}</MetaDataID>
    public class TaxViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TaxViewModel()
        {

        }

        public readonly FinanceFacade.ITax Tax;
        public TaxViewModel(FinanceFacade.ITax tax)
        {
            Tax = tax;
            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                Task.Run(() =>
                {

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FeePerUnit)));
                });

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
                return Tax.TaxRate*100;
            }
            set
            {
                Tax.TaxRate = value/100;
            }
        }

        public decimal TaxFee
        {
            get
            {
                if (Tax == null)
                    return default(decimal);
                return Tax.Fee;
            }
            set
            {
                Tax.Fee = value;
            }
        }

        public bool FeePerUnit
        {
            get
            {
                //if (!IsMaximized)
                //    return false;
                if (Tax == null)
                    return default(bool);
                return Tax.FeePerUnit;
            }
            set
            {
                Tax.FeePerUnit = value;
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

        internal bool Validate()
        {
            if (string.IsNullOrWhiteSpace(AccountID))
            {
                IsMaximized = true;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
                Task.Run(() =>
                {

                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FeePerUnit)));
                });
                
                AccountIDErrorBorder = 1;
                return false;
            }
            AccountIDErrorBorder = 0;

                return true;
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
