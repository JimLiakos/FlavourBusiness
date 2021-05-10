using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MenuDesigner.ViewModel.MenuCanvas
{
    /// <MetaDataID>{76f877e0-f045-45ad-ab04-a82901160715}</MetaDataID>
    public interface ICanvasItem
    {

        double Top { get;  }
        double Left { get;  }

        double Height { get;  }

        double Width { get;  }

        void Release();

        System.Windows.Visibility Visibility { get;  set; }
        //bool DragDropOn { get; set; }

        //bool IsHitTestVisible { get; set; }
    }
}
