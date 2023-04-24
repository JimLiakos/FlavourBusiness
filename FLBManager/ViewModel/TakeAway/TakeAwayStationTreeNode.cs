using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using WPFUIElementObjectBind;
using MenuItemsEditor.ViewModel;
using System.Windows;
using MenuDesigner.ViewModel;
using FinanceFacade;
using OOAdvantech.Transactions;
using StyleableWindow;

namespace FLBManager.ViewModel.TakeAway
{
    public class TakeAwayStationPresentation : FBResourceTreeNode, IGraphicMenusOwner, INotifyPropertyChanged, IDragDropTarget
    {

        /// <MetaDataID>{257c2b84-7663-4a88-a3e8-a55718e929d3}</MetaDataID>
        public ITakeAwayStation TakeAwayStation;

        /// <MetaDataID>{c01c2553-7f53-43f7-936d-2c3c8097abc7}</MetaDataID>
        public string TakeAwayStationIdentity
        {
            get
            {
                return TakeAwayStation.TakeAwayStationIdentity;
            }
            set
            {
            }
        }


        /// <MetaDataID>{6c648c6f-6213-44ab-aa7f-5a7359aa67ea}</MetaDataID>
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{dc68144e-838f-4acb-86ef-5b873c8733da}</MetaDataID>
        internal TakeAwayStationsTreeNode TakeAwayStationsTreeNode;

        /// <MetaDataID>{95fbc99b-2b06-40de-b5f8-3931f2e4e983}</MetaDataID>
        public TakeAwayStationPresentation(TakeAwayStationsTreeNode parent, ITakeAwayStation takeAwayStation) : base(parent)
        {




            TakeAwayStationsTreeNode = parent;
            this.TakeAwayStation = takeAwayStation;

            _Description=takeAwayStation.Description;

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });

            BeforeTransactionCommitCommand= new RelayCommand((object sender) =>
            {
                takeAwayStation.Description=_Description;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
            });

            EditCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
        {
            System.Windows.Window win = System.Windows.Window.GetWindow(EditCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            Edit(win);
        });

            AssignCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(RenameCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                var QRCodePopup = new Views.HumanResources.NewUserQRCodePopup("TakeAway Station", "Scan to register as TakeAway station") { CodeValue = this.TakeAwayStationIdentity };
                QRCodePopup.Owner = win;
                QRCodePopup.ShowDialog();

            });


            string graphicMenuStorageIdentity = takeAwayStation?.GraphicMenuStorageIdentity;
            if (!string.IsNullOrWhiteSpace(graphicMenuStorageIdentity))
            {
                var graphicMenuTreeNode = parent.ServiceContextInfrastructure.ServicesContextPresentation.Company.GraphicMenus.Where(x => x.GraphicMenuStorageRef.StorageIdentity==takeAwayStation.GraphicMenuStorageIdentity).FirstOrDefault();
                GraphicMenuTreeNode=    new GraphicMenuTreeNode(graphicMenuTreeNode.GraphicMenuStorageRef?.Clone(), graphicMenuTreeNode.MenuItemsStorageRef?.Clone(), this, this, false);
            }
        }



        /// <MetaDataID>{200a398f-8114-411b-a98e-0ebd3dc2a19b}</MetaDataID>
        public TakeAwayStationPresentation() : base(null)
        {

        }


        /// <MetaDataID>{28c9231b-65d7-4b05-8c1f-00b2803fd66b}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand BeforeTransactionCommitCommand { get; set; }





        /// <MetaDataID>{17e67b39-cb6c-4904-a27a-afaa6054ad9d}</MetaDataID>
        private void Edit(System.Windows.Window win)
        {
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Suppress))
            {

                var frame = PageDialogFrame.LoadedPageDialogFrames.FirstOrDefault();// WPFUIElementObjectBind.ObjectContext.FindChilds<PageDialogFrame>(win).Where(x => x.Name == "PageDialogHost").FirstOrDefault();
                Views.TakeAway.TakeAwayStationPage takeAwayStationItemsPage = new Views.TakeAway.TakeAwayStationPage();
                takeAwayStationItemsPage.GetObjectContext().SetContextInstance(this);

                frame.ShowDialogPage(takeAwayStationItemsPage);
                stateTransition.Consistent = true;
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }

        /// <MetaDataID>{71c2f750-95ce-41d4-87fc-67f6508e4d09}</MetaDataID>
        public void EditMode()
        {
            if (_Edit == true)
            {
                _Edit = !_Edit;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
            }
            _Edit = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Edit)));
        }

        /// <MetaDataID>{5f12c711-6a47-4014-bb5c-3fa8e2d0921a}</MetaDataID>
        private void Delete()
        {
            TakeAwayStationsTreeNode.RemoveTakeAwayStation(this);

            // (Company as CompanyPresentation).RemoveServicesContext(this);
        }

        /// <MetaDataID>{fa69d679-4c18-4a0b-a43b-de037ded305c}</MetaDataID>
        public RelayCommand RenameCommand { get; protected set; }
        /// <MetaDataID>{2edaf9c1-1d7a-4307-8bf5-0c7e8a107b45}</MetaDataID>
        public RelayCommand DeleteCommand { get; protected set; }
        /// <MetaDataID>{230c3103-e728-44fe-8843-b89e94b1b1a9}</MetaDataID>
        public RelayCommand EditCommand { get; protected set; }
        /// <MetaDataID>{730be9b9-4dff-456c-b5f3-a6e932daf943}</MetaDataID>
        public RelayCommand AssignCommand { get; protected set; }


        /// <MetaDataID>{25c0c6ed-772b-41df-9856-fd3894a84fd0}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{5bec9819-de6e-4418-9535-3016bf4f934c}</MetaDataID>
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

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Edit16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.EditObject;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = EditCommand;
                    _ContextMenuItems.Add(menuItem);

                    _ContextMenuItems.Add(null);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/Key16.png"));
                    menuItem.Header = Properties.Resources.AssignPreparationDevicePrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = AssignCommand;

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

        /// <MetaDataID>{4bc64657-897d-4d82-9dd8-e8aeef44cdde}</MetaDataID>
        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        /// <MetaDataID>{9a828883-53c2-4cf2-bc4c-cb1297371379}</MetaDataID>
        private DateTime DragEnterStartTime;

        /// <MetaDataID>{d75fc1d1-181d-4d02-9a5a-f5936bb2fa49}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var member = _Members.ToList();
                if (GraphicMenuTreeNode!=null)
                    member.Add(GraphicMenuTreeNode);
                return member;
            }
        }

        /// <MetaDataID>{554b3a7a-16b2-4d1d-8ca6-21de8b14f90c}</MetaDataID>
        string _Description;
        /// <MetaDataID>{13c829ae-37b5-4794-b7a5-b6516a9b10c6}</MetaDataID>
        public override string Name
        {
            get
            {
                return _Description;
            }

            set
            {
                TakeAwayStation.Description = value;
                _Description=value;
                Task.Run(() =>
                {
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                });
            }
        }

        //public string Description
        //{
        //    get
        //    {
        //        return _Description;
        //    }

        //    set
        //    {
        //        TakeAwayStation.Description = value;
        //        _Description=value;
        //        Task.Run(() =>
        //        {
        //            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        //        });


        //    }
        //}
        /// <MetaDataID>{3e30122d-dc05-4e2f-bab8-1421b5e1fdbc}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }
        /// <MetaDataID>{427dd535-c0bd-4357-9eb2-74ae479565e2}</MetaDataID>
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{980f7f31-39c5-4aad-b7bd-ef02393258f9}</MetaDataID>
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


        /// <MetaDataID>{6b6e22fc-b8ff-4235-beb0-da9bdb369563}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/pos-terminal16.png"));
            }
        }

        /// <MetaDataID>{92ff6c2c-34b6-4e50-b205-7110fa3b3c83}</MetaDataID>
        public List<GraphicMenuTreeNode> GraphicMenus => throw new NotImplementedException();

        /// <MetaDataID>{e918d8f8-b935-44f7-ab1c-7a452c7de778}</MetaDataID>
        public bool NewGraphicMenuAllowed => throw new NotImplementedException();

        /// <MetaDataID>{f96cfeb6-9d68-4e4f-aabb-1dd66692a7ec}</MetaDataID>
        public GraphicMenuTreeNode GraphicMenuTreeNode { get; private set; }

        /// <MetaDataID>{ee365912-3ed4-4bfa-89b3-06d62c3c683d}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{81487d1e-6fe8-4ade-b326-9e2bd621b437}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {
            DragEnterStartTime = DateTime.Now;

            MenuDesigner.ViewModel.GraphicMenuTreeNode graphicMenu = e.Data.GetData(typeof(MenuDesigner.ViewModel.GraphicMenuTreeNode)) as MenuDesigner.ViewModel.GraphicMenuTreeNode;
            if (graphicMenu != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{7685fc3f-422b-4f0f-bbc2-ce73147fc26b}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{50554c50-c0c4-4324-bc16-4e2494ee9e78}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {

            MenuDesigner.ViewModel.GraphicMenuTreeNode graphicMenu = e.Data.GetData(typeof(MenuDesigner.ViewModel.GraphicMenuTreeNode)) as MenuDesigner.ViewModel.GraphicMenuTreeNode;
            if (graphicMenu != null)
            {
                if ((DateTime.Now - DragEnterStartTime).TotalSeconds > 2)
                {
                    IsNodeExpanded = true;
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                }
            }
            else
                e.Effects = DragDropEffects.None;





            System.Diagnostics.Debug.WriteLine("DragOver InfrastructureTreeNode");
        }

        /// <MetaDataID>{6d4862ac-5434-4fc0-8e58-f8ec7710fec4}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
            MenuDesigner.ViewModel.GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(MenuDesigner.ViewModel.GraphicMenuTreeNode)) as MenuDesigner.ViewModel.GraphicMenuTreeNode;
            if (graphicMenuTreeNode  != null)
            {
                this.TakeAwayStation.GraphicMenuStorageIdentity=graphicMenuTreeNode.GraphicMenuStorageRef.StorageIdentity;
                GraphicMenuTreeNode=    new GraphicMenuTreeNode(graphicMenuTreeNode.GraphicMenuStorageRef?.Clone(), graphicMenuTreeNode.MenuItemsStorageRef?.Clone(), this, this, false);
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            }
        }

        /// <MetaDataID>{e2034f6d-373b-4368-9de3-dbd183b1bb45}</MetaDataID>
        public void AssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            //throw new NotImplementedException();
        }

        /// <MetaDataID>{d0f9e550-ba85-41cc-a1d2-b783844dd5b2}</MetaDataID>
        public bool CanAssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            return false;
        }

        /// <MetaDataID>{3ab8ea82-d8f9-4f00-b5fe-cc39cb88ea65}</MetaDataID>
        public bool RemoveGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            this.TakeAwayStation.GraphicMenuStorageIdentity=null;
            GraphicMenuTreeNode=null;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            return true;
        }

        /// <MetaDataID>{fb89e86d-1647-41d2-bcf9-602d5b5326b5}</MetaDataID>
        public void NewGraphicMenu()
        {

        }
    }

}
