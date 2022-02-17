﻿using OOAdvantech;
using OOAdvantech.Json;
using OOAdvantech.PersistenceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuModel.JsonViewModel
{
    /// <MetaDataID>{b8b3229f-5030-4d3d-82c1-09e1c3e018c0}</MetaDataID>
    public class MealCourseType : IMealCourseType
    {
        public MealCourseType(IMealCourseType mealCourseType, Dictionary<object, object> mappedObject)
        {
            mappedObject[mealCourseType] = this;
            _Name = new Multilingual(mealCourseType.MultilingualName);
            IsDefault = mealCourseType.IsDefault;
            AutoStart = mealCourseType.AutoStart;
            DurationInMinutes = mealCourseType.DurationInMinutes;
            Uri = ObjectStorage.GetStorageOfObject(mealCourseType)?.GetPersistentObjectUri(mealCourseType);
        }
        public MealCourseType()
        {

        }

        public string Uri { get; set; }


        /// <exclude>Excluded</exclude>
        protected Multilingual _Name = new Multilingual();

        public Multilingual MultilingualName { get => new Multilingual(_Name); set { _Name = value; } }

        public double DurationInMinutes { get; set; }
        [JsonIgnore]
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
        public bool IsDefault { get; set; }

        [JsonIgnore]
        public IMealType Meal => throw new NotImplementedException();

        public bool AutoStart { get ; set ; }
    }
}
