using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.Remoting;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{c31eb292-1b39-4081-9516-0e0e9e97b10a}</MetaDataID>
    public class MealsController :System.MarshalByRefObject, IExtMarshalByRefObject,IMealsController
    {
        public List<IMealCourse> MealCoursesInProgress
        {
            get
            {
                var mealCourses = (from openSession in ServicesContextRunTime.OpenSessions
                                    from mealCource in openSession.Meal.Courses
                                    select mealCource).ToList();



                                    //(from foodServiceSession in ServicesContextRunTime.OpenSessions
                // from ss in foodServiceSession.PartialClientSessions
                return mealCourses;
            }
        }

        public readonly ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime;
        public MealsController(ServicePointRunTime.ServicesContextRunTime servicesContextRunTime)
        {
            ServicesContextRunTime = servicesContextRunTime;
        }
        ~MealsController()
        {
            System.Diagnostics.Debug.WriteLine("MealsController");
        }
    }

   
}