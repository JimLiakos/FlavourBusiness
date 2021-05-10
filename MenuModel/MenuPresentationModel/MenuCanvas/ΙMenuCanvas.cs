using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{88c39a17-f063-4fa8-a8f8-97d068915ae3}</MetaDataID>
    public interface IMenuPageCanvas
    {
        event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{077e6dbc-cc82-46ea-af43-bd782c5e63fb}</MetaDataID>
        MenuStyles.Margin Margin { get; }

        /// <MetaDataID>{221ae959-d7f6-4dd7-98cc-7a1e005ad87b}</MetaDataID>
        double Height { get; }

        /// <MetaDataID>{51c63b68-4941-445a-8397-a9988ac0c7eb}</MetaDataID>
        double Width { get; }

        /// <MetaDataID>{d0040314-6ef2-4a42-8d5d-f6860e6ba881}</MetaDataID>
        [Association("PageItems", Roles.RoleA, true, "97fbbd68-9891-4e58-aafe-9e615fed5db2")]
        System.Collections.Generic.IList<IMenuCanvasItem> MenuCanvasItems { get; }

        /// <MetaDataID>{ca63be99-bd91-4481-ac7e-6d18e132c01d}</MetaDataID>
        int NumberofColumns { get; set; }


        /// <MetaDataID>{a8051112-c934-46c8-89c6-c477e03d7742}</MetaDataID>
        [Association("CanvasColumns", Roles.RoleA, true, "5a14aabe-41b9-45a0-b47b-5454df7c7bf0")]
        [RoleAMultiplicityRange(1)]
        [RoleBMultiplicityRange(1, 1)]
        System.Collections.Generic.IList<IMenuCanvasColumn> Columns { get; }



        /// <MetaDataID>{4d415e69-463d-43b3-8af3-8ba9c8849bea}</MetaDataID>
        MenuStyles.IStyleSheet Style { get;  }

        void MoveCanvasItemTo(IMenuCanvasItem menuCanvasItem, System.Windows.Point point);


        /// <MetaDataID>{3c6d04ca-722b-4e04-9e97-c471d29bb9b5}</MetaDataID>
        void AddMenuItem(IMenuCanvasItem manuCanvasitem);

        /// <MetaDataID>{d1351d2e-cdfb-4704-a955-7e2d56caad99}</MetaDataID>
        void InsertMenuItem(int pos, IMenuCanvasItem manuCanvasitem);
        /// <MetaDataID>{a2df9bf7-7ad3-4405-8464-9d410f0392ee}</MetaDataID>
        void InsertMenuItemAfter(IMenuCanvasItem manuCanvasitem,IMenuCanvasItem newManuCanvasitem);

        /// <MetaDataID>{c87bcefb-ae4d-468c-8628-0d1ec0034b4c}</MetaDataID>
        void Delete(IMenuCanvasItem manuCanvasitem);

    }
}