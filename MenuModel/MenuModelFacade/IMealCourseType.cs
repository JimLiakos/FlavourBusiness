using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{b65a0463-e880-4cac-80f8-d26a16bee319}</MetaDataID>
    public interface IMealCourseType
    {
        [Association("MealTypeCurses", Roles.RoleB, "4c25758e-e6d8-4771-9470-32e41c76a604")]
        [RoleBMultiplicityRange(1, 1)]
        IMealType Meal { get; }

        /// <MetaDataID>{befad4eb-20f1-4881-946a-3920f7be6414}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        Multilingual MultilingualName { get; }

        /// <MetaDataID>{d32c224e-19ac-4e86-9968-18ba25ed6cf4}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double DurationInMinutes { get; set; }

        /// <MetaDataID>{9593497e-bd77-4e59-87fd-d2928e26c624}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }

        /// <summary>
        /// All menu items where are not parts o of meal type assigned to meal course of default meal course type
        /// </summary>
        /// <MetaDataID>{8d8bec0f-5c00-4a1f-a87f-02eb7dff8fd6}</MetaDataID>
        [BackwardCompatibilityID("+4")]
        bool IsDefault { get; set; }


        /// <MetaDataID>{0c2711fe-112f-4e82-9d7a-f8fc0eda8183}</MetaDataID>
        [BackwardCompatibilityID("+5")]
        bool AutoStart { get; set; }
    }
}