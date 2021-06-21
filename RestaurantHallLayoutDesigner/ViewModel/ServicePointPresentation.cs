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
using MenuModel;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    /// <MetaDataID>{0cedc37f-b933-45e3-958f-1f8ec3dec749}</MetaDataID>
    public class ServicePointPresentation : FBResourceTreeNode, INotifyPropertyChanged, IDragDropTarget, IServicePointViewModel
    {
        List<AssignedMealTypeViewMode> _MealTypes;
        public List<AssignedMealTypeViewMode> MealTypes
        {
            get
            {
                if (_MealTypes == null)
                    _MealTypes = (from mealTypeViewModel in ServiceAreaPresentation.MealTypesViewModel.MealTypes
                                  select new AssignedMealTypeViewMode(mealTypeViewModel.MealType, this)).ToList();

                return _MealTypes;
            }
        }

        AssignedMealTypeViewMode _DefaultMealType;
        public AssignedMealTypeViewMode DefaultMealType
        {
            get
            {
                if (_DefaultMealType == null)
                    _DefaultMealType = MealTypes.FirstOrDefault();
                return _DefaultMealType;

            }
            set
            {
                _DefaultMealType = value;
                if (_DefaultMealType != null)
                    _DefaultMealType.Assigned = true;
            }
        }

        bool _MealTypesViewExpanded;
        public bool MealTypesViewExpanded
        {
            get => _MealTypesViewExpanded;
            set
            {
                _MealTypesViewExpanded = value;
                if (!value)
                {
                    if (_DefaultMealType != null)
                        _DefaultMealType.Assigned = true;
                }
            }
        }


        public RelayCommand MealTypeSelectCommand { get; set; }


        public override bool IsEditable => true;

        private readonly ServiceAreaPresentation ServiceAreaPresentation;

        public IServicePoint ServicePoint { get; private set; }

        public System.Windows.Visibility CheckBoxVisibility { get; set; } = Visibility.Collapsed;
        public ServicePointPresentation(IServicePoint servicePoint, FBResourceTreeNode parent ) : base(parent)
        {
            ServiceAreaPresentation = parent as ServiceAreaPresentation;
            ServicePoint = servicePoint;
            _Name = Properties.Resources.LoadingPrompt;
            Task.Run(() =>
            {
                _Name = servicePoint.Description;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
            });

            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                MealTypesViewExpanded = false;
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(MealTypesViewExpanded)));
                RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefaultMealType)));

            });


        }

        public ServicePointPresentation(IServicePoint servicePoint, FBResourceTreeNode parent, MenuItemsEditor.ViewModel.MealTypesViewModel mealTypesViewModel) : this(servicePoint, parent)
        {
            ServiceAreaPresentation = parent as ServiceAreaPresentation;
            _MealTypes = mealTypesViewModel.MealTypes.Select(x => new AssignedMealTypeViewMode(x.MealType, this)).ToList();
            _Members = _MealTypes.OfType<FBResourceTreeNode>().ToList();
            var _default = _Members.OfType<AssignedMealTypeViewMode>().Where(x => x.Assigned && x.Default).FirstOrDefault();
            if (_default != null)
            {
                _Members.Remove(_default);
                _Members.Insert(0, _default);
                _Members = _Members.ToList();
            }

        }

        public override List<MenuCommand> ContextMenuItems
        {
            get
            {
                return new List<MenuCommand>();
            }
        }

        List<FBResourceTreeNode> _Members = new List<FBResourceTreeNode>();
        public override List<FBResourceTreeNode> Members
        {
            get
            {
                return _Members;
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

        public void RefreshMealTypes()
        {

            foreach (var mealType in ServiceAreaPresentation.MealTypesViewModel.MealTypes)
            {
                if (MealTypes.Where(x => x.MealType == mealType.MealType).FirstOrDefault() == null)
                    MealTypes.Add(new AssignedMealTypeViewMode(mealType.MealType, ServiceAreaPresentation, this));
            }
            foreach (var assignedMealType in MealTypes.ToList())
            {
                if (ServiceAreaPresentation.MealTypesViewModel.MealTypes.Where(x => x.MealType == assignedMealType.MealType).FirstOrDefault() == null)
                {
                    if (assignedMealType.Assigned)
                        assignedMealType.Assigned = false;
                    MealTypes.Remove(assignedMealType);
                }
            }


            foreach (var mealTypeAssignment in _MealTypes)
                mealTypeAssignment.RefreshMealType();

            _Members = _MealTypes.OfType<FBResourceTreeNode>().ToList();


            var _default = _Members.OfType<AssignedMealTypeViewMode>().Where(x => x.Default).FirstOrDefault();
            if (_default != null)
            {
                _Members.Remove(_default);
                _Members.Insert(0, _default);
                _Members = _Members.ToList();
            }




            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(DefaultMealType)));
        }
    }



}

