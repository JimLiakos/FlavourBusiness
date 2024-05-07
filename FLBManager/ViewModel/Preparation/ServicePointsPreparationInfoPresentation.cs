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
using UIBaseEx;

namespace FLBManager.ViewModel.Preparation
{
    /// <MetaDataID>{38127261-e172-4d32-a9f9-7e052af99294}</MetaDataID>
    public class ServicePointsPreparationInfoPresentation : FBResourceTreeNode, INotifyPropertyChanged
    {
        public override void RemoveChild(FBResourceTreeNode treeNode)
        {
            throw new NotImplementedException();
        }
        public IPreparationForInfo ServicePointsPreparationInfo { get; private set; }

        PreparationStationPresentation PreparationStationPresentation;

        public ServicePointsPreparationInfoPresentation ServicePointsPreparationInfoTreeNode { get; }


        public bool IsCooked
        {
            get
            {
                return false;
            }
        }

        public bool HasAppearanceOrderValue
        {
            get
            {
                return false;
            }
        }
        public bool HasTags
        {
            get
            {
                return false;
            }
        }



        //ItemsPreparationInfoPresentation ItemsPreparationInfoTreeNode;
        internal IServiceArea ServiceArea;
        IServicePoint ServicePoint;

        //this, PreparationStationPresentation, servicePoint, SelectionCheckBox)
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










        public bool HasServicePoints
        {
            get
            {
                if (this.ServiceArea != null && this.PreparationStationPresentation.StationPreparesForServicePoints(this.ServiceArea))
                    return true;
                else if (this.ServicePoint != null && this.PreparationStationPresentation.StationPreparesForServicePoint(this.ServicePoint))
                    return true;
                return false;

            }
        }

        public bool PreparationTimeIsVisible
        {
            get => false;
        }
        public bool CanPrepared
        {
            get
            {

                if (this.ServiceArea != null && this.PreparationStationPresentation.StationPreparesForServicePoints(this.ServiceArea))
                    return true;
                else
                if (this.ServicePoint != null && this.PreparationStationPresentation.StationPreparesForServicePoint(this.ServicePoint))
                    return true;
                else
                    return false;
            }
            set
            {
                if (value && ServiceArea != null)
                    this.PreparationStationPresentation.IncludeServicePoints(ServiceArea);

                if (!value && ServiceArea != null)
                    this.PreparationStationPresentation.ExcludeServicePoints(ServiceArea);


                if (value && ServicePoint != null)
                {
                    this.ServicePointsPreparationInfo = this.PreparationStationPresentation.IncludeServicePoint(ServicePoint);
                    Refresh();
                }

                if (!value && ServicePoint != null)
                    this.PreparationStationPresentation.ExcludeServicePoint(ServicePoint);


            }
        }
        public System.Windows.Visibility CheckBoxVisibility
        {
            get; set;
        }
        private void Delete()
        {
            if (ServiceArea != null)
            {
                PreparationStationPresentation.RemoveChild(this);
            }

            if (this.ServicePoint != null)
                this.ServicePointsPreparationInfo = PreparationStationPresentation.ExcludeServicePoint(ServicePoint);

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
                    return new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServiceArea.png"));
                else if (this.ServicePoint != null)
                    return new BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/ServicePoint.png"));
                else
                    return null;
            }
        }
        ViewModelWrappers<IServicePoint, ServicePointsPreparationInfoPresentation> ServicePoints = new ViewModelWrappers<IServicePoint, ServicePointsPreparationInfoPresentation>();

        ViewModelWrappers<IServicePoint, ServicePointsPreparationInfoPresentation> MultiSelectServicePoints = new ViewModelWrappers<IServicePoint, ServicePointsPreparationInfoPresentation>();




        public override List<FBResourceTreeNode> Members
        {
            get
            {

                if (ServiceArea != null)
                {

                    if (SelectionCheckBox)
                    {
                        var servicePointsPreparationInfoPresentations = (from servicePoint in ServiceArea.ServicePoints
                                                                         select MultiSelectServicePoints.GetViewModelFor(servicePoint, this, PreparationStationPresentation, servicePoint, SelectionCheckBox));

                        var members = (from servicePointsPreparationInfoPresentation in servicePointsPreparationInfoPresentations
                                       select servicePointsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();
                        return members;
                    }
                    else
                    {
                        var servicePointsPreparationInfoPresentations = (from servicePoint in ServiceArea.ServicePoints
                                                                         select MultiSelectServicePoints.GetViewModelFor(servicePoint, this, PreparationStationPresentation, servicePoint, SelectionCheckBox));

                        var members = (from servicePointsPreparationInfoPresentation in servicePointsPreparationInfoPresentations
                                       where servicePointsPreparationInfoPresentation.HasServicePoints
                                       select servicePointsPreparationInfoPresentation).OfType<FBResourceTreeNode>().ToList();

                        return members;

                    }
                }
                else
                    return new List<FBResourceTreeNode>();
            }
        }

        public ServicePointsPreparationInfoPresentation(ServicePointsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, IPreparationForInfo servicePointsPreparationInfo, bool selectionCheckBox) : base(parent)
        {
            PreparationStationPresentation = preparationStationPresentation;
        }

        //public ServicePointsPreparationInfoPresentation(ServicePointsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, IServiceArea serviceArea, bool selectionCheckBox) : base(parent)
        //{
        //    SelectionCheckBox = selectionCheckBox;
        //    PreparationStationPresentation = preparationStationPresentation;

        //    ServicePointsPreparationInfoPresentation = parent;
        //    ServiceArea = serviceArea;

        //    DeleteCommand = new RelayCommand((object sender) =>
        //    {
        //        Delete();
        //    });
        //    CheckBoxVisibility = System.Windows.Visibility.Visible;
        //}


        public ServicePointsPreparationInfoPresentation(ServicePointsPreparationInfoPresentation parent, PreparationStationPresentation preparationStationPresentation, IServicePoint servicePoint, bool selectionCheckBox) : base(parent)
        {
            SelectionCheckBox = selectionCheckBox;
            PreparationStationPresentation = preparationStationPresentation;
            ServicePointsPreparationInfoTreeNode = parent;
            ServicePoint = servicePoint;
            DeleteCommand = new RelayCommand((object sender) =>
            {
                Delete();
            });
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


            foreach (var servicePointsPreparationInfoPresentation in Members.OfType<ServicePointsPreparationInfoPresentation>())
                servicePointsPreparationInfoPresentation.Refresh();

            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Members)));

        }

        internal List<IPreparationForInfo> GetAllPreparationForInfo()
        {
            if ((ServiceArea != null))
            {

                var preparationForInfoList = this.PreparationStationPresentation.PreparationForInfos.Where(x => x.ServicePoint is IHallServicePoint && (x.ServicePoint as IHallServicePoint).ServiceArea == ServiceArea).ToList();

                if (ServicePointsPreparationInfo != null)
                    preparationForInfoList.Add(ServicePointsPreparationInfo);

                return preparationForInfoList;

            }
            if (ServicePointsPreparationInfo != null)
                return new List<IPreparationForInfo>() { this.ServicePointsPreparationInfo };
            else
                return new List<IPreparationForInfo>();
        }
    }
}
