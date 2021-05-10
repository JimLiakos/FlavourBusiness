using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{a0ca7e03-bc94-42fb-b1d8-547fece40d09}</MetaDataID>
    public interface ILevel
    {
        [Association("TypeLevels", Roles.RoleB, "aaedcc45-fbdf-4f6c-ba38-fc00c1e343ba")]
        [RoleBMultiplicityRange(1, 1)]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        IScaleType DeclaringType { get; }

        /// <MetaDataID>{f8617cc8-9df2-44ba-8be2-65228da3d7fd}</MetaDataID>
        string Name
        {
            get;
            set;
        }
        /// <MetaDataID>{2b1a1218-5106-4ec7-ac2f-798041789a01}</MetaDataID>
        /// <summary>When this property is set true the preparation option can be selected via check box.</summary>
        bool UncheckOption
        {
            get;
            set;
        }
    }
}
