using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{d6b8d9b8-631e-41c9-95bc-21011b6812dd}</MetaDataID>
    public interface IMealType
    {
        /// <MetaDataID>{cde546b3-05c0-4510-8845-bfbd7369e9f8}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        Multilingual MultilingualName { get; }


        /// <MetaDataID>{a70c9aa4-9649-4cd9-b84e-967081201c1b}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }

        [Association("MealTypeCurses", Roles.RoleA, true, "4c25758e-e6d8-4771-9470-32e41c76a604")]
        System.Collections.Generic.List<IMealCourseType> Courses { get; }


        /// <MetaDataID>{1072bef3-adcc-4c58-87e4-e49446d7bfd5}</MetaDataID>
        void MoveMealCourseType(IMealCourseType mealCourseType, int newpos);

        /// <MetaDataID>{02cdcaf6-f959-4e89-9d88-22470bac2503}</MetaDataID>
        void RemoveMealCourseType(IMealCourseType mealCourseType);


        MenuModel.IMealCourseType NewMealCourseType();
    }
}