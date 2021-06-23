using OOAdvantech;
using OOAdvantech.PersistenceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{26ec7ce3-afd5-4867-a4de-b5407c088a62}</MetaDataID>
    public class MealType : IMealType
    {

        public MealType(IMealType mealType, Dictionary<object, object> mappedObject)
        {
            mappedObject[mealType] = this;
            _Name = new Multilingual(mealType.MultilingualName);
            _Courses = (from mealCourseType in mealType.Courses
                        select new MealCourseType(mealCourseType, mappedObject)).OfType<IMealCourseType>().ToList();

            Uri = ObjectStorage.GetStorageOfObject(mealType)?.GetPersistentObjectUri(mealType);
        }

        public string Uri { get; set; }


        public Multilingual MultilingualName { get => new Multilingual(_Name); set { } }


        /// <exclude>Excluded</exclude>
        protected Multilingual _Name = new Multilingual();

        /// <MetaDataID>{37620c73-4bf5-4280-a40d-67fb7ef64614}</MetaDataID>
        public string Name
        {
            get
            {
                return _Name.GetValue<string>();
            }
            set
            {
                _Name.SetValue<string>(value);
            }
        }

        List<IMealCourseType> _Courses;
        public List<IMealCourseType> Courses => _Courses;

        public void MoveMealCourseType(IMealCourseType mealCourseType, int newpos)
        {
            throw new NotImplementedException();
        }

        public IMealCourseType NewMealCourseType()
        {
            throw new NotImplementedException();
        }

        public void RemoveMealCourseType(IMealCourseType mealCourseType)
        {
            throw new NotImplementedException();
        }

        public void SetDefaultMealCourse(IMealCourseType mealCourseType)
        {
            throw new NotImplementedException();
        }
    }
}
