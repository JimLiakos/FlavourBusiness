using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CashierStationUI.ViewModel;
using FlavourBusinessFacade.ServicesContextResources;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    /// <MetaDataID>{ad3b8d96-a759-4d5b-b969-cae505b4949f}</MetaDataID>
    public class TreasuryTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        /// <MetaDataID>{1bdd8662-778b-44c2-93d9-8ca5120415d6}</MetaDataID>
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
                        CashierStations.Add(cashierStation, new CashierStationPresentation(this, cashierStation, this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext));

                    //foreach (var cashierStation in ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.CashierStations)
                    //    CashierStations.Add(cashierStation, new CashierStationPresentation( this, cashierStation));
                }
                catch (System.Exception error)
                {
                }
            });
        }


        /// <MetaDataID>{51198457-43f8-41d2-85d5-6965050d000e}</MetaDataID>
        internal void RemoveCashierStation(CashierStationPresentation cashierStationPresentation)
        {
            this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.RemoveCashierStation(cashierStationPresentation.CashierStation);
            CashierStations.Remove(cashierStationPresentation.CashierStation);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }
        /// <MetaDataID>{a6a82b8b-ca03-434b-aa75-7d0a40cf5912}</MetaDataID>
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            if (treeNode is CashierStationPresentation)
                RemoveCashierStation(treeNode as CashierStationPresentation);


        }

        /// <MetaDataID>{c854e482-273d-4b0f-b2d6-5800320c7ebb}</MetaDataID>
        private void NewCashierStation()
        {

            var cashierStation = ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext.NewCashierStation();
            CashierStations.Add(cashierStation, new CashierStationPresentation(this, cashierStation, this.ServiceContextInfrastructure.ServicesContextPresentation.ServicesContext));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{037e3606-37e6-483b-92e5-3d74ad3c0b9e}</MetaDataID>
        public RelayCommand NewCashierCommand { get; protected set; }

        /// <MetaDataID>{9c8d2733-b5f7-4ff7-9042-5f9b5c5250ae}</MetaDataID>
        public readonly InfrastructureTreeNode ServiceContextInfrastructure;


        /// <MetaDataID>{0e92ad0c-1a54-4aae-a36d-42aaa4e1a3ef}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{9917dcaa-d198-4c5b-9af7-8f7ae66ac5b0}</MetaDataID>
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


        /// <MetaDataID>{b1fc78fa-0b6b-4d39-8936-002779043da5}</MetaDataID>
        Dictionary<ICashierStation, CashierStationPresentation> CashierStations = new Dictionary<ICashierStation, CashierStationPresentation>();


        /// <MetaDataID>{6d3e6876-5750-4a40-9e5f-cd57b44f4d9b}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = this.CashierStations.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }

        /// <MetaDataID>{d995a4ee-b483-4b93-b00a-fd2157d54247}</MetaDataID>
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

        /// <MetaDataID>{68da3c22-7ca0-4fe0-8ab3-2cf8fb4c2e80}</MetaDataID>
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

        /// <MetaDataID>{58535afa-e9d7-456d-b057-af1fbb628df4}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/accounting16.png"));
            }
        }



        /// <MetaDataID>{1ceb2c01-a519-4095-8814-6a75178c70ff}</MetaDataID>
        public override void SelectionChange()
        {
        }
        /// <MetaDataID>{9b2617be-56c8-44c2-91f0-6c053678089c}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
    }
}
