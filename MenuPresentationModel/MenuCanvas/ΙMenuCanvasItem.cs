using System.Windows;
using OOAdvantech.MetaDataRepository;
using UIBaseEx;

namespace MenuPresentationModel.MenuCanvas
{
  /// <MetaDataID>{ea615815-9b4f-4769-b169-96dd85514bf4}</MetaDataID>
  public interface IMenuCanvasItem
  {





    [Association("ColumnItems", Roles.RoleB, "2ae6275c-9dfb-4623-b27b-220e657aae4a")]
    IMenuCanvasColumn Column { get; }

    /// <MetaDataID>{b651b848-0dab-4ca3-9d94-f9a29952d72b}</MetaDataID>
    double BaseLine { get; set; }


#if MenuPresentationModel
    event OOAdvantech.ObjectChangeStateHandle ObjectChangeState;
#endif

    /// <MetaDataID>{c903f861-4753-4876-a911-cb2ff3401f97}</MetaDataID>
    FontData Font
    {
      set;
      get;
    }


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

   

#if MenuPresentationModel
    /// <MetaDataID>{82704c66-09d4-45b8-8378-fbd1747332fa}</MetaDataID>
    ItemRelativePos GetRelativePos(Point point);

    /// <MetaDataID>{5f10e940-0866-4a21-aa5f-62db035ec129}</MetaDataID>
    void AlignOnBaseline(IMenuCanvasItem foodItemLineText);

#endif

    /// <MetaDataID>{457a4142-eca0-4fd8-882f-2df2cb723dd0}</MetaDataID>
    void ResetSize();
    /// <MetaDataID>{0711d774-f6be-42c5-bfc6-f6c8fcf2fe2f}</MetaDataID>
    void Remove();
  }
}
