using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using UIBaseEx;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{74c2aa5f-a48f-4845-98d8-06d0400de962}</MetaDataID>
    public class MenuPageCanvas : MenuCanvas.IMenuPageCanvas
    {
        public MenuPageCanvas()
        {
        }

        public Margin BorderMargin { get; set; }
        public Margin BackgroundMargin { get; set; }

#if MenuPresentationModel
        public MenuPageCanvas(IRestaurantMenu menu, IMenuPageCanvas menuPage, Dictionary<object, object> mappedObject)
        {
            _Menu = menu;
            _Height = menuPage.Height;
            _Width = menuPage.Width;
            _Margin = menuPage.Margin;
            _NumberofColumns = menuPage.NumberofColumns;
            _Ordinal = menuPage.Ordinal;
            BorderMargin = new Margin();
            BackgroundMargin= new Margin();


            if ((menuPage.Style.Styles["page"] as PageStyle).Background != null)
            {
                Background = new PageImage((menuPage.Style.Styles["page"] as PageStyle).Background);
                BackgroundMargin = (menuPage.Style.Styles["page"] as PageStyle).BackgroundMargin;
            }
            if ((menuPage.Style.Styles["page"] as PageStyle).Border != null)
            {
                Border = new PageImage((menuPage.Style.Styles["page"] as PageStyle).Border);
                BorderMargin = (menuPage.Style.Styles["page"] as PageStyle).BorderMargin;
            }

            foreach (var menuCanvasitem in menuPage.MenuCanvasItems)
            {
                if (menuCanvasitem is IMenuCanvasFoodItem)
                {
                    MenuCanvasFoodItem menuCanvasFoodItem = null;
                    object mappedValue = null;
                    if (!mappedObject.TryGetValue(menuCanvasitem, out mappedValue))
                    {
                        menuCanvasFoodItem = new MenuCanvasFoodItem().Init(menu, menuCanvasitem as IMenuCanvasFoodItem, this, mappedObject);
                        mappedObject[menuCanvasitem] = menuCanvasFoodItem;
                    }
                    else
                        menuCanvasFoodItem = (mappedValue as MenuCanvasFoodItem).Init(menu, menuCanvasitem as IMenuCanvasFoodItem, this, mappedObject);

                    _MenuCanvasItems.Add(menuCanvasFoodItem);
                }
                if (menuCanvasitem is IMenuCanvasHeading)
                {
                    MenuCanvasHeading menuCanvasHeading = null;
                    object mappedValue = null;
                    if (!mappedObject.TryGetValue(menuCanvasitem, out mappedValue))
                    {
                        menuCanvasHeading = new MenuCanvasHeading().Init(menu, menuCanvasitem as IMenuCanvasHeading, this, mappedObject);
                        mappedObject[menuCanvasitem] = menuCanvasHeading;
                    }
                    else
                        menuCanvasHeading = (mappedValue as MenuCanvasHeading).Init(menu, menuCanvasitem as IMenuCanvasHeading, this, mappedObject);
                    _MenuCanvasItems.Add(menuCanvasHeading);
                }
            }

            foreach (IMenuCanvasLine separationLine in menuPage.SeparationLines)
                _SeparationLines.Add(new MenuCanvasLine(this, separationLine));
            Type = GetType().Name;
        }
#endif
        public string Type { get; set; }





        /// <exclude>Excluded</exclude>
        List<IMenuCanvasPageColumn> _Columns = new List<IMenuCanvasPageColumn>();
        public IList<IMenuCanvasPageColumn> Columns
        {
            get
            {
                return _Columns.AsReadOnly();
            }
            set
            {
                if (value != null)
                    _Columns = value.ToList();
            }
        }

        /// <exclude>Excluded</exclude>
        double _Height;
        public double Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
            }
        }

        /// <exclude>Excluded</exclude>
        Margin _Margin;
        public Margin Margin
        {
            get
            {
                return _Margin;
            }
            set
            {
                _Margin = value;
            }
        }

        /// <exclude>Excluded</exclude>
        IRestaurantMenu _Menu;
        public IRestaurantMenu Menu
        {
            get
            {
                return _Menu;
            }
            set
            {
                _Menu = value;
            }
        }

        /// <exclude>Excluded</exclude>
        List<IMenuCanvasItem> _MenuCanvasItems = new List<IMenuCanvasItem>();
        public IList<IMenuCanvasItem> MenuCanvasItems
        {
            get
            {
                return _MenuCanvasItems.AsReadOnly();
            }
            set
            {
                if (value != null)
                    _MenuCanvasItems = value.ToList();
            }
        }

        /// <exclude>Excluded</exclude>
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

        /// <exclude>Excluded</exclude>
        int _Ordinal;
        public int Ordinal
        {
            get
            {
                return _Ordinal;
            }
            set
            {
                _Ordinal = value;
            }
        }

        /// <exclude>Excluded</exclude>
        List<IMenuCanvasLine> _SeparationLines = new List<IMenuCanvasLine>();
        public IList<IMenuCanvasLine> SeparationLines
        {
            get
            {
                return _SeparationLines.AsReadOnly();
            }
            set
            {
                if (value != null)
                    _SeparationLines = value.ToList();
            }
        }



        /// <exclude>Excluded</exclude>
        double _Width;
        public double Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
            }
        }

        public event ObjectChangeStateHandle ObjectChangeState;

        public void AddMenuItem(IMenuCanvasItem manuCanvasitem)
        {
            _MenuCanvasItems.Add(manuCanvasitem);
        }

        public void InsertMenuItem(int pos, IMenuCanvasItem manuCanvasitem)
        {
            _MenuCanvasItems.Insert(pos, manuCanvasitem);
        }

        public void InsertMenuItemAfter(IMenuCanvasItem manuCanvasitem, IMenuCanvasItem newManuCanvasitem)
        {
            int pos = _MenuCanvasItems.IndexOf(manuCanvasitem);
            _MenuCanvasItems.Insert(pos, newManuCanvasitem);
        }

        public bool MoveCanvasItemTo(IMenuCanvasItem menuCanvasItem, Point point)
        {
            throw new NotImplementedException();
        }

        public void RemoveMenuItem(IMenuCanvasItem manuCanvasitem)
        {
            _MenuCanvasItems.Remove(manuCanvasitem);
        }

        public IPageImage Border { get; set; }
        public IPageImage Background { get; set; }

#if MenuPresentationModel
        public IStyleSheet Style
        {
            get
            {
                return null;
            }
        }
#endif
    }
}
