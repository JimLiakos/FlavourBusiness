using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System;

namespace MenuModel
{
    /// <MetaDataID>{a2cc80f6-6e7b-4a07-b8de-3f34e903e16b}</MetaDataID>
    [BackwardCompatibilityID("{a2cc80f6-6e7b-4a07-b8de-3f34e903e16b}")]
    [Persistent()]
    public class PartofMeal : MarshalByRefObject, IPartofMeal
    {

        /// <MetaDataID>{9b3b903e-0efe-490f-aa4c-ece22ca47485}</MetaDataID>
        protected PartofMeal()
        {

        }
        /// <MetaDataID>{d822c544-d0bc-4f6e-afea-214278eefc9a}</MetaDataID>
        public PartofMeal(IMenuItem menuItem, IMealType mealType)
        {
            _MenuItem = menuItem;
            _MealType = mealType;
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;


        /// <exclude>Excluded</exclude>
        IMealType _MealType;

        /// <MetaDataID>{c520da20-0c9d-4305-b20f-714a62e4a52a}</MetaDataID>
        [BackwardCompatibilityID("+3")]
        [OOAdvantech.MetaDataRepository.AssociationClassRole(Roles.RoleA, nameof(_MealType))]
        public MenuModel.IMealType MealType => _MealType;

        /// <exclude>Excluded</exclude>
        IMenuItem _MenuItem;

        /// <MetaDataID>{5e41a546-2463-456e-97ca-fec558b195aa}</MetaDataID>
        [AssociationClassRole(Roles.RoleB, nameof(_MenuItem))]
        public IMenuItem MenuItem { get => _MenuItem; }






        /// <exclude>Excluded</exclude>
        IMealCourseType _MealCourseType;
        /// <MetaDataID>{fb4623e7-c46d-45f9-823a-4ca400949d39}</MetaDataID>
        [PersistentMember(nameof(_MealCourseType))]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [BackwardCompatibilityID("+2")]
        public MenuModel.IMealCourseType MealCourseType
        {
            get => _MealCourseType;
            set
            {

                if (_MealCourseType != value)
                {
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _MealCourseType = value;
                        stateTransition.Consistent = true;
                    }
                }

            }
        }
    }
}