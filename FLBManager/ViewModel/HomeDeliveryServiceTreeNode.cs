using FlavourBusinessFacade;
using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel.Delivery;
using OOAdvantech.Transactions;
using StyleableWindow;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{f1d7153e-8c05-4420-b64f-f6a874f26d50}</MetaDataID>
    public class HomeDeliveryServiceTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public HomeDeliveryServiceTreeNode(FlavoursServicesContextPresentation servicesContextPresentation, IHomeDeliveryServicePoint homeDeliveryService) : base(servicesContextPresentation)
        {
            HomeDeliveryService = homeDeliveryService;
            ServicesContextPresentation = servicesContextPresentation;


            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            SettingsCommand= DeleteCommand = new RelayCommand((object sender) =>
            {
                OpenHomeDeliverySettings();
            });



            NewTakeAwaySationCommand = new RelayCommand((object sender) =>
            {
                NewTakeAwaySation();
            });

            try
            {
                foreach (var preparationStation in servicesContextPresentation.ServiceContextResources.DeliveryCallCenterStations)
                    DeliveryCallCenterStations.Add(preparationStation, new DeliveryCallCenterStationPresentation(this, preparationStation));

            }
            catch (System.Exception error)
            {
            }
        }
        Dictionary<IHomeDeliveryCallCenterStation, DeliveryCallCenterStationPresentation> DeliveryCallCenterStations = new Dictionary<IHomeDeliveryCallCenterStation, DeliveryCallCenterStationPresentation>();

        private void NewTakeAwaySation()
        {
            //var menuViewModel = ServiceContextInfrastructure.ServicesContextPresentation.Company.RestaurantMenus.Members[0] as MenuViewModel;

            var DeliveryCallCenterStation = ServicesContextPresentation.ServicesContext.NewCallCenterStation();
            var preparationStationPresentation = new DeliveryCallCenterStationPresentation(this, DeliveryCallCenterStation);
            preparationStationPresentation.Edit = true;
            DeliveryCallCenterStations.Add(DeliveryCallCenterStation, preparationStationPresentation);


            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            IsNodeExpanded = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));

        }

        internal void RemoveDeliveryCallCenterStation(DeliveryCallCenterStationPresentation DeliveryCallCenterStationTreeNode)
        {
            this.ServicesContextPresentation.ServicesContext.RemoveCallCenterStation(DeliveryCallCenterStationTreeNode.HomeDeliveryCallCenterStation);
            DeliveryCallCenterStations.Remove(DeliveryCallCenterStationTreeNode.HomeDeliveryCallCenterStation);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        public RelayCommand NewTakeAwaySationCommand { get; protected set; }

        private void Delete()
        {
            throw new NotImplementedException();
        }

        public override string Name
        {
            get
            {
                return Properties.Resources.HomeDeliveryTreeNodeName;
            }
            set
            {
            }
        }
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/delivery-bike16.png"));
            }
        }

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = this.DeliveryCallCenterStations.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }
        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand LaunchHomeDeliveryCommand { get; protected set; }
        public RelayCommand SettingsCommand { get; protected set; }

        private void OpenHomeDeliverySettings()
        {
            System.Windows.Window win = System.Windows.Window.GetWindow(SettingsCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                Views.HomeDeliveryServicePage homeDeliveryServicePage = new Views.HomeDeliveryServicePage();
                homeDeliveryServicePage.GetObjectContext().SetContextInstance(new HomeDeliveryServicePresentation(HomeDeliveryService, ServicesContextPresentation.ServicesContext));

                frame.ShowDialogPage(homeDeliveryServicePage);
                stateTransition.Consistent = true;
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        public override bool HasContextMenu => true;
        /// <exclude>Excluded</exclude>
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
                    MenuCommand menuItem = new MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/pos-terminal16.png"));
                    menuItem.Header = Properties.Resources.NewDeliveryCallCenterStationPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewTakeAwaySationCommand;
                    _ContextMenuItems.Add(menuItem);


                    menuItem = new MenuCommand(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerIDLine16.png"));
                    //menuItem.Header = Properties.Resources.AddCallerIDLine;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddCallerIDLineCommand;
                    //_ContextMenuItems.Add(menuItem);

                    //_ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/settings16.png"));
                    menuItem.Header = Properties.Resources.SettingsMenuPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = SettingsCommand;
                    _ContextMenuItems.Add(menuItem);

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

        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;
        public IHomeDeliveryServicePoint HomeDeliveryService { get; }

        internal FlavoursServicesContextPresentation ServicesContextPresentation;


        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        public override void SelectionChange()
        {
            throw new NotImplementedException();
        }
    }
}
