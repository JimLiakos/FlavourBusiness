using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.ServicesContextResources;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{ad3b8d96-a759-4d5b-b969-cae505b4949f}</MetaDataID>
    public class TreasuryTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public TreasuryTreeNode(InfrastructureTreeNode parent) : base(parent)
        {
            ServiceContextInfrastructure = parent;

            NewCashierCommand = new RelayCommand((object sender) =>
             {
                 NewCashierStation();
             });



            Task.Run(() =>
            {

                try
                {

                    foreach (var cashierStation in ServiceContextInfrastructure.ServiceContextResources.CashierStations)
                        CashierStations.Add(cashierStation, new CashierStationPresentation(this, cashierStation));

                    //foreach (var cashierStation in ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.CashierStations)
                    //    CashierStations.Add(cashierStation, new CashierStationPresentation( this, cashierStation));
                }
                catch (System.Exception error)
                {
                }
            });
        }


        internal void RemoveCashierStation(CashierStationPresentation cashierStationPresentation)
        {
            this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.RemoveCashierStation(cashierStationPresentation.CashierStation);
            CashierStations.Remove(cashierStationPresentation.CashierStation);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void NewCashierStation()
        {
            var cashierStation= ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.NewCashierStation();
            CashierStations.Add(cashierStation, new CashierStationPresentation(this, cashierStation));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        public RelayCommand NewCashierCommand { get; protected set; }

        public readonly InfrastructureTreeNode ServiceContextInfrastructure;


        List<MenuCommand> _ContextMenuItems;
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/cashier16.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    //MenuComamnd menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    //menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = RenameCommand;
                    //_ContextMenuItems.Add(menuItem);


                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/ServiceArea.png"));
                    //menuItem.Header = Properties.Resources.AddServiceAreaHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddServiceAreaCommand;

                    //_ContextMenuItems.Add(menuItem);

                    MenuCommand menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/cashier16.png"));
                    menuItem.Header = Properties.Resources.NewCashierStationPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewCashierCommand;
                    _ContextMenuItems.Add(menuItem);

                    //_ContextMenuItems.Add(null);

                    //menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    //menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = DeleteCommand;

                    //_ContextMenuItems.Add(menuItem);



                }

                return _ContextMenuItems;
            }
        }


        Dictionary<ICashierStation, CashierStationPresentation> CashierStations = new Dictionary<ICashierStation, CashierStationPresentation>();


        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members =this.CashierStations.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }

        public override string Name
        {
            get
            {
                return Properties.Resources.TreasuryTitle;
            }

            set
            {
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
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/accounting16.png"));
            }
        }



        public override void SelectionChange()
        {
        }
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
    }
}
