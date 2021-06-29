using FlavourBusinessFacade.RoomService;
using FlavourBusinessFacade.ServicesContextResources;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System.Collections.Generic;
using System.Linq;

namespace FlavourBusinessManager.RoomService
{
    /// <MetaDataID>{2a2adf89-8607-46c7-844d-d075fcf5d18b}</MetaDataID>
    [BackwardCompatibilityID("{2a2adf89-8607-46c7-844d-d075fcf5d18b}")]
    [Persistent()]
    public class Meal : IMeal
    {

        /// <exclude>Excluded</exclude>
       OOAdvantech.Member< IFoodServiceSession> _Session=new OOAdvantech.Member<IFoodServiceSession>();

        /// <MetaDataID>{df1bef38-aa51-450a-a6c2-bdb6b6f960a5}</MetaDataID>
        [PersistentMember(nameof(_Session))]
        [BackwardCompatibilityID("+1")]
        public IFoodServiceSession Session => _Session.Value;

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <MetaDataID>{5f5f90f7-bfae-4b34-8dc6-c6aa57297db5}</MetaDataID>
        public List<IMealCourse> Courses => throw new System.NotImplementedException();


        protected Meal()
        {

        }
        internal Meal(MenuModel.MealType mealType,List<ItemPreparation> mealItems)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                foreach (var mealCourseItems in (from mealItem in mealItems
                                                 group mealItem by mealItem.SelectedMealCourseTypeUri into mealCourseItems
                                                 select mealCourseItems))
                {
                    var mealCourseType = mealType.Courses.OfType<MenuModel.MealCourseType>().Where(x => ObjectStorage.GetStorageOfObject(x)?.GetPersistentObjectUri(x) == mealCourseItems.Key).First();
                    MealCourse mealCourse = new MealCourse(mealCourseType, mealCourseItems.ToList());
                }

                stateTransition.Consistent = true;
            }



        }
        [BeforeCommitObjectStateInStorageCall]
        protected  void BeforeCommitObjectState()
        {
            foreach (var course in Courses)
            {
                if (ObjectStorage.GetStorageOfObject(course) == null)
                    OOAdvantech.PersistenceLayer.ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(course);
            }
        }
        ///// <MetaDataID>{df1bef38-aa51-450a-a6c2-bdb6b6f960a5}</MetaDataID>
        //public IFoodServiceSession Session => throw new System.NotImplementedException();
    }
}