
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
            foreach (var languageEntry in itemExtraInfoStyleSheet.MultilingualParagraphFirstLetterFont.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    ParagraphFirstLetterFont = itemExtraInfoStyleSheet.ParagraphFirstLetterFont;
                    if (ParagraphFirstLetterFont.HasValue)
                        ParagraphFirstLetterFontID = restaurantMenu.GetFontID(ParagraphFirstLetterFont.Value);
                    else
                        ParagraphFirstLetterFontID=null;
                }
            }

            foreach (var languageEntry in itemExtraInfoStyleSheet.MultilingualItemInfoFirstLetterLinesSpan.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    ItemInfoFirstLetterLinesSpan = itemExtraInfoStyleSheet.ItemInfoFirstLetterLinesSpan;
                }
            }

            foreach (var languageEntry in itemExtraInfoStyleSheet.MultilingualItemInfoFirstLetterLeftIndent.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    ItemInfoFirstLetterLeftIndent = itemExtraInfoStyleSheet.ItemInfoFirstLetterLeftIndent;
                }
            }

            foreach (var languageEntry in itemExtraInfoStyleSheet.MultilingualItemInfoFirstLetterRightIndent.Values)
            {
                using (OOAdvantech.CultureContext context = new OOAdvantech.CultureContext(System.Globalization.CultureInfo.GetCultureInfo(languageEntry.Key), false))
                {
                    ItemInfoFirstLetterRightIndent = itemExtraInfoStyleSheet.ItemInfoFirstLetterRightIndent;
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
        public FontData? ParagraphFirstLetterFont
        {
            get => MultilingualParagraphFirstLetterFont.GetValue<FontData?>();
            set => MultilingualParagraphFirstLetterFont.SetValue<FontData?>(value);
        }

        [JsonIgnore]
        public Multilingual MultilingualParagraphFirstLetterFont { get; set; } = new Multilingual();

        public int? ParagraphFirstLetterFontID
        {
            get => MultilingualParagraphFirstLetterFontID.GetValue<int?>();
            set => MultilingualParagraphFirstLetterFontID.SetValue<int?>(value);
        }
        public Multilingual MultilingualParagraphFirstLetterFontID = new Multilingual();


        [JsonIgnore]
        public FontData ParagraphFont { get => MultilingualParagraphFont.GetValue<FontData>(); set => MultilingualParagraphFont.SetValue<FontData>(value); }

        [JsonIgnore]
        public Multilingual MultilingualParagraphFont { get; set; } = new Multilingual();


        public int ParagraphFontID { get => MultilingualParagraphFontID.GetValue<int>(); set => MultilingualParagraphFontID.SetValue<int>(value); }
        public int? ItemInfoFirstLetterLeftIndent { get => MultilingualItemInfoFirstLetterLeftIndent.GetValue<int?>(); set => MultilingualItemInfoFirstLetterLeftIndent.SetValue<int?>(value); }
        public int? ItemInfoFirstLetterRightIndent { get => MultilingualItemInfoFirstLetterRightIndent.GetValue<int?>(); set => MultilingualItemInfoFirstLetterRightIndent.SetValue<int?>(value); }
        public int? ItemInfoFirstLetterLinesSpan { get => MultilingualItemInfoFirstLetterLinesSpan.GetValue<int?>(); set => MultilingualItemInfoFirstLetterLinesSpan.SetValue<int?>(value); }


        public Multilingual MultilingualItemInfoFirstLetterLeftIndent { get; set; } = new Multilingual();

        public Multilingual MultilingualItemInfoFirstLetterRightIndent { get; set; } = new Multilingual();

        public Multilingual MultilingualItemInfoFirstLetterLinesSpan { get; set; } = new Multilingual();

        public Multilingual MultilingualParagraphFontID = new Multilingual();




    }
}
