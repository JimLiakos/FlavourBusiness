using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade;
using FLBManager.ViewModel.HumanResources;
using MenuDesigner.ViewModel;
using MenuItemsEditor;
using OOAdvantech.Transactions;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel
{
    /// <MetaDataID>{7151c406-5113-431d-b326-a5f9e0955550}</MetaDataID>
    public class CompanyPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget, IGraphicMenusOwner
    {

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        public event EventHandler CompanySignedOut;

        /// <MetaDataID>{a7039ad1-e004-4dda-928e-82c77fb62774}</MetaDataID>
        public override void SelectionChange()
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ContextMenuItems)));

        }

        /// <MetaDataID>{6d46f836-9225-4f1a-a695-575492992670}</MetaDataID>
        Dictionary<string, GraphicMenuTreeNode> _GraphicMenus = new Dictionary<string, GraphicMenuTreeNode>();

        /// <MetaDataID>{c0187fd8-7667-46e9-9f74-a79fea183e2b}</MetaDataID>
        public RelayCommand RenameCommand { get; protected set; }


        /// <MetaDataID>{df4c9a39-7352-486c-999f-e2be541b3404}</MetaDataID>
        public RelayCommand SettingsCommand { get; protected set; }

        /// <MetaDataID>{7c904c84-fbd4-4482-92ec-bcfe1e895cdc}</MetaDataID>
        public RelayCommand NewTranslatorCommand { get; protected set; }

        public RelayCommand NewGraphicMenuDesignerCommand { get; protected set; }

        /// <MetaDataID>{a5f92a96-8245-4faf-a659-5e756d63334b}</MetaDataID>
        public RelayCommand AddFlavoursServicePointCommand { get; protected set; }

        /// <MetaDataID>{f6ec0086-8f66-46d0-92de-afb717627b1e}</MetaDataID>
        internal IOrganization Organization;

        ///// <MetaDataID>{e05e9042-d206-45d5-b904-2d3a22d45ba8}</MetaDataID>
        //public List<OrganizationStorageRef> GraphicMenus;

        /// <MetaDataID>{6cadf1cf-ced0-4d89-881a-ea07838a2c8a}</MetaDataID>
        internal MenusTreeNode Menus;
        /// <MetaDataID>{43f87a1e-7177-4b30-b5ea-4ea07e256e8b}</MetaDataID>
        GraphicMenusPresentation GraphicMenusPresentation;
        /// <MetaDataID>{c92f6458-76fa-4542-8247-e225722ee875}</MetaDataID>
        public CompanyPresentation(IOrganization organization, FBResourceTreeNode parent, GraphicMenusPresentation graphicMenusPresentation, MenuItemsEditor.RestaurantMenus restaurantMenus) : base(parent)
        {
            IsNodeExpanded = true;
            GraphicMenusPresentation = graphicMenusPresentation;
            GraphicMenusPresentation.PropertyChanged += GraphicMenusPresentation_PropertyChanged;
            this.Organization = organization;

            var translators = Organization.GetTranslators(WorkerState.All);
            if (translators.Count > 0)
                Translators = new HumanResources.TranslatorsTreeNode(this, translators);


            var menuMakers = Organization.GetMenuMakers(WorkerState.All);
            if (menuMakers.Count > 0)
                MenuMakers = new HumanResources.MenuMakersTreeNode(this, menuMakers);

            Menus = new MenusTreeNode(Properties.Resources.GraphicMenusTitle, this, this);

            foreach (var graphicMenuViewModel in graphicMenusPresentation.GraphicMenus)
                _GraphicMenus[graphicMenuViewModel.StorageRef.StorageIdentity] = new GraphicMenuTreeNode(graphicMenuViewModel.StorageRef, null, Menus,this, true, true);


            RestaurantMenus = restaurantMenus;

            RenameCommand = new RelayCommand((object sender) =>
            {
                EditMode();
            });

            SettingsCommand = new RelayCommand((object sender) =>
            {

                var businessResources = App.Current.MainWindow.GetDataContextObject<FlavourBusinessManagerViewModel>().BusinessResources;
                Views.CompanySettingsWindow companySettings = new Views.CompanySettingsWindow();
                companySettings.SetObjectContextInstance<FlavourBusinessResourcesPresentation>(businessResources);

                //businessResources.SignOut

                System.Windows.Window win = App.Current.MainWindow;//System.Windows.Window.GetWindow(e.OriginalSource as System.Windows.DependencyObject);
                companySettings.Owner = win;
                companySettings.ShowDialog();

            });

            NewTranslatorCommand = new RelayCommand((object sender) =>
            {
                AddTranslator();
                //OpenWaiterApp();
            });

            NewGraphicMenuDesignerCommand = new RelayCommand((object sender) =>
            {
                AddGraphicDesigner();
                //OpenWaiterApp();
            });

            AddFlavoursServicePointCommand = new RelayCommand((object sender) =>
            {
                AddServicesContextPoint();
            });

            Task.Run(() =>
            {
                foreach (var ServicePoint in Organization.ServicesContexts)
                {
                    ServicesContexts.Add(ServicePoint, new FlavoursServicesContextPresentation(ServicePoint, this));
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
            });
        }



        public RestaurantMenus RestaurantMenus { get; internal set; }

        private void GraphicMenusPresentation_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            foreach (var graphicMenuViewModel in GraphicMenusPresentation.GraphicMenus)
            {
                if (!_GraphicMenus.ContainsKey(graphicMenuViewModel.StorageRef.StorageIdentity))
                {
                    var graphicMenuTreeNode = new GraphicMenuTreeNode(graphicMenuViewModel.StorageRef, null, Menus,this, true);
                    graphicMenuTreeNode.IsNodeExpanded = true;
                    Menus.IsNodeExpanded = true;
                    _GraphicMenus[graphicMenuViewModel.StorageRef.StorageIdentity] = graphicMenuTreeNode;
                }
            }
            Menus.Refresh();
        }

        private void OpenWaiterApp()
        {
            System.Windows.Window owner = System.Windows.Window.GetWindow(NewTranslatorCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);

            WaiterApp.WPF.MainWindow mainWindow = new WaiterApp.WPF.MainWindow();
            mainWindow.Owner = owner;
            mainWindow.ShowDialog();
        }

        internal void RemoveTranslatorsNode()
        {
            this.Translators = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        internal void RemoveMenuMakersNode()
        {
            MenuMakers = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        private void AddGraphicDesigner()
        {
            System.Windows.Window owner = System.Windows.Window.GetWindow(NewGraphicMenuDesignerCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                var menuItemWindow = new Views.HumanResources.MenuMakerWindow();
                //new MenuItemsEditor. Views.MenuItemWindow();
                menuItemWindow.Owner = owner;



                ViewModel.HumanResources.MenuMakerViewModel menuMakerViewModel = new MenuMakerViewModel(this);
                menuItemWindow.GetObjectContext().SetContextInstance(menuMakerViewModel);

                if (menuItemWindow.ShowDialog().Value)
                {
                    if (menuMakerViewModel.UserData != null && !string.IsNullOrWhiteSpace(menuMakerViewModel.UserData.Identity))
                    {
                        var menuMaker = Organization.AssignMenuMakerRoleToUser(menuMakerViewModel.UserData);
                        if (this.MenuMakers == null)
                        {
                            this.MenuMakers = new MenuMakersTreeNode(this, new List<FlavourBusinessFacade.HumanResources.IAccountability>());
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                        }
                        this.MenuMakers.AddMenuMaker(menuMaker);

                    }

                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{2276b506-9fb6-413f-9a59-dfecc2aa57e8}</MetaDataID>
        private void AddTranslator()
        {
            System.Windows.Window owner = System.Windows.Window.GetWindow(NewTranslatorCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                var menuItemWindow = new Views.HumanResources.TranlatorWindow();
                //new MenuItemsEditor. Views.MenuItemWindow();
                menuItemWindow.Owner = owner;



                ViewModel.HumanResources.TranslatorViewModel translatorViewModel = new HumanResources.TranslatorViewModel(this);
                menuItemWindow.GetObjectContext().SetContextInstance(translatorViewModel);

                if (menuItemWindow.ShowDialog().Value)
                {
                    if (!string.IsNullOrWhiteSpace(translatorViewModel.UserData.Identity))
                    {
                        var translator = Organization.AssignTranslatorRoleToUser(translatorViewModel.UserData);
                        if (this.Translators == null)
                        {
                            this.Translators = new TranslatorsTreeNode(this, new List<FlavourBusinessFacade.HumanResources.ITranslator>());
                            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
                        }
                        this.Translators.AddTranslator(translator);

                    }

                    stateTransition.Consistent = true;
                }
            }
        }

        private void AddGraphicMenuDesigner()
        {
            //System.Windows.Window owner = System.Windows.Window.GetWindow(NewTranslatorCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            //using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            //{
            //    var menuItemWindow = new Views.HumanResources.TranlatorWindow();
            //    //new MenuItemsEditor. Views.MenuItemWindow();
            //    menuItemWindow.Owner = owner;



            //    ViewModel.HumanResources.TranslatorViewModel translatorViewModel = new HumanResources.TranslatorViewModel(this);
            //    menuItemWindow.GetObjectContext().SetContextInstance(translatorViewModel);

            //    if (menuItemWindow.ShowDialog().Value)
            //    {
            //        if (!string.IsNullOrWhiteSpace(translatorViewModel.UserData.Identity))
            //        {
            //            var translator = Organization.AssignTranslatorRoleToUser(translatorViewModel.UserData);
            //            if (this.Translators == null)
            //            {
            //                this.Translators = new TranslatorsTreeNode(this, new List<FlavourBusinessFacade.HumanResources.ITranslator>());
            //                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
            //            }
            //            this.Translators.AddTranslator(translator);

            //        }

            //        stateTransition.Consistent = true;
            //    }
            //}
        }



        /// <MetaDataID>{f1bd288b-8510-4caa-a041-3eaa9a8c2ef1}</MetaDataID>
        public void SignOut()
        {
            CompanySignedOut?.Invoke(this, EventArgs.Empty);
        }

        /// <MetaDataID>{878395c8-7771-4931-9ccc-5eb907bf469b}</MetaDataID>
        internal void RemoveServicesContext(FlavoursServicesContextPresentation flavoursServicePointPresentation)
        {

            Task.Run(() =>
            {
                Organization.DeleteServicesContext(flavoursServicePointPresentation.ServicesContext);
                ServicesContexts.Remove(flavoursServicePointPresentation.ServicesContext);
                foreach (var ServicePoint in Organization.ServicesContexts)
                {
                    if (!ServicesContexts.ContainsKey(ServicePoint))
                        ServicesContexts.Add(ServicePoint, new FlavoursServicesContextPresentation(ServicePoint, this));
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
            });


        }

        /// <MetaDataID>{6896887b-f8c2-4fcc-b089-129d1c545ec1}</MetaDataID>
        Dictionary<IFlavoursServicesContext, FlavoursServicesContextPresentation> ServicesContexts = new Dictionary<IFlavoursServicesContext, FlavoursServicesContextPresentation>();

        /// <MetaDataID>{45133261-b18f-405e-80e1-f2541360c206}</MetaDataID>
        private async void AddServicesContextPoint()
        {
            await Task.Run(() =>
           {
               var newFlavoursServicePoint = Organization.NewFlavoursServicesContext();
               //foreach (var ServicePoint in Organization.ServicePoints)
               ServicesContexts.Add(newFlavoursServicePoint, new FlavoursServicesContextPresentation(newFlavoursServicePoint, this));

               PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Members)));
           });
        }

        /// <MetaDataID>{1f0e7c61-a76c-4691-a475-579a7e8cf8e6}</MetaDataID>
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

        /// <MetaDataID>{6783d5b3-4a82-4797-9220-a1e7c50c08ec}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {

        }

        /// <MetaDataID>{0143331b-3810-450b-b64d-3b5693739c6a}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {

        }

        /// <MetaDataID>{5709f2bf-4d17-4ba6-9efc-bc3f8cedc51d}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {

        }

        /// <MetaDataID>{6aececaa-5d88-43e7-acfe-2a0345399883}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {

        }

        public bool RemoveGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNodeToRemove)
        {

            if( global::MenuDesigner.ViewModel.Menu.MenuDesignerHost.Current.Menu!=null&& global::MenuDesigner.ViewModel.Menu.MenuDesignerHost.Current.Menu.GraphicMenustorageRef.StorageIdentity== graphicMenuTreeNodeToRemove.GraphicMenuStorageRef.StorageIdentity)
            {
                StyleableWindow.WpfMessageBox.Show("Graphic menu", string.Format("The graphic menu '{0}' is opened from menu designer.{1} Close menu designer and try again.", graphicMenuTreeNodeToRemove.Name, System.Environment.NewLine), MessageBoxButton.OK, StyleableWindow.MessageBoxImage.Information);
                return false;
            }

            List<FBResourceTreeNode> graphicStorageReferences = null;
            if ((HeaderNode).FBResourceTreeNodesDictionary.TryGetValue(graphicMenuTreeNodeToRemove.GraphicMenuStorageRef.StorageIdentity, out graphicStorageReferences))
            {
                string ownerNames = null;
                foreach (GraphicMenuTreeNode graphicMenuTreeNode in graphicStorageReferences)
                {
                    if (graphicMenuTreeNode != graphicMenuTreeNodeToRemove)
                    {
                        if (ownerNames != null)
                            ownerNames = ",";
                        ownerNames += graphicMenuTreeNode.Parent.Parent.Name;
                    }
                }
                if (!string.IsNullOrWhiteSpace(ownerNames))
                {
                    StyleableWindow.WpfMessageBox.Show("Graphic menu", string.Format("You must remove menu '{0}' from{1}{2}", graphicMenuTreeNodeToRemove.Name, System.Environment.NewLine, ownerNames), MessageBoxButton.OK, StyleableWindow.MessageBoxImage.Information);
                    return false;
                }
            }


            var messageBoxResult = StyleableWindow.WpfMessageBox.Show("Graphic menu", string.Format("The menu '{0}' will be permanently deleted !{1}Are you sure?", graphicMenuTreeNodeToRemove.Name, System.Environment.NewLine), MessageBoxButton.YesNo, StyleableWindow.MessageBoxImage.Information);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                string graphicMenuStorageIdentity = graphicMenuTreeNodeToRemove.GraphicMenuStorageRef.StorageIdentity;

                bool graphicMenuRemoved = GraphicMenusPresentation.RemoveGraphicMenu(graphicMenuStorageIdentity);
                if (graphicMenuRemoved)
                {
                    this._GraphicMenus.Remove(graphicMenuStorageIdentity);
                    this.Menus.Refresh();
                    return true;
                }
                else
                    return false;

            }

            else
                return false;


        }

        public void AssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {

        }

        public bool CanAssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            return false;
        }

        public void NewGraphicMenu()
        {
            MenuDesigner.ViewModel.Menu.MenuDesignerHost.NewGraphicMenu();
        }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{77ff6d5f-c772-439e-af29-a2e29bbfc382}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {

                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };

                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Delete.png"));
                    //MenuComamnd menuItem = new MenuComamnd();
                    //menuItem.Header = Properties.Resources.DeleteMenuItemHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    ////menuItem.Command = DeleteMenuCommand;
                    //_ContextMenuItems.Add(menuItem);

                    MenuCommand menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    menuItem.Header = MenuItemsEditor.Properties.Resources.TreeNodeRenameMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = RenameCommand;

                    _ContextMenuItems.Add(menuItem);




                    //menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Rename16.png"));
                    ////menuItem.Header = Properties.Resources.TreeNodeRenameMenuItemHeader;
                    //menuItem.Icon = new Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = RenameCommand;

                    _ContextMenuItems.Add(null);


                    //menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/cafe16.png"));
                    //menuItem.Header = Properties.Resources.AddServicePointHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    //menuItem.Command = AddFlavoursServicePointCommand;

                    //_ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/Restaurant16.png"));
                    menuItem.Header = Properties.Resources.AddServicesContextHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = AddFlavoursServicePointCommand;

                    _ContextMenuItems.Add(menuItem);


                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/settings16.png"));
                    menuItem.Header = Properties.Resources.SettingsMenuPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = SettingsCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/bilingual16.png"));
                    menuItem.Header = Properties.Resources.NewTranslatorMenuPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewTranslatorCommand;

                    _ContextMenuItems.Add(menuItem);

                    menuItem = new MenuCommand();
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/graphic-designer16.png"));
                    menuItem.Header = Properties.Resources.NewGraphicMenuDesignerMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewGraphicMenuDesignerCommand;

                    _ContextMenuItems.Add(menuItem);


                    //menuItem = new MenuComamnd(); ;
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    //menuItem.Header = Properties.Resources.RemoveServicesContextHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    ////menuItem.Command = NewMenuItemCommand;

                    //_ContextMenuItems.Add(menuItem);

                    //menuItem = new MenuComamnd();
                    //imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Type16.png"));
                    //menuItem.Header = MenuItemsEditor.Properties.Resources.ShowCategoryOptionTypesMenuItemHeader;
                    //menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    ////menuItem.Command = EditOptionsTypesCommand;

                    //_ContextMenuItems.Add(menuItem);

                }
                return _ContextMenuItems;
            }
        }



        /// <MetaDataID>{88194e0d-32f2-4c52-b24d-2aa24879d01b}</MetaDataID>
        public virtual bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{db4d579a-ef7b-4659-850c-911987cd2bbe}</MetaDataID>
        public virtual bool IsEditable
        {
            get
            {
                return true;
            }
        }




        /// <MetaDataID>{f7caeec8-b39b-41e6-8d0e-87973a947975}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = ServicesContexts.Values.OfType<FBResourceTreeNode>().ToList();
                if (Menus != null)
                    members.Add(Menus);

                if (this.MenuMakers != null)
                    members.Add(this.MenuMakers);

                if (this.Translators != null)
                    members.Add(this.Translators);


                return members;

            }
        }

        /// <MetaDataID>{5a7caa22-485f-481f-81eb-42ed07c5f88b}</MetaDataID>
        public override string Name
        {
            get
            {
                return this.Organization.Name;
            }

            set
            {
            }
        }


        /// <MetaDataID>{53208d59-1a9a-4d08-9053-2fffcf4468cc}</MetaDataID>
        FBResourceTreeNode _Parent;
        /// <MetaDataID>{0398ff9f-8532-4cd2-bcdb-1b3dc7831ed4}</MetaDataID>
        private TranslatorsTreeNode Translators;
        private MenuMakersTreeNode MenuMakers;

        /// <MetaDataID>{7312b185-975d-4372-b901-0d1c040134b7}</MetaDataID>
        public FBResourceTreeNode Parent
        {
            get
            {
                return _Parent;
            }

            set
            {
                _Parent = value;
            }
        }

        /// <MetaDataID>{a9614c2a-ddc4-4985-aeb0-d100a971c4d8}</MetaDataID>
        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return null;
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

        /// <MetaDataID>{78f55219-4c83-4300-a10b-9783e867af31}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/FlavourBusiness.png"));

            }
        }

        public List<GraphicMenuTreeNode> GraphicMenus => _GraphicMenus.Values.ToList();

        public bool NewGraphicMenuAllowed => true;

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
