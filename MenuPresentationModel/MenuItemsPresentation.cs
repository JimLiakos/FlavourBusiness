using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel
{
    /// <MetaDataID>{3cfe3f97-b97c-4478-abe0-e3163b893979}</MetaDataID>
    [BackwardCompatibilityID("{3cfe3f97-b97c-4478-abe0-e3163b893979}")]
    [Persistent()]
    public class MenuItemsPresentation : PresentationItem
    {

        /// <exclude>Excluded</exclude>
        /// <MetaDataID>{b353b070-6e17-415d-be28-33e8ddd26fba}</MetaDataID>
        OOAdvantech.Collections.Generic.Set<MenuModel.MenuItem> _MenuItems = new OOAdvantech.Collections.Generic.Set<MenuModel.MenuItem>();

        /// <MetaDataID>{74404dd7-90b8-4f44-b4cc-df37a1b5de61}</MetaDataID>
        [Association("MenuItemPresentation", Roles.RoleA, true, "633b3b7d-479f-47b3-a787-912c6935f008")]
        [RoleAMultiplicityRange(1)]
        [PersistentMember("_MenuItems")]
        public OOAdvantech.Collections.Generic.Set<MenuModel.MenuItem> MenuItems
        {
            get
            {
                return _MenuItems.AsReadOnly();
            }
        }



        /// <MetaDataID>{2e9bd73a-a2ce-48a3-a94e-10583314b402}</MetaDataID>
        public void MoveMenuItem(MenuModel.MenuItem menuItem, int pos)
        {
            _MenuItems.Remove(menuItem);
            _MenuItems.Insert(pos, menuItem);
        }

        /// <MetaDataID>{bfb8ede1-62f0-4dd3-847b-ddeba138f676}</MetaDataID>
        public void AddMenuItem(MenuModel.MenuItem menuItem)
        {
            _MenuItems.Add(menuItem);
            

        }

        /// <MetaDataID>{0ce5090e-5302-4be6-8f4f-8191ca1f4389}</MetaDataID>
        public void RemoveMenuItem(MenuModel.MenuItem menuItem)
        {
            _MenuItems.Remove(menuItem);
            
        }
    }
}