using MenuPresentationModel.MenuCanvas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{c8c257b2-7004-406a-ba37-031bf4a60660}</MetaDataID>
    public interface IMenuCanvasItemEx
    {
        IMenuPageCanvas Page
        {
            get;
            set;
        }
    }
}
