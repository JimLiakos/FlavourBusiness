using OOAdvantech;

namespace MenuPresentationModel.MenuStyles
{
    /// <MetaDataID>{9399b994-5271-4848-9f26-8a57a5a30ada}</MetaDataID>
    public interface IPageImage
    {
        /// <MetaDataID>{a4b5caff-e014-4b55-8272-16469cf02d0a}</MetaDataID>
        double Opacity { get; set; }

        /// <MetaDataID>{c1be6bf8-7766-42ed-8fa7-6b822ce895ed}</MetaDataID>
        bool Mirror { get; set; }

        /// <MetaDataID>{b2949538-4b32-4ea1-98bf-637a1fbba72b}</MetaDataID>
        bool Flip { get; set; }

        /// <MetaDataID>{ae2c7923-062b-4c84-933d-8e7061b986dd}</MetaDataID>
        string Color { get; set; }

        /// <MetaDataID>{7d991e0d-4f6a-4516-993a-8d6303462c73}</MetaDataID>
        string Name { get; set; }

        /// <MetaDataID>{e84e3bef-8f36-46bd-bd7f-39a488ca0418}</MetaDataID>
        string LandscapeUri { get; }

        /// <MetaDataID>{57eed5f1-13d3-4b40-935c-3e69118a668c}</MetaDataID>
        double LandscapeHeight { get; set; }

        /// <MetaDataID>{382834f9-14e4-4ec9-81f0-3e94335ab98a}</MetaDataID>
        double LandscapeWidth { get; set; }

        /// <MetaDataID>{b25aaf2d-0fb3-49b1-8ab9-2e60c155ed2f}</MetaDataID>
        Resource LandscapeImage { get; set; }

        /// <MetaDataID>{a9813cbe-1122-48e6-8382-170fe2dc7674}</MetaDataID>
        string PortraitUri { get; }

        /// <MetaDataID>{ea7bba97-273c-4292-89fa-527c0d87cf6a}</MetaDataID>
        double PortraitHeight { get; set; }

        /// <MetaDataID>{6208f355-568b-455e-a5fc-c543f22f2203}</MetaDataID>
        double PortraitWidth { get; set; }

        /// <MetaDataID>{e677e1eb-a479-42ee-80d2-46dfbaf4d716}</MetaDataID>
        Resource PortraitImage { get; set; }

        /// <MetaDataID>{5564eafd-e314-4951-a606-f16902acf8fc}</MetaDataID>
        bool IsVectorImage { get; }

        event ObjectChangeStateHandle ObjectChangeState;
    }
}