using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceFacade;
using FlavourBusinessManager.RoomService;
using MenuModel;
using MenuPresentationModel.MenuStyles;
using OOAdvantech;
using OOAdvantech.MetaDataRepository;
using OOAdvantech.Remoting;
using UIBaseEx;

namespace MenuItemsEditor.ViewModel
{


    public delegate void CultureChangeHandle(string newLanguage);

    public delegate void PreparationItemChangedHandle(ItemPreparation preparationItem, MenuModel.IMenuItem MenuItem);

    /// <MetaDataID>{b87abe4e-a090-4874-9b02-80faa27f979f}</MetaDataID>
    [OOAdvantech.MetaDataRepository.HttpVisible]
    public interface IMenuItemTSViewModel
    {

        FlavourBusinessManager.RoomService.ItemPreparation PreparationItem { get; }

        [GenerateEventConsumerProxy]
        event CultureChangeHandle CultureChange;

        [GenerateEventConsumerProxy]
        event PreparationItemChangedHandle PreparationItemChanged;

       

        string CurrentLanguage { get; }

        MenuModel.IMenuItem MenuItem { get; }

        MealType MealType { get; }

      

    }

    /// <MetaDataID>{81ddc8c1-f978-4cdd-a091-4816f29760a0}</MetaDataID>
    public class MenuItemTSViewModel : ExtMarshalByRefObject, IMenuItemTSViewModel, IStyleSheetItemInfo
    {
        private MenuItemViewModel MenuItemViewModel;

        public MenuItemTSViewModel(MenuItemViewModel menuItemViewModel)
        {
            this.MenuItemViewModel = menuItemViewModel;

            _ExtraInfoJson = menuItemViewModel.MenuItem.ItemInfo;

        }

        public List<IMenuStyleSheet> MenusStyleSheets
        {
            get
            {
                var styleSheets = MenuItemViewModel.OrganizationMenusStyleSheets?.StyleSheets;
                if (styleSheets != null && ActiveMenuStyleSheet == null)
                {
                    ActiveMenuStyleSheet = MenuItemViewModel.OrganizationMenusStyleSheets?.StyleSheets.FirstOrDefault();
                }
                else if (ActiveMenuStyleSheet != null && (styleSheets == null || styleSheets.Contains(ActiveMenuStyleSheet)))
                {
                    ActiveMenuStyleSheet = styleSheets?.FirstOrDefault();
                }
                return styleSheets;

            }
        }

        string _ExtraInfoJson;
        public string ExtraInfoHtml
        {
            get => _ExtraInfoJson;// MenuItemViewModel.MenuItem.ItemInfo; 
            set
            {
                _ExtraInfoJson = value;
                MenuItemViewModel.MenuItem.ItemInfo = value;
                ItemChanged();

            }
        }



        IMenuStyleSheet _ActiveMenuStyleSheet;
        public IMenuStyleSheet ActiveMenuStyleSheet
        {
            get => _ActiveMenuStyleSheet;
            set
            {
                if (_ActiveMenuStyleSheet != value)
                {
                    _ActiveMenuStyleSheet = value;

                    OOAdvantech.Transactions.Transaction.RunAsynch(new Action(async () =>
                        {
                            
                            await GetExtraInfoStyleSheet(ActiveMenuStyleSheet);

                            MenuItemViewModel.CurrentMenuStyleSheet = value;
                        }));

                }



            }
        }


        public Task<ExtraInfoStyleSheet> MenuItemStyleSheet
        {
            get
            {
               

                    
                var menuItemStyleSheet=  GetExtraInfoStyleSheet(ActiveMenuStyleSheet);

                return menuItemStyleSheet;

               
            }
        }


        public async Task<ExtraInfoStyleSheet> GetExtraInfoStyleSheet(IMenuStyleSheet menuStyleSheet)
        {

            using (CultureContext cultureContext = new CultureContext(MenuItemViewModel.AddPartofMealCommand.UserInterfaceObjectConnection.Culture, MenuItemViewModel.AddPartofMealCommand.UserInterfaceObjectConnection.UseDefaultCultureWhenValueMissing))
            {
                if (menuStyleSheet == null)
                    return null;
                var styleSheet = await menuStyleSheet?.StyleSheet;

                if (CurrentMenuItemStyle != null)
                    CurrentMenuItemStyle.ObjectChangeState -= CurrentMenuItemStyle_ObjectChangeState;

                CurrentMenuItemStyle = (styleSheet?.Styles["menu-item"] as IMenuItemStyle);
                CurrentMenuItemStyle.ObjectChangeState += CurrentMenuItemStyle_ObjectChangeState;

                if (CurrentPriceStyle != null)
                    CurrentPriceStyle.ObjectChangeState -= CurrentPriceStyle_ObjectChangeState;

                CurrentPriceStyle = (styleSheet?.Styles["price-options"] as IPriceStyle);
                CurrentPriceStyle.ObjectChangeState += CurrentPriceStyle_ObjectChangeState;



                var extraInfoStyleSheet = new ExtraInfoStyleSheet()
                {
                    HeadingFont = CurrentMenuItemStyle.ItemInfoHeadingFont,
                    ParagraphFont = CurrentMenuItemStyle.ItemInfoParagraphFont,
                    ItemNameFont = CurrentMenuItemStyle.Font,
                    ItemPriceFont = CurrentPriceStyle.Font,
                    ParagraphFirstLetterFont = CurrentMenuItemStyle.ItemInfoParagraphFirstLetterFont,
                    ItemInfoFirstLetterLeftIndent = CurrentMenuItemStyle.ItemInfoFirstLetterLeftIndent,
                    ItemInfoFirstLetterRightIndent = CurrentMenuItemStyle.ItemInfoFirstLetterRightIndent,
                    ItemInfoFirstLetterLinesSpan = CurrentMenuItemStyle.ItemInfoFirstLetterLinesSpan

                };

                menuStyleSheet.UpdateItemExtraInfoStyling();


                return extraInfoStyleSheet;
            }
        }

        private void CurrentPriceStyle_ObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(this, nameof(MenuItemStyleSheet));
        }

        private void CurrentMenuItemStyle_ObjectChangeState(object _object, string member)
        {
            ObjectChangeState?.Invoke(this, nameof(MenuItemStyleSheet));
        }

        public bool IsEditToolBarVisible => true;

        public ItemPreparation PreparationItem
        {
            get
            {
                try
                {
                    //List<MenuModel.IMenuItem> menuFoodItems = new List<MenuModel.IMenuItem> { new ItemPreparation(MenuItemViewModel.MenuItem).MenuItem };

                    //var jSetttings = OOAdvantech.Remoting.RestApi.Serialization.JsonSerializerSettings.TypeRefSerializeSettings;
                    //string jsonEx = OOAdvantech.Json.JsonConvert.SerializeObject(menuFoodItems, jSetttings);


                    return new ItemPreparation(MenuItemViewModel.MenuItem);
                }
                catch (Exception error)
                {

                    throw;
                }
            }
        }

        CultureInfo _SelectedCulture;
        public CultureInfo SelectedCulture
        {
            get => _SelectedCulture;
            internal set
            {
                if (_SelectedCulture != value)
                {
                    _SelectedCulture = value;
                    CultureChange?.Invoke(_SelectedCulture.Name);
                }
            }
        }

        public string CurrentLanguage
        {
            get
            {
                if (_SelectedCulture != null)
                    return _SelectedCulture.Name;
                else
                    return null;
            }
        }
        public MealType MealType
        {
            get => MenuItemViewModel?.SelectedMealTypeViewModel?.MealType;
        }

        public IMenuItem MenuItem => new ItemPreparation(MenuItemViewModel.MenuItem).MenuItem;

        public IMenuItemStyle CurrentMenuItemStyle { get; private set; }
        public IPriceStyle CurrentPriceStyle { get; private set; }

        public event CultureChangeHandle CultureChange;

        public event PreparationItemChangedHandle PreparationItemChanged;
        public event ObjectChangeStateHandle ObjectChangeState;

        internal void ItemChanged()
        {
            var itemPreparation = new ItemPreparation(MenuItemViewModel.MenuItem);
            PreparationItemChanged?.Invoke(itemPreparation, itemPreparation.MenuItem);
        }

        public string FontsLink { get; set; } = "https://angularhost.z16.web.core.windows.net/graphicmenusresources/Fonts/Fonts.css";
    }

    public class ExtraInfoStyleSheet
    {
        public FontData HeadingFont { get; set; }

        public FontData ParagraphFont { get; set; }
        public FontData ItemNameFont { get; set; }
        public FontData ItemPriceFont { get; set; }

        public int FirstLetterLines { get; set; }

        public FontData? ParagraphFirstLetterFont { get; set; }
        public int? ItemInfoFirstLetterLeftIndent { get; set; }

        public int? ItemInfoFirstLetterRightIndent { get; set; }
        public int? ItemInfoFirstLetterLinesSpan { get; set; }
    }
}
