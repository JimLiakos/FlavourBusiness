using OOAdvantech;
using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{b65a0463-e880-4cac-80f8-d26a16bee319}</MetaDataID>
    public interface IMealCourseType
    {
        /// <MetaDataID>{befad4eb-20f1-4881-946a-3920f7be6414}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        Multilingual MultilingualName { get; }

        /// <MetaDataID>{d32c224e-19ac-4e86-9968-18ba25ed6cf4}</MetaDataID>
        [BackwardCompatibilityID("+2")]
        double DurationInMinutes { get; set; }

        /// <MetaDataID>{9593497e-bd77-4e59-87fd-d2928e26c624}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }
    }
}