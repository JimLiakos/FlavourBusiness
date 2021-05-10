using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.Json;
using UIBaseEx;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{6febabeb-e9dd-4b16-8802-345db919b8f6}</MetaDataID>
    public class HeadingAccent : MenuStyles.IAccent
    {
        /// <MetaDataID>{f66a2c1e-ac79-4acb-bdf2-cebefe1625c8}</MetaDataID>
        public HeadingAccent()
        {
            
        }
        /// <MetaDataID>{bfe161a9-0946-4ad1-a878-0a40bd045ef1}</MetaDataID>
        public HeadingAccent(IAccent headingAccent)
        {

            AccentColor = headingAccent.AccentColor;
            DoubleImage = headingAccent.DoubleImage;
            FullRowImage = headingAccent.FullRowImage;
            Height = headingAccent.Height;
            MarginLeft = headingAccent.MarginLeft;
            MarginTop = headingAccent.MarginTop;
            MarginRight = headingAccent.MarginRight;
            MarginBottom = headingAccent.MarginBottom;
            Name = headingAccent.Name;
            OverlineImage = headingAccent.OverlineImage;
            UnderlineImage = headingAccent.UnderlineImage;
            SelectionAccentImageUri = headingAccent.SelectionAccentImageUri;
            TextBackgroundImage = headingAccent.TextBackgroundImage;
            MultipleItemsAccent = headingAccent.MultipleItemsAccent;

            GetImages(headingAccent);

            Type = GetType().Name;
        }

        public void GetImages(IAccent headingAccent)
        {
            AccentImages = (from image in headingAccent.AccentImages select new MenuImage(image)).OfType<IImage>().ToList();
        }


        /// <MetaDataID>{4b9943c9-87e4-4926-9167-e48f13369e96}</MetaDataID>
        public string Type { get; set; }

        /// <MetaDataID>{b41550de-7ac3-4507-ac64-43a662372ae8}</MetaDataID>
        [JsonIgnore]
        public string AccentColor { get; set; }


        public Multilingual MultilingualAccentImages { get => _AccentImages; set { } }

        /// <MetaDataID>{15a02b3e-5c94-47d5-9d64-8713d573af62}</MetaDataID>
        Multilingual _AccentImages = new Multilingual();
        /// <MetaDataID>{82256430-cc1b-4c00-97a2-a6a6aa02c047}</MetaDataID>
        [JsonIgnore]
        public IList<IImage> AccentImages
        {
            get
            {
                if (_AccentImages.GetValue<List<IImage>>() == null)
                    _AccentImages.SetValue<List<IImage>>(new List<IImage>());


                return _AccentImages.GetValue<List<IImage>>().AsReadOnly();
            }
            set
            {
                if (_AccentImages.GetValue<List<IImage>>() == null)
                    _AccentImages.SetValue<List<IImage>>(new List<IImage>());

                if (value != null)
                    _AccentImages.SetValue<List<IImage>>(value.ToList());
                
                    

            }
        }

        /// <MetaDataID>{8d836533-8ffc-482d-b174-bbd77e278b30}</MetaDataID>
        [JsonIgnore]
        public bool DoubleImage { get; set; }

        /// <MetaDataID>{69f2af48-dcc1-41c9-bf9f-c73b3a3f883d}</MetaDataID>
        [JsonIgnore]
        public bool FullRowImage { get; set; }

        /// <MetaDataID>{d5d8c69f-ecd5-43c5-9175-0b2fef52a1f9}</MetaDataID>
        [JsonIgnore]
        public double Height { get; set; }

        /// <MetaDataID>{3d98b31d-1cec-4fcc-b73f-6074f475425f}</MetaDataID>
        [JsonIgnore]
        public double MarginBottom { get; set; }

        /// <MetaDataID>{af267083-09a5-4f12-865a-d3db2abbb204}</MetaDataID>
        [JsonIgnore]
        public double MarginLeft { get; set; }

        /// <MetaDataID>{fb46273a-e761-4efb-bdcb-c91f13718755}</MetaDataID>
        [JsonIgnore]
        public double MarginRight { get; set; }

        /// <MetaDataID>{476f517d-1f72-4dea-95c7-5d0f6795a253}</MetaDataID>
        [JsonIgnore]
        public double MarginTop { get; set; }

        /// <MetaDataID>{16323a71-adfd-4bb3-a096-ef1b823861ee}</MetaDataID>
        public string Name { get; set; }

        /// <MetaDataID>{059fe6be-06de-45a8-b231-fc3b7059f629}</MetaDataID>
        [JsonIgnore]
        public bool OverlineImage { get; set; }

        /// <MetaDataID>{c9d26f44-1ff1-449a-a670-36a26cf796e0}</MetaDataID>
        [JsonIgnore]
        public string SelectionAccentImageUri { get; set; }
        /// <MetaDataID>{b3c9bd5f-b31b-4a1a-b21a-7cec7055df09}</MetaDataID>
        [JsonIgnore]
        public bool TextBackgroundImage { get; set; }

        /// <MetaDataID>{770925aa-1794-4175-bf52-f48ff86de19e}</MetaDataID>
        [JsonIgnore]
        public bool UnderlineImage { get; set; }
        public bool MultipleItemsAccent { get; set; }
        [JsonIgnore]
        public Unit MarginUnit { get; set; }
        [JsonIgnore]
        public double MinHeight { get; set; }
        [JsonIgnore]
        public double MinWidth { get; set; }
        [JsonIgnore]
        public bool OrgSize { get; set; }
        [JsonIgnore]
        public double Width { get; set; }

        public event ObjectChangeStateHandle ObjectChangeState;

        /// <MetaDataID>{60924e53-51fa-41e7-90f2-4e7535ff3393}</MetaDataID>
        public void AddAccentImage(IImage accentImage)
        {

            if (_AccentImages.GetValue<List<IImage>>() == null)
                _AccentImages.SetValue<List<IImage>>(new List<IImage>());

            _AccentImages.GetValue<List<IImage>>().Add(accentImage);
        }

        /// <MetaDataID>{e6e921de-2eb8-4340-b3c6-cb1e12e02a0f}</MetaDataID>
        public void ChangeOrgStyle(IAccent orgHeadingAccent)
        {
            throw new NotImplementedException();
        }

        /// <MetaDataID>{e344e61a-44c3-402c-a782-d1e2b9a278da}</MetaDataID>
        public void DeleteAccentImage(IImage accentImage)
        {
            if (_AccentImages.GetValue<List<IImage>>() == null)
                return;
            _AccentImages.GetValue<List<IImage>>().Remove(accentImage);
        }

        /// <MetaDataID>{28e7f92d-ed4d-45ad-b082-ee0bd613b56a}</MetaDataID>
        public bool IsTheSameWith(IAccent accent)
        {
            throw new NotImplementedException();
        }
    }
}
