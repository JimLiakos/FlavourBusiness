using System.Windows;
using OOAdvantech.MetaDataRepository;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{ea615815-9b4f-4769-b169-96dd85514bf4}</MetaDataID>
    public interface IMenuCanvasItem
    {

       event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{c903f861-4753-4876-a911-cb2ff3401f97}</MetaDataID>
        MenuStyles.FontData Font
        {
            set;
            get;
        }

        /// <MetaDataID>{5f10e940-0866-4a21-aa5f-62db035ec129}</MetaDataID>
        void AlignOnBaseline(IMenuCanvasItem foodItemLineText);

        [Association("PageItems", Roles.RoleB, "97fbbd68-9891-4e58-aafe-9e615fed5db2")]
        IMenuPageCanvas Page
        {
            get;
        }

        /// <MetaDataID>{27d68a19-cde8-49b7-9833-f9bcf54a0abf}</MetaDataID>
        double XPos { get; set; }

        /// <MetaDataID>{5930047f-0943-4f39-8102-67cd63f0fb84}</MetaDataID>
        double YPos { get; set; }

        /// <MetaDataID>{ca886e45-ad13-4cfc-854e-84933016bc7e}</MetaDataID>
        double Width { get; set; }


        /// <MetaDataID>{d00bf109-f890-4fb3-a8b9-831eee9c0320}</MetaDataID>
        double Height { get; set; }

        /// <MetaDataID>{f2793ee2-fd34-4987-ab35-11e1d6e54c59}</MetaDataID>
        string Description { get; set; }

        /// <MetaDataID>{82704c66-09d4-45b8-8378-fbd1747332fa}</MetaDataID>
        ItemRelativePos GetRelativePos(Point point);

      
    }
}