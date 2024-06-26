﻿using CashierStationUI.ViewModel;
using CashierStationUI.Views;
using CashierStationWindow.Views;
using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace CashierStationUI.ViewModel
{
    /// <MetaDataID>{417f9100-3809-4886-8902-bcb2acd74f3d}</MetaDataID>
    public class CashierStationPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {
        public CashierStationPresentation() : base(null)
        {

        }
        public readonly ICashierStation CashierStation;
        public readonly IFlavoursServicesContext ServicesContext;
        public CashierStationPresentation(FBResourceTreeNode parent, ICashierStation cashierStation, IFlavoursServicesContext servicesContext) : base(parent)
        {
            
            CashierStation = cashierStation;
            ServicesContext = servicesContext;
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
                var fisicalParty = ServicesContext.NewFisicalParty();
                FisicalPartiesMap.GetViewModelFor(fisicalParty, fisicalParty);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(FisicalParties)));

            });
            EditSelectedFisicalPartyCommand = new RelayCommand((object sender) =>
            {
                FisicalPartiesExpanded = false;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(FisicalPartiesExpanded)));
                Window win = System.Windows.Window.GetWindow(EditSelectedFisicalPartyCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

                FisicalPartyWindow fisicalPartyWindow = new FisicalPartyWindow();
                fisicalPartyWindow.Owner = win;

                using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiredNested))
                {
                    fisicalPartyWindow.GetObjectContext().SetContextInstance(_SelectedFisicalParty);
                    if (fisicalPartyWindow.ShowDialog().Value)
                    {
                        ServicesContext.UpdateFisicalParty(_SelectedFisicalParty.FisicalParty);
                        stateTransition.Consistent = true;
                    }
                }


            });
            foreach (var fisicalParty in ServicesContext.FisicalParties)
                FisicalPartiesMap.GetViewModelFor(fisicalParty, fisicalParty);


            var isd= CashierStation.Issuer;

            _SelectedFisicalParty = FisicalPartiesMap.Values.Where(x => x.FisicalParty.FisicalPartyUri == CashierStation.Issuer?.FisicalPartyUri).FirstOrDefault();



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

            CashierStationUI.Views.CashierStationWindow cashierStationWindow = new CashierStationUI.Views.CashierStationWindow();
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
            Parent.RemoveChild(this);
            

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
                    menuItem.Header = Properties.Resources.RemoveNode;
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

        public override void RemoveChild(FBResourceTreeNode FBResourceTreeNode)
        {
            throw new NotImplementedException();
        }
    }
}
