using OOAdvantech.MetaDataRepository;
using OOAdvantech.Transactions;

namespace MenuPresentationModel
{

    public delegate void MenuStyleChangedHandle(MenuStyles.IStyleSheet oldStyle, MenuStyles.IStyleSheet newStyle);

    /// <MetaDataID>{a5e33434-afd5-4612-8375-01d50a7e7cfb}</MetaDataID>
    [BackwardCompatibilityID("{a5e33434-afd5-4612-8375-01d50a7e7cfb}")]
    [Persistent()]
    public class RestaurantMenu
    {



       public static MenuStyles.IStyleSheet ConntextStyleSheet;

        public event MenuStyleChangedHandle MenuStyleChanged;
        /// <exclude>Excluded</exclude>
        MenuStyles.IStyleSheet _Style;

        
        [Association("MenuStyle", Roles.RoleA, "d0d8fa48-b643-4d17-aa7e-df449e985844")]
        [AssociationEndBehavior(PersistencyFlag.OnConstruction)]
        [PersistentMember(nameof(_Style))]
        public MenuStyles.IStyleSheet Style
        {
            get
            {
                return _Style;
            }

            set
            {

                if (_Style != value)
                {
                    var oldStyle = _Style;
                    var newStyle = value;
                    using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                    {
                        _Style = value;
                        stateTransition.Consistent = true;
                    }
                    MenuStyleChanged?.Invoke(oldStyle, newStyle);
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.ObjectStateManagerLink StateManagerLink;

        /// <exclude>Excluded</exclude>
        string _Name;

        /// <MetaDataID>{bde5666b-a84e-40ab-b978-cf94e41e6d10}</MetaDataID>
        [PersistentMember("_Name")]
        [BackwardCompatibilityID("+1")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {

                using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
                {
                    _Name = value;
                    stateTransition.Consistent = true;
                }

            }
        }

        /// <exclude>Excluded</exclude>
        OOAdvantech.Collections.Generic.Set<MenuPage> _Pages = new OOAdvantech.Collections.Generic.Set<MenuPage>();

        /// <MetaDataID>{b3b1feaa-7f39-483e-9af8-b0739b58536d}</MetaDataID>
        [Association("MenuPages", Roles.RoleA, true, "9537b191-c58c-42ac-a37d-e2c62a4dcdf4")]
        [PersistentMember(nameof(_Pages))]
        [RoleAMultiplicityRange(1)]
        public OOAdvantech.Collections.Generic.Set<MenuPage> Pages
        {
            get
            {
                return _Pages.AsReadOnly();
            }
        }

        /// <MetaDataID>{4952e0dd-c560-4bd5-8f69-930e8faa101d}</MetaDataID>
        public void AddPage(MenuPage page)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Pages.Add(page);
                stateTransition.Consistent = true;
            }

        }

        /// <MetaDataID>{529e0142-498d-489c-9b5b-be8321011e1c}</MetaDataID>
        public void MovePage(int newPos, MenuPage page)
        {
            if (newPos == page.Ordinal)
                return;

            if (newPos > _Pages.Count - 2)
            {
                /// move page to end.
                RemovePage(page);
                AddPage(page);
            }
            else
            {
                RemovePage(page);
                InsertPage(newPos, page);
            }
        }

        /// <MetaDataID>{d2b2be3f-38c4-440f-924a-29e8746fbf4d}</MetaDataID>
        public void InsertPage(int index, MenuPage page)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Pages.Insert(index, page);
                stateTransition.Consistent = true;
            }

        }
        /// <MetaDataID>{ebe69fc5-a014-4ff1-84e9-784d199bb6e6}</MetaDataID>
        public void RemovePage(MenuPage page)
        {

            using (ObjectStateTransition stateTransition = new ObjectStateTransition(this))
            {
                _Pages.Remove(page);
                stateTransition.Consistent = true;
            }

        }
    }
}