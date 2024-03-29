﻿using FlavourBusinessFacade.HumanResources;
using OOAdvantech.Transactions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{e96a3281-b3a4-4e64-8a22-735ff79db2dd}</MetaDataID>
    public class MenuMakersTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{feb2c678-24b0-4beb-a0d4-e6172e0e4d4c}</MetaDataID>
        public MenuMakersTreeNode(CompanyPresentation parent, List<IAccountability> menuMakers) : base(parent)
        {
            Company = parent;

            NewMenuMakerCommand = new WPFUIElementObjectBind.RelayCommand((object sender) =>
            {
                AddGraphicMenuDesigner();
            });


            try
            {

                foreach (var menuMaker in menuMakers)
                    MenuMakers.Add(menuMaker, new MenuMakerTreeNode(this, menuMaker));

            }
            catch (System.Exception error)
            {
            }


        }
        /// <MetaDataID>{5e44378e-4a30-4a0c-99b5-521ea00e0683}</MetaDataID>
        internal CompanyPresentation Company;
        /// <MetaDataID>{1581239d-326e-40d8-a16a-8ea668027669}</MetaDataID>
        private void AddGraphicMenuDesigner()
        {
            System.Windows.Window owner = System.Windows.Window.GetWindow(NewMenuMakerCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.RequiresNew))
            {
                var tranlatorWindow = new Views.HumanResources.TranlatorWindow();
                //new MenuItemsEditor. Views.MenuItemWindow();
                tranlatorWindow.Owner = owner;




                ViewModel.HumanResources.MenuMakerViewModel translatorViewModel = new HumanResources.MenuMakerViewModel(Company);
                tranlatorWindow.GetObjectContext().SetContextInstance(translatorViewModel);

                if (tranlatorWindow.ShowDialog().Value)
                {
                    //if (!string.IsNullOrWhiteSpace(translatorViewModel.UserData.Identity))
                    //{
                    //    var translator = Company.Organization.AssignMenuMakerRoleToUser(translatorViewModel.UserData);
                    //    if (translator != null && !MenuMakers.ContainsKey(translator))
                    //    {
                    //        MenuMakers.Add(translator, new MenuMakerTreeNode(this, translator));
                    //        IsNodeExpanded = true;
                    //        RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
                    //        RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
                    //    }
                    //}

                    stateTransition.Consistent = true;
                }
            }
        }


        /// <MetaDataID>{eaba0514-daf6-4d9a-bf4f-be313590f02d}</MetaDataID>
        Dictionary<FlavourBusinessFacade.HumanResources.IAccountability, MenuMakerTreeNode> MenuMakers = new Dictionary<IAccountability, MenuMakerTreeNode>();

        /// <MetaDataID>{bdd8df5f-419f-4f6e-9735-601e7e9d70ce}</MetaDataID>
        internal void RemoveMenuMaker(MenuMakerTreeNode translatorTreeNode)
        {
            this.Company.Organization.RemoveMenuMaker(translatorTreeNode.MenuMakingAccountability);
            MenuMakers.Remove(translatorTreeNode.MenuMakingAccountability);
            if (MenuMakers.Count == 0)
                Company.RemoveMenuMakersNode();
            else
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{c9ea7b6a-7b53-4120-bd03-70b1b9e23861}</MetaDataID>
        internal void RefreshPresentation()
        {
            foreach (var translator in Company.Organization.GetMenuMakers(FlavourBusinessFacade.WorkerState.All))
            {
                //if (MenuMakers.Values.Where(x => x.MenuMaker == translator).FirstOrDefault() == null)
                //    MenuMakers.Add(translator, new MenuMakerTreeNode(this, translator));
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            IsNodeExpanded = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
        }


        /// <MetaDataID>{36447c1a-b568-4c1c-8711-d1f25523dc68}</MetaDataID>
        public override string Name
        {
            get
            {
                return Properties.Resources.MenuMakersTitle;
            }

            set
            {
            }
        }

        /// <MetaDataID>{5c11be9a-5baf-49e5-993b-3f5627a1cf18}</MetaDataID>
        public override System.Windows.Media.ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/MenuDesign16.png"));
            }
        }
        /// <MetaDataID>{f276f22e-c86f-4955-b3b6-8f705ac6d1cb}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = this.MenuMakers.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }
        /// <MetaDataID>{a26c2e8e-f15c-4a54-8791-1d707c4e2503}</MetaDataID>
        public WPFUIElementObjectBind.RelayCommand NewMenuMakerCommand { get; protected set; }

        /// <MetaDataID>{e76bf560-c2b9-47b0-a4fb-e33c19a748f4}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{525576fa-b24b-493b-b26c-1ab2a453b132}</MetaDataID>
        List<WPFUIElementObjectBind.MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{6dcbec8a-e5d1-4a48-9b19-f7104d4f204f}</MetaDataID>
        public override List<WPFUIElementObjectBind.MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<WPFUIElementObjectBind.MenuCommand>();

                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                    var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };



                    WPFUIElementObjectBind.MenuCommand menuItem = new WPFUIElementObjectBind.MenuCommand(); ;
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/graphic-designer16.png"));
                    menuItem.Header = Properties.Resources.NewGraphicMenuDesignerMenuItemHeader;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewMenuMakerCommand;
                    _ContextMenuItems.Add(menuItem);




                }

                return _ContextMenuItems;
            }
        }

        internal void AddMenuMaker(IAccountability menuMakingActivity)
        {
            MenuMakers.Add(menuMakingActivity, new MenuMakerTreeNode(this, menuMakingActivity));

            IsNodeExpanded = true;

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
        }

        /// <MetaDataID>{52894160-59b0-49c4-b5a9-f74d7295490a}</MetaDataID>
        public override List<WPFUIElementObjectBind.MenuCommand> SelectedItemContextMenuItems
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
        /// <MetaDataID>{92968518-f1e9-4b93-b6eb-7d1869153678}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{297744c7-6107-45c5-8601-3125245e8c85}</MetaDataID>
        DateTime DragEnterStartTime;
        /// <MetaDataID>{6b608ca5-4444-4deb-a780-c2651a11697d}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {

            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{c5dac256-6aad-494b-a3a5-8e8c9f8ff6ce}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{36360d44-66ae-403f-b319-c4fff957d8ca}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {

            MenuItemsEditor.ViewModel.DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(MenuItemsEditor.ViewModel.DragItemsCategory)) as MenuItemsEditor.ViewModel.DragItemsCategory;
            if (dragItemsCategory != null)
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

        /// <MetaDataID>{d433c86d-2f81-4514-b830-61e0330c7b31}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
        }

    }
}
