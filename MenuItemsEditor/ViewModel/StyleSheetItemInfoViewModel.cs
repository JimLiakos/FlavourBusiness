using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using WPFUIElementObjectBind;

namespace MenuItemsEditor.ViewModel
{
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IStyleSheetItemInfo
    {
        [GenerateEventConsumerProxy]
        event ObjectChangeStateHandle ObjectChangeState;

        string ExtraInfoHtml { get; set; }

        List<IMenuStyleSheet> MenusStyleSheets { get; }

        Task<ExtraInfoStyleSheet> GetExtraInfoStyleSheet(IMenuStyleSheet menuStyleSheet);

        IMenuStyleSheet ActiveMenuStyleSheet { get; set; }

        Task<ExtraInfoStyleSheet> MenuItemStyleSheet { get; }

        string FontsLink { get; }

        bool IsEditToolBarVisible { get; }
    }
    public class StyleSheetItemInfoViewModel : ExtMarshalByRefObject, IStyleSheetItemInfo, INotifyPropertyChanged
    {

        public StyleSheetItemInfoViewModel(IMenuStyleSheet menuStyleSheet)
        {
            ActiveMenuStyleSheet = menuStyleSheet;

            var task = menuStyleSheet.StyleSheet;
            task.Wait();

            GetExtraInfoStyleSheet(ActiveMenuStyleSheet).Wait();

            var styleSheet = task.Result;


            MenuItemStyle = styleSheet.Styles["menu-item"] as IMenuItemStyle;
            this.FirstLetterStylingIsActive = !(MenuItemStyle.ItemInfoFirstLetterLeftIndent == null && MenuItemStyle.ItemInfoFirstLetterLinesSpan == null && MenuItemStyle.ItemInfoParagraphFirstLetterFont == null);

            HeadingFontChangeCommand = new RelayCommand((object sender) =>
            {
                ActiveMenuStyleSheet.ChangeItemInfoHeadingFont();
            });

            ParagraphFontChangeCommand = new RelayCommand((object sender) =>
            {
                ActiveMenuStyleSheet.ChangeItemInfoParagraphFont();
            });
            ParagraphFirstLetterFontChangeCommand = new RelayCommand((object sender) =>
            {
                ActiveMenuStyleSheet.ChangeItemInfoParagraphFirstLetterFont();
            });


        }

        public RelayCommand HeadingFontChangeCommand { get; protected set; }

        public RelayCommand ParagraphFontChangeCommand { get; protected set; }

        public RelayCommand ParagraphFirstLetterFontChangeCommand { get; protected set; }

        public bool FirstLetterStylingIsActive
        {
            get
            {
                return this.FirstLetterStylingIsActive = MenuItemStyle.ItemInfoFirstLetterLeftIndent != null || MenuItemStyle.ItemInfoFirstLetterLinesSpan == null || MenuItemStyle.ItemInfoParagraphFirstLetterFont == null;

            }
            set
            {
                if (value && MenuItemStyle.ItemInfoParagraphFirstLetterFont == null)
                    MenuItemStyle.ItemInfoParagraphFirstLetterFont = MenuItemStyle.ItemInfoParagraphFont;
                if (!value)
                {
                    MenuItemStyle.ItemInfoFirstLetterLeftIndent = null;
                    MenuItemStyle.ItemInfoFirstLetterLinesSpan = null;
                    MenuItemStyle.ItemInfoParagraphFirstLetterFont = null;
                }
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstLetterLines)));
                //

                ObjectChangeState?.Invoke(this, nameof(MenuItemStyleSheet));
            }
        }
        public string ExtraInfoHtml
        {
            get
            {
                return Properties.Resources.ItemInfoViewSampleHtml;

  
            }
            set
            {

            }
        }


        public int FirstLetterLeftIndent
        {
            get
            {
                if (CurrentMenuItemStyle == null)
                    return 0;
                return CurrentMenuItemStyle.ItemInfoFirstLetterLeftIndent.Value;
            }
            set
            {
                if (CurrentMenuItemStyle != null)
                    CurrentMenuItemStyle.ItemInfoFirstLetterLeftIndent = value;

            }
        }

        public int FirstLetterRightIndent
        {
            get
            {
                if (CurrentMenuItemStyle == null)
                    return 0;
                return CurrentMenuItemStyle.ItemInfoFirstLetterRightIndent.Value;
            }
            set
            {
                if (CurrentMenuItemStyle != null)
                    CurrentMenuItemStyle.ItemInfoFirstLetterRightIndent = value;

            }
        }
        public int FirstLetterLines
        {
            get
            {
                if (CurrentMenuItemStyle == null)
                    return 1;
                return CurrentMenuItemStyle.ItemInfoFirstLetterLinesSpan.Value;
            }
            set
            {
                if (CurrentMenuItemStyle != null)
                    CurrentMenuItemStyle.ItemInfoFirstLetterLinesSpan = value;

            }
        } 

        public List<int> FirstLetterLinesOptions
        {
            get;
        } = new List<int>() { 1, 2, 3, 4 };

        public List<IMenuStyleSheet> MenusStyleSheets => new List<IMenuStyleSheet>() { ActiveMenuStyleSheet };

        public IMenuStyleSheet ActiveMenuStyleSheet { get; set; }

        public Task<ExtraInfoStyleSheet> MenuItemStyleSheet => GetExtraInfoStyleSheet(ActiveMenuStyleSheet);

        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";

        public bool IsEditToolBarVisible => false;

        public IMenuItemStyle CurrentMenuItemStyle { get; private set; }
        public IMenuItemStyle MenuItemStyle { get; private set; }

        public event ObjectChangeStateHandle ObjectChangeState;
        public event PropertyChangedEventHandler PropertyChanged;

        public async Task<ExtraInfoStyleSheet> GetExtraInfoStyleSheet(IMenuStyleSheet menuStyleSheet)
        {
            if (menuStyleSheet == null)
                return null;
            var styleSheet = await menuStyleSheet?.StyleSheet;

            if (CurrentMenuItemStyle != null)
                CurrentMenuItemStyle.ObjectChangeState -= CurrentMenuItemStyle_ObjectChangeState;

            CurrentMenuItemStyle = (styleSheet?.Styles["menu-item"] as IMenuItemStyle);
            CurrentMenuItemStyle.ObjectChangeState += CurrentMenuItemStyle_ObjectChangeState;

            
            var extraInfoStyleSheet = new ExtraInfoStyleSheet()
            {
                HeadingFont = CurrentMenuItemStyle.ItemInfoHeadingFont,
                ParagraphFont = CurrentMenuItemStyle.ItemInfoParagraphFont,
                ItemNameFont = CurrentMenuItemStyle.Font,
                ParagraphFirstLetterFont = CurrentMenuItemStyle.ItemInfoParagraphFirstLetterFont,
                ItemInfoFirstLetterLeftIndent = CurrentMenuItemStyle.ItemInfoFirstLetterLeftIndent,
                ItemInfoFirstLetterRightIndent = CurrentMenuItemStyle.ItemInfoFirstLetterRightIndent,
                ItemInfoFirstLetterLinesSpan = CurrentMenuItemStyle.ItemInfoFirstLetterLinesSpan
            };


            return extraInfoStyleSheet;
        }
        private void CurrentMenuItemStyle_ObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(this, nameof(MenuItemStyleSheet));
        }

    }

}
