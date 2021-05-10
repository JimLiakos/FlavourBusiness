
using OOAdvantech.MetaDataRepository;
namespace MenuModel
{
    /// <MetaDataID>{045c9257-9642-4479-ba52-73bf7f75348a}</MetaDataID>
    public interface IPreparationOption
    {
        /// <MetaDataID>{715ce8bf-8265-4797-9ff9-0649954ff582}</MetaDataID>
        [Association("ItemTypePreparation", Roles.RoleB, "102fcd8d-cc58-42cd-8dcc-4bd51cd424e9")]
        IMenuItemType Owner { get; }

        /// <MetaDataID>{dd06cb4c-2872-4b2f-a24b-8e1fabb03d6a}</MetaDataID>
        string Name
        {
            get;
            set;
        }



        /// <MetaDataID>{0ad40c79-a850-49eb-aab1-9ea5aeadf58b}</MetaDataID>
        IMenuItemType MenuItemType
        {
            get;
        }
    }
}
