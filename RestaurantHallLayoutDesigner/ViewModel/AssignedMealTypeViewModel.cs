using FlavourBusinessFacade.ServicesContextResources;
using FLBManager.ViewModel;
using OOAdvantech.PersistenceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    public class AssignedMealTypeViewMode : FBResourceTreeNode
    {
       public readonly  MenuModel.IMealType MealType;
       public readonly string MealTypeUri;

        ServicePointPresentation ServicePoint;
        ServiceAreaPresentation ServiceArea;
        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, ServicePointPresentation servicePoint, FLBManager.ViewModel.FBResourceTreeNode parent = null) : base(parent)
        {
            MealType = mealType;

            MealTypeUri = ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType);

            ServicePoint = servicePoint;

            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                System.Diagnostics.Debug.WriteLine("sss");
            });

        }



        public override void SelectionChange()
        {

        }

        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, ServiceAreaPresentation serviceArea, FLBManager.ViewModel.FBResourceTreeNode parent = null) : base(parent)
        {
            MealType = mealType;
            MealTypeUri = ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType);

            ServiceArea = serviceArea;
            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                System.Diagnostics.Debug.WriteLine("sss");
            });

        }

        public WPFUIElementObjectBind.RelayCommand MealTypeSelectCommand { get; set; }




        public bool Default { get; set; }

        bool? _Assigned;
        public bool Assigned
        {
            get
            {
                //return _Assigned;
                if (_Assigned != null)
                    return _Assigned.Value;

                Default = false;
                if (ServicePoint != null)
                {

                    if (ServicePoint.ServicePoint.ServesMealTypesUris.Contains(MealTypeUri))
                    {
                        Default = ServicePoint.ServicePoint.ServesMealTypesUris.IndexOf(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)) == 0;
                        _Assigned = true;
                    }
                    else
                    if (ServicePoint.ServicePoint.ServiceArea.ServesMealTypesUris.Contains(MealTypeUri))
                    {
                        if(ServicePoint.ServicePoint.ServesMealTypesUris.Count==0)
                            Default = ServicePoint.ServicePoint.ServiceArea.ServesMealTypesUris.IndexOf(MealTypeUri) == 0;
                        _Assigned = true;
                    }
                }
                else
                if (ServiceArea != null && ServiceArea.ServiceArea.ServesMealTypesUris.Contains(MealTypeUri))
                {
                    Default = ServiceArea.ServiceArea.ServesMealTypesUris.IndexOf(MealTypeUri) == 0;
                    _Assigned = true;
                }
                if (_Assigned != null)
                    return _Assigned.Value;
                else
                    return false;
            }
            set
            {
                if (_Assigned != value)
                {
                    _Assigned = value;
                    if (value)
                    {
                        
                        if (ServiceArea != null)
                        {
                            ServiceArea.ServiceArea.AddMealType(MealTypeUri);
                        }
                        if (ServicePoint != null)
                            ServicePoint.ServicePoint.AddMealType(MealTypeUri);
                    }
                    else
                    {
                        
                        if (ServiceArea != null)
                        {
                            ServiceArea.ServiceArea.RemoveMealType(MealTypeUri);
                        }
                        if (ServicePoint != null)
                            ServicePoint.ServicePoint.RemoveMealType(MealTypeUri);
                    }
                    if (_Assigned != null && ServiceArea != null)
                    {
                        Default = ServiceArea.ServiceArea.ServesMealTypesUris.IndexOf(MealTypeUri) == 0;
                    }

                    if (ServicePoint != null)
                        ServicePoint.RefreshMealTypes();

                    RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
                    if (ServiceArea != null)
                    {
                        Task.Run(() =>
                        {
                            ServiceArea.RefreshMealTypes();
                        });
                    }

                }


            }
        }

        public override string Name
        {
            get
            {
                var name = MealType.Name;
                if (Default)
                    name = name + " (default)";
                return name;
            }
            set { }
        }

        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/MealCoursesClock16.png"));
            }
        }

        public override List<FBResourceTreeNode> Members => new List<FBResourceTreeNode>();

        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

        public override List<MenuCommand> SelectedItemContextMenuItems => throw new NotImplementedException();

        internal void RefreshMealType()
        {
            _Assigned = null;
            var assigned = Assigned;
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Assigned)));
            RunPropertyChanged(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
}
