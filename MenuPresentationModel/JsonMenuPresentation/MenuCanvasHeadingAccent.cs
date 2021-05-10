using MenuPresentationModel.MenuStyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OOAdvantech;
using MenuPresentationModel.MenuCanvas;
using System.Windows;
using OOAdvantech.Json;
using UIBaseEx;
using OOAdvantech.MetaDataRepository;
#if MenuPresentationModel
using OOAdvantech.Json;
#else
using OOAdvantech.Json;
#endif

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{a9babdd8-8599-46f1-99f9-0a58b2121f18}</MetaDataID>
    public class MenuCanvasHeadingAccent : MenuCanvas.IMenuCanvasAccent
    {


        /// <exclude>Excluded</exclude>
        MultilingualMember<IMenuCanvasColumn> _Column = new OOAdvantech.MultilingualMember<IMenuCanvasColumn>();

        /// <MetaDataID>{4e7a2e45-2abd-49dd-bba4-8b2715890ec2}</MetaDataID>
        [JsonIgnore]
        [ImplementationMember(nameof(_Column))]
        public IMenuCanvasColumn Column
        {
            get
            {
                return _Column.Value;
            }
        }


        /// <MetaDataID>{050cd9c4-b119-42ce-81d5-7025024cf7cc}</MetaDataID>
        public void ResetSize()
        {
        }
        /// <MetaDataID>{603129ac-47d4-45dd-951e-1f2c344d2bc7}</MetaDataID>
        public void Remove()
        {
            (this.Page.Menu as RestaurantMenu).RemoveMenuItem(this);
            this.Page.RemoveMenuItem(this);
        }
        /// <MetaDataID>{8b941a47-7b90-4dbd-9c19-535c936edc81}</MetaDataID>
        public MenuCanvasHeadingAccent()
        {

        }


        /// <MetaDataID>{74f646ee-efe9-4828-abf9-433882f45c53}</MetaDataID>
        [JsonIgnore]
        public double BaseLine { get; set; }
        /// <MetaDataID>{865a78a3-519d-44de-86d6-8527d82cbaf3}</MetaDataID>
        IMenuCanvasHeading _Heading;



#if MenuPresentationModel
     
        /// <MetaDataID>{c0862b8e-2f1c-480b-a8bf-a6d719b18840}</MetaDataID>
        public MenuCanvasHeadingAccent Init(IMenuCanvasAccent headingAccent, Dictionary<object, object> mappedObjects)
        {
            //Culture = OOAdvantech.CultureContext.CurrentNeutralCultureInfo.Name;
            ID = Guid.NewGuid().ToString();
            mappedObjects[headingAccent] = this;
            //_Heading = heading;
            Description = headingAccent.Description;
            XPos = headingAccent.XPos;
            YPos = headingAccent.YPos;
            Height = headingAccent.Height;
            Width = headingAccent.Width;
            if (Accent == null)
            {
                _Accent = new HeadingAccent(headingAccent.Accent);
                int i = 0;
                foreach (var image in _Accent.AccentImages)
                {
                    var rect = headingAccent.GetAccentImageRect(i++);
                    (image as MenuImage).X = rect.X;
                    (image as MenuImage).Y = rect.Y;
                    (image as MenuImage).Width = rect.Width;
                    (image as MenuImage).Height = rect.Height;
                }
            }
            else
            {
                if (!(_Accent as HeadingAccent).MultilingualAccentImages.HasValue)
                {
                    (_Accent as HeadingAccent).GetImages(headingAccent.Accent);
                    int i = 0;
                    foreach (var image in _Accent.AccentImages)
                    {
                        var rect = headingAccent.GetAccentImageRect(i++);
                        (image as MenuImage).X = rect.X;
                        (image as MenuImage).Y = rect.Y;
                        (image as MenuImage).Width = rect.Width;
                        (image as MenuImage).Height = rect.Height;
                    }
                }
            }
            MultipleItemsAccent = _Accent.MultipleItemsAccent;
            Type = GetType().Name;
            return this;
        }

        public bool MultipleItemsAccent { get; set; }
#endif
        /// <MetaDataID>{6590a5d3-fb56-41c6-8743-09d8cb9493b6}</MetaDataID>
        public string Type { get; set; }


        /// <MetaDataID>{dca6079b-4e77-4f2c-b348-49e44143652d}</MetaDataID>
        IAccent _Accent;
        /// <MetaDataID>{59496a0c-6601-4265-8444-52b27aea63d5}</MetaDataID>
        public IAccent Accent
        {
            get
            {
                return _Accent;
            }
            set
            {
                _Accent = value;
            }
        }

        /// <MetaDataID>{b2752a8a-e725-40dd-bdbd-a2134b1e37bd}</MetaDataID>
        [JsonIgnore]
        public string AccentColor
        {
            get
            {
                return Accent.AccentColor;
            }
        }

        /// <MetaDataID>{8bd4aaf5-26c3-471b-864c-bffde9737d8b}</MetaDataID>
        [JsonIgnore]
        public string Description { get; set; }

        public string ID { get; set; }

        /// <MetaDataID>{b62cfd60-2a22-4417-a077-3e8d64864b4b}</MetaDataID>
        [JsonIgnore]
        public FontData Font { get; set; }

        /// <MetaDataID>{529d6921-1d13-4153-ad81-63df1f26ed79}</MetaDataID>
        [JsonIgnore]
        public bool FullRowImage { get; set; }

        /// <MetaDataID>{462ae24e-fa7b-48b8-a272-15c767921808}</MetaDataID>
        [JsonIgnore]
        public IMenuCanvasHeading Heading
        {
            get
            {
                return _Heading;
            }

            set
            {
                _Heading = value;
            }
        }

        /// <MetaDataID>{4ac63b37-2617-45e9-a717-8f2e1783faf9}</MetaDataID>
        [JsonIgnore]
        public double Height { get; set; }


        /// <MetaDataID>{9add8fe9-2b72-42ec-b584-de454329d8ba}</MetaDataID>
        [JsonIgnore]
        public IMenuPageCanvas Page
        {
            get
            {
                if (_Heading != null)
                    return _Heading.Page;
                else
                    return null;
            }
        }

        /// <MetaDataID>{349245ac-b9f7-4b9c-80ab-57f96483e973}</MetaDataID>
        [JsonIgnore]
        public double Width { get; set; }


        /// <MetaDataID>{2876ed4e-fc5d-46e1-8da5-9f1b78500fc2}</MetaDataID>
        [JsonIgnore]
        public double XPos { get; set; }
        /// <MetaDataID>{a529a02e-907d-4a1c-a65b-45cc47e905a9}</MetaDataID>
        [JsonIgnore]
        public double YPos { get; set; }




        /// <MetaDataID>{255a1381-980a-4b46-a29c-74bb407a0a71}</MetaDataID>
        [JsonIgnore]
        public List<IHighlightedMenuCanvasItem> HighlightedItems => throw new NotImplementedException();


#if MenuPresentationModel
        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{07ff5430-c8b5-4fd4-b821-a0349d7fc89b}</MetaDataID>
        public void AlignOnBaseline(IMenuCanvasItem foodItemLineText)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{3632ab6b-3fac-445d-8cb2-9439c9d50f73}</MetaDataID>
        public MenuCanvas.Rect GetAccentImageRect(int accentImageIndex)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{d9478c8e-0e3d-4b00-9cfe-c1c80d82bc67}</MetaDataID>
        public ItemRelativePos GetRelativePos(Point point)
        {
            throw new NotImplementedException();
        }
#endif
    }
}
