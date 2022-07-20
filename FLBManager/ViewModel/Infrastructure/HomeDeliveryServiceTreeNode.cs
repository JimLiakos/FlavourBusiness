using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.Infrastructure
{
    public class HomeDeliveryServiceTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public HomeDeliveryServiceTreeNode(InfrastructureTreeNode infrastructureTreeNode, IHomeDeliveryServicePoint homeDeliveryService) : base(infrastructureTreeNode)
        {
            HomeDeliveryService = homeDeliveryService;
            InfrastructureTreeNode = infrastructureTreeNode;
            //CallerIDServer.ObjectStateChanged += CallerIDServer_ObjectStateChanged;
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
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
        public InfrastructureTreeNode InfrastructureTreeNode { get; }

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
