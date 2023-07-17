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
        /// <MetaDataID>{92c785b8-30fc-4a87-a4b8-0759176acbaf}</MetaDataID>
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{289f3a66-6424-4d0d-83ff-cea51c458eae}</MetaDataID>
        internal ICallerIDServer CallerIDServer;
        /// <MetaDataID>{64c9c6e5-be58-4abc-a5d3-4890ceee67df}</MetaDataID>
        InfrastructureTreeNode InfrastructureTreeNode;

        /// <MetaDataID>{da229753-ba51-4e36-bf29-cd2524b546f6}</MetaDataID>
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

        /// <MetaDataID>{479cb93d-0821-4cfc-ab27-847291aa3211}</MetaDataID>
        private void CallerIDServer_ObjectStateChanged(object _object, string member)
        {

        }

        /// <MetaDataID>{0c38336b-9d88-4ae6-9e57-31594a2c1bde}</MetaDataID>
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

        /// <MetaDataID>{5fd31cc1-b8b8-41bb-bae7-109e3965f8d2}</MetaDataID>
        private void AddCallerIDLine()
        {
            ICallerIDLine callerIDLine = CallerIDServer.NewCallerIDLine();

            _Members.Add(new CallerIDLineTreeNode(this, callerIDLine));

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{8d411d03-b62c-4bec-b756-a06fe945411a}</MetaDataID>
        private void Delete()
        {
            InfrastructureTreeNode.RemoveCallerIDServer();
        }
        /// <MetaDataID>{13baf424-d5a8-46ec-bfa9-32e17b5d1907}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }



        /// <MetaDataID>{4da7cd64-bf6f-4cc5-a672-ae00496e10d4}</MetaDataID>
        public RelayCommand AddCallerIDLineCommand { get; protected set; }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{e2bc0bfb-cdc3-4937-bdbc-cd61d6138b06}</MetaDataID>
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

        /// <MetaDataID>{ccaf8709-87f3-42f1-a5d3-e3fd3633ad47}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        /// <MetaDataID>{d8e5f3be-ca4d-4ef7-89ac-343cd5d4df0f}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members.ToList();
            }
        }

        /// <MetaDataID>{f08b03b3-998d-40c6-a93e-1e684fb0da9c}</MetaDataID>
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

        /// <MetaDataID>{9e3f475c-943e-405d-870f-45ee481c29d3}</MetaDataID>
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

        /// <MetaDataID>{f58e8c68-5f7d-4410-9248-52c7dc454af9}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/CallerID16.png"));
            }
        }

        /// <MetaDataID>{85b597bb-ebb2-4219-8493-2b347fa5100f}</MetaDataID>
        public override void SelectionChange()
        {

        }
    }
}
