using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace CashierStationDevice.ViewModel
{
    internal class PaymentTerminalsViewModel : MarshalByRefObject, INotifyPropertyChanged
    {

        public PaymentTerminalsViewModel(IPaymentTerminal paymentTerminal, FlavourBusinessFacade.IFlavoursServicesContext servicesContext)
        {
            ServicesContext = servicesContext;
            PaymentTerminal = paymentTerminal;

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

                CashierStationUI.Views.CashierStationWindow cashierStationWindow = new CashierStationUI.Views.CashierStationWindow();
                cashierStationWindow.Owner = win;

                //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                //{
                //    CashierStationUI.ViewModel.CashierStationPresentation cashierStationPresentation = new CashierStationUI.ViewModel.CashierStationPresentation(null, cashierStation, servicesContext);
                //    cashierStationWindow.GetObjectContext().SetContextInstance(cashierStationPresentation);
                //    if (cashierStationWindow.ShowDialog().Value)
                //    {

                //        foreach (var printReceiptsItemState in cashierStationPresentation.PrintReceiptsItemStates)
                //            CashierStation.SetPrintReceiptCondition(printReceiptsItemState.ServicePointType, printReceiptsItemState.PrintReceiptCondition);
                //        stateTransition.Consistent = true;
                //    }
                //}

            });
        }

        public IFlavoursServicesContext ServicesContext { get; }
        public IPaymentTerminal PaymentTerminal { get; }
        public RelayCommand EditCommand { get; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
