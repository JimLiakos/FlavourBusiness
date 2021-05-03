using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel;
using FloorLayoutDesigner.ViewModel;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    /// <MetaDataID>{0cedc37f-b933-45e3-958f-1f8ec3dec749}</MetaDataID>
    public class ServicePointPresentation :  FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget, IServicePointViewModel
    {

        public override bool IsEditable => true;

        public IServicePoint ServicePoint { get; private set; }
        public ServicePointPresentation(IServicePoint servicePoint, FBResourceTreeNode parent) : base(parent)
        {
            ServicePoint = servicePoint;
            _Name = Properties.Resources.LoadingPrompt;
            Task.Run(() =>
            {
                _Name = servicePoint.Description;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
            });
        }

        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                return new List<MenuCommand>();
            }
        }

        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return new List<FBResourceTreeNode>();
            }
        }


        /// <exclude>Excluded</exclude>
        string _Name;

        public override string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    ServicePoint.Description = value;

                    if ((Parent as ServiceAreaPresentation).RestaurantHallLayout != null &&
                        HallLayoutDesignerHost.Current.HallLayout != null &&
                        HallLayoutDesignerHost.Current.HallLayout.HallLayout == (Parent as ServiceAreaPresentation).RestaurantHallLayout)
                    {
                        var shape = (Parent as ServiceAreaPresentation).RestaurantHallLayout.Shapes.Where(x => x.ServicesPointIdentity == ServicePoint.ServicesPointIdentity).FirstOrDefault();
                        shape.Label = value;
                    }
                    
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }


        public override List<MenuCommand> SelectedItemContextMenuItems
        {
            get
            {
                if (IsSelected)
                    return ContextMenuItems;
                //else
                //    foreach (var treeNode in Members)
                //    {
                //        var contextMenuItems = treeNode.SelectedItemContextMenuItems;
                //        if (contextMenuItems != null)
                //            return contextMenuItems;
                //    }

                return null;
            }
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
            }
        }

        public void DragEnter(object sender, DragEventArgs e)
        {

            e.Effects = DragDropEffects.None;

            //e.Data.GetData(typeof(GraphicMenuTreeNode))
        }

        public void DragLeave(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        public void DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
        }

        public void Drop(object sender, DragEventArgs e)
        {

        }

        public override bool IsSelected
        {
            get => base.IsSelected;
            set
            {
                if (base.IsSelected != value)
                {
                    base.IsSelected = value;
                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(IsSelected)));
                }
            }
        }


        public override void SelectionChange()
        {
        }
    }


  
}

