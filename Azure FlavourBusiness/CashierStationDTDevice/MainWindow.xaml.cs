using CashierStationDevice.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashierStationDTDevice
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// <MetaDataID>{57e42df5-aabd-483b-aa9c-ecfd5f5b7e90}</MetaDataID>
    public partial class MainWindow : Window
    {
        /// <MetaDataID>{60a25c8b-0556-49d6-804d-f506ca31745a}</MetaDataID>
        public MainWindow()
        {
            InitializeComponent();
            this.GetObjectContext().Initialize(this);
            CashierStationDevicePresentation = new CashierStationDevicePresentation();
            this.GetObjectContext().SetContextInstance(CashierStationDevicePresentation);
        }

        public CashierStationDevicePresentation CashierStationDevicePresentation { get; }



        /// <MetaDataID>{3165c8ff-7fa7-4860-ad78-ea1c02be69b3}</MetaDataID>
        private void button_Click(object sender, RoutedEventArgs e)
        {

            FinanceFacade.Transaction transaction = new FinanceFacade.Transaction();
            var Spinach = new FinanceFacade.Item() { Name = "Spinach", Quantity = 1, Price = 7.5m };

            Spinach.AddTax(new FinanceFacade.TaxAmount() { AccountID = "c1", Amount = 1.45m });
            transaction.AddItem(Spinach);
            var pasta = new FinanceFacade.Item() { Name = "Πένες Μπολονέζ", Quantity = 1, Price = 6.5m };
            pasta.AddTax(new FinanceFacade.TaxAmount() { AccountID = "c1", Amount = 1.3m });
            transaction.AddItem(pasta);
            var beer = new FinanceFacade.Item() { Name = "Μπύρα Μύθος", Quantity = 1, Price = 2.5m };
            beer.AddTax(new FinanceFacade.TaxAmount() { AccountID = "c1", Amount = 0.48m });
            transaction.AddItem(beer);

            transaction.PayeeRegistrationNumber = "800696676";

            CashierStationDevice.DocumentSignDevice.CurrentDocumentSignDevice.PrintReceipt(transaction);
        }
    }
}
