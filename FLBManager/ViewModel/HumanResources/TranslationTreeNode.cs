using FlavourBusinessManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace FLBManager.ViewModel.HumanResources
{

    /// <MetaDataID>{4df8bca5-94d2-4085-a4c9-23bc99b8bb21}</MetaDataID>
    public class TranslationTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {

        List<SupervisorTreeNode> SupervisorTreeNodes = new List<SupervisorTreeNode>();

        public TranslationTreeNode(StaffTreeNode parent) : base(parent)
        {
            ServiceContextStaff = parent;

            NewSupervisorCommand = new RelayCommand((object sender) =>
            {
                NewSupervisor();
            });

            //foreach (var supervisor in ServiceContextStaff.ServiceContextHumanResources.Supervisors)
            //    SupervisorTreeNodes.Add(new SupervisorTreeNode(this, supervisor));

        }

        internal void RefreshPresentation()
        {

            //foreach (var supervisor in ServiceContextStaff.ServiceContextHumanResources.Supervisors)
            //{
            //    if (SupervisorTreeNodes.Where(x => x.Supervisor == supervisor).FirstOrDefault() == null)
            //    {

            //        SupervisorTreeNodes.Add(new SupervisorTreeNode(this, supervisor));

            //        if (NewSupervisorQRCodePopup != null && NewSupervisorQRCodePopup.CodeValue.Split(';').ToList().Count == 3 && supervisor.Identity == NewSupervisorQRCodePopup.CodeValue.Split(';')[1])
            //            Application.Current.Dispatcher.Invoke(new Action(() => { NewSupervisorQRCodePopup.Close(); }));


            //    }
            //}
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }
        Views.HumanResources.NewUserQRCodePopup NewSupervisorQRCodePopup;
        private void NewSupervisor()
        {
            try
            {
                System.Windows.Window win = System.Windows.Window.GetWindow(NewSupervisorCommand.UserInterfaceObjectConnection.ContainerControl as System.Windows.DependencyObject);
                string supervisorAssignKey = Organization.CurrentOrganization.NewSupervisor(ServiceContextStaff.ServicesContextPresentation.ServicesContext.ServicesContextIdentity);
                NewSupervisorQRCodePopup = new Views.HumanResources.NewUserQRCodePopup("New Supervisor", "Scan to register as supervisor") { CodeValue = supervisorAssignKey };
                NewSupervisorQRCodePopup.Owner = win;
                NewSupervisorQRCodePopup.ShowDialog();
            }
            catch (Exception error)
            {
            }

        }

        StaffTreeNode ServiceContextStaff;

        //  Dictionary<IServiceContextSupervisor, SupervisorTreeNode> Managers = new Dictionary<IServiceContextSupervisor, SupervisorTreeNode>();





        public override string Name
        {
            get
            {
                return Properties.Resources.AdministrationTitle;
            }

            set
            {
            }
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/administration16.png"));
            }
        }

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                //var members = this.Managers.Values.OfType<FBResourceTreeNode>().ToList();
                //foreach (var supervisorTreeNode in SupervisorTreeNodes)
                //    members.Add(supervisorTreeNode);
                return new List<FBResourceTreeNode>();
            }
        }
        public RelayCommand NewSupervisorCommand { get; protected set; }

        public override bool HasContextMenu
        {
            get
            {
                return true;
            }
        }

        List<MenuCommand> _ContextMenuItems;
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {

                    _ContextMenuItems = new List<MenuCommand>();
                    MenuCommand menuItem = new MenuCommand(); ;
                    var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/FLBManager;Component/Resources/Images/Metro/businessman24.png"));
                    menuItem.Header = Properties.Resources.NewWaiterPrompt;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = NewSupervisorCommand;
                    _ContextMenuItems.Add(menuItem);
                }

                return _ContextMenuItems;
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
        public override void SelectionChange()
        {
        }


    }

}
