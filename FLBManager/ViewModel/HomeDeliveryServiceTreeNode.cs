using FlavourBusinessFacade.ServicesContextResources;
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
            //CallerIDServer.ObjectStateChanged += CallerIDServer_ObjectStateChanged;
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            SettingsCommand= DeleteCommand = new RelayCommand((object sender) =>
            {
                OpenHomeDeliverySettings();
            });
        }

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

        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members.ToList();
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
                homeDeliveryServicePage.GetObjectContext().SetContextInstance(new HomeDeliveryServicePresentation(this.HomeDeliveryService));

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

        FlavoursServicesContextPresentation ServicesContextPresentation;


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
