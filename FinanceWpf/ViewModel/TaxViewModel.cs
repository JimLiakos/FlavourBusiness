using FinanceFacade;
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
    /// <MetaDataID>{283b34d8-6d5c-4dc8-af79-a05945c11bbc}</MetaDataID>
    public class TaxViewModel : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{16e43564-38a3-4fd4-aba8-5028fec56393}</MetaDataID>
        public TaxViewModel()
        {

        }

        /// <MetaDataID>{402c83c8-95af-4e19-bba1-6a53a4ccd183}</MetaDataID>
        public readonly FinanceFacade.ITax Tax;
        /// <MetaDataID>{a4e015ed-e562-4a93-a96a-9dcd644cd685}</MetaDataID>
        public TaxViewModel(FinanceFacade.ITax tax)
        {
            Tax = tax;
            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));


            });
        }

        /// <MetaDataID>{91220521-4f1b-4a08-8542-3e6659b6b983}</MetaDataID>
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
        /// <MetaDataID>{51769adb-9f7e-4a49-b232-0c704bcd7fb3}</MetaDataID>
        public RelayCommand MaximizeCommand { get; set; }

        /// <MetaDataID>{3fcb4859-efc9-44ed-a3c7-e8b7c7ea3cf6}</MetaDataID>
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

        /// <MetaDataID>{908075d5-960f-4497-bff8-1685b152ba49}</MetaDataID>
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
        /// <MetaDataID>{67f9e923-6ecf-4505-8840-39a264578422}</MetaDataID>
        public RelayCommand DeleteCommand { get; set; }


        /// <MetaDataID>{3bd63e3e-2409-4133-9367-3d27e9318112}</MetaDataID>
        public Visibility DeleteVisibility
        {
            get
            {
                return Visibility.Collapsed;
            }
        }

        /// <MetaDataID>{b689ac22-1a8d-4344-ac7f-b802a077256f}</MetaDataID>
        public double OverridenOpacity
        {
            get
            {
                return 1;
            }

        }

        /// <MetaDataID>{419e2c07-1fce-41cc-a280-97214b1d6107}</MetaDataID>
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

        /// <MetaDataID>{c6a238ca-9d82-449c-aa26-685761d5f7c2}</MetaDataID>
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
        /// <MetaDataID>{95e9ac99-72ad-4af8-90c8-f2bb44f33dcb}</MetaDataID>
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

        /// <MetaDataID>{8af52766-89a6-4e61-bb37-c791107074d1}</MetaDataID>
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

        /// <MetaDataID>{ea79827d-77a7-45ea-83c0-854df2e17e15}</MetaDataID>
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

        /// <MetaDataID>{d4d0bc06-cd0c-453e-882f-0cb2d3ce946a}</MetaDataID>
        public bool IsMaximized { get; private set; }
        /// <MetaDataID>{d2012e94-27d2-4773-b68a-d6f646083614}</MetaDataID>
        public bool IsMimized
        {
            get
            {
                return !IsMaximized;

            }
        }


    }
}
