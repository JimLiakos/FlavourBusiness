using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using FlavourBusinessFacade.ServicesContextResources;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FLBManager.ViewModel.Preparation
{
    public class ServicePointsPreparationInfoPresentation: FBResourceTreeNode, INotifyPropertyChanged
    {

        public readonly IPreparationForInfo ServicePointsPreparationInfo;
        PreparationStationPresentation PreparationStationPresentation;
        //ItemsPreparationInfoPresentation ItemsPreparationInfoTreeNode;
        internal IServiceArea ServiceArea;
        IServicePoint ServicePoint;


        public ServicePointsPreparationInfoPresentation(PreparationStationPresentation parent, IPreparationForInfo servicePointsPreparationInfo, bool selectionCheckBox) : base(parent)
        {
            SelectionCheckBox = selectionCheckBox;
            ServicePointsPreparationInfo = servicePointsPreparationInfo;
            PreparationStationPresentation = parent;
            ServiceArea = servicePointsPreparationInfo.ServiceArea;

            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });


        }

        bool SelectionCheckBox;





        ViewModelWrappers<IServicePoint,ServicePointsPreparationInfoPresentation> ServicePoints = new ViewModelWrappers<IServicePoint, ServicePointsPreparationInfoPresentation>();


        
        



        public bool HasServicePoints
        {
            get
            {
                if (this.ServiceArea != null && this.PreparationStationPresentation.StationPrepareForServicePoints(this.ServiceArea))
                    return true;
                else if (this.ServicePoint != null && this.PreparationStationPresentation.StationPrepareForServicePoint(this.ServicePoint))
                    return true;
                else
                {
                    if (ServiceArea != null)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ServiceArea.ServicePoints

                                                                  select MultiSelectSubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, SelectionCheckBox)).Union(
                             (from menuItem in ServiceArea.ClassifiedItems.OfType<MenuModel.MenuItem>()
                              select MultiSelectMenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, SelectionCheckBox)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();

                        foreach (var member in members.OfType<ItemsPreparationInfoPresentation>())
                        {
                            if (member.HasItemsToPrepared)
                                return true;
                        }
                    }
                    return false;
                }
            }
        }

        public double PreparationTimeSpanInMin
        {
            get
            {

                if (this.ServiceArea != null)
                    return this.PreparationStationPresentation.GetPreparationTimeSpanInMin(this.ServiceArea);

                if (this.MenuItem != null)
                    return this.PreparationStationPresentation.GetPreparationTimeSpanInMin(this.MenuItem);

                return 1;

            }
            set
            {

                if (this.ServiceArea != null)
                    this.PreparationStationPresentation.SetPreparationTimeSpanInMin(this.ServiceArea, value);

                if (this.MenuItem != null)
                    this.PreparationStationPresentation.SetPreparationTimeSpanInMin(this.MenuItem, value);
            }
        }

        public bool CanPrepared
        {
            get
            {
                if (this.ServiceArea != null && this.PreparationStationPresentation.StationPrepareItems(this.ServiceArea))
                    return true;
                else if (this.MenuItem != null && this.PreparationStationPresentation.StationPrepareItem(this.MenuItem))
                    return true;
                else
                    return false;
            }
            set
            {
                if (value && ServiceArea != null)
                    this.PreparationStationPresentation.IncludeItems(ServiceArea);

                if (!value && ServiceArea != null)
                    this.PreparationStationPresentation.ExcludeItems(ServiceArea);


                if (value && MenuItem != null)
                {
                    this.PreparationStationPresentation.IncludeItem(MenuItem);
                    Refresh();
                }

                if (!value && MenuItem != null)
                    this.PreparationStationPresentation.ExcludeItem(MenuItem);


            }
        }
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }
        private void Delete()
        {
            if (ServiceArea != null)
                PreparationStationPresentation.ExcludeServicePoints(ServiceArea);

            if (this.ServicePoint != null)
                PreparationStationPresentation.ExcludeServicePoint(ServicePoint);

        }

        

        public override string Name
        {
            get
            {
                if (ServiceArea != null)
                    return ServiceArea.Description;
                else if (this.ServicePoint != null)
                    return this.ServicePoint.Description;
                else
                    return "";
            }
            set
            {
            }
        }



        public override ImageSource TreeImage
        {
            get
            {
                if (ServiceArea != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/category16.png"));
                else if (this.MenuItem != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/MenuItem16.png"));
                else
                    return null;
            }
        }

        public override List<FBResourceTreeNode> Members
        {
            get
            {

                if (ServiceArea != null)
                {

                    if (SelectionCheckBox)
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ServiceArea.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select MultiSelectSubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, SelectionCheckBox)).Union(
                           (from menuItem in ServiceArea.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MultiSelectMenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, SelectionCheckBox)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();
                        return members;
                    }
                    else
                    {
                        var itemsPreparationInfosPresentations = (from subCategory in ServiceArea.ClassifiedItems.OfType<MenuModel.IItemsCategory>()
                                                                  select SubCategories.GetViewModelFor(subCategory, this, PreparationStationPresentation, subCategory, SelectionCheckBox)).Union(
                           (from menuItem in ServiceArea.ClassifiedItems.OfType<MenuModel.MenuItem>()
                            select MenuItems.GetViewModelFor(menuItem, this, PreparationStationPresentation, menuItem, SelectionCheckBox)));

                        var members = (from itemsPreparationInfoPresentation in itemsPreparationInfosPresentations
                                       where itemsPreparationInfoPresentation.HasItemsToPrepared
                                       select itemsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();
                        return members;

                    }
                }
                else
                    return new List<FBResourceTreeNode>();
            }
        }
        public RelayCommand DeleteCommand { get; protected set; }

        /// <exclude>Excluded</exclude>
        List<MenuCommand> _ContextMenuItems;

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
                    imageSource = new BitmapImage(new Uri(@"pack://application:,,,/MenuItemsEditor;Component/Image/delete.png"));
                    menuItem.Header = Properties.Resources.RemoveCallerIDServer;
                    menuItem.Icon = new System.Windows.Controls.Image() { Source = imageSource, Width = 16, Height = 16 };
                    menuItem.Command = DeleteCommand;

                    _ContextMenuItems.Add(menuItem);



                }

                return _ContextMenuItems;
            }
        }
        public override bool HasContextMenu
        {
            get
            {
                return true;
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


        public void Refresh()
        {
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(CanPrepared)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(PreparationTimeSpanInMin)));


            foreach (var itemsPreparationInfoPresentation in Members.OfType<ItemsPreparationInfoPresentation>())
                itemsPreparationInfoPresentation.Refresh();

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }
    }
}
