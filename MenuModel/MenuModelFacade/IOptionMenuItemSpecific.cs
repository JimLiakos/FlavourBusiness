using OOAdvantech.MetaDataRepository;

namespace MenuModel
{
    /// <MetaDataID>{a8691eca-a90c-4ead-b6b4-702845297c35}</MetaDataID>
    [AssociationClass(typeof(IMenuItem), typeof(IPreparationScaledOption), "OptionMenuItemSpecific")]
    public interface IOptionMenuItemSpecific
    {
        /// <MetaDataID>{5ca686d1-76da-4b7f-96c3-826288a109a7}</MetaDataID>
        bool Hide { get; set; }

        /// <MetaDataID>{f4e4ac90-774d-4c10-84ca-9de8aab71219}</MetaDataID>
        [AssociationClassRole(Roles.RoleA)]
        [BackwardCompatibilityID("+2")]
        IMenuItem MenuItemOptionSpecific {get;}

        /// <MetaDataID>{413655a3-ad4d-4ca4-8fb5-8c32a288efe6}</MetaDataID>
        [AssociationClassRole(Roles.RoleB)]
        [BackwardCompatibilityID("+1")]
        IPreparationScaledOption Option { get;  }
        [RoleBMultiplicityRange(0)]
        [Association("CustomInitialLevel", Roles.RoleA, "ea0c898b-c84c-42ea-a29d-99f5e56e7f0c")]
        ILevel InitialLevel { get; set; }

        string Uri { get; }





        }
}