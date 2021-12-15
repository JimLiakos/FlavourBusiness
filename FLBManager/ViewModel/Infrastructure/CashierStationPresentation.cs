using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel.HumanResources;
using FLBManager.ViewModel.Taxes;
using OOAdvantech.Transactions;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{417f9100-3809-4886-8902-bcb2acd74f3d}</MetaDataID>
    public class CashierStationPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {
        public CashierStationPresentation() : base(null)
        {

        }
        public readonly ICashierStation CashierStation;
        TreasuryTreeNode Treasury;
        public CashierStationPresentation(TreasuryTreeNode parent, ICashierStation cashierStation) : base(parent)
        {
            Treasury = parent;
            CashierStation = cashierStation;
            PrintReceiptsItemStates = new List<PrintReceiptsItemStateViewModel>();
            PrintReceiptsItemStates.Add(new PrintReceiptsItemStateViewModel(cashierStation.GetPrintReceiptCondition(ServicePointType.Delivery)));
            PrintReceiptsItemStates.Add(new PrintReceiptsItemStateViewModel(cashierStation.GetPrintReceiptCondition(ServicePointType.HallServicePoint)));
            PrintReceiptsItemStates.Add(new PrintReceiptsItemStateViewModel(cashierStation.GetPrintReceiptCondition(ServicePointType.TakeAway)));




            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            EditCommand = new RelayCommand((object sender) =>
            {
                CashierStationEdit();
            });

            AddFisicalPartyCommand = new RelayCommand((object sender) =>
            {
                var fisicalParty = Treasury.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.NewFisicalParty();
                FisicalPartiesMap.GetViewModelFor(fisicalParty, fisicalParty);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(FisicalParties)));

            });
            EditSelectedFisicalPartyCommand = new RelayCommand((object sender) =>
            {
                FisicalPartiesExpanded = false;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(FisicalPartiesExpanded)));
                System.Windows.Window win = System.Windows.Window.GetWindow(EditSelectedFisicalPartyCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

                Views.HumanResources.FisicalPartyWindow fisicalPartyWindow = new Views.HumanResources.FisicalPartyWindow();
                fisicalPartyWindow.Owner = win;

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
                {
                    fisicalPartyWindow.GetObjectContext().SetContextInstance(_SelectedFisicalParty);
                    if (fisicalPartyWindow.ShowDialog().Value)
                    {
                        Treasury.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.UpdateFisicalParty(_SelectedFisicalParty.FisicalParty);
                        stateTransition.Consistent = true;
                    }
                }


            });
            foreach (var fisicalParty in Treasury.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.FisicalParties)
                FisicalPartiesMap.GetViewModelFor(fisicalParty, fisicalParty);

            _SelectedFisicalParty = FisicalPartiesMap.Values.Where(x => x.FisicalParty.FisicalPartyUri == CashierStation.Issuer.FisicalPartyUri).FirstOrDefault();



        }

        public bool FisicalPartiesExpanded { get; set; }

        internal ViewModelWrappers<FinanceFacade.IFisicalParty, FisicalPartyPresentation> FisicalPartiesMap = new ViewModelWrappers<FinanceFacade.IFisicalParty, FisicalPartyPresentation>();



        public List<FisicalPartyPresentation> FisicalParties
        {
            get
            {
                return FisicalPartiesMap.Values.ToList();

            }
        }

        FisicalPartyPresentation _SelectedFisicalParty;

        public List<PrintReceiptsItemStateViewModel> PrintReceiptsItemStates { get; }

        public FisicalPartyPresentation SelectedFisicalParty
        {
            get
            {
                return _SelectedFisicalParty;
            }
            set
            {
                _SelectedFisicalParty = value;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsEditSelectedEnabled)));
                //CashierStation.Issuer = _SelectedFisicalParty.FisicalParty;
            }
        }

        //052790304
        private void CashierStationEdit()
        {
            System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

            Views.Infrastructure.CashierStationWindow cashierStationWindow = new Views.Infrastructure.CashierStationWindow();
            cashierStationWindow.Owner = win;

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                cashierStationWindow.GetObjectContext().SetContextInstance(this);
                if (cashierStationWindow.ShowDialog().Value)
                {
                    if (_SelectedFisicalParty != null)
                        CashierStation.Issuer = _SelectedFisicalParty.FisicalParty;
                    foreach (var printReceiptsItemState in PrintReceiptsItemStates)
                        CashierStation.SetPrintReceiptCondition(printReceiptsItemState.ServicePointType, printReceiptsItemState.PrintReceiptCondition);
                    stateTransition.Consistent = true;
                }
            }
        }

        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CashierStationEdit)));
            }
            _Edit = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CashierStationEdit)));
        }

        private void Delete()
        {
            Treasury.RemoveCashierStation(this);

            // (Company as CompanyPresentation).RemoveServicesContext(this);
        }

        public RelayCommand RenameCommand { get; protected set; }
        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand EditCommand { get; protected set; }

        public RelayCommand AddFisicalPartyCommand { get; protected set; }

        public RelayCommand DeleteFisicalPartyCommand { get; protected set; }

        public RelayCommand EditSelectedFisicalPartyCommand { get; protected set; }

        public bool IsEditSelectedEnabled => _SelectedFisicalParty != null;



        List<MenuCommand> _ContextMenuItems;

        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(menuItem);
                    menuItem = new WPFUIElementObjectBind.MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;
                    _ContextMenuItems.Add(menuItem);



                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/ServiceArea.png"));
                    //menuItem.Header = Properties.Resources.AddServiceAreaHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddServiceAreaCommand;

                    //_ContextMenuItems.Add(menuItem);

                    //MenuComamnd menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerIDLine16.png"));
                    //menuItem.Header = Properties.Resources.AddCallerIDLine;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddCallerIDLineCommand;
                    //_ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);



                }
                //if (_ContextMenuItems == null)
                //{
                //    _ContextMenuItems = new List<MenuComamnd>();
                //}
                return _ContextMenuItems;
            }
        }

        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members;
            }
        }


        public override string Name
        {
            get
            {
                return CashierStation.Description;
            }

            set
            {
                CashierStation.Description = value;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                else
                    foreach (var treeNode in Members)
                    {
                        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                        if (contextMenuItems != null)
                            return contextMenuItems;
                    }

                return null;
            }
        }


        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/cashier16.png"));
            }
        }

        public override void SelectionChange()
        {

        }

    }
}
