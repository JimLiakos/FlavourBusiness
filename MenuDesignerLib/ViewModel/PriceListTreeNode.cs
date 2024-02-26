using FlavourBusinessFacade;
using FlavourBusinessFacade.PriceList;
using FlavourBusinessToolKit;
using FLBManager.ViewModel;
using OOAdvantech.MetaDataRepository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel
{
    public class PriceListTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {

        public PriceListTreeNode(FBResourceTreeNode parent, OrganizationStorageRef priceListStorageRef) : base(parent)
        {
            PriceListStorageRef = priceListStorageRef;

            //string localFileName = $"{System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData)}\\Microneme\\DontWaitWater\\{priceListStorageRef.Name}.xml";
            //var rawStorageData = new RawStorageData(localFileName, priceListStorageRef, priceListStorageRef.UploadService);

            //var priceListObjectStorage = OOAdvantech.PersistenceLayer.ObjectStorage.OpenStorage("PriceList", rawStorageData, "OOAdvantech.MetaDataLoadingSystem.MetaDataStorageProvider");
            //OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(priceListObjectStorage);

            //var priceList = (from m_priceList in storage.GetObjectCollection<IPriceList>()
            //                 select m_priceList).FirstOrDefault();
        }
        bool Editable=true;

        OrganizationStorageRef PriceListStorageRef = null;

        public override string Name
        {
            get => PriceListStorageRef.Name;
            set
            {
                if (PriceListStorageRef.Name != value)
                {
                    string oldName = PriceListStorageRef.Name;
                    if (!string.IsNullOrWhiteSpace(value) && System.IO.Path.GetInvalidFileNameChars().Where(x => value.IndexOf(x) != -1).Count() > 0)
                    {
                        var messageBoxResult = StyleableWindow.WpfMessageBox.Show("Graphic menu name", "The graphic name has invalid characters !", MessageBoxButton.OK, StyleableWindow.MessageBoxImage.Error);



                        Task.Run(() =>
                        {
                            SetOtherPriceListTreeNodesName(oldName);
                            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                            
                        });
                        return;
                    }
                    PriceListStorageRef.Name = value;
                    SetOtherPriceListTreeNodesName(value);
                    if (Editable)
                    {
                        OrganizationStorageRef storageRef = null;
                        try
                        {
                            storageRef = (FlavourBusinessManager.Organization.CurrentOrganization as FlavourBusinessFacade.IResourceManager).UpdateStorage(PriceListStorageRef.Name, PriceListStorageRef.Description, PriceListStorageRef.StorageIdentity);
                        }
                        catch (Exception error)
                        {
                            Task.Run(() =>
                            {
                                PriceListStorageRef.Name = oldName;
                                SetOtherPriceListTreeNodesName(oldName);
                                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                            });
                            return;
                        }

                        PriceListStorageRef.StorageUrl = storageRef.StorageUrl;
                        PriceListStorageRef.Name = storageRef.Name;
                        PriceListStorageRef.TimeStamp = storageRef.TimeStamp;
                        PriceListStorageRef.StorageIdentity = storageRef.StorageIdentity;
                        PriceListStorageRef.Name = storageRef.Name;
                    }
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));

                }

            }
        }
        private void SetOtherPriceListTreeNodesName(string name)
        {
            List<FBResourceTreeNode> priceListStorageReferences = null;
            if ((HeaderNode).FBResourceTreeNodesDictionary.TryGetValue(PriceListStorageRef.StorageIdentity, out priceListStorageReferences))
            {
                foreach (PriceListTreeNode graphicMenuTreeNode in priceListStorageReferences)
                {
                    if (graphicMenuTreeNode != this)
                        graphicMenuTreeNode.Name = name;
                }
            }
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/price-list16.png"));
            }
        }


        public override List<FBResourceTreeNode> Members => new List<FBResourceTreeNode>();


        public RelayCommand RenameCommand { get; protected set; }

        public RelayCommand DeleteCommand { get; protected set; }
        public RelayCommand EditCommand { get; protected set; }



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



                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;

                    _ContextMenuItems.Add(menuItem);

               

                }

                return _ContextMenuItems;

            }
        }


        public override List<MenuCommand> SelectedItemContextMenuItems => throw new System.NotImplementedException();

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            Parent.RemoveChild(treeNode);
        }

        public override void SelectionChange()
        {
        }


    }
}