using OOAdvantech.MetaDataRepository;
using OOAdvantech.PersistenceLayer;
using OOAdvantech.Transactions;
using System;
using System.Linq;
using System.Collections.Generic;
using OOAdvantech;
using OOAdvantech.Json;

namespace MenuModel
{
    /// <MetaDataID>{692b8238-03dd-48de-a7df-a734878d6791}</MetaDataID>
    [BackwardCompatibilityID("{692b8238-03dd-48de-a7df-a734878d6791}")]
    [Persistent()]
    public class MealType : IMealType
    {
        /// <MetaDataID>{97478e3f-39d5-4caa-bae7-3114a53758ca}</MetaDataID>
        public void SetDefaultMealCourse(IMealCourseType mealCourseType)
        {
            lock (CoursesLock)
            {
                if (mealCourseType != null)
                {
                    var defaultMealCourseType = Courses.OfType<MealCourseType>().Where(x => x.IsDefault).FirstOrDefault();
                    if (defaultMealCourseType == mealCourseType)
                        return;
                    else
                    {

                        if (defaultMealCourseType != null)
                            defaultMealCourseType.IsDefault = false;
                        (mealCourseType as MealCourseType).IsDefault = true;
                    }
                }



            }

        }

        /// <MetaDataID>{5810ad84-2397-4473-8fb1-84fc190dea02}</MetaDataID>
        public MealType()
        {
            //var dd= new MealType() { MultilingualName=new Multilingual(),Courses=null,}
        }

        /// <MetaDataID>{b3800a3d-8285-4c95-a202-2a46a7c0e82c}</MetaDataID>
        string _MealTypeUri;
        public string MealTypeUri
        {
            get
            {
                if (_MealTypeUri == null)
                {
                    var objectStorage = ObjectStorage.GetStorageOfObject(this);
                    if (objectStorage != null)
                        _MealTypeUri = objectStorage.GetPersistentObjectUri(this);
                    else
                        _MealTypeUri = "";
                }
                return _MealTypeUri;
            }
        }



        [JsonConstructor]
        public MealType(Multilingual multilingualName, List<IMealCourseType> courses, string mealTypeUri)
        {
            _MealTypeUri = mealTypeUri;
            _Name = new MultilingualMember<string>(multilingualName.Values);

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this, TransactionOption.Suppress))
            {
                _Courses.AddRange(courses);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{07379802-01d0-4732-8c85-fe4a4bd4a0ab}</MetaDataID>
        public MealType(string mealTypeDefaultName)
        {
            _Name.Value = mealTypeDefaultName;
        }




        /// <MetaDataID>{3afdc45d-df0d-47d0-962a-a39ad502a427}</MetaDataID>
        public virtual IMealCourseType NewMealCourseType()
        {
            lock (CoursesLock)
            {
                MealCourseType mealCourseType;
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    mealCourseType = new MealCourseType(MenuModel.Properties.Resources.NewMealCourseName);
                    if (_Courses.Where(x => x.IsDefault).Count() == 0)
                        mealCourseType.IsDefault = true;

                    ObjectStorage.GetStorageOfObject(this).CommitTransientObjectState(mealCourseType);
                    _Courses.Add(mealCourseType);
                    stateTransition.Consistent = true;
                }
                return mealCourseType;
            }
        }

        /// <MetaDataID>{f9bb0ec8-6183-47c2-999a-fe16d254855c}</MetaDataID>
        public virtual void RemoveMealCourseType(IMealCourseType mealCourseType)
        {
            lock (CoursesLock)
            {
                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Courses.Remove(mealCourseType);
                    if (mealCourseType.IsDefault)
                    {
                        var firstMealCourseType = _Courses.OfType<MealCourseType>().FirstOrDefault();
                        if (firstMealCourseType != null)
                            firstMealCourseType.IsDefault = true;
                    }

                    stateTransition.Consistent = true;
                }
            }
        }

        /// <MetaDataID>{ba1dee2a-7878-469a-8f5d-6c99f22d810c}</MetaDataID>
        public virtual void MoveMealCourseType(IMealCourseType mealCourseType, int newpos)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Courses.Remove(mealCourseType);
                _Courses.Insert(newpos, mealCourseType);
                stateTransition.Consistent = true;
            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;



        object CoursesLock = new object();

        /// <exclude>Excluded</exclude>
        protected OOAdvantech.Collections.Generic.Set<IMealCourseType> _Courses = new OOAdvantech.Collections.Generic.Set<IMealCourseType>();
        /// <MetaDataID>{42d87915-6995-487a-b79b-140a9aa86819}</MetaDataID>
        [PersistentMember(nameof(_Courses))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+2")]
        public List<IMealCourseType> Courses => _Courses.ToThreadSafeList();

        /// <exclude>Excluded</exclude> 
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{5e64b818-62ab-43f0-bfc1-f8b1182b5ae1}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+1")]
        [JsonIgnore]
        public string Name
        {
            get => _Name;
            set
            {
                if (_Name != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Name.Value = value;
                        stateTransition.Consistent = true;
                    }

                }
            }
        }

        /// <MetaDataID>{89908af3-c554-41ae-84a5-529d8898619e}</MetaDataID>
        public Multilingual MultilingualName => new Multilingual(_Name);


        /// <MetaDataID>{171ffe1b-291f-443e-af84-a644b7dc164b}</MetaDataID>
        public static void UpdateStorage(ObjectStorage objectStorage)
        {
            OOAdvantech.Linq.Storage storage = new OOAdvantech.Linq.Storage(objectStorage);

            var mealTypes = (from mealType in storage.GetObjectCollection<MenuModel.FixedMealType>()
                             select mealType).ToList();
            var oneCourseMealType = mealTypes.Where(x => x.Courses.Count() == 1).FirstOrDefault();//Takeaway HomeDelivery meal type

            var twoCourseMealType = mealTypes.Where(x => x.Courses.Count() == 2).FirstOrDefault();
            var threeCourseMealType = mealTypes.Where(x => x.Courses.Count() == 3).FirstOrDefault();
            var fourCourseMealType = mealTypes.Where(x => x.Courses.Count() == 4).FirstOrDefault();

            using (SystemStateTransition stateTransition = new SystemStateTransition(TransactionOption.Required))
            {

                if (oneCourseMealType == null)
                {
                    List<MenuModel.IMealCourseType> mealCourses = new List<IMealCourseType>();

                    var mealCourse = new MealCourseType(MenuModel.Properties.Resources.MainMealCourseName);
                    mealCourse.IsDefault = true;
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    oneCourseMealType = new FixedMealType(MenuModel.Properties.Resources.OneCourseMealTypeName, mealCourses);
                    objectStorage.CommitTransientObjectState(oneCourseMealType);

                }

                if (twoCourseMealType == null)
                {
                    List<MenuModel.IMealCourseType> mealCourses = new List<IMealCourseType>();
                    var mealCourse = new MealCourseType(MenuModel.Properties.Resources.AppetizerMealCourseName);
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    mealCourse = new MealCourseType(MenuModel.Properties.Resources.MainMealCourseName);
                    mealCourse.IsDefault = true;
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    twoCourseMealType = new FixedMealType(MenuModel.Properties.Resources.TwoCourseMealTypeName, mealCourses);
                    objectStorage.CommitTransientObjectState(twoCourseMealType);

                }

                if (threeCourseMealType == null)
                {

                    List<MenuModel.IMealCourseType> mealCourses = new List<IMealCourseType>();
                    var mealCourse = new MealCourseType(MenuModel.Properties.Resources.AppetizerMealCourseName);
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    mealCourse = new MealCourseType(MenuModel.Properties.Resources.MainMealCourseName);
                    mealCourse.IsDefault = true;
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    mealCourse = new MealCourseType(MenuModel.Properties.Resources.DessertMealCourseName);
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);

                    threeCourseMealType = new FixedMealType(MenuModel.Properties.Resources.ThreeCourseMealTypeName, mealCourses);
                    objectStorage.CommitTransientObjectState(threeCourseMealType);

                }

                if (fourCourseMealType == null)
                {

                    List<MenuModel.IMealCourseType> mealCourses = new List<IMealCourseType>();
                    var mealCourse = new MealCourseType(MenuModel.Properties.Resources.AppetizerMealCourseName);
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    mealCourse = new MealCourseType(MenuModel.Properties.Resources.HorsDOeuvreMealCourseName);
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    mealCourse = new MealCourseType(MenuModel.Properties.Resources.MainMealCourseName);
                    mealCourse.IsDefault = true;
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    mealCourse = new MealCourseType(MenuModel.Properties.Resources.DessertMealCourseName);
                    objectStorage.CommitTransientObjectState(mealCourse);
                    mealCourses.Add(mealCourse);
                    fourCourseMealType = new FixedMealType(MenuModel.Properties.Resources.FourCourseMealTypeName, mealCourses);
                    objectStorage.CommitTransientObjectState(fourCourseMealType);


                }


                stateTransition.Consistent = true;
            }

        }
    }
}