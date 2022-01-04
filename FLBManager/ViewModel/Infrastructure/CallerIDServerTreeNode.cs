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
    /// <MetaDataID>{3e327e5b-4198-4a33-880c-27c33a110ec3}</MetaDataID>
    public class CallerIDServerTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        internal ICallerIDServer CallerIDServer;
        InfrastructureTreeNode InfrastructureTreeNode;

        public CallerIDServerTreeNode(InfrastructureTreeNode infrastructureTreeNode, ICallerIDServer callerIDServer) : base(infrastructureTreeNode)
        {

            CallerIDServer = callerIDServer;
            InfrastructureTreeNode = infrastructureTreeNode;

            CallerIDServer.ObjectStateChanged += CallerIDServer_ObjectStateChanged;

            foreach (var callerIDLine in CallerIDServer.Lines)
            {
                CallerIDLineTreeNode callerIDLineTreeNode = new CallerIDLineTreeNode(this, callerIDLine);
                _Members.Add(callerIDLineTreeNode);
            }

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
            AddCallerIDLineCommand = new RelayCommand((object sender) =>
            {
                AddCallerIDLine();
            });
            //ServicesContext = servicesContext;



        }

        private void CallerIDServer_ObjectStateChanged(object _object, string member)
        {
            
        }

        internal void RemoveCallerIDLine(CallerIDLineTreeNode callerIDLineTreeNode)
        {

            try
            {
                this.CallerIDServer.RemoveCallerIDLine(callerIDLineTreeNode.CallerIDLine);
                _Members.Remove(callerIDLineTreeNode);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

            }
            catch (Exception error)
            {

                
            }

        }

        private void AddCallerIDLine()
        {
            ICallerIDLine callerIDLine = CallerIDServer.NewCallerIDLine();

            _Members.Add(new CallerIDLineTreeNode(this, callerIDLine));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void Delete()
        {
            InfrastructureTreeNode.RemoveCallerIDServer();
        }
        public RelayCommand DeleteCommand { get; protected set; }



        public RelayCommand AddCallerIDLineCommand { get; protected set; }

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
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerIDLine16.png"));
                    menuItem.Header = Properties.Resources.AddCallerIDLine;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = AddCallerIDLineCommand;
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

        public override bool HasContextMenu
        {
            get
            {
                return true;
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

        public override string Name
        {
            get
            {
                return "Caller ID Server";
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
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerID16.png"));
            }
        }

        public override void SelectionChange()
        {
            
        }
    }
}
