using System.Collections.Generic;
using OOAdvantech.MetaDataRepository;
namespace MenuModel
{ 
    /// <MetaDataID>{833f995c-ee01-43ca-8b54-1e7fd367767a}</MetaDataID>
    public interface IScaleType
    {
        /// <MetaDataID>{96564faa-77a9-430e-8d0d-e5b285e48c0b}</MetaDataID>
        void InsertLevel(int index, MenuModel.ILevel level);

        /// <MetaDataID>{f1512db1-7e56-442b-91c0-b371dd0f824c}</MetaDataID>
        [Association("TypeLevels", Roles.RoleA, true, "aaedcc45-fbdf-4f6c-ba38-fc00c1e343ba"), OOAdvantech.MetaDataRepository.RoleAMultiplicityRange(2)]
        [AssociationEndBehavior(PersistencyFlag.CascadeDelete)]
        IList<MenuModel.ILevel> Levels
        {
            get;
        }
        /// <MetaDataID>{f8617cc8-9df2-44ba-8be2-65228da3d7fd}</MetaDataID>
        string Name
        {
            get;
            set;
        }

        /// <MetaDataID>{94dccad3-35bf-47ba-8899-4a07ed255043}</MetaDataID>
        void MoveLevel(MenuModel.ILevel level, int newpos);
        /// <MetaDataID>{bf2156a6-863f-4ef2-b37e-1f90099cfdb7}</MetaDataID>
        void AddLevel(ILevel level);
        /// <MetaDataID>{8f76214c-19c3-4a31-b076-953e5c75162b}</MetaDataID>
        void RemoveLevel(ILevel level);
    }
}
