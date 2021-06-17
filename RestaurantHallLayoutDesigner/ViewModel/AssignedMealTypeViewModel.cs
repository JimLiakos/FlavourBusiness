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
        IServicePoint ServicePoint;
        IServiceArea ServiceArea;
        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, IServicePoint servicePoint, FLBManager.ViewModel.FBResourceTreeNode parent =null):base(parent)
        {
            MealType = mealType;
            ServicePoint = servicePoint;
            ServicePoint.ObjectChangeState += ServicePoint_ObjectChangeState;
            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                System.Diagnostics.Debug.WriteLine("sss");
            });

        }

        private void ServicePoint_ObjectChangeState(object _object, string member)
        {
            var sdsd = ServicePoint.ServesMealTypesUris;
        }

        public override void SelectionChange()
        {
            throw new NotImplementedException();
        }

        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, IServiceArea serviceArea, FLBManager.ViewModel.FBResourceTreeNode parent = null) : base(parent)
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

                    if (ServicePoint.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                        _Assigned = true;
                    else
                    if (ServicePoint.ServiceArea.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                        _Assigned = true;
                }
                else
                if (ServiceArea != null && ServiceArea.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
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
                            ServiceArea.AddMealType(mealTypeUri);
                        if (ServicePoint != null)
                            ServicePoint.AddMealType(mealTypeUri);
                    }
                    else
                    {
                        string mealTypeUri = ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType);
                        if (ServiceArea != null)
                            ServiceArea.RemoveMealType(mealTypeUri);
                        if (ServicePoint != null)
                            ServicePoint.RemoveMealType(mealTypeUri);
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
    }
}
