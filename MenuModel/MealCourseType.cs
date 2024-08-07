using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace MenuModel
{
    /// <MetaDataID>{100393e9-4e48-4662-a167-91ee8b96e484}</MetaDataID>
    [BackwardCompatibilityID("{100393e9-4e48-4662-a167-91ee8b96e484}")]
    [Persistent()]
    public class MealCourseType : IMealCourseType
    {


        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        OOAdvantech.MultilingualMember<string> _Name = new OOAdvantech.MultilingualMember<string>();

        /// <MetaDataID>{ac3dd59f-6e2c-4779-b7f8-35d2baa9d1c5}</MetaDataID>
        [PersistentMember(nameof(_Name))]
        [BackwardCompatibilityID("+2")]
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


        /// <MetaDataID>{ba2d7888-3e6e-45a4-9d61-aebe9c1007e0}</MetaDataID>
        public MealCourseType()
        {

        }
        /// <MetaDataID>{f34af47b-91e1-4889-8094-d51ac0b34704}</MetaDataID>
        public MealCourseType(string courseName)
        {
            _Name.Value = courseName;
        }

        /// <exclude>Excluded</exclude>
        double _DurationInMinutes = 1;

        /// <MetaDataID>{53b9c2e0-3a04-45c2-b6fe-4f510e270fae}</MetaDataID>
        [PersistentMember(nameof(_DurationInMinutes))]
        [BackwardCompatibilityID("+1")]
        public double DurationInMinutes
        {
            get => _DurationInMinutes;
            set
            {
                if (_DurationInMinutes != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _DurationInMinutes = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }

        /// <MetaDataID>{b896d9eb-3624-45c6-a301-6ee6940b056b}</MetaDataID>
        public Multilingual MultilingualName => new Multilingual(_Name);


        /// <exclude>Excluded</exclude>
        private bool _IsDefault;

        /// <MetaDataID>{dab697e1-e1c9-4cd9-a5d4-7362440f8373}</MetaDataID>
        [PersistentMember(nameof(_IsDefault))]
        [BackwardCompatibilityID("+3")]
        public bool IsDefault
        {
            get => _IsDefault;
            set
            {

                if (_IsDefault != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _IsDefault = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }


        /// <exclude>Excluded</exclude>
        Member<IMealType> _Meal = new Member<IMealType>();

        /// <MetaDataID>{fdaa7b95-292a-45bd-9b85-9e29deedc0b6}</MetaDataID>
        [PersistentMember(nameof(_Meal))]
        [BackwardCompatibilityID("+4")]
        public MenuModel.IMealType Meal => _Meal.Value;


        /// <exclude>Excluded</exclude>
        bool _AutoStart=true;

        /// <MetaDataID>{b9b01d92-e482-4f92-a76d-e616e802fb5b}</MetaDataID>
        [PersistentMember(nameof(_AutoStart))]
        [BackwardCompatibilityID("+5")]
        public bool AutoStart
        {
            get => _AutoStart; set
            {
                if (_AutoStart != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _AutoStart = value;
                        stateTransition.Consistent = true;
                    }
                }
            }
        }
    }
}