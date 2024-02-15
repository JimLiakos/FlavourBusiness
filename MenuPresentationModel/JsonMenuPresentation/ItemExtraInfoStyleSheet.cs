
using OOAdvantech;
using OOAdvantech.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UIBaseEx;

namespace MenuPresentationModel.JsonMenuPresentation
{
    /// <MetaDataID>{44cf9a02-978d-40f8-aa8d-ffc749775396}</MetaDataID>
    public class ItemExtraInfoStyleSheet : IItemExtraInfoStyleSheet
    {

        public ItemExtraInfoStyleSheet()
        {

        }
        public ItemExtraInfoStyleSheet(RestaurantMenu restaurantMenu, IItemExtraInfoStyleSheet itemExtraInfoStyleSheet)
        {

            foreach (var languageEntry in itemExtraInfoStyleSheet.MultilingualHeadingFont.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    HeadingFont = itemExtraInfoStyleSheet.HeadingFont;
                    HeadingFontID = restaurantMenu.GetFontID(HeadingFont);
                }
            }
            foreach (var languageEntry in itemExtraInfoStyleSheet.MultilingualParagraphFont.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    ParagraphFont = itemExtraInfoStyleSheet.ParagraphFont;
                    ParagraphFontID = restaurantMenu.GetFontID(ParagraphFont);
                }
            }
            //menuItemStyle.ItemInfoHeadingFont.

        }


        [JsonIgnore]
        public Multilingual MultilingualHeadingFont { get; set; } = new Multilingual();

        [JsonIgnore]
        public FontData HeadingFont { get => MultilingualHeadingFont.GetValue<FontData>(); set => MultilingualHeadingFont.SetValue<FontData>(value); }

        public int HeadingFontID { get => MultilingualHeadingFont.GetValue<int>(); set => MultilingualHeadingFontID.SetValue<int>(value); }
        public Multilingual MultilingualHeadingFontID = new Multilingual();





        [JsonIgnore]
        public FontData ParagraphFirstLetterFont
        {
            get => MultilingualParagraphFirstLetterFont.GetValue<FontData>();
            set => MultilingualParagraphFirstLetterFont.SetValue<FontData>(value);
        }

        [JsonIgnore]
        public Multilingual MultilingualParagraphFirstLetterFont { get; set; } = new Multilingual();

        public int ParagraphFirstLetterFontID { get => MultilingualParagraphFirstLetterFontID.GetValue<int>(); set => MultilingualParagraphFirstLetterFontID.SetValue<int>(value); }
        public Multilingual MultilingualParagraphFirstLetterFontID = new Multilingual();


        [JsonIgnore]
        public FontData ParagraphFont { get => MultilingualParagraphFont.GetValue<FontData>(); set => MultilingualParagraphFont.SetValue<FontData>(value); }

        [JsonIgnore]
        public Multilingual MultilingualParagraphFont { get; set; }=new Multilingual();


        public int ParagraphFontID { get => MultilingualParagraphFontID.GetValue<int>(); set => MultilingualParagraphFontID.SetValue<int>(value); }
        public Multilingual MultilingualParagraphFontID = new Multilingual();




    }
}
