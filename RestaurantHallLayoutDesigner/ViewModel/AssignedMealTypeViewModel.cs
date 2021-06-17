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
    public class AssignedMealTypeViewMode : FBResourceTreeNode,  INotifyPropertyChanged
    {
        MenuModel.IMealType MealType;
        ServicePointPresentation ServicePoint;
        ServiceAreaPresentation ServiceArea;
        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, ServicePointPresentation servicePoint, FLBManager.ViewModel.FBResourceTreeNode parent =null):base(parent)
        {
            MealType = mealType;
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
            ServiceArea = serviceArea;
            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                System.Diagnostics.Debug.WriteLine("sss");
            });

        }

        public WPFUIElementObjectBind.RelayCommand MealTypeSelectCommand { get; set; }
        


        bool? _Assigned;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Assigned
        {
            get
            {
                //return _Assigned;
                if (ServicePoint != null)
                {

                    if (ServicePoint.ServicePoint.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                        _Assigned = true;
                    else
                    if (ServicePoint.ServicePoint.ServiceArea.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                        _Assigned = true;
                }
                else
                if (ServiceArea != null && ServiceArea.ServiceArea.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                    _Assigned = true;
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
                        string mealTypeUri = ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType);
                        if (ServiceArea != null)
                        {
                            ServiceArea.ServiceArea.AddMealType(mealTypeUri);
                            ServiceArea.RefreshMealTypes();
                        }
                        if (ServicePoint != null)
                            ServicePoint.ServicePoint.AddMealType(mealTypeUri);
                    }
                    else
                    {
                        string mealTypeUri = ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType);
                        if (ServiceArea != null)
                        {
                            ServiceArea.ServiceArea.RemoveMealType(mealTypeUri);
                            ServiceArea.RefreshMealTypes();
                        }
                        if (ServicePoint != null)
                            ServicePoint.ServicePoint.RemoveMealType(mealTypeUri);
                    }
                    //Task.Run(() =>
                    //{
                    //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assigned)));
                    //});
                }


            }
        }

        public override string Name { get => MealType.Name; set { } }

        public override ImageSource TreeImage
        {
            get
            {
                return new System.Windows.Media.Imaging.BitmapImage(new Uri(@"pack://application:,,,/RestaurantHallLayoutDesigner;Component/Resources/Images/Metro/MealCoursesClock16.png"));
            }
        }

        public override List<FBResourceTreeNode> Members =>new List<FBResourceTreeNode>();

        public override List<MenuCommand> ContextMenuItems => new List<MenuCommand>();

        public override List<MenuCommand> SelectedItemContextMenuItems => throw new NotImplementedException();

        internal void RefreshMealType()
        {
            _Assigned = null;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assigned)));
        }
    }
}
