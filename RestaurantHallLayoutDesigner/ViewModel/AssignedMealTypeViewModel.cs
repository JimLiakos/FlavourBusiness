using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.PersistenceLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace FloorLayoutDesigner.ViewModel
{
    public class AssignedMealTypeViewMode : MarshalByRefObject, INotifyPropertyChanged
    {
        MenuModel.IMealType MealType;
        IServicePoint ServicePoint;
        IServiceArea ServiceArea;
        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, IServicePoint servicePoint)
        {
            MealType = mealType;
            ServicePoint = servicePoint;
            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                System.Diagnostics.Debug.WriteLine("sss");
            });

        }
        public AssignedMealTypeViewMode(MenuModel.IMealType mealType, IServiceArea serviceArea)
        {
            MealType = mealType;
            ServiceArea = serviceArea;
            MealTypeSelectCommand = new RelayCommand((object sender) =>
            {
                System.Diagnostics.Debug.WriteLine("sss");
            });

        }

        public WPFUIElementObjectBind.RelayCommand MealTypeSelectCommand { get; set; }
        public string Name => MealType.Name;


        bool _Assigned;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool Assigned
        {
            get
            {
                //return _Assigned;
                if (ServicePoint != null)
                {
                    if (ServicePoint.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                        return true;
                    if (ServicePoint.ServiceArea.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject( MealType).GetPersistentObjectUri(MealType)))
                        return true;
                }
                if (ServiceArea != null && ServiceArea.ServesMealTypesUris.Contains(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)))
                    return true;

                return false;
            }
            set
            {
                _Assigned = value;
                if (value)
                {
                    if (ServiceArea != null)
                        ServiceArea.AddMealType(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType));
                    if (ServicePoint != null)
                        ServicePoint.AddMealType(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType));
                }
                else
                {
                    if (ServiceArea != null)
                        ServiceArea.RemoveMealType(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType)); 
                    if (ServicePoint != null)
                        ServicePoint.RemoveMealType(ObjectStorage.GetStorageOfObject(MealType).GetPersistentObjectUri(MealType));
                }

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Assigned)));
            }
        }
    }
}
