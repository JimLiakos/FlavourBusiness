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
using FLBManager.ViewModel;
using WPFUIElementObjectBind;

namespace MenuDesigner.ViewModel
{
    /// <MetaDataID>{ad48a862-f965-49f9-85c4-013d7583888d}</MetaDataID>
    public class MenusTreeNode : FBResourceTreeNode, IDragDropTarget
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }

        ///// <MetaDataID>{ab37fd26-6d11-4d08-a682-06e6aeabed58}</MetaDataID>
        //Dictionary<string, GraphicMenuTreeNode> GraphicMenus = new Dictionary<string, GraphicMenuTreeNode>();


        public IGraphicMenusOwner GraphicMenusOwner;
        /// <MetaDataID>{1e43c1f7-cb54-4477-9fe9-e23266155dba}</MetaDataID>
        public MenusTreeNode(string name, FBResourceTreeNode parent, IGraphicMenusOwner graphicMenusOwner) : base(parent)
        {
            _Name = name;
            //GraphicMenus = graphicMenus;
            GraphicMenusOwner = graphicMenusOwner;
            NewGraphicMenuCommand = new RelayCommand((object sender) =>
            {
                GraphicMenusOwner.NewGraphicMenu();
            });
        }

        public RelayCommand NewGraphicMenuCommand { get; protected set; }
        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;

        /// <MetaDataID>{c4da04cb-dc5a-4a5f-802d-a93f28bd555a}</MetaDataID>
        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                if (_ContextMenuItems == null)
                {
                    _ContextMenuItems = new List<MenuCommand>();

                    if (GraphicMenusOwner.NewGraphicMenuAllowed)
                    {
                        //var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/Empty.png"));
                        //var emptyImage = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        MenuCommand menuItem = new MenuCommand();
                        var imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/OpenFolder.png"));
                        menuItem.Header = Properties.Resources.NewGraphicMenuMenuItemHeader;
                        menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                        menuItem.Command = NewGraphicMenuCommand;
                        _ContextMenuItems.Add(menuItem);
                    }
                }
                return _ContextMenuItems;
            }
        }


        public override bool HasContextMenu => true;

        /// <MetaDataID>{fe8bc732-cc51-4d31-9bfc-3672b1aa1ab3}</MetaDataID>
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return this.GraphicMenusOwner.GraphicMenus.OfType<FBResourceTreeNode>().ToList();
            }
        }
        string _Name;
        /// <MetaDataID>{aaa86b73-066e-40f5-bf31-10cc2b3b77c6}</MetaDataID>
        public override string Name
        {
            get
            {
                return _Name;
            }
            set
            {
            }
        }


        /// <MetaDataID>{6bf427d4-ed47-4ac2-9d2f-593220975e87}</MetaDataID>
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

        /// <MetaDataID>{3db7321d-bbae-491d-b20f-2a4c63b20503}</MetaDataID>
        public override ImageSource TreeImage
        {
            get
            {
                return new BitmapImage(new Uri(@"pack://application:,,,/MenuDesignerLib;Component/Resources/Images/Metro/menusfolder.png"));
                //
            }
        }

        /// <MetaDataID>{45793ca8-6e5c-4774-be49-e4463d4dba5d}</MetaDataID>
        public override void SelectionChange()
        {
        }

        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
        }


        public void DragEnter(object sender, DragEventArgs e)
        {

            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
            {
                e.Effects = DragDropEffects.None;
                return;
            }

            if (!GraphicMenusOwner.CanAssignGraphicMenu(graphicMenuTreeNode))
                e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{57c420f9-1928-45c6-9d77-546e492574fb}</MetaDataID>
        public void DragLeave(object sender, DragEventArgs e)
        {
            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;

        }

        /// <MetaDataID>{475a0786-9795-489f-8dd0-96ce3002d55c}</MetaDataID>
        public void DragOver(object sender, DragEventArgs e)
        {
            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;
            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;
            if (!GraphicMenusOwner.CanAssignGraphicMenu(graphicMenuTreeNode))
                e.Effects = DragDropEffects.None;
        }

        /// <MetaDataID>{d4522845-7554-4c71-b910-220bc917f56a}</MetaDataID>
        public void Drop(object sender, DragEventArgs e)
        {

            GraphicMenuTreeNode graphicMenuTreeNode = e.Data.GetData(typeof(GraphicMenuTreeNode)) as GraphicMenuTreeNode;


            if (!GraphicMenusOwner.CanAssignGraphicMenu(graphicMenuTreeNode))
                return;

            if (graphicMenuTreeNode == null)
                e.Effects = DragDropEffects.None;
            else
                GraphicMenusOwner.AssignGraphicMenu(graphicMenuTreeNode);

        }


    }



    /// <MetaDataID>{cfbb910e-8e6b-451b-a4a0-f713a33c9da1}</MetaDataID>
    public interface IGraphicMenusOwner
    {
        /// <MetaDataID>{1447e203-3caf-44cf-87b3-1d7553be40d5}</MetaDataID>
        void AssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode);
        /// <MetaDataID>{6145fb52-5ba7-4e5c-bf04-d69f377b76ee}</MetaDataID>
        bool CanAssignGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode);
        /// <MetaDataID>{f03fbc94-4edf-44ab-8584-584eeacc6d84}</MetaDataID>
        bool RemoveGraphicMenu(GraphicMenuTreeNode graphicMenuTreeNode);
        /// <MetaDataID>{a244212b-a172-4eca-9103-1ef4ffb76c5c}</MetaDataID>
        List<GraphicMenuTreeNode> GraphicMenus { get; }

        /// <MetaDataID>{717680ce-943c-42f3-add9-e55557d3b2d8}</MetaDataID>
        bool NewGraphicMenuAllowed { get; }

        /// <MetaDataID>{06e0e951-abc5-438c-8617-5971d7b7feba}</MetaDataID>
        void NewGraphicMenu();

    }

}
