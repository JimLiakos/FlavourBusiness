using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashierStationDevice.ViewModel
{
    /// <MetaDataID>{2262dee6-0251-4960-843c-99e57dfb66d1}</MetaDataID>
    public class CashierStationPresentation : MarshalByRefObject, INotifyPropertyChanged
    {
        /// <MetaDataID>{3c259bbc-16a9-456f-81f6-a3c53fd4de2f}</MetaDataID>
        public readonly ICashierStation CashierStation;
        FlavourBusinessFacade.IFlavoursServicesContext ServicesContext;
        /// <MetaDataID>{0365cb2d-6a98-4f10-bf4c-a349e1335711}</MetaDataID>
        public CashierStationPresentation(ICashierStation cashierStation, FlavourBusinessFacade.IFlavoursServicesContext servicesContext)
        {
            ServicesContext = servicesContext;
            CashierStation = cashierStation;

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

                CashierStationUI.Views.CashierStationWindow cashierStationWindow = new CashierStationUI.Views.CashierStationWindow();
                cashierStationWindow.Owner = win;

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                {
                    CashierStationUI.ViewModel.CashierStationPresentation cashierStationPresentation = new CashierStationUI.ViewModel.CashierStationPresentation(null, cashierStation, servicesContext);
                    cashierStationWindow.GetObjectContext().SetContextInstance(cashierStationPresentation);
                    if (cashierStationWindow.ShowDialog().Value)
                    {
                    
                        foreach (var printReceiptsItemState in cashierStationPresentation.PrintReceiptsItemStates)
                            CashierStation.SetPrintReceiptCondition(printReceiptsItemState.ServicePointType, printReceiptsItemState.PrintReceiptCondition);
                        stateTransition.Consistent = true;
                    }
                }

            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <MetaDataID>{6b8e3355-15f1-4596-9542-f9469901dbb0}</MetaDataID>
        public string Name
        {
            get
            {
                return CashierStation.Description;
            }
        }

        public WPFUIElementObjectBind.RelayCommand EditCommand { get; set; }


    }
}
