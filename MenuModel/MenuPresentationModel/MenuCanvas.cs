using System;
using System.Collections.Generic;

namespace MenuPresentationModel.MenuCanvas
{
    /// <MetaDataID>{673d8968-5f3e-4525-9ca4-79d899de4bac}</MetaDataID>
    public class MenuCanvas : �MenuCanvas
    {

        public MenuCanvas()
        {
            _NumberofColumns = 1;

        }
        public List<�MenuCanvasColumn> Columns
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        int _NumberofColumns;
        public int NumberofColumns
        {
            get
            {
                return _NumberofColumns;
            }
            set
            {
                _NumberofColumns = value;
            }
        }
    }
}