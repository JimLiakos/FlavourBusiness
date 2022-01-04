using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using FlavourBusinessFacade.HumanResources;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.HumanResources
{
    /// <MetaDataID>{8402a742-7cf0-489b-9c16-f387563ec11b}</MetaDataID>
    public class WaitersTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {

        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{f8f3e3a9-4025-44f0-a5eb-aaf51aa04270}</MetaDataID>
        public WaitersTreeNode(StaffTreeNode parent) : base(parent)
        {
            ServiceContextStaff = parent;

            NewWaiterCommand = new RelayCommand((object sender) =>
            {
                NewWaiter();
            });



            try
            {

                foreach (var Waiter in ServiceContextStaff.ServiceContextHumanResources.Waiters)
                    Waiters.Add(Waiter, new WaiterTreeNode(this, Waiter));

            }
            catch (System.Exception error)
            {
            }


        }
        /// <MetaDataID>{b40f57a7-1c85-4b7c-8047-56b0c8bc9593}</MetaDataID>
        StaffTreeNode ServiceContextStaff;

        /// <MetaDataID>{59dc0adf-57d3-4c05-b448-25aea40862ae}</MetaDataID>
        Dictionary<IWaiter, WaiterTreeNode> Waiters = new Dictionary<IWaiter, WaiterTreeNode>();

        /// <MetaDataID>{2b56e034-091b-4f79-8a2e-e9414703710b}</MetaDataID>
        internal void RemoveWaiter(WaiterTreeNode waiterTreeNode)
        {
            this.ServiceContextStaff.ServicesContextPresentation.ServicesContext.RemoveWaiter(waiterTreeNode.Waiter);
            Waiters.Remove(waiterTreeNode.Waiter);
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }

        /// <MetaDataID>{8c4aa6d6-fdcd-4912-89e6-a26dbfeeb3ae}</MetaDataID>
        internal void RefreshPresentation()
        {
            foreach (var waiter in ServiceContextStaff.ServiceContextHumanResources.Waiters)
            {
                if (Waiters.Values.Where(x => x.Waiter == waiter).FirstOrDefault() == null)
                {
                    Waiters.Add(waiter, new WaiterTreeNode(this, waiter));
                    if (NewWaiterQRCodePopup != null && NewWaiterQRCodePopup.CodeValue.Split(';').ToList().Count == 3 && waiter.Identity == NewWaiterQRCodePopup.CodeValue.Split(';')[1])
                        Application.Current.Dispatcher.Invoke(new Action(() => { NewWaiterQRCodePopup.Close(); }));
                }
            }
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            IsNodeExpanded = true;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));
        }

        /// <MetaDataID>{347fdb85-bca7-4686-af5c-9d8c828496ad}</MetaDataID>
        Views.HumanResources.NewUserQRCodePopup NewWaiterQRCodePopup;
        /// <MetaDataID>{36776d00-6861-4d9c-ac00-3a44164915aa}</MetaDataID>
        private void NewWaiter()
        {

            try
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(NewWaiterCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                string supervisorAssignKey = ServiceContextStaff.ServicesContextPresentation.ServicesContext.NewWaiter();// Organization.CurrentOrganization.NewSupervisor(ServiceContextStaff.ServicesContextPresentation.ServicesContext.ServicesContextIdentity);
                NewWaiterQRCodePopup = new Views.HumanResources.NewUserQRCodePopup("New Waiter", "Scan to register as waiter") { CodeValue = supervisorAssignKey };
                NewWaiterQRCodePopup.Owner = win;
                NewWaiterQRCodePopup.ShowDialog();
            }
            catch (Exception error)
            {
            }


            //IWaiter Waiter = ServiceContextStaff.ServicesContextPresentation.ServicesContext.NewWaiter();
            //Waiters.Add(Waiter, new WaiterTreeNode(this, Waiter));


            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            //IsNodeExpanded = true;
            //RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsNodeExpanded)));

        }

        /// <MetaDataID>{1c1fdb00-69e6-4693-9fe7-c928aa993686}</MetaDataID>
        public override string Name
        {
            get
            {
                return Properties.Resources.WaitersTitle;
            }

            set
            {
            }
        }

        /// <MetaDataID>{a60bed74-a032-4b94-96c4-984a9777de52}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/waiters16.png"));
            }
        }
        /// <MetaDataID>{114d0c5e-ec17-4c77-bb92-1c4b35cc0d87}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                var members = this.Waiters.Values.OfType<FBResourceTreeNode>().ToList();

                return members;
            }
        }
        /// <MetaDataID>{c624799e-0841-4269-b4c3-3d9483fed61a}</MetaDataID>
        public RelayCommand NewWaiterCommand { get; protected set; }

        /// <MetaDataID>{ba0ffaaf-f2fc-405e-8c28-402cffff4cc9}</MetaDataID>
        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        /// <MetaDataID>{1fb820f8-582a-45bf-8e7d-406e216bc663}</MetaDataID>
        List<MenuCommand> _ContextMenuItems;
        /// <MetaDataID>{166a0626-a072-4d7c-a8a6-d2bb9bc68ed5}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();




                    MenuCommand menuItem = new MenuCommand(); ;
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/waiter16.png"));
                    menuItem.Header = Properties.Resources.NewWaiterPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewWaiterCommand;
                    _ContextMenuItems.Add(menuItem);




                }

                return _ContextMenuItems;
            }
        }
        /// <MetaDataID>{1ed6e75e-6703-46ab-8aef-9e2d68624ed5}</MetaDataID>
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
        /// <MetaDataID>{5826e443-cc12-4cb1-b412-8cee502049d3}</MetaDataID>
        public override void SelectionChange()
        {
        }

        /// <MetaDataID>{a099a6e9-d220-46d9-9091-4a7cb4fe62a1}</MetaDataID>
        DateTime DragEnterStartTime;
        /// <MetaDataID>{66ba4080-a3c7-4e2d-a40c-b64f6ee99dee}</MetaDataID>
        public void DragEnter(object sender, DragEventArgs e)
        {
            DragEnterStartTime = DateTime.Now;

            MenuItemsEditor.ViewModel.DragItemsCategory dragItemsCategory = e.Data.GetData(typeof(MenuItemsEditor.ViewModel.DragItemsCategory)) as MenuItemsEditor.ViewModel.DragItemsCategory;
            if (dragItemsCategory != null)
            {
                e.Effects = DragDropEffects.Copy;
                DragEnterStartTime = DateTime.Now;
            }
            else
                e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{35db33c2-61ea-4094-9a85-6222b6d035a9}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{85ffb4ad-f124-47a2-b687-cc8d4a651d8e}</MetaDataID>
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

        /// <MetaDataID>{37425591-d16e-4dbe-a3af-a0c732ace74a}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {
        }

    }
}
