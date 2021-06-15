using FlavourBusinessFacade.ServicesContextResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloorLayoutDesigner.ViewModel
{
    public class AssignedMealTypeViewMode:System.MarshalByRefObject
    {
        MenuModel.IMealType MealType;
        IServicePoint ServicePoint;
        IServiceArea ServiceArea;
        public AssignedMealTypeViewMode(MenuModel.IMealType mealType,IServicePoint servicePoint )
        {
            MealType = mealType;
            ServicePoint = servicePoint;
        }
        public AssignedMealTypeViewMode(MenuModel.MealType mealType, IServiceArea serviceArea)
        {
            MealType = mealType;
            ServiceArea = serviceArea;
        }

        public string Name => MealType.Name;


        bool _Assigned;

        public bool Assigned
        {
            get
            {
                return _Assigned;
                //if (ServicePoint != null)
                //{
                //    if (ServicePoint.ServesMealTypes.Contains(MealType))
                //        return true;
                //    if (ServicePoint.ServiceArea.ServesMealTypes.Contains(MealType))
                //        return true;
                //}
                //if (ServiceArea != null && ServicePoint.ServiceArea.ServesMealTypes.Contains(MealType))
                //    return true;

                //return false;
            }
            set
            {
                _Assigned = value;
                //if (value)
                //{
                //    if (ServiceArea != null)
                //        ServiceArea.AddMealType(MealType);
                //    if (ServicePoint != null)
                //        ServicePoint.AddMealType(MealType);
                //}
                //else
                //{
                //    if (ServiceArea != null)
                //        ServiceArea.RemoveMealType(MealType);
                //    if (ServicePoint != null)
                //        ServicePoint.RemoveMealType(MealType);
                //}

            }
        }
    }
}
