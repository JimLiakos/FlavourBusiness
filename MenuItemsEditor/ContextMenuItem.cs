using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;

namespace MenuItemsEditor
{
    public class ContextMenuItem
    {

        public string Text { get; set; }

        public ICommand OnClick { get; set; }


        Uri _ImageUri;
        public Uri ImageUri
        {
            get
            {
                return _ImageUri;
            }
            set
            {
                _ImageUri = value;
            }

        }

        public System.Windows.Controls.Image Image { get; set; }
    }

    public class ContextMenu
    {
        List<ContextMenuItem> _ContextMenuItems;
        public List<ContextMenuItem> ContextMenuItems
        {
            set
            {
                _ContextMenuItems = value;
            }
            get
            {
                return _ContextMenuItems;
            }
        }
    }
}
