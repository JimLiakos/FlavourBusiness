using MenuPresentationModel.MenuStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{6b8235a8-b314-4622-b8b7-ce27bd5758d6}</MetaDataID>
    public class PageImage : IPageImage
    {

        public PageImage()
        {

        }
        public PageImage(IPageImage pageImage)
        {
            Color = pageImage.Color;
            Flip = pageImage.Flip;
            IsVectorImage = pageImage.IsVectorImage;
            LandscapeHeight = pageImage.LandscapeHeight;
            LandscapeImage = new Resource() { Name = pageImage.LandscapeImage.Name, Uri = MakeRelativeUri(pageImage.LandscapeImage.Uri) };
            //LandscapeUri = MakeRelativeUri(pageImage.LandscapeUri);
            LandscapeWidth = pageImage.LandscapeWidth;
            Name = pageImage.Name;
            Opacity = pageImage.Opacity;
            PortraitHeight = pageImage.PortraitHeight;
            PortraitImage = new Resource() { Name = pageImage.PortraitImage.Name, Uri = MakeRelativeUri(pageImage.PortraitImage.Uri) };
            //PortraitUri = MakeRelativeUri(pageImage.PortraitUri);
            PortraitWidth = pageImage.PortraitWidth;
            Type = GetType().Name;
        }
        public string Type { get; set; }

        private string MakeRelativeUri(string uri)
        {
            string appDataPath = System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Microneme";
            appDataPath += "\\DontWaitWater\\";
            if (!string.IsNullOrWhiteSpace(uri))
                uri = uri.Replace(appDataPath, "").Replace(@"\", "/");
            return uri;
        }

        public string Color { get; set; }
        public bool Flip { get; set; }
        public bool IsVectorImage { get; set; }
        public double LandscapeHeight { get; set; }
        public Resource LandscapeImage { get; set; }
        public string LandscapeUri
        {
            get
            {
                if (LandscapeImage != null)
                    return LandscapeImage.Uri;
                else
                    return "";
            }

        }
        public double LandscapeWidth { get; set; }
        public bool Mirror { get; set; }
        public string Name { get; set; }
        public double Opacity { get; set; }
        public double PortraitHeight { get; set; }
        public Resource PortraitImage { get; set; }
        public string PortraitUri
        {
            get
            {
                if (PortraitImage != null)
                    return PortraitImage.Uri;
                else
                    return "";
            }
        }
        public double PortraitWidth { get; set; }
        public event ObjectChangeStateHandle ObjectChangeState;
    }
}
