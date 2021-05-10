using OOAdvantech.Collections.Generic;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;
using System.Linq;
using System;
using System.Collections.Generic;

namespace MenuModel
{
    /// <MetaDataID>{76e5080c-6610-4022-906f-0e960e6aba63}</MetaDataID>
    [BackwardCompatibilityID("{76e5080c-6610-4022-906f-0e960e6aba63}")]
    [Persistent()]
    public class ItemSelectorOption :PreparationScaledOption, IPricingContext
    {

        /// <MetaDataID>{28531f65-3332-4601-9eb7-3eb38f70d1f7}</MetaDataID>
        public MenuModel.MenuItemPrice GetMenuItemPrice(MenuModel.IMenuItem menuItem)
        {
            MenuModel.MenuItemPrice menuItemPrice = null;
            MenuItemPrices.TryGetValue(menuItem, out menuItemPrice);

            return menuItemPrice;
        }

        /// <exclude>Excluded</exclude>
        Set<MenuItemPrice> _MenuItemPrices;

        [Association("MenuItemOptionPrice", Roles.RoleA, "de4dea3d-cb9c-4e14-99c5-bec3cf9f766b")]
        [RoleAMultiplicityRange(0)]
        [AssociationEndBehavior(PersistencyFlag.AllowTransient)]
        [PersistentMember(nameof(_MenuItemPrices))]
        public System.Collections.Generic.Dictionary<IMenuItem, MenuItemPrice> MenuItemPrices
        {
            get
            {
              return  (from menuItemPrice in _MenuItemPrices
                 where menuItemPrice.MenuItem!=null
                 select menuItemPrice).ToDictionary(x => x.MenuItem);
                //return _MenuItemPrices.ToDictionary(x => x.MenuItem);
            }
        }
        /// <MetaDataID>{0f5e064e-1dcb-4485-8a8f-3f55e6fd32e5}</MetaDataID>
        public void AddMenuItemPrice(MenuItemPrice menuItemPrice)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuItemPrices.Add(menuItemPrice);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{4c28a6b4-53b6-4740-8785-b9b4a112b79f}</MetaDataID>
        public void RemoveMenuItemPrice(MenuItemPrice menuItemPrice)
        {
            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _MenuItemPrices.Remove(menuItemPrice);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{fd0d3268-c4c6-4268-827d-a8eb84a2c325}</MetaDataID>
        bool _isa;
        /// <MetaDataID>{f5ed3184-66ae-4d0f-818c-5090770866f9}</MetaDataID>
        public bool isa
        {
            get
            {
                return _isa;
            }
            set
            {
                _isa = value;
            }
        }

        /// <MetaDataID>{4531fbde-bb9a-4e71-8764-4b77d23b7fb8}</MetaDataID>
        public ICustomizedPrice GetCustomazedPrice(IPricedSubject pricedSubject)
        {
            return (from customizedPrice in _PricedSubjects
                    where customizedPrice.PricedSubject == pricedSubject
                    select customizedPrice).FirstOrDefault();
        }

        /// <MetaDataID>{6f8a725d-4e93-4f8a-830c-41ada7b7f6d4}</MetaDataID>
        public override decimal Price
        {
            get
            {
                return base.Price;
            }

            set
            {
                base.Price = value;
            }
        }

        /// <exclude>Excluded</exclude>
        Set<ICustomizedPrice> _PricedSubjects = new Set<ICustomizedPrice>();

        /// <MetaDataID>{9dbd1cda-f5c2-4ff2-8eb6-4a0d5abd0d05}</MetaDataID>
        [PersistentMember(nameof(_PricedSubjects))]
        [BackwardCompatibilityID("+2")]
        public IList<ICustomizedPrice> PricedSubjects
        {
            get
            {
                return _PricedSubjects.AsReadOnly();
            }
        }

    }
}