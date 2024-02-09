using OOAdvantech.MetaDataRepository;
using OOAdvantech;
using OOAdvantech.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MenuPresentationModel.MenuStyles;

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
    public class StyleSheetItemInfoViewModel : ExtMarshalByRefObject, IStyleSheetItemInfo
    {

        public StyleSheetItemInfoViewModel(IMenuStyleSheet menuStyleSheet)
        {
            ActiveMenuStyleSheet = menuStyleSheet;
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

        public List<IMenuStyleSheet> MenusStyleSheets => new List<IMenuStyleSheet>() { ActiveMenuStyleSheet };

        public IMenuStyleSheet ActiveMenuStyleSheet { get; set; }

        public Task<ExtraInfoStyleSheet> MenuItemStyleSheet => GetExtraInfoStyleSheet(ActiveMenuStyleSheet);

        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";

        public bool IsEditToolBarVisible => false;

        public IMenuItemStyle CurrentMenuItemStyle { get; private set; }

        public event ObjectChangeStateHandle ObjectChangeState;

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
            };


            return extraInfoStyleSheet;
        }
        private void CurrentMenuItemStyle_ObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(this, nameof(MenuItemStyleSheet));
        }

    }

}
