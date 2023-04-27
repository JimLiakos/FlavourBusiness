using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MenuPresentationModel.MenuCanvas;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Json;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{f59ddc23-752d-474e-a198-07f5ef4faa80}</MetaDataID>
    public class MenuCanvasLine : MenuCanvas.IMenuCanvasLine
    {
        public MenuCanvasLine()
        {

        }

        MenuPageCanvas _Page;
        public MenuCanvasLine(MenuPageCanvas page, MenuCanvas.IMenuCanvasLine menuCanvasLine)
        {
            _Page = page;
            Stroke = menuCanvasLine?.Stroke;
            StrokeThickness = menuCanvasLine.StrokeThickness;
            X1 = menuCanvasLine.X1;
            X2 = menuCanvasLine.X2;
            Y1 = menuCanvasLine.Y1;
            Y2 = menuCanvasLine.Y2;
            Type = GetType().Name;
        }
        public string Type { get; set; }


        public LineType LineType { get; set; }

        [JsonIgnore]
        public IMenuPageCanvas Page
        {
            get
            {
                return _Page;
            }
        }

        public string Stroke { get; set; }

        public double StrokeThickness { get; set; }

        public double X1 { get; set; }

        public double X2 { get; set; }
        

        public double Y1 { get; set; }

        public double Y2 { get; set; }
    }
}
