using OOAdvantech.Json;
using OOAdvantech.PersistenceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{92e66ca3-9998-4bbb-b241-9ded05b52f43}</MetaDataID>
    public class PartofMeal : IPartofMeal
    {
        public PartofMeal(IMealType mealType, IMenuItem menuItem, IMealCourseType mealCourseType)
        {
            MealType = new MealType(mealType);
            MenuItem = menuItem;

            var uri = ObjectStorage.GetStorageOfObject(mealCourseType)?.GetPersistentObjectUri(mealCourseType);
            _MealCourseType = MealType.Courses.OfType<MealCourseType>().Where(x => x.Uri == uri).FirstOrDefault(); ;
        }
        public readonly IMealCourseType _MealCourseType;
        public IMealCourseType MealCourseType { get => _MealCourseType; set => throw new NotImplementedException(); }

        public IMealType MealType { get; }

        [JsonIgnore]
        public IMenuItem MenuItem { get; }
    }
}
