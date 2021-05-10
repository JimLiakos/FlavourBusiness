using OOAdvantech.MetaDataRepository;
using System;

namespace MenuModel
{
    /// <MetaDataID>{2bdb24af-64a1-4e2f-a5a8-926120aba035}</MetaDataID>
    [BackwardCompatibilityID("{2bdb24af-64a1-4e2f-a5a8-926120aba035}")]
    [Persistent()]
    public class FixedMealType : MealType
    {
        /// <MetaDataID>{e5dff1b6-43c7-40df-8197-47effa1a0929}</MetaDataID>
        public FixedMealType(string mealTypeDefaultName):base(mealTypeDefaultName)
        {
        }
        protected FixedMealType()
        {

        }

        public override void MoveMealCourseType(IMealCourseType mealCourseType, int newpos)
        {
            throw new NotImplementedException("You can't change meal course position of fixed meal type.");
        }
        //public override void AddMealCourseType(IMealCourseType mealCourseType)
        //{
        //    throw new NotImplementedException("You can't add meal course to the fixed meal type.");
        //}

        public override IMealCourseType NewMealCourseType()
        {
            throw new NotImplementedException("You can't add meal course to the fixed meal type.");
        }

        public override void RemoveMealCourseType(IMealCourseType mealCourseType)
        {
            throw new NotImplementedException("You can't remove meal course to the fixed meal type.");
        }
    }
}