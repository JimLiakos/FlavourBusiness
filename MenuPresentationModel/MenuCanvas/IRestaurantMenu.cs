using OOAdvantech.MetaDataRepository;
using System.Globalization;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{96919c6e-eb1e-401a-a1af-4c2efea34e0a}</MetaDataID>
    public interface IRestaurantMenu
    {
        [Association("MenuStyle", Roles.RoleA, "d0d8fa48-b643-4d17-aa7e-df449e985844")]
        
        MenuStyles.IStyleSheet Style { get; set; }

        /// <MetaDataID>{d62e229a-8afd-45ad-9794-44703f53f045}</MetaDataID>
        [BackwardCompatibilityID("+1")]
        string Name { get; set; }
        [Association("RestaurantMenuItems", Roles.RoleA, true, "8151a670-a2e4-4df1-ae30-326b6cae2287")]
        [AssociationEndBehavior(OOAdvantech.MetaDataRepository.PersistencyFlag.ReferentialIntegrity | OOAdvantech.MetaDataRepository.PersistencyFlag.CascadeDelete)]
        System.Collections.Generic.IList<IMenuCanvasItem> MenuCanvasItems { get; }
        [RoleAMultiplicityRange()]

        [Association("MenuPages", Roles.RoleB, true, "6ea5e619-ad98-493f-9ff6-8af80dbb22dc")]
        [RoleBMultiplicityRange(1)]
        System.Collections.Generic.IList<IMenuPageCanvas> Pages { get; }


        /// <MetaDataID>{713feaf3-7061-4219-8bd9-d06722ae872f}</MetaDataID>
        OOAdvantech.Multilingual MultilingualPages { get; }

        /// <MetaDataID>{3d3859ae-6583-40aa-935a-e01cddc31432}</MetaDataID>
        void RemovePage(IMenuPageCanvas page);

        /// <MetaDataID>{5a36d80c-ea7d-4799-a3b6-a2f47a1664c8}</MetaDataID>
        void InsertPage(int index, IMenuPageCanvas page);

        /// <MetaDataID>{1ca5966c-a5ef-4f66-b8ae-e04d2dc8c34d}</MetaDataID>
        void AddPage(IMenuPageCanvas page);

        /// <MetaDataID>{470ea473-dc65-4fdc-b476-301078c94a8d}</MetaDataID>
        IMenuCanvasFoodItem GetMenuCanvasFoodItem(string menuItemUri);


    }
}