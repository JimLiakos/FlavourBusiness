using CashierStationDevice.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using WPFUIElementObjectBind;

namespace CashierStationDevice.ViewModel
{
    /// <MetaDataID>{15827090-17db-482e-8b0a-50171b2ed232}</MetaDataID>
    public class TransactionPrinterPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public TransactionPrinterPresentation()
        {
        }


        public List<string> AvailablePrinters { get; set; }
        public readonly TransactionPrinter TransactionPrinter;
        public TransactionPrinterPresentation(TransactionPrinter tax)
        {
            TransactionPrinter = tax;
            MaximizeCommand = new RelayCommand((object sender) =>
            {
                IsMaximized = !IsMaximized;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
            });

            AvailablePrinters = PrinterSettings.InstalledPrinters.OfType<string>().ToList();


            if (!string.IsNullOrWhiteSpace(TransactionPrinter.PrinterName) && AvailablePrinters.Where(x => x.ToLower() == TransactionPrinter.PrinterName.ToLower()).FirstOrDefault() == null)
                AvailablePrinters.Insert(0, TransactionPrinter.PrinterName);
            AvailablePrinters.Insert(0, "(none)");
        }


        public string Description
        {
            get
            {
                return TransactionPrinter?.Description;
            }
            set
            {
                TransactionPrinter.Description = value;
            }
        }




        public RelayCommand MaximizeCommand { get; set; }

        public string PrinterName
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TransactionPrinter.PrinterName))
                    return "(none)";

                return TransactionPrinter.PrinterName;
            }
            set
            {
                if (value == "(none)")
                    TransactionPrinter.PrinterName = null;

                TransactionPrinter.PrinterName = value;
            }
        }

        public string Series
        {
            get
            {
                return TransactionPrinter.Series;
            }
            set
            {
                TransactionPrinter.Series = value;
            }
        }

        public int AutoNumber
        {
            get
            {
                return TransactionPrinter.AutoNumber;
            }
            set
            {
                TransactionPrinter.AutoNumber = value;
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
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/CashierStationDevice;component/Resources/Images/Metro/MinimizeWindow.png"));
                else
                    return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/CashierStationDevice;component/Resources/Images/Metro/MaximizeWindow.png"));
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

        //internal bool Validate()
        //{
        //    if (string.IsNullOrWhiteSpace(AccountID))
        //    {
        //        IsMaximized = true;

        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMaximized)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsMimized)));
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MaxMinImage)));
        //        AccountIDErrorBorder = 1;
        //        return false;
        //    }
        //    AccountIDErrorBorder = 0;

        //    return true;
        //}

        public string RawPrinterAddress
        {
            get
            {
                return TransactionPrinter?.RawPrinterAddress;
            }
            set
            {
                TransactionPrinter.RawPrinterAddress = value;
            }
        }

        public int PrinterCodePage
        {
            get
            {
                if (TransactionPrinter == null)
                    return 0;
                return TransactionPrinter.PrinterCodePage;
            }
            set
            {
                TransactionPrinter.PrinterCodePage = value;
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
