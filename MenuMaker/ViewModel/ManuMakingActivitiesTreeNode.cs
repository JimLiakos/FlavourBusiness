using FLBManager.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using UIBaseEx;
using WPFUIElementObjectBind;

namespace MenuMaker.ViewModel
{
    /// <MetaDataID>{e8fded29-f251-4dca-ae0e-ae338f0aed97}</MetaDataID>
    public class ManuMakingActivitiesTreeNode : FBResourceTreeNode, INotifyPropertyChanged
    {

        internal MenuMakerTreeNode MenuMakerTreeNode;
        public ManuMakingActivitiesTreeNode(MenuMakerTreeNode menuMeker) : base(menuMeker)
        {
            MenuMakerTreeNode = menuMeker;

            Task.Run(() =>
            {
                var responsibilities = MenuMakerTreeNode.MenuMaker.Responsibilities;
                foreach (var responsibility in responsibilities)
                    MenuMakingCommissioner.GetViewModelFor(responsibility, this, responsibility);

                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            });
        }


        ViewModelWrappers<FlavourBusinessFacade.HumanResources.IAccountability, MenuMakingCommissionerTreeNode> MenuMakingCommissioner = new ViewModelWrappers<FlavourBusinessFacade.HumanResources.IAccountability, MenuMakingCommissionerTreeNode>();


        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuMaker;Component/Resources/Images/Metro/job-description16.png"));
            }
        }

        public override string Name
        {
            get => Properties.Resources.MenuMakingActivitiesTreeNodeTitle;
            set { }
        }

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return MenuMakingCommissioner.Values.OfType<FBResourceTreeNode>().ToList();
            }
        }

        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                return new List<MenuCommand>();
            }
        }

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

        public override void SelectionChange()
        {
        }
    }
}
