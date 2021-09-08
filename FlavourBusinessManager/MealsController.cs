using FlavourBusinessFacade.RoomService;
using FlavourBusinessManager.ServicesContextResources;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{c31eb292-1b39-4081-9516-0e0e9e97b10a}</MetaDataID>
    public class MealsController : IMealsController
    {
        public List<IMealCourse> MealCoursesInProgress
        {
            get
            {
                
                //(from foodServiceSession in ServicesContextRunTime.OpenSessions
                // from ss in foodServiceSession.PartialClientSessions
                return new List<IMealCourse>();
            }
        }

        public readonly ServicePointRunTime.ServicesContextRunTime ServicesContextRunTime;
        public MealsController(ServicePointRunTime.ServicesContextRunTime servicesContextRunTime)
        {
            ServicesContextRunTime = servicesContextRunTime;
        }
    }
}