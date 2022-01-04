using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
using FlavourBusinessFacade;
using FlavourBusinessToolKit;
using FLBManager.ViewModel;
using MenuDesigner.ViewModel.Menu;
using MenuDesigner.ViewModel.MenuCanvas;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel
{
    /// <MetaDataID>{053cbc90-bd7f-4bd7-af0e-104817d2b971}</MetaDataID>
    public class GraphicMenuTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        public event EventHandler Deleted;
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly OrganizationStorageRef GraphicMenuStorageRef;
        public readonly OrganizationStorageRef MenuItemsStorageRef;
        readonly IGraphicMenusOwner GraphicMenusOwner;
        bool Editable;
        bool PublishAllowed;
        public GraphicMenuTreeNode(OrganizationStorageRef graphicMenuStorageRef, OrganizationStorageRef menuItemsStorageRef, FBResourceTreeNode parent, IGraphicMenusOwner owner, bool editable, bool publishAllowed = false) : base(parent)
        {
            GraphicMenusOwner = owner;
            Editable = editable;
            PublishAllowed = publishAllowed;
            GraphicMenuStorageRef = graphicMenuStorageRef;
            if (graphicMenuStorageRef != null && graphicMenuStorageRef.UploadService == null)
            {

            }
            MenuItemsStorageRef = menuItemsStorageRef;
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            PublishCommand = new RelayCommand((object sender) =>
            {
                Publish();
            });
            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();

                //Delete();
            });


            DesigneCommand = new RelayCommand(async (object sender) =>
           {
               await SowOnGrpaphicMenuDesigner();

           });

            List<FBResourceTreeNode> graphicStorageReferences = null;
            if (!HeaderNode.FBResourceTreeNodesDictionary.TryGetValue(GraphicMenuStorageRef.StorageIdentity, out graphicStorageReferences))
            {
                graphicStorageReferences = new List<FBResourceTreeNode>();
                graphicStorageReferences.Add(this);
                HeaderNode.FBResourceTreeNodesDictionary[GraphicMenuStorageRef.StorageIdentity] = graphicStorageReferences;
            }
            else
            {
                graphicStorageReferences.Add(this);
            }

        }

        private async void Publish()
        {

            if (MenuDesignerHost.Current != null)
                await MenuDesignerHost.Publish(GraphicMenuStorageRef);

        }

        private async Task SowOnGrpaphicMenuDesigner()
        {
            if (MenuDesignerHost.Current != null)
            {
                if ((HeaderNode).FBResourceTreeNodesDictionary.TryGetValue(GraphicMenuStorageRef.StorageIdentity, out List<FBResourceTreeNode> graphicStorageReferences))
                {

                    foreach (GraphicMenuTreeNode graphicMenuTreeNode in graphicStorageReferences)
                    {
                        if (GraphicMenuStorageRef.UploadService == null && graphicMenuTreeNode.GraphicMenuStorageRef.UploadService != null)
                        {
                            GraphicMenuStorageRef.UploadService = graphicMenuTreeNode.GraphicMenuStorageRef.UploadService;
                            GraphicMenuStorageRef.Version = graphicMenuTreeNode.GraphicMenuStorageRef.Version;
                        }
                    }
                }

                await MenuDesignerHost.OpenGrephicMenu(GraphicMenuStorageRef, MenuItemsStorageRef);
            }
        }



        public override bool IsEditable
        {
            get
            {
                if (Editable)//(this.Parent.Parent is CompanyPresentation)
                    return true;
                return false;

            }
        }
        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Edit)));
        }


        private void Delete()
        {
            this.GraphicMenusOwner.RemoveGraphicMenu(this);


            //List<FBResourceTreeNode> graphicStorageReferences = null;
            //if (HeaderNode.FBResourceTreeNodesDictionary.TryGetValue(GraphicMenuStorageRef.StorageIdentity, out graphicStorageReferences))
            //    graphicStorageReferences.Remove(this);
        }
        public RelayCommand RenameCommand { get; protected set; }

        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand PublishCommand { get; protected set; }

        public RelayCommand DesigneCommand { get; protected set; }

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

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;
                    _ContextMenuItems.Add(menuItem);


                    //_ContextMenuItems.Add(null);



                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/sketch16.png"));
                    menuItem.Header = MenuDesigner.Properties.Resources.DesignGraphicMenuContextHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DesigneCommand;
                    _ContextMenuItems.Add(menuItem);

                    if (PublishAllowed)
                    {
                        _ContextMenuItems.Add(null);
                        menuItem = new MenuCommand();
                        imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/cloud-computing16.png"));
                        menuItem.Header = MenuDesigner.Properties.Resources.PublishMenuContextMenuHeader;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = PublishCommand;
                        _ContextMenuItems.Add(menuItem);
                    }

                    _ContextMenuItems.Add(null);
                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = MenuDesigner.Properties.Resources.RemoveGraphicMenuContextHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;
                    _ContextMenuItems.Add(menuItem);

                    //if (!(this.Parent.Parent is CompanyPresentation))
                    //    _ContextMenuItems.Add(menuItem);

                }

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




        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return new List<FBResourceTreeNode>();
            }
        }

        public override string Name
        {
            get
            {
                return GraphicMenuStorageRef.Name;
            }

            set
            {
                if (GraphicMenuStorageRef.Name != value)
                {
                    string oldName = GraphicMenuStorageRef.Name;
                    if (!string.IsNullOrWhiteSpace(value) && System.IO.Path.GetInvalidFileNameChars().Where(x => value.IndexOf(x) != -1).Count() > 0)
                    {
                        var messageBoxResult = StyleableWindow.WpfMessageBox.Show("Graphic menu name", "The graphic name has invalid characters !", MessageBoxButton.OK, StyleableWindow.MessageBoxImage.Error);



                        Task.Run(() =>
                        {
                            SetOtherGraphicTreeNodesName(oldName);
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                        });
                        return;
                    }
                    GraphicMenuStorageRef.Name = value;
                    SetOtherGraphicTreeNodesName(value);
                    if (Editable)
                    {
                        OrganizationStorageRef storageRef = null;
                        try
                        {
                            storageRef = (FlavourBusinessManager.Organization.CurrentOrganization as FlavourBusinessFacade.IResourceManager).UpdateStorage(GraphicMenuStorageRef.Name, GraphicMenuStorageRef.Description, GraphicMenuStorageRef.StorageIdentity);
                        }
                        catch (Exception error)
                        {
                            Task.Run(() =>
                            {
                                GraphicMenuStorageRef.Name = oldName;
                                SetOtherGraphicTreeNodesName(oldName);
                                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                            });
                            return;
                        }

                        GraphicMenuStorageRef.StorageUrl = storageRef.StorageUrl;
                        GraphicMenuStorageRef.Name = storageRef.Name;
                        GraphicMenuStorageRef.TimeStamp = storageRef.TimeStamp;
                        GraphicMenuStorageRef.StorageIdentity = storageRef.StorageIdentity;
                        GraphicMenuStorageRef.Name = storageRef.Name;
                    }
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));

                }
            }
        }

        private void SetOtherGraphicTreeNodesName(string name)
        {


            List<FBResourceTreeNode> graphicStorageReferences = null;
            if ((HeaderNode).FBResourceTreeNodesDictionary.TryGetValue(GraphicMenuStorageRef.StorageIdentity, out graphicStorageReferences))
            {
                foreach (GraphicMenuTreeNode graphicMenuTreeNode in graphicStorageReferences)
                {
                    if (graphicMenuTreeNode != this)
                        graphicMenuTreeNode.Name = name;
                }
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
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/Menu.png"));
            }
        }

        public override void SelectionChange()
        {
        }
    }
}
