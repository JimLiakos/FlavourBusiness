using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Remoting;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{c31eb292-1b39-4081-9516-0e0e9e97b10a}</MetaDataID>
    public class MealsController : System.MarshalByRefObject, IExtMarshalByRefObject, IMealsController
    {
        public List<IMealCourse> MealCoursesInProgress
        {
            get
            {
                var mealCourses = (from openSession in ServicesContextRunTime.OpenSessions
                                   where openSession.Meal != null
                                   from mealCource in openSession.Meal.Courses
                                   orderby mealCource.Meal.Session.ServicePoint.Description, (mealCource as MealCourse).MealCourseTypeOrder//.Courses.IndexOf(mealCource)
                                   select mealCource).ToList();
                //you have to  filter mealcourses by state. 


                
                //(from foodServiceSession in ServicesContextRunTime.OpenSessions
                // from ss in foodServiceSession.PartialClientSessions
                return mealCourses;
            }
        }

        public readonly ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime;

        public event NewMealCoursesInrogressHandel NewMealCoursesInrogress;
        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        public MealsController(ServicePointRunTime.ServicesContextRunTime servicesContextRunTime)
        {
            ServicesContextRunTime = servicesContextRunTime;
        }
        ~MealsController()
        {
            System.Diagnostics.Debug.WriteLine("MealsController");
        }

        internal void OnNewMealCoursesInrogress(List<IMealCourse> mealCourses)
        {
            NewMealCoursesInrogress?.Invoke(mealCourses);
            
            ServicesContextRunTime.OnNewServingBatch(mealCourses);
            //you have to  filter mealcourses by state.
        }

        internal void OnRemoveMealCoursesInrogress(List<IMealCourse> mealCourses)
        {
            ObjectChangeState?.Invoke(this, nameof(MealCoursesInProgress));
            
            //you have to  filter mealcourses by state.
        }
    }


}