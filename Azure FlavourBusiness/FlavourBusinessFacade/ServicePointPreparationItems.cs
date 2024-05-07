using FlavourBusinessFacade.RoomService;
using OOAdvantech.MetaDataRepository;
using System;
using System.Linq;

namespace FlavourBusinessFacade.ServicesContextResources
{
    /// <MetaDataID>{359a649b-220c-434b-b676-181d2160d449}</MetaDataID>
    public class ServicePointPreparationItems : IDisposable
    {
        [Association("MealCoursePreparation", Roles.RoleA, "e0d04b7f-7608-49f4-80ef-301904e97df2")]
        [RoleAMultiplicityRange(1, 1)]
        public IMealCourse MealCourse;


        public event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{0f0aafc5-6824-4bf8-b2f1-787b2175f9fd}</MetaDataID>
        public ServicePointPreparationItems(IMealCourse mealCourse, System.Collections.Generic.List<RoomService.IItemPreparation> preparationItems)
        {
            MealCourse = mealCourse;
            MealCourse.ObjectChangeState += MealCourse_ObjectChangeState;

            ServicePoint = mealCourse.Meal.Session.ServicePoint;
            PreparationItems = preparationItems;

            if (MealCourse.Meal.Session.SessionType == EndUsers.SessionType.Hall)
                _Description = (MealCourse.Meal.Session.ServicePoint as IHallServicePoint).ServiceArea.Description + " / " + MealCourse.Meal.Session.ServicePoint.Description;

            if (MealCourse.Meal.Session.SessionType == EndUsers.SessionType.HomeDelivery)
                _Description = MealCourse.Meal.Session.DeliveryPlace.Description;


            //_Description = (ServicePoint as IHallServicePoint) .ServiceArea.Description + " / " + ServicePoint.Description;
            _Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(mealCourse)?.GetPersistentObjectUri(mealCourse);


        }

        private void MealCourse_ObjectChangeState(object _object, string member)
        {
            if (nameof(IFoodServiceSession.ServicePoint) == member)
            {
                ServicePoint = MealCourse.Meal.Session.ServicePoint;
                if (ServicePoint is IHallServicePoint)
                    Description = (ServicePoint as IHallServicePoint).ServiceArea.Description + " / " + ServicePoint.Description;
                else if (MealCourse.Meal.Session.SessionType == EndUsers.SessionType.HomeDelivery)
                    Description = MealCourse.Meal.Session.DeliveryPlace.Description;
                else
                    Description = ServicePoint.Description;

                ObjectChangeState?.Invoke(this, null);
            }

        }

        /// <MetaDataID>{a75bdf3b-c001-4d49-816d-384119f61d70}</MetaDataID>
        [OOAdvantech.Json.JsonConstructor]
        public ServicePointPreparationItems(IServicePoint servicePoint, System.Collections.Generic.List<RoomService.IItemPreparation> preparationItems, string description, string uri)
        {
            ServicePoint = servicePoint;
            PreparationItems = preparationItems;
            Description = description;
            _Uri = uri;
        }

        /// <exclude>Excluded</exclude>
        [OOAdvantech.Json.JsonIgnore]
        string _Uri;

        public string Uri
        {
            get
            {
                if (_Uri == null)
                    _Uri = OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(MealCourse)?.GetPersistentObjectUri(MealCourse);
                return _Uri;
            }
        }




        /// <exclude>Excluded</exclude>
        string _Description;
        public string Description
        {
            get
            {
                if (MealCourse != null)
                {
                    if (ServicePoint is IHallServicePoint)
                        _Description = (ServicePoint as IHallServicePoint).ServiceArea.Description + " / " + ServicePoint.Description;
                    else if (MealCourse.Meal.Session.SessionType == EndUsers.SessionType.HomeDelivery)
                        Description = MealCourse.Meal.Session.DeliveryPlace.Description;
                    else
                        _Description = ServicePoint.Description;

                }
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }


        [Association("ServicePointItems", Roles.RoleA, "1b18cbff-1a92-4751-b00e-9fba52c6d00a")]
        [RoleAMultiplicityRange(1, 1)]
        public IServicePoint ServicePoint;



        [Association("StationPreparationItems", Roles.RoleA, "f98b8c21-af79-4d47-861a-654378f15fc1")]
        [RoleAMultiplicityRange(1)]
        [OOAdvantech.MetaDataRepository.RoleBMultiplicityRange(1, 1)]
        public System.Collections.Generic.IList<IItemPreparation> PreparationItems;



        /// <MetaDataID>{eec93dcd-5cb0-40cf-b586-ead4b44c305a}</MetaDataID>
        public string CodeCard
        {
            get
            {
                return PreparationItems.Where(x => !string.IsNullOrWhiteSpace(x.CodeCard)).Select(x => x.CodeCard).FirstOrDefault();
            }
            set
            {
                foreach (var preparationItem in PreparationItems)
                    preparationItem.CodeCard = value;
            }
        }

        public DateTime? ServedAtForecast { get; set; }

        /// <MetaDataID>{c65cde2f-17f7-4077-9b40-6d29e81dac5d}</MetaDataID>
        public void AddPreparationItem(IItemPreparation flavourItem)
        {
            PreparationItems.Add(flavourItem);
        }

        /// <MetaDataID>{b452dae6-2d24-40af-a627-633fcf7e759f}</MetaDataID>
        public void RemovePreparationItem(IItemPreparation flavourItem)
        {
            PreparationItems.Remove(flavourItem);
        }

        public void Dispose()
        {
            MealCourse.ObjectChangeState -= MealCourse_ObjectChangeState;
        }
    }
}