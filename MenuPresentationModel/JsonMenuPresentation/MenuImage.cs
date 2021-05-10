using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using MenuPresentationModel.MenuStyles;
using OOAdvantech.Json;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{5c76cdf5-a754-496d-86e3-57f0f8d62f9a}</MetaDataID>
    public class MenuImage : MenuStyles.IImage
    {
        public MenuImage()
        {

        }
        public MenuImage(IImage image)
        {
            Height = image.Height;
            Width = image.Width;
            Uri = MakeRelativeUri(image.Uri);
            Image =new Resource() { Name = image.Image.Name, Uri = MakeRelativeUri(image.Image.Uri),TimeStamp=DateTime.Now};

            Type = GetType().Name;
        }

        [JsonIgnore]
        public Size Size
        {
            get
            {
                return new Size(Width, Height);
            }
        }
        public string Type { get; set; }

        public double X { get; set; }

        public double Y { get; set; }
        public double Height { get; set; }

        public Resource Image { get; set; }
        public string Uri { get; set; }


        public double Width { get; set; }


        private string MakeRelativeUri(string uri)
        {
            string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            appDataPath += "\\DontWaitWater\\";
            if (!string.IsNullOrWhiteSpace(uri))
                uri = uri.Replace(appDataPath, "").Replace(@"\", "/");
            return uri;
        }
    }
}
