using FLBManager.ViewModel;
using MenuDesigner.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFUIElementObjectBind;

namespace MenuMaker.ViewModel
{
    /// <MetaDataID>{303d10ff-21fa-4012-8eac-de8b56142352}</MetaDataID>
    public class MenuMakingCommissionerTreeNode : FBResourceTreeNode, INotifyPropertyChanged, IGraphicMenusOwner
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        readonly FlavourBusinessFacade.IParty Commissioner;
        readonly FlavourBusinessFacade.HumanResources.IAccountability Accountability;
        public MenuMakingCommissionerTreeNode(ManuMakingActivitiesTreeNode parent, FlavourBusinessFacade.HumanResources.IAccountability accountability) : base(parent)
        {
            Accountability = accountability;


            Commissioner = accountability.Commissioner;
            _Name = Commissioner.Name;

            foreach (var activity in accountability.Activities)
            {
                if ((activity as FlavourBusinessFacade.HumanResources.IMenuDesignActivity).DesignActivityType == FlavourBusinessFacade.HumanResources.DesignSubjectType.Menu)
                {
                    var graphicMenu = parent.MenuMakerTreeNode.MenuMaker.GetGraphicMenu(activity);
                    
                    var menuItems = parent.MenuMakerTreeNode.MenuMaker.GetGraphicMenuItems(activity);
                    _Members.Add(new MenuDesigner.ViewModel.GraphicMenuTreeNode(graphicMenu, menuItems, this,this, false));
                    IsNodeExpanded = true;
                }
            }




            //
        }
        string _Name;
        public override string Name { get => _Name; set { } }

        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuMaker;Component/Resources/Images/Metro/FlavourBusiness.png"));

            }
        }


        /// <exclude>Excluded</exclude>
        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members;
            }
        }
        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems = new List<MenuCommand>();
        public override List<MenuCommand> ContextMenuItems => _ContextMenuItems;

        public override List<MenuCommand> SelectedItemContextMenuItems => ContextMenuItems;

        public List<GraphicMenuTreeNode> GraphicMenus => throw new NotImplementedException();

        public bool NewGraphicMenuAllowed => false;

        public override void SelectionChange()
        {

        }

        public void AssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            throw new NotImplementedException();
        }

        public bool CanAssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            throw new NotImplementedException();
        }

        public bool RemoveGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode)
        {
            throw new NotImplementedException();
        }

        public void NewGraphicMenu()
        {
            
        }
    }
}
